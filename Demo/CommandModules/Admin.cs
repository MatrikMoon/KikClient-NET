using KikClient.CommandService.Attributes;
using KikClient.Database;
using KikClient.Networking;
using Mobile.Profile.V1;
using System.Threading.Tasks;
using ExecutionContext = KikClient.CommandService.ExecutionContext;

namespace Demo.CommandModules
{
    [CommandModule]
    class Admin
    {
        public ExecutionContext ExecutionContext { get; set; }

        [Command("setBio")]
        [RequireAdmin]
        public async Task SetBio(string bio)
        {
            await ExecutionContext.Client.Send(StanzaGenerator.SetUserProfile(new SetUserProfileRequest
            {
                Id = new Common.XiBareUserJid
                {
                    LocalPart = ExecutionContext.Client.Jid.Substring(0, ExecutionContext.Client.Jid.IndexOf("@"))
                },
                Bio = new Common.Profile.V1.BioAction
                {
                    ElementBio = new Common.Entity.V1.BioElement
                    {
                        Bio = bio
                    }
                }
            }).Item1);
        }

        [Command("leave")]
        [RequireAdmin]
        public async Task Leave(Group group)
        {
            if (group != null)
            {
                await ExecutionContext.Client.Send(StanzaGenerator.LeaveGroup(group.GroupJid).Item1);
            }
            else await ExecutionContext.Client.Message(ExecutionContext.GroupJid, "That group is not currently in the bot's database. This command only works on groups the bot has interacted with before.");
        }
    }
}
