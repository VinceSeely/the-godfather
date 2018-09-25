﻿#region USING_DIRECTIVES
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

using System.Collections.Generic;
using System.Threading.Tasks;

using TheGodfather.Common.Attributes;
using TheGodfather.Exceptions;
using TheGodfather.Extensions;
using TheGodfather.Modules.Owner.Extensions;
using TheGodfather.Services;
#endregion

namespace TheGodfather.Modules.Owner
{
    public partial class OwnerModule
    {
        [Group("statuses"), NotBlocked]
        [Description("Bot status manipulation. If invoked without command, either lists or adds status depending if argument is given.")]
        [Aliases("status", "botstatus", "activity", "activities")]
        [RequireOwner]
        public class StatusModule : TheGodfatherModule
        {

            public StatusModule(SharedData shared, DBService db) 
                : base(shared, db)
            {
                this.ModuleColor = DiscordColor.NotQuiteBlack;
            }


            [GroupCommand, Priority(1)]
            public Task ExecuteGroupAsync(CommandContext ctx)
                => this.ListAsync(ctx);

            [GroupCommand, Priority(0)]
            public Task ExecuteGroupAsync(CommandContext ctx,
                                         [Description("Activity type (Playing/Watching/Streaming/ListeningTo).")] ActivityType activity,
                                         [RemainingText, Description("Status.")] string status)
                => this.AddAsync(ctx, activity, status);


            #region COMMAND_STATUS_ADD
            [Command("add")]
            [Description("Add a status to running status queue.")]
            [Aliases("+", "a", "<", "<<", "+=")]
            [UsageExamples("!owner status add Playing CS:GO",
                           "!owner status add Streaming on Twitch")]
            public async Task AddAsync(CommandContext ctx,
                                      [Description("Activity type (Playing/Watching/Streaming/ListeningTo).")] ActivityType activity,
                                      [RemainingText, Description("Status.")] string status)
            {
                if (string.IsNullOrWhiteSpace(status))
                    throw new InvalidCommandUsageException("Missing status.");

                if (status.Length > 60)
                    throw new CommandFailedException("Status length cannot be greater than 60 characters.");

                await this.Database.AddBotStatusAsync(status, activity);
                await this.InformAsync(ctx, $"Added new status: {Formatter.InlineCode(status)}", important: false);
            }
            #endregion

            #region COMMAND_STATUS_DELETE
            [Command("delete")]
            [Description("Remove status from running queue.")]
            [Aliases("-", "remove", "rm", "del", ">", ">>", "-=")]
            [UsageExamples("!owner status delete 1")]
            public async Task DeleteAsync(CommandContext ctx,
                                         [Description("Status ID.")] int id)
            {
                await this.Database.RemoveBotStatusAsync(id);
                await this.InformAsync(ctx, $"Removed status with ID {Formatter.Bold(id.ToString())}", important: false);
            }
            #endregion

            #region COMMAND_STATUS_LIST
            [Command("list")]
            [Description("List all bot statuses.")]
            [Aliases("ls", "l", "print")]
            [UsageExamples("!owner status list")]
            public async Task ListAsync(CommandContext ctx)
            {
                IReadOnlyDictionary<int, string> statuses = await this.Database.GetAllBotStatusesAsync();
                await ctx.SendCollectionInPagesAsync(
                    "Statuses:",
                    statuses,
                    kvp => $"{Formatter.InlineCode($"{kvp.Key:D2}")}: {kvp.Value}",
                    this.ModuleColor,
                    10
                );
            }
            #endregion

            #region COMMAND_STATUS_SETROTATION
            [Command("setrotation")]
            [Description("Set automatic rotation of bot statuses.")]
            [Aliases("sr", "setr")]
            [UsageExamples("!owner status setrotation",
                           "!owner status setrotation false")]
            public Task SetRotationAsync(CommandContext ctx,
                                        [Description("Enabled?")] bool enable = true)
            {
                this.Shared.StatusRotationEnabled = enable;
                return this.InformAsync(ctx, $"Status rotation {(enable ? "enabled" : "disabled")}");
            }
            #endregion

            #region COMMAND_STATUS_SETSTATUS
            [Command("set"), Priority(1)]
            [Description("Set status to given string or status with given index in database. This sets rotation to false.")]
            [Aliases("s")]
            [UsageExamples("!owner status set Playing with fire",
                           "!owner status set 5")]
            public async Task SetAsync(CommandContext ctx,
                                      [Description("Activity type (Playing/Watching/Streaming/ListeningTo).")] ActivityType type,
                                      [RemainingText, Description("Status.")] string status)
            {
                if (string.IsNullOrWhiteSpace(status))
                    throw new InvalidCommandUsageException("Missing status.");

                if (status.Length > 60)
                    throw new CommandFailedException("Status length cannot be greater than 60 characters.");

                var activity = new DiscordActivity(status, type);

                this.Shared.StatusRotationEnabled = false;
                await ctx.Client.UpdateStatusAsync(activity);
                await this.InformAsync(ctx, $"Successfully switched current status to: {activity.ToString()}", important: false);
            }

            [Command("set"), Priority(0)]
            public async Task SetAsync(CommandContext ctx,
                                      [Description("Status ID.")] int id)
            {
                (ActivityType type, string status) = await this.Database.GetBotStatusByIdAsync(id);
                if (string.IsNullOrWhiteSpace(status))
                    throw new CommandFailedException("Status with given ID doesn't exist!");

                var activity = new DiscordActivity(status, type);

                this.Shared.StatusRotationEnabled = false;
                await ctx.Client.UpdateStatusAsync(activity);
                await this.InformAsync(ctx, $"Successfully switched current status to: {activity.ToString()}", important: false);
            }
            #endregion
        }
    }
}
