// Author: Dakota Methvin

using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using TheGodfather.Common;

using TheGodfather.Common.Attributes;
using TheGodfather.Services;


namespace TheGodfather.Modules.R6Siege
{
    using static R6SiegeAPI.API;
    using static R6SiegeAPI.Enums;
    using static R6SiegeAPI.Models;

    [Group("siege"), Module(ModuleType.R6Siege), NotBlocked]
    [Description("View R6S player statistics.")]
    [Cooldown(1, 1, CooldownBucketType.Channel)] // NumUses, CooldownTime, CooldownType

    public class R6SiegeStatsModule : TheGodfatherModule
    {

        public R6SiegeStatsModule(SharedData shared, DBService db)
            : base (shared, db)
        {
            this.ModuleColor = DiscordColor.HotPink;
        }


        [Command("r6stats")]
        [Description("This command has no functionality, it is merely a temporary prototype.")]
        [Aliases("r6", "6stats")]
        [UsageExamples("!r6stats playername")]
        public async Task temp(CommandContext ctx)
        {
            
            
        }

    }
}
