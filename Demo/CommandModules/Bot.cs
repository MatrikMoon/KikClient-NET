using KikClient;
using KikClient.CommandService.Attributes;
using KikClient.Database;
using KikClient.Networking;
using KikClient.Utilities;
using System;
using System.Linq;
using System.Threading.Tasks;
using ExecutionContext = KikClient.CommandService.ExecutionContext;

/**
 * Created by Moon on 1/29/2021 as research module - repurposed for demo on 8/4/2021
 * This is an example of a module. A module holds your commands. You don't need more than one module,
 * but you may choose to have more than one to categorize your commands. See the below commands for examples
 */

namespace Demo.CommandModules
{
    [CommandModule] //This tag lets the command service know that commands live in this class
    class Bot
    {
        //ExecutionContext will hold all sorts of information about the current situation when a command runs.
        //For exapmle, this is where you'd find the current group the command is coming from, who ran it, and more
        public ExecutionContext ExecutionContext { get; set; }

        //This is an example of a command.
        //The string parameters to the attribute are all the strings that can be used to run the command.
        //You can have as many as you'd like!
        //This is a complicated method body though, so you should jump down and read some of the ones below to get an idea of what you can do.
        //Keep an eye out for my commentary as you scroll through
        [Command("help", "h")]
        public async Task Help()
        {
            var reply = "Command Modules:\n\n";

            foreach (var module in ExecutionContext.Modules)
            {
                reply += $"\t⚫{module.Name}\n";
            }

            reply += "\nTo get a list of the commands contained in the module, type \".help (module name)\"";

            await ExecutionContext.Client.Message(ExecutionContext.GroupJid, reply);
        }

        //Notice how this command takes a parameter!
        //If you add a single string parameter, you'll be able to read the entire rest of the command
        //after the initial command word. You can also use other types like `long` and `int` and even `User`,
        //though the latter may still be broken.
        //An exapmle of such a command might be:
        //".command hello 191 username"
        //And the corresponding method declaration would be:
        //`public async Task Command(string text, int number, User user)`
        [Command("help", "h")]
        public async Task Help(string name)
        {
            var reply = string.Empty;

            var module = ExecutionContext.Modules.FirstOrDefault(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            var command =
                ExecutionContext.Modules
                .SelectMany(x => x.Commands.Where(y => y.Names.Any(z => z.Equals(name, StringComparison.OrdinalIgnoreCase))))
                .Where(x => !string.IsNullOrEmpty(x.Summary))
                .FirstOrDefault();

            if (module != null)
            {
                reply += $"{module.Name} Commands:\n\n";

                var uniqueCommandNames = module.Commands.Select(x => x.Names[0]).Distinct();
                foreach (var commandName in uniqueCommandNames)
                {
                    var uniqueCommand = module.Commands.First(x => x.Names[0] == commandName);
                    reply += $"\t⚫{uniqueCommand.Names[0]}{(uniqueCommand.Names.Length > 1 ? $"\t({string.Join(")\t(", uniqueCommand.Names.Skip(1))})" : "")}\n";
                }

                reply += "To get more info about a specific command, type \".help (command name)\"";
            }
            else if (command != null)
            {
                reply += command.Summary;
            }
            else
            {
                reply += "That module/command does not exist, or does not have a summary attached";
            }

            await ExecutionContext.Client.Message(ExecutionContext.GroupJid, reply.Trim());
        }

        //This command demonstrates the sticker generation capabilities.
        //It just takes the text and turns it into an image, but if you're interested in
        //that sort of thing feel free to open up the StanzaGenerator to see how it works
        [Command("sticker")]
        public async Task Sticker(string text)
        {
            await ExecutionContext.Client.Send(StanzaGenerator.Sticker(text, ExecutionContext.GroupJid, fontSize: 12).Item1);
        }

        //Like the above sticker generator, this uses the input text, but this one turns it into an image instead
        //Note `Media` has multiple constructors, one of which accepts a stream. So you can pass in just about any image you want
        [Command("image")]
        public async Task Image(string text)
        {
            await ExecutionContext.Client.Send((await StanzaGenerator.Media(text, ExecutionContext.GroupJid, ExecutionContext.Client.Jid, ExecutionContext.Client.Passkey, Client.versionName, fontSize: 120)).Item1);
        }

        //This command takes an image and converts it into a sticker.
        //Hop into the method body real quick and let me show you some specifics
        [Command("convertToSticker")]
        public async Task ConvertToSticker()
        {
            //When you generate a message stanza, it returns a tuple. The first part is the xmpp, the second is the message's id.
            //This is irrelevant in thsi case, but for some purposes it can be useful. For example, if we're waiting for a server response on that stanza
            var messageStanza = StanzaGenerator.Message(ExecutionContext.GroupJid, message: "Waiting 15 seconds for you to send an image");

            //Notice we can wait for a particular response. The second parameter is a callback which parses incoming xmpp.
            //It returns `true` if the stanza matched the response we're waiting for, `false` if not. THIS IS IMPORTANT.
            //I'm not going to explain how xmpp works, but you're free to modify this to your purposes
            await ExecutionContext.Client.SendAndAwaitResponse(messageStanza.Item1, async (element) =>
            {
                if (element.Name.LocalName == "message")
                {
                    var type = element.Attribute("type").Value;
                    var from = element.Attribute("from").Value;

                    if (from == ExecutionContext.UserJid)
                    {
                        var content = element.ElementAnyNS("content");
                        var images = content?.ElementAnyNS("images");
                        var preview = images?.ElementAnyNS("preview")?.Value;

                        if (preview != null)
                        {
                            await ExecutionContext.Client.Send(StanzaGenerator.Sticker(Convert.FromBase64String(preview), ExecutionContext.GroupJid).Item1);
                            return true;
                        }
                    }
                }
                return false;
            },
            
            //We can also pass in a timeout function, so if the above callback hasn't seen a valid response in the time allotted, this callback will run.
            onTimeout: async () =>
            {
                await ExecutionContext.Client.Message(ExecutionContext.GroupJid, "Image not received in time. Please try again.");
            },

            //This is the designated timeout
            timeout: 15000);
        }

        //A simple echo command
        [Command("say")]
        public async Task Say(string message)
        {
            await ExecutionContext.Client.Message(ExecutionContext.GroupJid, message);
        }

        //Notice the new attribute! This command will only run for admins
        [Command("roulette")]
        [RequireAdmin]
        public async Task Roulette()
        {
            var members = ExecutionContext.Client.GetGroupMembers(ExecutionContext.GroupJid).Where(x => x.Role == Role.None).ToList();
            if (members.Count > 0)
            {
                var unluckyOne = members[Misc.Utilities.Random.Next(members.Count - 1)];
                await ExecutionContext.Client.RemoveUsersFromGroup(ExecutionContext.GroupJid, unluckyOne.UserJid);
            }
        }

        //Yet another new attribute! This command will only run in groups
        [Command("groupInfo")]
        [RequireGroup]
        public async Task GroupInfo()
        {
            var group = ExecutionContext.Group;
            var groupInfo = $"Jid: {group.GroupJid}\n" +
                $"Name: {group.Name}\n" +
                $"Code: {group.Code}\n" +
                $"Pic {group.Pic}/orig.jpg";
            await ExecutionContext.Client.Message(ExecutionContext.GroupJid, groupInfo);
        }

        //This command takes a Group as a parameter. This is passed in the form of a jid. For example:
        //`.groupInfo 1100252868910_g@groups.kik.com` OR `.groupInfo #anime`
        [Command("groupInfo")]
        public async Task GroupInfo(Group group)
        {
            if (group != null)
            {
                var groupInfo = $"Jid: {group.GroupJid}\n" +
                    $"Name: {group.Name}\n" +
                    $"Code: {group.Code}\n" +
                    $"Pic {group.Pic}/orig.jpg";
                await ExecutionContext.Client.Message(ExecutionContext.GroupJid, groupInfo);
            }
            else await ExecutionContext.Client.Message(ExecutionContext.GroupJid, "That group is not currently in the bot's database. This command only works on groups the bot has interacted with before.");
        }

        [Command("userInfo")]
        [ResolveAlias]
        public async Task UserInfo()
        {
            var user = ExecutionContext.User;
            var userInfo = $"Jid: {user.UserJid}\n" +
                $"Name: {user.DisplayName}\n" +
                $"Account created: {new DateTime(user.AccountCreationDate * TimeSpan.TicksPerMillisecond)}\n" +
                $"Pic {user.ProfilePic}/orig.jpg";
            await ExecutionContext.Client.Message(ExecutionContext.GroupJid, userInfo);
        }

        [Command("userInfo")]
        public async Task UserInfo(User user)
        {
            if (user != null)
            {
                var userInfo = $"Jid: {user.UserJid}\n" +
                    $"Name: {user.DisplayName}\n" +
                    $"Account created: {new DateTime(user.AccountCreationDate * TimeSpan.TicksPerMillisecond)}\n" +
                    $"Pic {user.ProfilePic}/orig.jpg";
                await ExecutionContext.Client.Message(ExecutionContext.GroupJid, userInfo);
            }
            else await ExecutionContext.Client.Message(ExecutionContext.GroupJid, "That user is not currently in the bot's database. This command only works on users the bot has interacted with before.");
        }
    }
}
