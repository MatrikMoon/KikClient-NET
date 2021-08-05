using KikClient.CommandService.Attributes;
using KikClient.CommandService.Models;
using KikClient.Database;
using KikClient.Utilities;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml.Linq;

/**
 * Created by Moon on 1/30/2021
 * An optional feature, processes commands
 * from the kik client
 */

namespace KikClient.CommandService
{
    public class CommandService
    {
        public string CommandPrefix { get; set; } = ".";
        private List<CommandModule> CommandModules { get; set; } = new List<CommandModule>();
        private Client Client { get; set; }
        private IServiceProvider Services { get; set; }

        public CommandService(Client client)
        {
            Client = client;
            Client?.RegisterCommandService(this);
        }

        /// <summary>
        /// Initializes the CommandServices by searching the provided assembly for
        /// the relevant attributes and building the corresponding model list
        /// </summary>
        /// <param name="assembly">The assembly to be searched for command modules</param>
        /// <param name="services">Optionally, you can include a ServiceProvider to be included in the ExecutionContext which is provided to your commands on execution</param>
        public void Initialize(Assembly assembly, IServiceProvider services = null)
        {
            Services = services;

            foreach (var type in assembly.GetTypes())
            {
                if (type.IsClass)
                {
                    var moduleAttribute = type.GetCustomAttribute<CommandModuleAttribute>();
                    if (moduleAttribute != null)
                    {
                        var commandsInModule = new List<Command>();

                        foreach (var method in type.GetMethods())
                        {
                            var commandAttribute = method.GetCustomAttribute<CommandAttribute>();
                            if (commandAttribute != null)
                            {
                                var parameterTypes = method.GetParameters().Select(x => x.ParameterType);
                                commandsInModule.Add(new Command(commandAttribute.Names, commandAttribute.Summary, method, parameterTypes.ToList()));
                            }
                        }

                        CommandModules.Add(new CommandModule(moduleAttribute.Name ?? type.Name, type, commandsInModule));
                    }
                }
            }
        }

        /// <summary>
        /// Parses a message string and invokes the appropriate registered commands.
        /// If there's only one parameter and it is a string, we will simply
        /// give the target method the whole string after the command invocation.
        /// Otherwise, if any one parameter can't be parsed into its proper type, we abort parsing.
        /// </summary>
        /// <param name="message">The message to parse</param>
        /// <returns></returns>
        public async Task ParseMessage(string groupJid, string userJid, string message, long timestamp, XElement element)
        {
            //Resolve alias if we have to
            async Task HandleCommandAttributes(Command command, Func<Task> runIfNoResolveNeeded)
            {
                if (command.Method.GetCustomAttribute(typeof(ResolveAliasAttribute)) != null && userJid.IsAlias())
                {
                    await Client.ResolveAlias(userJid, groupJid, async (resolvedJid) =>
                    {
                        await ParseMessage(groupJid, resolvedJid, message, timestamp, element);
                    },
                    async () =>
                    {
                        await Client.Message(groupJid, "Failed to resolve your username. Do you have DMs enabled?");
                    });
                }
                else if (command.Method.GetCustomAttribute(typeof(RequireAdminAttribute)) != null && !Client.IsAdmin(groupJid, userJid))
                {
                    await Client.Message(groupJid, "That is an admin-only command.");
                }
                else if (command.Method.GetCustomAttribute(typeof(RequireAdminAttribute)) != null && groupJid.GetUsernameFromJid() != "GROUP")
                {
                    await Client.Message(groupJid, "That command can only be used in groups.");
                }
                else
                {
                    await runIfNoResolveNeeded();
                }
            }

            //If a method is async, return the Task. If not, invoke and return CompletedTask
            async Task InvokeMethodAsAsync(MethodInfo method, object instance, params object[] parameters)
            {
                try
                {
                    if (method.ReturnType == typeof(Task))
                    {
                        await (method.Invoke(instance, parameters) as Task);
                    }
                    else
                    {
                        await Task.FromResult(method.Invoke(instance, parameters));
                    }
                }
                catch (Exception e)
                {
                    Logger.Error(e);
                }
            }

            if (!string.IsNullOrEmpty(message))
            {
                var commands = message.Split(" ");
                var inputParameters = commands[1..];

                foreach (var module in CommandModules)
                {
                    //For every command that has a prefix+name that matches our command...
                    foreach (var command in module.Commands.Where(x => x.Names.Select(y => $"{CommandPrefix}{y}").Contains(commands[0])))
                    {
                        var parameters = new List<object>();
                        var expectedParameters = new List<Type>(command.Parameters);

                        //If the first parameter is an ExecutionContext, pass in the context with us
                        //Commented out since I implemented the ExecutionContext Property check. Didn't wanna lose this, though, just in case
                        /*if (expectedParameters[0] == typeof(ExecutionContext))
                        {
                            parameters.Add(context);
                            expectedParameters.Remove(typeof(ExecutionContext));
                        }*/

                        //If there's only one parameter and it is a string, we will simply
                        //give the target method the whole string after the command invocation
                        if (expectedParameters.Count == 1 && expectedParameters[0] == typeof(string) && inputParameters.Length > 0)
                        {
                            await HandleCommandAttributes(command, async () =>
                            {
                                var context = new ExecutionContext(CommandModules, Client, groupJid, userJid, timestamp, element);
                                var instantiatedModule = module.Type.CreateWithServices(Services, context);
                                await InvokeMethodAsAsync(command.Method, instantiatedModule, string.Join(" ", inputParameters));
                            });
                        }

                        //Otherwise we'll take each individual command as a parameter
                        else if (expectedParameters.Count == inputParameters.Length)
                        {
                            var incompatibleParameterType = false;

                            //We can automatically convert ints and longs for the target Methods
                            //If any one parameter can't be parsed into its proper type, we abort parsing
                            for (var i = 0; i < expectedParameters.Count && !incompatibleParameterType; i++)
                            {
                                if (expectedParameters[i] == typeof(int))
                                {
                                    var parseSuccess = int.TryParse(inputParameters[i], out var result);
                                    if (!parseSuccess) incompatibleParameterType = true;
                                    else
                                    {
                                        parameters.Add(result);
                                    }
                                }
                                else if (expectedParameters[i] == typeof(long))
                                {
                                    var parseSuccess = long.TryParse(inputParameters[i], out var result);
                                    if (!parseSuccess) incompatibleParameterType = true;
                                    else
                                    {
                                        parameters.Add(result);
                                    }
                                }
                                else if (expectedParameters[i] == typeof(bool))
                                {
                                    var parseSuccess = bool.TryParse(inputParameters[i], out var result);
                                    if (!parseSuccess) incompatibleParameterType = true;
                                    else
                                    {
                                        parameters.Add(result);
                                    }
                                }
                                else if (expectedParameters[i] == typeof(User))
                                {
                                    parameters.Add(Client.GetUserByUsername(inputParameters[i]));
                                }
                                else if (expectedParameters[i] == typeof(Group))
                                {
                                    parameters.Add(Client.GetGroupByTag(inputParameters[i]));
                                }
                                else parameters.Add(inputParameters[i]);
                            }

                            //If all the arguments parsed correctly, invoke the command
                            if (!incompatibleParameterType)
                            {
                                await HandleCommandAttributes(command, async () =>
                                {
                                    var context = new ExecutionContext(CommandModules, Client, groupJid, userJid, timestamp, element);
                                    var instantiatedModule = module.Type.CreateWithServices(Services, context);
                                    await InvokeMethodAsAsync(command.Method, instantiatedModule, parameters.ToArray());
                                });
                            }
                        }

                        //If we get to this point, we have a command invoked whose input parameters don't match
                        //this particular instance of a registered command by that name, so we continue looping
                        //to see if there are any other matches
                    }
                }
            }
        }
    }
}
