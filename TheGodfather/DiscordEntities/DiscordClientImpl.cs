
using System.Collections.Generic;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity;
using DSharpPlus.VoiceNext;

namespace TheGodfather.DiscordEntities
{
    public interface IDiscordClient
    {
        DebugLogger DebugLogger { get; }
        IReadOnlyDictionary<ulong, DiscordGuild> Guilds { get; }
        DiscordClient Client { get; }

        event AsyncEventHandler<ClientErrorEventArgs> ClientErrored;
        event AsyncEventHandler<SocketErrorEventArgs> SocketErrored;
        event AsyncEventHandler SocketOpened;
        event AsyncEventHandler<SocketCloseEventArgs> SocketClosed;
        event AsyncEventHandler<ReadyEventArgs> Resumed;
        event AsyncEventHandler<ChannelCreateEventArgs> ChannelCreated;
        event AsyncEventHandler<DmChannelCreateEventArgs> DmChannelCreated;
        event AsyncEventHandler<ChannelUpdateEventArgs> ChannelUpdated;
        event AsyncEventHandler<ChannelDeleteEventArgs> ChannelDeleted;
        event AsyncEventHandler<DmChannelDeleteEventArgs> DmChannelDeleted;
        event AsyncEventHandler<ChannelPinsUpdateEventArgs> ChannelPinsUpdated;
        event AsyncEventHandler<GuildCreateEventArgs> GuildCreated;
        event AsyncEventHandler<GuildCreateEventArgs> GuildAvailable;
        event AsyncEventHandler<GuildUpdateEventArgs> GuildUpdated;
        event AsyncEventHandler<GuildDeleteEventArgs> GuildDeleted;
        event AsyncEventHandler<GuildDeleteEventArgs> GuildUnavailable;
        event AsyncEventHandler<MessageCreateEventArgs> MessageCreated;
        event AsyncEventHandler<PresenceUpdateEventArgs> PresenceUpdated;
        event AsyncEventHandler<GuildBanAddEventArgs> GuildBanAdded;
        event AsyncEventHandler<GuildBanRemoveEventArgs> GuildBanRemoved;
        event AsyncEventHandler<GuildEmojisUpdateEventArgs> GuildEmojisUpdated;
        event AsyncEventHandler<GuildIntegrationsUpdateEventArgs> GuildIntegrationsUpdated;
        event AsyncEventHandler<ReadyEventArgs> Ready;
        event AsyncEventHandler<GuildMemberAddEventArgs> GuildMemberAdded;
        event AsyncEventHandler<GuildMemberRemoveEventArgs> GuildMemberRemoved;
        event AsyncEventHandler<GuildMemberUpdateEventArgs> GuildMemberUpdated;
        event AsyncEventHandler<GuildRoleCreateEventArgs> GuildRoleCreated;
        event AsyncEventHandler<GuildRoleUpdateEventArgs> GuildRoleUpdated;
        event AsyncEventHandler<GuildRoleDeleteEventArgs> GuildRoleDeleted;
        event AsyncEventHandler<MessageAcknowledgeEventArgs> MessageAcknowledged;
        event AsyncEventHandler<MessageUpdateEventArgs> MessageUpdated;
        event AsyncEventHandler<MessageDeleteEventArgs> MessageDeleted;
        event AsyncEventHandler<MessageBulkDeleteEventArgs> MessagesBulkDeleted;
        event AsyncEventHandler<TypingStartEventArgs> TypingStarted;
        event AsyncEventHandler<UserSettingsUpdateEventArgs> UserSettingsUpdated;
        event AsyncEventHandler<UserUpdateEventArgs> UserUpdated;
        event AsyncEventHandler<VoiceStateUpdateEventArgs> VoiceStateUpdated;
        event AsyncEventHandler<VoiceServerUpdateEventArgs> VoiceServerUpdated;
        event AsyncEventHandler<GuildMembersChunkEventArgs> GuildMembersChunked;
        event AsyncEventHandler<UnknownEventArgs> UnknownEvent;
        event AsyncEventHandler<MessageReactionAddEventArgs> MessageReactionAdded;
        event AsyncEventHandler<MessageReactionRemoveEventArgs> MessageReactionRemoved;
        event AsyncEventHandler<MessageReactionsClearEventArgs> MessageReactionsCleared;
        event AsyncEventHandler<WebhooksUpdateEventArgs> WebhooksUpdated;
        event AsyncEventHandler<HeartbeatEventArgs> Heartbeated;

        Task ConnectAsync();
        Task DisconnectAsync();
        void Dispose();
        CommandsNextExtension UseCommandsNext(CommandsNextConfiguration commandsNextConfiguration);
        InteractivityExtension UseInteractivity(InteractivityConfiguration interactivityConfiguration);
        VoiceNextExtension UseVoiceNext();
        Task<DiscordChannel> GetChannelAsync(ulong smtiChannelId);
        Task<DiscordUser> GetUserAsync(ulong smtiInitiatorId);
        Task<DiscordChannel> CreateDmChannelAsync(ulong infoInitiatorId);
        Task<DiscordGuild> GetGuildAsync(ulong infoGuildId);
    }

    public class DiscordClientImpl : IDiscordClient
    {
        private readonly DiscordClient _Client;
        public DebugLogger DebugLogger => _Client.DebugLogger;
        public IReadOnlyDictionary<ulong, DiscordGuild> Guilds => _Client.Guilds;
        public DiscordClient Client => _Client;

        public event AsyncEventHandler<ClientErrorEventArgs> ClientErrored;
        public event AsyncEventHandler<SocketErrorEventArgs> SocketErrored;
        public event AsyncEventHandler SocketOpened;
        public event AsyncEventHandler<SocketCloseEventArgs> SocketClosed;
        public event AsyncEventHandler<ReadyEventArgs> Resumed;
        public event AsyncEventHandler<ChannelCreateEventArgs> ChannelCreated;
        public event AsyncEventHandler<DmChannelCreateEventArgs> DmChannelCreated;
        public event AsyncEventHandler<ChannelUpdateEventArgs> ChannelUpdated;
        public event AsyncEventHandler<ChannelDeleteEventArgs> ChannelDeleted;
        public event AsyncEventHandler<DmChannelDeleteEventArgs> DmChannelDeleted;
        public event AsyncEventHandler<ChannelPinsUpdateEventArgs> ChannelPinsUpdated;
        public event AsyncEventHandler<GuildCreateEventArgs> GuildCreated;
        public event AsyncEventHandler<GuildCreateEventArgs> GuildAvailable;
        public event AsyncEventHandler<GuildUpdateEventArgs> GuildUpdated;
        public event AsyncEventHandler<GuildDeleteEventArgs> GuildDeleted;
        public event AsyncEventHandler<GuildDeleteEventArgs> GuildUnavailable;
        public event AsyncEventHandler<MessageCreateEventArgs> MessageCreated;
        public event AsyncEventHandler<PresenceUpdateEventArgs> PresenceUpdated;
        public event AsyncEventHandler<GuildBanAddEventArgs> GuildBanAdded;
        public event AsyncEventHandler<GuildBanRemoveEventArgs> GuildBanRemoved;
        public event AsyncEventHandler<GuildEmojisUpdateEventArgs> GuildEmojisUpdated;
        public event AsyncEventHandler<GuildIntegrationsUpdateEventArgs> GuildIntegrationsUpdated;
        public event AsyncEventHandler<ReadyEventArgs> Ready;
        public event AsyncEventHandler<GuildMemberAddEventArgs> GuildMemberAdded;
        public event AsyncEventHandler<GuildMemberRemoveEventArgs> GuildMemberRemoved;
        public event AsyncEventHandler<GuildMemberUpdateEventArgs> GuildMemberUpdated;
        public event AsyncEventHandler<GuildRoleCreateEventArgs> GuildRoleCreated;
        public event AsyncEventHandler<GuildRoleUpdateEventArgs> GuildRoleUpdated;
        public event AsyncEventHandler<GuildRoleDeleteEventArgs> GuildRoleDeleted;
        public event AsyncEventHandler<MessageAcknowledgeEventArgs> MessageAcknowledged;
        public event AsyncEventHandler<MessageUpdateEventArgs> MessageUpdated;
        public event AsyncEventHandler<MessageDeleteEventArgs> MessageDeleted;
        public event AsyncEventHandler<MessageBulkDeleteEventArgs> MessagesBulkDeleted;
        public event AsyncEventHandler<TypingStartEventArgs> TypingStarted;
        public event AsyncEventHandler<UserSettingsUpdateEventArgs> UserSettingsUpdated;
        public event AsyncEventHandler<UserUpdateEventArgs> UserUpdated;
        public event AsyncEventHandler<VoiceStateUpdateEventArgs> VoiceStateUpdated;
        public event AsyncEventHandler<VoiceServerUpdateEventArgs> VoiceServerUpdated;
        public event AsyncEventHandler<GuildMembersChunkEventArgs> GuildMembersChunked;
        public event AsyncEventHandler<UnknownEventArgs> UnknownEvent;
        public event AsyncEventHandler<MessageReactionAddEventArgs> MessageReactionAdded;
        public event AsyncEventHandler<MessageReactionRemoveEventArgs> MessageReactionRemoved;
        public event AsyncEventHandler<MessageReactionsClearEventArgs> MessageReactionsCleared;
        public event AsyncEventHandler<WebhooksUpdateEventArgs> WebhooksUpdated;
        public event AsyncEventHandler<HeartbeatEventArgs> Heartbeated;

        public DiscordClientImpl(DiscordConfiguration cfg)
        {
            _Client = new DiscordClient(cfg);
            _Client.ClientErrored += OnClientErrored;
            _Client.SocketErrored += OnSocketErrored;
            _Client.SocketOpened += OnSocketOpened;
            _Client.SocketClosed += OnSocketClosed;
            _Client.Resumed += OnResumed;
            _Client.ChannelCreated += OnChannelCreated;
            _Client.DmChannelCreated += OnDmChannelCreated;
            _Client.ChannelUpdated += OnChannelUpdated;
            _Client.ChannelDeleted += OnChannelDeleted;
            _Client.DmChannelDeleted += OnDmChannelDeleted;
            _Client.ChannelPinsUpdated += OnChannelPinsUpdated;
            _Client.GuildCreated += OnGuildCreated;
            _Client.GuildAvailable += OnGuildAvailable;
            _Client.GuildUpdated += OnGuildUpdated;
            _Client.GuildDeleted += OnGuildDeleted;
            _Client.GuildUnavailable += OnGuildUnavailable;
            _Client.MessageCreated += OnMessageCreated;
            _Client.PresenceUpdated += OnPresenceUpdated;
            _Client.GuildBanAdded += OnGuildBanAdded;
            _Client.GuildBanRemoved += OnGuildBanRemoved;
            _Client.GuildEmojisUpdated += OnGuildEmojisUpdated;
            _Client.GuildIntegrationsUpdated += OnGuildIntegrationsUpdated;
            _Client.Ready += OnReady;
            _Client.GuildMemberAdded += OnGuildMemberAdded;
            _Client.GuildMemberRemoved += OnGuildMemberRemoved;
            _Client.GuildMemberUpdated += OnGuildMemberUpdated;
            _Client.GuildRoleCreated += OnGuildRoleCreated;
            _Client.GuildRoleUpdated += OnGuildRoleUpdated;
            _Client.GuildRoleDeleted += OnGuildRoleDeleted;
            _Client.MessageAcknowledged += OnMessageAcknowledged;
            _Client.MessageUpdated += OnMessageUpdated;
            _Client.MessageDeleted += OnMessageDeleted;
            _Client.MessagesBulkDeleted += OnMessagesBulkDeleted;
            _Client.TypingStarted += OnTypingStarted;
            _Client.UserSettingsUpdated += OnUserSettingsUpdated;
            _Client.UserUpdated += OnUserUpdated;
            _Client.VoiceStateUpdated += OnVoiceStateUpdated;
            _Client.VoiceServerUpdated += OnVoiceServerUpdated;
            _Client.GuildMembersChunked += OnGuildMembersChunked;
            _Client.UnknownEvent += OnUnknownEvent;
            _Client.MessageReactionAdded += OnMessageReactionAdded;
            _Client.MessageReactionRemoved += OnMessageReactionRemoved;
            _Client.MessageReactionsCleared += OnMessageReactionsCleared;
            _Client.WebhooksUpdated += OnWebhooksUpdated;
            _Client.Heartbeated += OnHeartbeated;
        }


        private async Task OnGuildMembersChunked(GuildMembersChunkEventArgs e)
        {
            if (GuildMembersChunked != null)
            {
                await GuildMembersChunked(e);
            }
        }
        
        private async Task OnClientErrored(ClientErrorEventArgs e)
        {
            if (ClientErrored!= null)
            {
                await ClientErrored(e);
            }
        }
        
        private async Task OnSocketErrored(SocketErrorEventArgs e)
        {
            if (SocketErrored != null)
            {
                await SocketErrored(e);
            }
        }

        private async Task OnSocketOpened()
        {
            if (SocketOpened != null)
            {
                await SocketOpened();
            }
        }

        private async Task OnSocketClosed(SocketCloseEventArgs e)
        {
            if (SocketClosed != null)
            {
                await SocketClosed(e);
            }
        }

        private async Task OnResumed(ReadyEventArgs e)
        {
            if (Resumed != null)
            {
                await Resumed(e);
            }
        }

        private async Task OnChannelCreated(ChannelCreateEventArgs e)
        {
            if (ChannelCreated != null)
            {
                await ChannelCreated(e);
            }
        }

        private async Task OnDmChannelCreated(DmChannelCreateEventArgs e)
        {
            if (DmChannelCreated != null)
            {
                await DmChannelCreated(e);
            }
        }

        private async Task OnChannelUpdated(ChannelUpdateEventArgs e)
        {
            if (ChannelUpdated != null)
            {
                await ChannelUpdated(e);
            }
        }

        private async Task OnChannelDeleted(ChannelDeleteEventArgs e)
        {
            if (ChannelDeleted != null)
            {
                await ChannelDeleted(e);
            }
        }

        private async Task OnDmChannelDeleted(DmChannelDeleteEventArgs e)
        {
            if (DmChannelDeleted != null)
            {
                await DmChannelDeleted(e);
            }
        }

        private async Task OnChannelPinsUpdated(ChannelPinsUpdateEventArgs e)
        {
            if (ChannelPinsUpdated != null)
            {
                await ChannelPinsUpdated(e);
            }
        }

        private async Task OnGuildCreated(GuildCreateEventArgs e)
        {
            if (GuildCreated != null)
            {
                await GuildCreated(e);
            }
        }

        private async Task OnGuildAvailable(GuildCreateEventArgs e)
        {
            if (GuildAvailable != null)
            {
                await GuildAvailable(e);
            }
        }

        private async Task OnGuildUpdated(GuildUpdateEventArgs e)
        {
            if (GuildUpdated != null)
            {
                await GuildUpdated(e);
            }
        }

        private async Task OnGuildDeleted(GuildDeleteEventArgs e)
        {
            if (GuildDeleted != null)
            {
                await GuildDeleted(e);
            }
        }

        private async Task OnGuildUnavailable(GuildDeleteEventArgs e)
        {
            if (GuildUnavailable != null)
            {
                await GuildUnavailable(e);
            }
        }

        private async Task OnPresenceUpdated(PresenceUpdateEventArgs e)
        {
            if (PresenceUpdated != null)
            {
                await PresenceUpdated(e);
            }
        }

        private async Task OnGuildBanAdded(GuildBanAddEventArgs e)
        {
            if (GuildBanAdded != null)
            {
                await GuildBanAdded(e);
            }
        }

        private async Task OnGuildBanRemoved(GuildBanRemoveEventArgs e)
        {
            if (GuildBanRemoved != null)
            {
                await GuildBanRemoved(e);
            }
        }

        private async Task OnGuildEmojisUpdated(GuildEmojisUpdateEventArgs e)
        {
            if (GuildEmojisUpdated != null)
            {
                await GuildEmojisUpdated(e);
            }
        }

        private async Task OnGuildIntegrationsUpdated(GuildIntegrationsUpdateEventArgs e)
        {
            if (GuildIntegrationsUpdated != null)
            {
                await GuildIntegrationsUpdated(e);
            }
        }

        private async Task OnReady(ReadyEventArgs e)
        {
            if (Ready != null)
            {
                await Ready(e);
            }
        }

        private async Task OnGuildMemberAdded(GuildMemberAddEventArgs e)
        {
            if (GuildMemberAdded != null)
            {
                await GuildMemberAdded(e);
            }
        }

        private async Task OnGuildMemberRemoved(GuildMemberRemoveEventArgs e)
        {
            if (GuildMemberRemoved != null)
            {
                await GuildMemberRemoved(e);
            }
        }

        private async Task OnGuildMemberUpdated(GuildMemberUpdateEventArgs e)
        {
            if (GuildMemberUpdated != null)
            {
                await GuildMemberUpdated(e);
            }
        }

        private async Task OnGuildRoleCreated(GuildRoleCreateEventArgs e)
        {
            if (GuildRoleCreated != null)
            {
                await GuildRoleCreated(e);
            }
        }

        private async Task OnGuildRoleUpdated(GuildRoleUpdateEventArgs e)
        {
            if (GuildRoleUpdated != null)
            {
                await GuildRoleUpdated(e);
            }
        }

        private async Task OnGuildRoleDeleted(GuildRoleDeleteEventArgs e)
        {
            if (GuildRoleDeleted != null)
            {
                await GuildRoleDeleted(e);
            }
        }

        private async Task OnMessageAcknowledged(MessageAcknowledgeEventArgs e)
        {
            if (MessageAcknowledged != null)
            {
                await MessageAcknowledged(e);
            }
        }

        private async Task OnMessageUpdated(MessageUpdateEventArgs e)
        {
            if (MessageUpdated != null)
            {
                await MessageUpdated(e);
            }
        }

        private async Task OnMessageDeleted(MessageDeleteEventArgs e)
        {
            if (MessageDeleted != null)
            {
                await MessageDeleted(e);
            }
        }

        private async Task OnMessagesBulkDeleted(MessageBulkDeleteEventArgs e)
        {
            if (MessagesBulkDeleted != null)
            {
                await MessagesBulkDeleted(e);
            }
        }

        private async Task OnTypingStarted(TypingStartEventArgs e)
        {
            if (TypingStarted != null)
            {
                await TypingStarted(e);
            }
        }

        private async Task OnUserSettingsUpdated(UserSettingsUpdateEventArgs e)
        {
            if (UserSettingsUpdated != null)
            {
                await UserSettingsUpdated(e);
            }
        }

        private async Task OnUserUpdated(UserUpdateEventArgs e)
        {
            if (UserUpdated != null)
            {
                await UserUpdated(e);
            }
        }

        private async Task OnVoiceStateUpdated(VoiceStateUpdateEventArgs e)
        {
            if (VoiceStateUpdated != null)
            {
                await VoiceStateUpdated(e);
            }
        }

        private async Task OnVoiceServerUpdated(VoiceServerUpdateEventArgs e)
        {
            if (VoiceServerUpdated != null)
            {
                await VoiceServerUpdated(e);
            }
        }

        private async Task OnUnknownEvent(UnknownEventArgs e)
        {
            if (UnknownEvent != null)
            {
                await UnknownEvent(e);
            }
        }

        private async Task OnMessageReactionAdded(MessageReactionAddEventArgs e)
        {
            if (MessageReactionAdded != null)
            {
                await MessageReactionAdded(e);
            }
        }

        private async Task OnMessageReactionRemoved(MessageReactionRemoveEventArgs e)
        {
            if (MessageReactionRemoved != null)
            {
                await MessageReactionRemoved(e);
            }
        }

        private async Task OnMessageReactionsCleared(MessageReactionsClearEventArgs e)
        {
            if (MessageReactionsCleared != null)
            {
                await MessageReactionsCleared(e);
            }
        }

        private async Task OnWebhooksUpdated(WebhooksUpdateEventArgs e)
        {
            if (WebhooksUpdated != null)
            {
                await WebhooksUpdated(e);
            }
        }

        private async Task OnHeartbeated(HeartbeatEventArgs e)
        {
            if (Heartbeated != null)
            {
                await Heartbeated(e);
            }
        }

        private async Task OnMessageCreated(MessageCreateEventArgs e)
        {
            if (MessageCreated != null)
            {
                await MessageCreated(e);
            }
        }

        public Task ConnectAsync()
        {
            return _Client.ConnectAsync();
        }

        public Task DisconnectAsync()
        {
            return _Client.DisconnectAsync();
        }

        public void Dispose()
        {
            _Client.Dispose();
        }

        public CommandsNextExtension UseCommandsNext(CommandsNextConfiguration commandsNextConfiguration)
        {
            return _Client.UseCommandsNext(commandsNextConfiguration);
        }

        public InteractivityExtension UseInteractivity(InteractivityConfiguration interactivityConfiguration)
        {
            return _Client.UseInteractivity(interactivityConfiguration);
        }

        public VoiceNextExtension UseVoiceNext()
        {
            return _Client.UseVoiceNext();
        }

        public Task<DiscordChannel> GetChannelAsync(ulong smtiChannelId)
        {
            return _Client.GetChannelAsync(smtiChannelId);
        }

        public Task<DiscordUser> GetUserAsync(ulong smtiInitiatorId)
        {
            return _Client.GetUserAsync(smtiInitiatorId);
        }

        public Task<DiscordChannel> CreateDmChannelAsync(ulong infoInitiatorId)
        {
            return CreateDmChannelAsync(infoInitiatorId);
        }

        public Task<DiscordGuild> GetGuildAsync(ulong infoGuildId)
        {
            return _Client.GetGuildAsync(infoGuildId);
        }

        private DiscordClientImpl(DiscordClient client)
        {
            _Client = client;
        }

        public static implicit operator DiscordClientImpl(DiscordClient v)
        {
            var retval = new DiscordClientImpl(v);
            return retval;
        }
    }
}