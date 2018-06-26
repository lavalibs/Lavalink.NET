using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;
using DSharpPlus.Net.WebSocket;
using Lavalink.NET;
using Lavalink.NET.Types;
using TestBot_DSharpPlus.Lavalink;

namespace TestBot_DSharpPlus
{
	public class Client
    {
		public static LavalinkClient _lavalinkClient;

		private DiscordClient _client;
		private CommandsNextExtension _commands;

		public async Task MainAsync(string[] args)
		{
			_client = new DiscordClient(new DiscordConfiguration
			{
				Token = "",
				TokenType = TokenType.Bot,
				WebSocketClientFactory = WebsocketStorage.CreateNew,
				LogLevel = DSharpPlus.LogLevel.Debug,
				UseInternalLogHandler = true
			});

			_commands = _client.UseCommandsNext(new CommandsNextConfiguration {
				StringPrefixes = new string[] { "." }
			});

			_commands.RegisterCommands<Commands.Commands>();

			await _client.ConnectAsync();

			_client.Ready += InitLavalink;

			await Task.Delay(-1);
		}

		private Task InitLavalink(ReadyEventArgs args)
		{
			_lavalinkClient = new LavalinkClient(new ClientOptions {
				UserID = _client.CurrentUser.Id,
				HostRest = "http://localhost:2333",
				HostWS = "ws://localhost:8060",
				Password = "youshallnotpass",
				UseLogging = true,
				LogLevel = global::Lavalink.NET.Types.LogLevel.Debug },
				_client);
			_client.VoiceStateUpdated += async e =>
			{
				await _lavalinkClient.VoiceStateUpdateAsync(new VoiceStateUpdate
				{
					ChannelID = e.Channel.Id,
					GuildID = e.Guild.Id,
					SessionID = e.SessionId,
					UserID = e.User.Id,
					Deaf = e.After.IsServerDeafened,
					Mute = e.After.IsServerMuted,
					Suppress = e.After.IsSuppressed,
					SelfDeaf = e.After.IsSelfDeafened,
					SelfMute = e.After.IsSelfMuted
				});
			};
			_client.VoiceServerUpdated += async e =>
			{
				await _lavalinkClient.VoiceServerUpdateAsync(new VoiceServerUpdate
				{
					Endpoint = e.Endpoint,
					GuildID = e.Guild.Id,
					Token = e.VoiceToken
				});
			};
			_lavalinkClient.Start();

			return Task.CompletedTask;
		}
	}

	public abstract class WebsocketStorage
	{
		internal static SortedDictionary<int, BaseWebSocketClient> storage = new SortedDictionary<int, BaseWebSocketClient>();

		internal static BaseWebSocketClient CreateNew(IWebProxy proxy)
		{
			BaseWebSocketClient webSocketClient = WebSocketClient.CreateNew(proxy);
			storage.Add(storage.Count + 1, webSocketClient);
			return webSocketClient;
		}
	}
}
