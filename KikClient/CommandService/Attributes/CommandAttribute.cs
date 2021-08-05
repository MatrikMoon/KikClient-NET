using System;

/**
 * Created by Moon on 1/30/2021
 * Attribute which designates a method as a command parser
 */

namespace KikClient.CommandService.Attributes
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public class CommandAttribute : Attribute
    {
        public string[] Names { get; private set; }
        public string Summary { get; set; }

        public CommandAttribute(params string[] names) => Names = names;
    }
}
