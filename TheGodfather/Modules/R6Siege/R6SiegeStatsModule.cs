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
using global::TheGodfather.Exceptions;
using TheGodfather.Common.Attributes;
using TheGodfather.Services;


namespace TheGodfather.Modules.R6Siege
{

    using static R6SiegeAPI.API;
    using static R6SiegeAPI.Enums;
    using static R6SiegeAPI.Models;

    [Group("r6stats"), Module(ModuleType.R6Siege), NotBlocked]
    [Description("View R6S player statistics.")]
    [Aliases("r6", "6stats")]
    [Cooldown(1, 1, CooldownBucketType.Channel)] // NumUses, CooldownTime, CooldownType

    public class R6SiegeStatsModule : TheGodfatherModule
    {
        private R6SiegeAPI.API api;

        public R6SiegeStatsModule(SharedData shared, DBService db)
            : base (shared, db)
        {
            api = InitAPI("botaccess101@gmail.com", "EYAVv92CafRsP5W", null);
            this.ModuleColor = DiscordColor.HotPink;
        }

        [Command("all")]
        [Description("This command has no functionality, it is merely a temporary prototype.")]
        [Aliases("gen", "general")]
        [UsageExamples("!r6stats all playername",
                        "!r6s gen playername")]
        public async Task GeneralStats(CommandContext ctx, [Description("Player to get stats for.")] string playerName)
        {
            try
            {
                R6SiegeAPI.Models.Player player = await api.GetPlayer(playerName, R6SiegeAPI.Enums.Platform.UPLAY, R6SiegeAPI.Enums.UserSearchType.Name);
                R6SiegeAPI.Models.General genStats = await player.GetGeneral();

                await ctx.RespondAsync(embed: R6SiegeStatsModule.GeneralStatsToDiscordEmbed(genStats.Kills, genStats.Deaths, genStats.KillAssists, genStats.MatchWon, genStats.MatchLost, genStats.MatchPlayed, playerName, player.IconUrl, genStats.BarricadesDeployed, genStats.ReinforcementsDeployed, genStats.DistanceTravelled));
            }
            catch 
            {
                throw new CommandFailedException($"Player {Formatter.InlineCode(playerName)} does not exist accoring to Ubisoft.");
            }
        }

        [Command("ranked")]
        [Description("")]
        [Aliases("rnk", "comp")]
        [UsageExamples("!r6stats ranked playername seasonname",
                        "!r6stats rnk playername seasonname")]
        public async Task RankedStats(CommandContext ctx, [Description("Player to get stats for.")] string playerName, [Description("Season to get stats for")] string seasonName)
        {
            try
            {
                R6SiegeAPI.Models.Player player = await api.GetPlayer(playerName, R6SiegeAPI.Enums.Platform.UPLAY, R6SiegeAPI.Enums.UserSearchType.Name);
                int seasonID = GetSeasonID(seasonName);
                if (seasonID != -1)
                {
                    R6SiegeAPI.Models.Rank rank = await player.GetRank(R6SiegeAPI.Enums.RankedRegion.NA, seasonID);
                    await ctx.RespondAsync(embed: R6SiegeStatsModule.RankedStatsToDiscordEmbed(rank.MMR, rank.MaxMMR, rank.Wins, rank.Losses, rank.Abandons, rank.RankName, playerName, seasonName, rank.GetIconUrl));
                }
                throw new CommandFailedException($"{Formatter.InlineCode(seasonName)} is not a valid season or we don't currently support it. Try {Formatter.InlineCode("parabellum")}, {Formatter.InlineCode("chimera")}, {Formatter.InlineCode("whitenoise")}, or {Formatter.InlineCode("bloodorchid")}.");
            }
            catch(Exception e)
            {
                if (e.GetType() == typeof(CommandFailedException))
                {
                    throw e;
                }
                throw new CommandFailedException($"Player {Formatter.InlineCode(playerName)} does not exist accoring to Ubisoft.");
            }
        }

        public static DiscordEmbed GeneralStatsToDiscordEmbed(int kills, int deaths, int assists, int matchWon, int matchLost, int matchPlayed, string userName, string userImgURL, int barricades, int reinforcements, int distance)
        {
            var emb = new DiscordEmbedBuilder()
            {
                Title = $"{StaticDiscordEmoji.Trophy} " + userName + $"{StaticDiscordEmoji.Trophy}",
                Color = DiscordColor.HotPink,
                ThumbnailUrl = userImgURL
            };

            emb.AddField("Life Stats", $"Kills: {Formatter.InlineCode(kills.ToString())} Deaths: {Formatter.InlineCode(deaths.ToString())} Assists: {Formatter.InlineCode(assists.ToString())}");
            emb.AddField("Match Stats", $"Played: {Formatter.InlineCode(matchPlayed.ToString())} Wins: {Formatter.InlineCode(matchWon.ToString())} Losses: {Formatter.InlineCode(matchLost.ToString())}");
            emb.AddField("Other Stats", $"Barricades Deployed: {Formatter.InlineCode(barricades.ToString())} Reinforcements Deployed: {Formatter.InlineCode(reinforcements.ToString())} Distance Travelled: {Formatter.InlineCode(distance.ToString())}");

            return emb.Build();
        }

        public static DiscordEmbed RankedStatsToDiscordEmbed(float mmr, float maxMmr, int wins, int losses, int abandons, string rankName, string userName, string seasonName, string rankURL)
        {
            var emb = new DiscordEmbedBuilder()
            {
                Title = $"{StaticDiscordEmoji.Trophy} " + userName + $"{StaticDiscordEmoji.Trophy}",
                Description = "Rank: " + rankName,
                Color = DiscordColor.HotPink,
                ThumbnailUrl = rankURL
            };

            emb.AddField("MMR Stats", $"Current: {Formatter.InlineCode(mmr.ToString("N2"))} Max: {Formatter.InlineCode(maxMmr.ToString("N2"))}");
            emb.AddField("Game Stats", $"Wins: {Formatter.InlineCode(wins.ToString())} Losses: {Formatter.InlineCode(losses.ToString())} Abandons: {Formatter.InlineCode(abandons.ToString())}");
            return emb.Build();
        }

        private static int GetSeasonID(string seasonName)
        {
            switch (seasonName.ToLower())
            {
                case "parabellum":
                    return 10;
                case "chimera":
                    return 9;
                case "whitenoise":
                    return 8;
                case "bloodorchid":
                    return 7;
                default:
                    return -1;
            }
        }
    }
}
