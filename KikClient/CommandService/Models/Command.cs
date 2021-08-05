using System;
using System.Collections.Generic;
using System.Reflection;

/**
 * Created by Moon on 1/30/2021
 * Model for a Command. Should contain all the necessary components
 * to identify and invoke one
 */

namespace KikClient.CommandService.Models
{
    public class Command
    {
        public string[] Names { get; private set; }
        public string Summary { get; private set; }
        public MethodInfo Method { get; private set; }
        public List<Type> Parameters { get; private set; }

        public Command(string[] names, string summary, MethodInfo method, List<Type> parameters)
        {
            Names = names;
            Summary = summary;
            Method = method;
            Parameters = parameters;
        }
    }
}
