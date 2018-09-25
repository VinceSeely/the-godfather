﻿#region USING_DIRECTIVES
using DSharpPlus;
using DSharpPlus.Entities;

using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using TheGodfather.Common;
using TheGodfather.Common.Collections;
using TheGodfather.Extensions;
using TheGodfather.Modules.Administration.Common;
using TheGodfather.Modules.Misc.Extensions;
using TheGodfather.Modules.Reactions.Common;
using TheGodfather.Services;
#endregion

namespace TheGodfather
{
    public sealed class SharedData : IDisposable
    {
        public AsyncExecutor AsyncExecutor { get; }
        public ConcurrentHashSet<ulong> BlockedChannels { get; internal set; }
        public ConcurrentHashSet<ulong> BlockedUsers { get; internal set; }
        public BotConfig BotConfiguration { get; internal set; }
        public ConcurrentDictionary<ulong, ConcurrentHashSet<EmojiReaction>> EmojiReactions { get; internal set; }
        public ConcurrentDictionary<ulong, ConcurrentHashSet<Filter>> Filters { get; internal set; }
        public ConcurrentDictionary<ulong, CachedGuildConfig> GuildConfigurations { get; internal set; }
        public Logger LogProvider { get; internal set; }
        public bool ListeningStatus { get; internal set; }
        public CancellationTokenSource MainLoopCts { get; internal set; }
        public ConcurrentDictionary<ulong, ulong> MessageCount { get; internal set; }
        public bool StatusRotationEnabled { get; internal set; }
        public ConcurrentDictionary<ulong, ConcurrentHashSet<SavedTaskExecutor>> RemindExecuters { get; internal set; }
        public ConcurrentDictionary<int, SavedTaskExecutor> TaskExecuters { get; internal set; }
        public ConcurrentDictionary<ulong, ConcurrentHashSet<TextReaction>> TextReactions { get; internal set; }

        private ConcurrentDictionary<ulong, ChannelEvent> ChannelEvents { get; }
        private ConcurrentDictionary<ulong, ConcurrentHashSet<ulong>> PendingResponses { get; }


        public SharedData()
        {
            this.AsyncExecutor = new AsyncExecutor();
            this.BlockedChannels = new ConcurrentHashSet<ulong>();
            this.BlockedUsers = new ConcurrentHashSet<ulong>();
            this.BotConfiguration = BotConfig.Default;
            this.ChannelEvents = new ConcurrentDictionary<ulong, ChannelEvent>();
            this.EmojiReactions = new ConcurrentDictionary<ulong, ConcurrentHashSet<EmojiReaction>>();
            this.Filters = new ConcurrentDictionary<ulong, ConcurrentHashSet<Filter>>();
            this.GuildConfigurations = new ConcurrentDictionary<ulong, CachedGuildConfig>();
            this.ListeningStatus = true;
            this.MainLoopCts = new CancellationTokenSource();
            this.MessageCount = new ConcurrentDictionary<ulong, ulong>();
            this.PendingResponses = new ConcurrentDictionary<ulong, ConcurrentHashSet<ulong>>();
            this.RemindExecuters = new ConcurrentDictionary<ulong, ConcurrentHashSet<SavedTaskExecutor>>();
            this.StatusRotationEnabled = true;
            this.TaskExecuters = new ConcurrentDictionary<int, SavedTaskExecutor>();
            this.TextReactions = new ConcurrentDictionary<ulong, ConcurrentHashSet<TextReaction>>();
        }


        public void Dispose()
        {
            this.MainLoopCts.Dispose();
            foreach ((int tid, SavedTaskExecutor texec) in this.TaskExecuters)
                texec.Dispose();
        }

        public async Task SyncDataWithDatabaseAsync(DBService db)
        {
            foreach ((ulong uid, ulong count) in this.MessageCount)
                await db.ModifyXpAsync(uid, count);
        }


        #region CHANNEL_EVENT_HELPERS
        public ChannelEvent GetEventInChannel(ulong cid)
            => this.ChannelEvents.ContainsKey(cid) ? this.ChannelEvents[cid] : null;

        public bool IsEventRunningInChannel(ulong cid)
            => this.ChannelEvents.ContainsKey(cid) && this.ChannelEvents[cid] != null;

        public void RegisterEventInChannel(ChannelEvent cevent, ulong cid)
            => this.ChannelEvents.AddOrUpdate(cid, cevent, (c, e) => cevent);

        public void UnregisterEventInChannel(ulong cid)
        {
            if (!this.ChannelEvents.ContainsKey(cid))
                return;
            if (!this.ChannelEvents.TryRemove(cid, out _))
                this.ChannelEvents[cid] = null;
        }
        #endregion

        #region RANK_HELPERS
        public ushort CalculateRankForMessageCount(ulong msgcount)
            => (ushort)Math.Floor(Math.Sqrt(msgcount / 10));

        public ushort CalculateRankForUser(ulong uid)
            => this.MessageCount.ContainsKey(uid) ? this.CalculateRankForMessageCount(this.MessageCount[uid]) : (ushort)0;

        public uint CalculateXpNeededForRank(ushort index)
            => (uint)(index * index * 10);

        public ulong GetMessageCountForUser(ulong uid)
            => this.MessageCount.ContainsKey(uid) ? this.MessageCount[uid] : 0;

        public ushort IncrementMessageCountForUser(ulong uid)
        {
            if (this.MessageCount.ContainsKey(uid)) {
                this.MessageCount[uid]++;
            } else if (!this.MessageCount.TryAdd(uid, 1)) {
                return 0;
            }

            ushort prev = this.CalculateRankForMessageCount(this.MessageCount[uid] - 1);
            ushort curr = this.CalculateRankForMessageCount(this.MessageCount[uid]);

            return curr != prev ? curr : (ushort)0;
        }
        #endregion

        #region GUILD_DATA_HELPERS
        public CachedGuildConfig GetGuildConfig(ulong gid)
            => this.GuildConfigurations.GetOrAdd(gid, CachedGuildConfig.Default);

        public string GetGuildPrefix(ulong gid)
        {
            if (this.GuildConfigurations.ContainsKey(gid) && !string.IsNullOrWhiteSpace(this.GuildConfigurations[gid].Prefix))
                return this.GuildConfigurations[gid].Prefix;
            else
                return this.BotConfiguration.DefaultPrefix;
        }

        public DiscordChannel GetLogChannelForGuild(DiscordClient client, DiscordGuild guild)
        {
            var gcfg = this.GetGuildConfig(guild.Id);
            return gcfg.LoggingEnabled ? guild.GetChannel(gcfg.LogChannelId) : null;
        }

        public bool GuildHasTextReaction(ulong gid, string trigger)
            => this.TextReactions.ContainsKey(gid) && this.TextReactions[gid] != null && this.TextReactions[gid].Any(tr => tr.ContainsTriggerPattern(trigger));

        public bool GuildHasEmojiReaction(ulong gid, string trigger)
            => this.EmojiReactions.ContainsKey(gid) && this.EmojiReactions[gid] != null && this.EmojiReactions[gid].Any(er => er.ContainsTriggerPattern(trigger));

        public bool MessageContainsFilter(ulong gid, string message)
        {
            if (!this.Filters.ContainsKey(gid) || this.Filters[gid] == null)
                return false;

            message = message.ToLowerInvariant();
            return this.Filters[gid].Any(f => f.Trigger.IsMatch(message));
        }
        #endregion

        #region PENDING_RESPONSES_HELPERS
        public void AddPendingResponse(ulong cid, ulong uid)
        {
            this.PendingResponses.AddOrUpdate(
                cid,
                new ConcurrentHashSet<ulong> { uid },
                (k, v) => { v.Add(uid); return v; }
            );
        }

        public bool PendingResponseExists(ulong cid, ulong uid)
            => this.PendingResponses.ContainsKey(cid) && this.PendingResponses[cid].Contains(uid);

        public bool TryRemovePendingResponse(ulong cid, ulong uid)
        {
            bool success = this.PendingResponses[cid].TryRemove(uid);
            if (!this.PendingResponses[cid].Any())
                this.PendingResponses.TryRemove(cid, out _);
            return success;
        }
        #endregion
    }
}
