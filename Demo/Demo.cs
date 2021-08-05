using KikClient.CommandService;
using Microsoft.Extensions.DependencyInjection;
using Demo.Misc;
using System;
using System.Reflection;
using System.Threading.Tasks;

/**
 * Created as Casino client by Moon on 1/12/2021 - modified to be demo client on 8/4/2021
 * This file is the entry point for the demo bot.
 * Read along in the comments to see what each line does
 * 
 * Heavy inspiration from Discord.NET
 */

namespace Demo
{
    class Demo
    {
        //The command service is what finds your command definitions and makes sure incoming commands get there.
        //If you want to peek under the hood, go for it, but you shouldn't have to if you're just writing a quick bot
        private CommandService CommandService { get; set; }

        //The Client is the thing that actually talks to kik, and acts as an instance of the app.
        //Again, you shoudln't need to peek inside unless you want to
        private KikClient.Client Client { get; set; }

        //Our entry point. We start an instance of this class and let it run asynchronously
        public static void Main(string[] args)
        {
#pragma warning disable CS4014
            new Demo(args).Start();
#pragma warning restore CS4014
            Console.ReadLine();
        }

        private Demo(string[] args)
        {
            //We can take a username/password as parameters. For example:
            //`./bot -username trainzboy -password thisisnotmypassword`
            var argString = string.Join(" ", args);
            var username = Utilities.ParseArgs(argString, "username");
            var password = Utilities.ParseArgs(argString, "password");

            //If there's no username provided, we'll need to ask the user for account details on the spot
            if (string.IsNullOrEmpty(username))
            {
                Console.Write("Username: ");
                username = Console.ReadLine().Trim();

                Console.Write("Password (optional): ");
                password = Console.ReadLine().Trim();
            }

            //The client can log in with a username/password, or just a username if it's logged in before.
            //It remembers the generated passkey for resuming sessions and will do so automatically if it can
            if (!string.IsNullOrEmpty(username))
            {
                if (!string.IsNullOrEmpty(password))
                {
                    Client = new KikClient.Client(username, password);
                }
                else Client = new KikClient.Client(username);

                //You can optionally enable history retrival on login. The bot will request all the new messages since it was logged out,
                //and parse them as if they were just run. This is usually not expected behavior for a bot.
                //Client.EnableMessageHistoryRetrival = true;
            }
            else
            {
                throw new ArgumentException("You must provide a username to log in to, either by command line parameter or the interactive shell.");
            }

            //Set up the command service. Once this is done, the command service will handle getting incoming commands to the correct module/command definition
            //Now's the time to see Modules if you've read this far
            CommandService = new CommandService(Client);
            CommandService.Initialize(Assembly.GetExecutingAssembly(), new ServiceCollection()
                //.AddSingleton<PictureService>()       //You can add services here. To learn more about services, see how they're used in Discord.NET (https://github.com/discord-net/Discord.Net/blob/dev/samples/02_commands_framework/Services/PictureService.cs)
                .BuildServiceProvider());
        }

        private async Task Start()
        {
            await Client.Start();
        }
    }
}
