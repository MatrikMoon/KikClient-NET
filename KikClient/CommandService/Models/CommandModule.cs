using System;
using System.Collections.Generic;

/**
 * Created by Moon on 1/30/2021
 * Model for a CommandModule. Contains basic info such as the module name
 */

namespace KikClient.CommandService.Models
{
    public class CommandModule
    {
        public string Name { get; private set; }
        public Type Type { get; private set; }
        public List<Command> Commands { get; private set; }

        public CommandModule(string name, Type type, List<Command> commands)
        {
            Name = name;
            Type = type;
            Commands = commands;
        }
    }
}
