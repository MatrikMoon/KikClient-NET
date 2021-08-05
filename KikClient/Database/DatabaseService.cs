using KikClient.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

/**
* Created by Moon on 5/18/2019
* A service for interfacing with a EF database
*/

namespace KikClient.Database
{
    public class DatabaseService
    {
        public DatabaseContext DatabaseContext { get; private set; }

        public enum ActivityType
        {
            Talking,
            Reading
        }

        public DatabaseService(string location)
        {
            DatabaseContext = new DatabaseContext(location);

            //Ensure database is created
            DatabaseContext.Database.EnsureCreated();
        }

        public string AliasLookup(string alias)
        {
            return DatabaseContext.Aliases.FirstOrDefault(x => x.AliasJid == alias && !x.Old)?.ResolvedJid;
        }

        public async Task AddAlias(string alias, string resolvedJid, string groupJid, string displayName, string profilePic, long profilePicLastUpdated)
        {
            await DatabaseContext.Aliases.AddAsync(new Alias
            {
                AliasJid = alias,
                ResolvedJid = resolvedJid,
                GroupJid = groupJid,
                DisplayName = displayName,
                ProfilePic = profilePic,
                ProfilePicLastUpdated = profilePicLastUpdated,
                Old = false
            });

            await DatabaseContext.SaveChangesAsync();
        }

        //Tracks the last time someone talks in a group
        public async Task UpdateGroupActivity(string groupJid, string userJid, ActivityType activityType)
        {
            var activityToUpdate = DatabaseContext.GroupActivity.FirstOrDefault(x => x.GroupJid == groupJid && x.UserJid == userJid && !x.Old);
            if (activityToUpdate == null)
            {
                activityToUpdate = new GroupActivity
                {
                    GroupJid = groupJid,
                    UserJid = userJid
                };
                await DatabaseContext.GroupActivity.AddAsync(activityToUpdate);
            }

            if (activityType == ActivityType.Talking) activityToUpdate.LastTalkDate = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            else if (activityType == ActivityType.Reading) activityToUpdate.LastReadDate = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

            await DatabaseContext.SaveChangesAsync();
        }

        public User GetUser(string userJid)
        {
            if (userJid.IsAlias()) userJid = AliasLookup(userJid);
            return DatabaseContext.Users.FirstOrDefault(x => x.UserJid == userJid);
        }

        public User GetUserByUsername(string username)
        {
            return DatabaseContext.Users.FirstOrDefault(x => x.Username == username);
        }

        public Group GetGroup(string groupJid)
        {
            return DatabaseContext.Groups.FirstOrDefault(x => x.GroupJid == groupJid);
        }

        public Group GetGroupByTag(string groupTag)
        {
            if (!groupTag.StartsWith("#")) groupTag = $"#{groupTag}";
            return DatabaseContext.Groups.FirstOrDefault(x => x.Code == groupTag);
        }

        public List<GroupMember> GetGroupMembers(string groupJid)
        {
            return DatabaseContext.GroupMembers.Where(x => x.GroupJid == groupJid && !x.Old).ToList();
        }

        //Parses a single roster "item" item and updates the database
        public async Task ParseItem(XElement item, bool save = true)
        {
            var jid = item.Attribute("jid").Value;
            var username = item.Element("{jabber:iq:roster}username").Value;
            var displayname = item.Element("{jabber:iq:roster}display-name").Value;
            var picElement = item.Element("{jabber:iq:roster}pic");
            var picTimestamp = picElement?.Attribute("ts")?.Value;
            var pic = picElement?.Value;

            var updatedItem = DatabaseContext.Users.FirstOrDefault(x => x.UserJid == jid && !x.Old);
            if (updatedItem == null)
            {
                updatedItem = new User();
                DatabaseContext.Users.Add(updatedItem);
            }

            updatedItem.UserJid = jid;
            updatedItem.Username = username;
            updatedItem.DisplayName = displayname;
            updatedItem.ProfilePic = pic;
            updatedItem.ProfilePicLastUpdated = string.IsNullOrEmpty(picTimestamp) ? 0 : long.Parse(picTimestamp);

            if (save) await DatabaseContext.SaveChangesAsync();
        }

        //Parses a single roster "group" item and updates the database, group members too
        public async Task ParseGroup(XElement group) => await ParseGroup(group, true);

        public async Task ParseGroup(XElement group, bool save = true)
        {
            var jid = group.Attribute("jid").Value;
            var name = group.Element("{jabber:iq:roster}n")?.Value;
            var code = group.Element("{jabber:iq:roster}code")?.Value;
            var picElement = group.Element("{jabber:iq:roster}pic");
            var picTimestamp = picElement?.Attribute("ts")?.Value;
            var pic = picElement?.Value;

            var updatedGroup = DatabaseContext.Groups.FirstOrDefault(x => x.GroupJid == jid && !x.Old);
            if (updatedGroup == null)
            {
                updatedGroup = new Group();
                DatabaseContext.Groups.Add(updatedGroup);
            }

            updatedGroup.GroupJid = jid;
            updatedGroup.Name = name;
            updatedGroup.Code = code;
            updatedGroup.Pic = pic;
            updatedGroup.PicTimestamp = string.IsNullOrEmpty(picTimestamp) ? 0 : long.Parse(picTimestamp);

            var members = group.Elements("{jabber:iq:roster}m");

            //Dump group members. Some may have left or been banned. We're guaranteed to have all the current members here anyway
            DatabaseContext.GroupMembers.RemoveRange(GetGroupMembers(jid));

            foreach (var member in members)
            {
                var memberJid = member.Value;
                var admin = member.Attribute("a")?.Value == "1";
                var superAdmin = member.Attribute("s")?.Value == "1";
                var disabledDms = member.Attribute("dmd")?.Value == "1";

                var newMember = new GroupMember();
                DatabaseContext.GroupMembers.Add(newMember);

                if (superAdmin) newMember.Role = Role.Owner;
                else if (admin) newMember.Role = Role.Admin;
                newMember.UserJid = memberJid;
                newMember.GroupJid = jid;
                newMember.DisabledDms = disabledDms;
            }

            if (save) await DatabaseContext.SaveChangesAsync();
        }
    }
}