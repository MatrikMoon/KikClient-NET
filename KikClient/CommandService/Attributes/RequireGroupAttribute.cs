using System;

/**
 * Created by Moon on 2/10/2021 - 3:28am
 * This attribute designates a command as a command that must be run
 * after resolving aliases.
 */

namespace KikClient.CommandService.Attributes
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public class RequireGroupAttribute : Attribute
    {
    }
}
