#region USING_DIRECTIVES
using DSharpPlus.CommandsNext;

using System;
using System.Linq;
#endregion

namespace TheGodfather.Common.Attributes
{
    internal enum ModuleType
    {
        Administration,
        Chickens,
        Currency,
        Games,
        Miscellaneous,
        Music,
        Owner,
        Polls,
        Reactions,
        R6Siege, // Added by Dakota Methvin for use in TheGodfather.Modules.R6Siege
        Searches,
        SWAT,
        Uncategorized
    }


    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    internal sealed class ModuleAttribute : Attribute
    {
        public static ModuleAttribute ForCommand(Command cmd)
        {
            var mattr = cmd.CustomAttributes.FirstOrDefault(attr => attr is ModuleAttribute) as ModuleAttribute;
            return mattr ?? (cmd.Parent != null ? ForCommand(cmd.Parent) : new ModuleAttribute(ModuleType.Uncategorized));
        }


        public ModuleType Module { get; private set; }


        public ModuleAttribute(ModuleType module)
        {
            this.Module = module;
        }
    }
}
