// Author: Dakota Methvin

using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using TheGodfather.Common;
using TheGodfather.Common.Attributes;


namespace TheGodfather.Modules.R6Siege
{
    [Group("siege"), Module(ModuleType.R6Siege), NotBlocked]
    [Description("View R6S player statistics.")]
    [Cooldown(1, 1, CooldownBucketType.Channel)] // NumUses, CooldownTime, CooldownType
    [UsageExamples("!r6stats")]


    public class R6SiegeStatsModule
    {

        [Command("temp")]
        [Description("This command has no functionality, it is merely a temporary prototype.")]
        [Aliases("t", "tmp")]
        [UsageExamples("!temp", 
                       "!r6stats temp @someone")]
        public async Task temp(CommandContext ctx)
        {

        }

    }
}
