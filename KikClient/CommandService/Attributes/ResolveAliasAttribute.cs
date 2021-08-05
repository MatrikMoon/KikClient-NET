using System;

/**
 * Created by Moon on 1/31/2021 - 12:13am
 * This attribute designates a command as a command that must be run
 * after resolving aliases.
 */

namespace KikClient.CommandService.Attributes
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public class ResolveAliasAttribute : Attribute
    {
    }
}
