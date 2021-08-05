using System;

/**
 * Created by Moon on 1/30/2021
 * Attribute which designates a class as a command module,
 * which is to say it contains commands
 */

namespace KikClient.CommandService.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class CommandModuleAttribute : Attribute
    {
        public string Name { get; set; }
    }
}
