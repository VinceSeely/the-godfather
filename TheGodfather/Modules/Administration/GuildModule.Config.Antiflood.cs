﻿#region USING_DIRECTIVES
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

using System;
using System.Text;
using System.Threading.Tasks;

using TheGodfather.Common;
using TheGodfather.Common.Attributes;
using TheGodfather.DiscordEntities;
using TheGodfather.Exceptions;
using TheGodfather.Modules.Administration.Common;
using TheGodfather.Modules.Administration.Extensions;
using TheGodfather.Modules.Administration.Services;
using TheGodfather.Services;
#endregion

namespace TheGodfather.Modules.Administration
{
    public partial class GuildModule
    {
        public partial class GuildConfigModule
        {
            [Group("antiflood")]
            [Description("Prevents guild raids (groups of users purposely flooding the guild). " +
                         "You can specify the action, sensitivity (number of users allowed to join before " +
                         "the action is performed) as well as the cooldown (timespan after which the user " +
                         "is removed from the watch. For example, an active watch with sensitivity 5 and " +
                         "cooldown of 10s will execute action if 5 or more users join the guild in period of " +
                         "10s. The action is applied to all of the users that are currently under watch.")]
            [Aliases("antiraid", "ar", "af")]
            [UsageExamples("!guild cfg antiflood",
                           "!guild cfg antiflood on",
                           "!guild cfg antiflood on kick 5s")]
            public class AntifloodModule : TheGodfatherServiceModule<AntifloodService>
            {

                public AntifloodModule(AntifloodService service, SharedData shared, DBService db)
                    : base(service, shared, db)
                {
                    this.ModuleColor = DiscordColor.HotPink;
                }


                [GroupCommand, Priority(5)]
                public async Task ExecuteGroupAsync(CommandContext ctx,
                                                   [Description("Enable?")] bool enable,
                                                   [Description("Sensitivity (number of users allowed to join within a given timespan).")] short sensitivity,
                                                   [Description("Action type.")] PunishmentActionType action = PunishmentActionType.Kick,
                                                   [Description("Cooldown.")] TimeSpan? cooldown = null)
                {
                    if (sensitivity < 2 || sensitivity > 20)
                        throw new CommandFailedException("The sensitivity is not in the valid range ([2, 20]).");

                    if (cooldown?.TotalSeconds < 5 || cooldown?.TotalSeconds > 60)
                        throw new CommandFailedException("The cooldown timespan is not in the valid range ([5, 60] seconds).");

                    var settings = new AntifloodSettings() {
                        Action = action,
                        Cooldown = (short?)cooldown?.TotalSeconds ?? 10,
                        Enabled = enable,
                        Sensitivity = sensitivity
                    };

                    await this.Database.SetAntifloodSettingsAsync(ctx.Guild.Id, settings);

                    DiscordChannel logchn = this.Shared.GetLogChannelForGuild((DiscordClientImpl)ctx.Client, ctx.Guild);
                    if (logchn != null) {
                        var emb = new DiscordEmbedBuilder() {
                            Title = "Guild config changed",
                            Description = $"Antiflood {(enable ? "enabled" : "disabled")}",
                            Color = this.ModuleColor
                        };
                        emb.AddField("User responsible", ctx.User.Mention, inline: true);
                        emb.AddField("Invoked in", ctx.Channel.Mention, inline: true);
                        if (enable) {
                            emb.AddField("Antiflood sensitivity", settings.Sensitivity.ToString(), inline: true);
                            emb.AddField("Antiflood cooldown", settings.Cooldown.ToString(), inline: true);
                            emb.AddField("Antiflood action", settings.Action.ToTypeString(), inline: true);
                        }
                        await logchn.SendMessageAsync(embed: emb.Build());
                    }

                    await this.InformAsync(ctx, $"{Formatter.Bold(enable ? "Enabled" : "Disabled")} antiflood actions.", important: false);
                }

                [GroupCommand, Priority(4)]
                public Task ExecuteGroupAsync(CommandContext ctx,
                                             [Description("Enable?")] bool enable,
                                             [Description("Action type.")] PunishmentActionType action,
                                             [Description("Sensitivity (number of users allowed to join within a given timespan).")] short sensitivity = 5,
                                             [Description("Cooldown.")] TimeSpan? cooldown = null)
                    => this.ExecuteGroupAsync(ctx, enable, sensitivity, action, cooldown);

                [GroupCommand, Priority(3)]
                public Task ExecuteGroupAsync(CommandContext ctx,
                                             [Description("Enable?")] bool enable,
                                             [Description("Action type.")] PunishmentActionType action,
                                             [Description("Cooldown.")] TimeSpan? cooldown = null,
                                             [Description("Sensitivity (number of users allowed to join within a given timespan).")] short sensitivity = 5)
                    => this.ExecuteGroupAsync(ctx, enable, sensitivity, action, cooldown);

                [GroupCommand, Priority(2)]
                public Task ExecuteGroupAsync(CommandContext ctx,
                                             [Description("Enable?")] bool enable,
                                             [Description("Cooldown.")] TimeSpan? cooldown,
                                             [Description("Action type.")] PunishmentActionType action = PunishmentActionType.Kick,
                                             [Description("Sensitivity (number of users allowed to join within a given timespan).")] short sensitivity = 5)
                    => this.ExecuteGroupAsync(ctx, enable, sensitivity, action, cooldown);

                [GroupCommand, Priority(1)]
                public Task ExecuteGroupAsync(CommandContext ctx,
                                             [Description("Enable?")] bool enable)
                    => this.ExecuteGroupAsync(ctx, enable, 5, PunishmentActionType.Kick, null);

                [GroupCommand, Priority(0)]
                public async Task ExecuteGroupAsync(CommandContext ctx)
                {
                    AntifloodSettings settings = await this.Database.GetAntifloodSettingsAsync(ctx.Guild.Id);
                    if (settings.Enabled) {
                        var sb = new StringBuilder();
                        sb.Append("Sensitivity: ").AppendLine(settings.Sensitivity.ToString());
                        sb.Append("Cooldown: ").AppendLine(settings.Cooldown.ToString());
                        sb.Append("Action: ").AppendLine(settings.Action.ToString());
                        await this.InformAsync(ctx, $"Antiflood watch for this guild is {Formatter.Bold("enabled")}\n{sb.ToString()}");
                    } else {
                        await this.InformAsync(ctx, $"Antiflood watch for this guild is {Formatter.Bold("disabled")}");
                    }
                }


                #region COMMAND_ANTIFLOOD_ACTION
                [Command("action")]
                [Description("Set the action to execute on the users when they flood/raid the guild.")]
                [Aliases("setaction", "a")]
                [UsageExamples("!guild cfg antiflood action mute",
                               "!guild cfg antiflood action temporaryban")]
                public async Task SetActionAsync(CommandContext ctx,
                                                [Description("Action type.")] PunishmentActionType action)
                {
                    AntifloodSettings settings = await this.Database.GetAntifloodSettingsAsync(ctx.Guild.Id);
                    settings.Action = action;
                    await this.Database.SetAntifloodSettingsAsync(ctx.Guild.Id, settings);

                    DiscordChannel logchn = this.Shared.GetLogChannelForGuild((DiscordClientImpl)ctx.Client, ctx.Guild);
                    if (logchn != null) {
                        var emb = new DiscordEmbedBuilder() {
                            Title = "Guild config changed",
                            Color = this.ModuleColor
                        };
                        emb.AddField("User responsible", ctx.User.Mention, inline: true);
                        emb.AddField("Invoked in", ctx.Channel.Mention, inline: true);
                        emb.AddField("Antiflood action changed to", action.ToTypeString());
                        await logchn.SendMessageAsync(embed: emb.Build());
                    }

                    await this.InformAsync(ctx, $"Antiflood action for this guild has been changed to {Formatter.Bold(settings.Action.ToTypeString())}", important: false);
                }
                #endregion

                #region COMMAND_ANTIFLOOD_SENSITIVITY
                [Command("sensitivity")]
                [Description("Set the antiflood sensitivity. Antiflood action will be executed if the specified " +
                             "amount of users join the guild in the given cooldown period.")]
                [Aliases("setsensitivity", "setsens", "sens", "s")]
                [UsageExamples("!guild cfg antiflood sensitivity 9")]
                public async Task SetSensitivityAsync(CommandContext ctx,
                                                     [Description("Sensitivity (number of users allowed to join within a given timespan).")] short sensitivity)
                {
                    if (sensitivity < 2 || sensitivity > 20)
                        throw new CommandFailedException("The sensitivity is not in the valid range ([2, 20]).");

                    AntifloodSettings settings = await this.Database.GetAntifloodSettingsAsync(ctx.Guild.Id);
                    settings.Sensitivity = sensitivity;
                    await this.Database.SetAntifloodSettingsAsync(ctx.Guild.Id, settings);

                    DiscordChannel logchn = this.Shared.GetLogChannelForGuild((DiscordClientImpl)ctx.Client, ctx.Guild);
                    if (logchn != null) {
                        var emb = new DiscordEmbedBuilder() {
                            Title = "Guild config changed",
                            Color = this.ModuleColor
                        };
                        emb.AddField("User responsible", ctx.User.Mention, inline: true);
                        emb.AddField("Invoked in", ctx.Channel.Mention, inline: true);
                        emb.AddField("Antiflood sensitivity changed to", $"Max {settings.Sensitivity} users per {settings.Cooldown}s");
                        await logchn.SendMessageAsync(embed: emb.Build());
                    }

                    await this.InformAsync(ctx, $"Antiflood sensitivity for this guild has been changed to {Formatter.Bold(sensitivity.ToString())} users per {settings.Cooldown}s", important: false);
                }
                #endregion

                #region COMMAND_ANTIFLOOD_COOLDOWN
                [Command("cooldown")]
                [Description("Set the antiflood sensitivity. Antiflood action will be executed if the specified " +
                             "amount of users join the guild in the given cooldown period.")]
                [Aliases("setcooldown", "setcool", "cool", "c")]
                [UsageExamples("!guild cfg antiflood sensitivity 9")]
                public async Task SetCooldownAsync(CommandContext ctx,
                                                  [Description("Cooldown.")] TimeSpan cooldown)
                {
                    if (cooldown.TotalSeconds < 5 || cooldown.TotalSeconds > 60)
                        throw new CommandFailedException("The cooldown timespan is not in the valid range ([5, 60] seconds).");

                    AntifloodSettings settings = await this.Database.GetAntifloodSettingsAsync(ctx.Guild.Id);
                    settings.Cooldown = (short)cooldown.TotalSeconds;
                    await this.Database.SetAntifloodSettingsAsync(ctx.Guild.Id, settings);

                    DiscordChannel logchn = this.Shared.GetLogChannelForGuild((DiscordClientImpl)ctx.Client, ctx.Guild);
                    if (logchn != null) {
                        var emb = new DiscordEmbedBuilder() {
                            Title = "Guild config changed",
                            Color = this.ModuleColor
                        };
                        emb.AddField("User responsible", ctx.User.Mention, inline: true);
                        emb.AddField("Invoked in", ctx.Channel.Mention, inline: true);
                        emb.AddField("Antiflood cooldown changed to", $"{settings.Cooldown}s");
                        await logchn.SendMessageAsync(embed: emb.Build());
                    }

                    await this.InformAsync(ctx, $"Antiflood cooldown for this guild has been changed to {settings.Cooldown}s", important: false);
                }
                #endregion
            }
        }
    }
}
