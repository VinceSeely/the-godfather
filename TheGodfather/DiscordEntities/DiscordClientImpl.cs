
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
            _Client.ClientErrored += ClientErrored;
            _Client.SocketErrored += SocketErrored;
            _Client.SocketOpened += SocketOpened;
            _Client.SocketClosed += SocketClosed;
            _Client.Resumed += Resumed;
            _Client.ChannelCreated += ChannelCreated;
            _Client.DmChannelCreated += DmChannelCreated;
            _Client.ChannelUpdated += ChannelUpdated;
            _Client.ChannelDeleted += ChannelDeleted;
            _Client.DmChannelDeleted += DmChannelDeleted;
            _Client.ChannelPinsUpdated += ChannelPinsUpdated;
            _Client.GuildCreated += GuildCreated;
            _Client.GuildAvailable += GuildAvailable;
            _Client.GuildUpdated += GuildUpdated;
            _Client.GuildDeleted += GuildDeleted;
            _Client.GuildUnavailable += GuildUnavailable;
            _Client.MessageCreated += MessageCreated;
            _Client.PresenceUpdated += PresenceUpdated;
            _Client.GuildBanAdded += GuildBanAdded;
            _Client.GuildBanRemoved += GuildBanRemoved;
            _Client.GuildEmojisUpdated += GuildEmojisUpdated;
            _Client.GuildIntegrationsUpdated += GuildIntegrationsUpdated;
            _Client.Ready += Ready;
            _Client.GuildMemberAdded += GuildMemberAdded;
            _Client.GuildMemberRemoved += GuildMemberRemoved;
            _Client.GuildMemberUpdated += GuildMemberUpdated;
            _Client.GuildRoleCreated += GuildRoleCreated;
            _Client.GuildRoleUpdated += GuildRoleUpdated;
            _Client.GuildRoleDeleted += GuildRoleDeleted;
            _Client.MessageAcknowledged += MessageAcknowledged;
            _Client.MessageUpdated += MessageUpdated;
            _Client.MessageDeleted += MessageDeleted;
            _Client.MessagesBulkDeleted += MessagesBulkDeleted;
            _Client.TypingStarted += TypingStarted;
            _Client.UserSettingsUpdated += UserSettingsUpdated;
            _Client.UserUpdated += UserUpdated;
            _Client.VoiceStateUpdated += VoiceStateUpdated;
            _Client.VoiceServerUpdated += VoiceServerUpdated;
            _Client.GuildMembersChunked += GuildMembersChunked;
            _Client.UnknownEvent += UnknownEvent;
            _Client.MessageReactionAdded += MessageReactionAdded;
            _Client.MessageReactionRemoved += MessageReactionRemoved;
            _Client.MessageReactionsCleared += MessageReactionsCleared;
            _Client.WebhooksUpdated += WebhooksUpdated;
            _Client.Heartbeated += Heartbeated;
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