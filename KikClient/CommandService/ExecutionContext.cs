using KikClient.CommandService.Models;
using KikClient.Database;
using KikClient.Utilities;
using System.Collections.Generic;
using System.Xml.Linq;

/**
 * Created by Moon on 1/32/2021
 * When a command from a command module is executed,
 * it needs a way to access the context, ie:
 * information about the group and user the command
 * came from
 */

namespace KikClient.CommandService
{
    public class ExecutionContext
    {
        public List<CommandModule> Modules { get; private set; }
        public Client Client { get; private set; }
        public string GroupJid { get; private set; }
        public Group Group => Client.GetGroup(GroupJid);
        public string UserJid { get; private set; }
        public User User => Client.GetUser(UserJid);
        public string Username => UserJid.GetUsernameFromJid();
        public long Timestamp { get; private set; }
        public XElement Element { get; private set; }
        public ExecutionContext(List<CommandModule> modules, Client client, string groupJid, string userJid, long timestamp, XElement element)
        {
            Modules = modules;
            Client = client;
            GroupJid = groupJid;
            UserJid = userJid;
            Timestamp = timestamp;
            Element = element;
        }
    }
}
