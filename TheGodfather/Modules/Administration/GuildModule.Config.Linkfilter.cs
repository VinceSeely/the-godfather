﻿#region USING_DIRECTIVES
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

using System.Threading.Tasks;

using TheGodfather.Common;
using TheGodfather.Common.Attributes;
using TheGodfather.DiscordEntities;
using TheGodfather.Modules.Administration.Common;
using TheGodfather.Services;
#endregion

namespace TheGodfather.Modules.Administration
{
    public partial class GuildModule
    {
        public partial class GuildConfigModule
        {
            [Group("linkfilter")]
            [Description("Linkfilter configuration. Group call prints current configuration, or enables/disables linkfilter if specified.")]
            [Aliases("lf", "linkf", "linkremove", "filterlinks")]
            [UsageExamples("!guild cfg linkfilter",
                           "!guild cfg linkfilter on")]
            public class LinkfilterModule : TheGodfatherModule
            {

                public LinkfilterModule(SharedData shared, DBService db) 
                    : base(shared, db)
                {
                    this.ModuleColor = DiscordColor.DarkRed;
                }


                [GroupCommand, Priority(1)]
                public async Task ExecuteGroupAsync(CommandContext ctx,
                                                   [Description("Enable?")] bool enable)
                {
                    CachedGuildConfig gcfg = this.Shared.GetGuildConfig(ctx.Guild.Id);
                    gcfg.LinkfilterSettings.Enabled = enable;

                    await this.Database.UpdateGuildSettingsAsync(ctx.Guild.Id, gcfg);
                    await this.InformAsync(ctx, $"{(enable ? "Enabled" : "Disabled")} link filtering!", important: false);
                    await this.LogConfigChangeAsync(ctx, "Linkfilter", gcfg.LinkfilterSettings.Enabled);
                }

                [GroupCommand, Priority(0)]
                public Task ExecuteGroupAsync(CommandContext ctx)
                {
                    LinkfilterSettings settings = this.Shared.GetGuildConfig(ctx.Guild.Id).LinkfilterSettings;
                    if (settings.Enabled) {
                        var emb = new DiscordEmbedBuilder() {
                            Title = "Linkfilter modules for this guild",
                            Color = this.ModuleColor
                        };
                        emb.AddField("Discord invites filter", settings.BlockDiscordInvites ? "enabled" : "disabled", inline: true);
                        emb.AddField("DDoS/Booter websites filter", settings.BlockBooterWebsites ? "enabled" : "disabled", inline: true);
                        emb.AddField("Disturbing websites filter", settings.BlockDisturbingWebsites ? "enabled" : "disabled", inline: true);
                        emb.AddField("IP logging websites filter", settings.BlockIpLoggingWebsites ? "enabled" : "disabled", inline: true);
                        emb.AddField("URL shortening websites filter", settings.BlockDisturbingWebsites ? "enabled" : "disabled", inline: true);

                        return ctx.RespondAsync(embed: emb.Build());
                    } else {
                        return this.InformAsync(ctx, $"Link filtering for this guild is: {Formatter.Bold("disabled")}!");
                    }
                }


                #region COMMAND_LINKFILTER_BOOTERS
                [Command("booters"), Priority(1)]
                [Description("Enable or disable DDoS/Booter website filtering.")]
                [Aliases("ddos", "boot", "dos")]
                [UsageExamples("!guild cfg linkfilter booters",
                               "!guild cfg linkfilter booters on")]
                public async Task BootersAsync(CommandContext ctx,
                                              [Description("Enable?")] bool enable)
                {
                    CachedGuildConfig gcfg = this.Shared.GetGuildConfig(ctx.Guild.Id);
                    gcfg.LinkfilterSettings.BlockBooterWebsites = enable;

                    await this.Database.UpdateGuildSettingsAsync(ctx.Guild.Id, gcfg);
                    await this.InformAsync(ctx, $"{(enable ? "Enabled" : "Disabled")} DDoS/Booter website filtering!", important: false);
                    await this.LogConfigChangeAsync(ctx, "DDoS/Booter websites filtering", gcfg.LinkfilterSettings.BlockBooterWebsites);
                }

                [Command("booters"), Priority(0)]
                public Task BootersAsync(CommandContext ctx)
                {
                    CachedGuildConfig gcfg = this.Shared.GetGuildConfig(ctx.Guild.Id);
                    return this.InformAsync(ctx, $"DDoS/Booter website filtering for this guild is: {Formatter.Bold(gcfg.LinkfilterSettings.BlockBooterWebsites ? "enabled" : "disabled")}!");
                }
                #endregion

                #region COMMAND_LINKFILTER_INVITES
                [Command("invites"), Priority(1)]
                [Description("Enable or disable Discord invite filters.")]
                [Aliases("invite", "inv", "i")]
                [UsageExamples("!guild cfg linkfilter invites",
                               "!guild cfg linkfilter invites on")]
                public async Task InvitesAsync(CommandContext ctx,
                                              [Description("Enable?")] bool enable)
                {
                    CachedGuildConfig gcfg = this.Shared.GetGuildConfig(ctx.Guild.Id);
                    gcfg.LinkfilterSettings.BlockDiscordInvites = enable;

                    await this.Database.UpdateGuildSettingsAsync(ctx.Guild.Id, gcfg);
                    await this.InformAsync(ctx, $"{(enable ? "Enabled" : "Disabled")} Discord invites filtering!", important: false);
                    await this.LogConfigChangeAsync(ctx, "Discord invites filtering", gcfg.LinkfilterSettings.BlockDiscordInvites);
                }

                [Command("invites"), Priority(0)]
                public Task InvitesAsync(CommandContext ctx)
                {
                    CachedGuildConfig gcfg = this.Shared.GetGuildConfig(ctx.Guild.Id);
                    return this.InformAsync(ctx, $"Invite link filtering for this guild is: {Formatter.Bold(gcfg.LinkfilterSettings.BlockDiscordInvites ? "enabled" : "disabled")}!");
                }
                #endregion

                #region COMMAND_LINKFILTER_SHOCKSITES
                [Command("disturbingsites"), Priority(1)]
                [Description("Enable or disable shock website filtering.")]
                [Aliases("disturbing", "shock", "shocksites")]
                [UsageExamples("!guild cfg linkfilter disturbing",
                               "!guild cfg linkfilter disturbing on")]
                public async Task DisturbingSitesAsync(CommandContext ctx,
                                                      [Description("Enable?")] bool enable)
                {
                    CachedGuildConfig gcfg = this.Shared.GetGuildConfig(ctx.Guild.Id);
                    gcfg.LinkfilterSettings.BlockDisturbingWebsites = enable;

                    await this.Database.UpdateGuildSettingsAsync(ctx.Guild.Id, gcfg);
                    await this.InformAsync(ctx, $"{(enable ? "Enabled" : "Disabled")} disturbing website filtering!", important: false);
                    await this.LogConfigChangeAsync(ctx, "Disturbing websites filtering", gcfg.LinkfilterSettings.BlockDisturbingWebsites);
                }

                [Command("disturbingsites"), Priority(0)]
                public Task DisturbingSitesAsync(CommandContext ctx)
                {
                    CachedGuildConfig gcfg = this.Shared.GetGuildConfig(ctx.Guild.Id);
                    return this.InformAsync(ctx, $"Shock website filtering for this guild is: {Formatter.Bold(gcfg.LinkfilterSettings.BlockDisturbingWebsites ? "enabled" : "disabled")}!");
                }
                #endregion

                #region COMMAND_LINKFILTER_IPLOGGERS
                [Command("iploggers"), Priority(1)]
                [Description("Enable or disable filtering of IP logger websites.")]
                [Aliases("ip", "loggers", "ipleech")]
                [UsageExamples("!guild cfg linkfilter iploggers",
                               "!guild cfg linkfilter iploggers on")]
                public async Task IpLoggersAsync(CommandContext ctx,
                                                [Description("Enable?")] bool enable)
                {
                    CachedGuildConfig gcfg = this.Shared.GetGuildConfig(ctx.Guild.Id);
                    gcfg.LinkfilterSettings.BlockIpLoggingWebsites = enable;

                    await this.Database.UpdateGuildSettingsAsync(ctx.Guild.Id, gcfg);
                    await this.InformAsync(ctx, $"{(enable ? "Enabled" : "Disabled")} IP logging website filtering!", important: false);
                    await this.LogConfigChangeAsync(ctx, "IP logging websites filtering", gcfg.LinkfilterSettings.BlockIpLoggingWebsites);
                }

                [Command("iploggers"), Priority(0)]
                public Task IpLoggersAsync(CommandContext ctx)
                {
                    CachedGuildConfig gcfg = this.Shared.GetGuildConfig(ctx.Guild.Id);
                    return this.InformAsync(ctx, $"IP logging websites filtering for this guild is: {Formatter.Bold(gcfg.LinkfilterSettings.BlockIpLoggingWebsites ? "enabled" : "disabled")}!");
                }
                #endregion

                #region COMMAND_LINKFILTER_SHORTENERS
                [Command("shorteners"), Priority(1)]
                [Description("Enable or disable filtering of URL shortener websites.")]
                [Aliases("urlshort", "shortenurl", "urlshorteners")]
                [UsageExamples("!guild cfg linkfilter shorteners",
                               "!guild cfg linkfilter shorteners on")]
                public async Task ShortenersAsync(CommandContext ctx,
                                                 [Description("Enable?")] bool enable)
                {
                    CachedGuildConfig gcfg = this.Shared.GetGuildConfig(ctx.Guild.Id);
                    gcfg.LinkfilterSettings.BlockUrlShorteners = enable;

                    await this.Database.UpdateGuildSettingsAsync(ctx.Guild.Id, gcfg);
                    await this.InformAsync(ctx, $"{(enable ? "Enabled" : "Disabled")} URL shortener website filtering!", important: false);
                    await this.LogConfigChangeAsync(ctx, "URL shorteners filtering", gcfg.LinkfilterSettings.BlockUrlShorteners);
                }

                [Command("shorteners"), Priority(0)]
                public Task ShortenersAsync(CommandContext ctx)
                {
                    CachedGuildConfig gcfg = this.Shared.GetGuildConfig(ctx.Guild.Id);
                    return this.InformAsync(ctx, $"URL shortening websites filtering for this guild is: {Formatter.Bold(gcfg.LinkfilterSettings.BlockUrlShorteners ? "enabled" : "disabled")}!");
                }
                #endregion
                

                #region HELPER_FUNCTIONS
                protected Task LogConfigChangeAsync(CommandContext ctx, string module, bool value)
                {
                    DiscordChannel logchn = this.Shared.GetLogChannelForGuild((DiscordClientImpl)ctx.Client, ctx.Guild);
                    if (logchn != null) {
                        var emb = new DiscordEmbedBuilder() {
                            Title = "Guild config changed",
                            Color = this.ModuleColor
                        };
                        emb.AddField("User responsible", ctx.User.Mention, inline: true);
                        emb.AddField("Invoked in", ctx.Channel.Mention, inline: true);
                        emb.AddField(module, value ? "on" : "off", inline: true);
                        return logchn.SendMessageAsync(embed: emb.Build());
                    } else {
                        return Task.CompletedTask;
                    }
                }
                #endregion
            }
        }
    }
}
