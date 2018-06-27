using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Lavalink.NET;
using Lavalink.NET.Types;
using Microsoft.Extensions.DependencyInjection;
using Testbot_Discord.Net.Lavalink;

namespace Testbot_Discord.Net
{
    class Client
    {
		public static global::Lavalink.NET.Client _lavalinkClient;

		private CommandService _commands;
		private DiscordSocketClient _client;
		private IServiceProvider _services;

		private Task Log(LogMessage msg)
		{
			Console.WriteLine(msg.ToString());
			return Task.CompletedTask;
		}

		public async Task MainAsync()
		{
			var _config = new DiscordSocketConfig { MessageCacheSize = 100, WebSocketProvider = WebsocketStorage.Instance };
			_client = new DiscordSocketClient(_config);
			_commands = new CommandService();

			_services = new ServiceCollection()
				.AddSingleton(_client)
				.AddSingleton(_commands)
				.BuildServiceProvider();

			_client.Log += Log;

			await _client.LoginAsync(TokenType.Bot, "");
			await _client.StartAsync();

			_client.Ready += InitLavalink;
			_client.UserVoiceStateUpdated += VoiceStateUpdate;
			_client.VoiceServerUpdated += VoiceServerUpdate;

			await InstallCommands();

			await Task.Delay(-1);
		}

		private async Task VoiceStateUpdate(SocketUser user, SocketVoiceState before, SocketVoiceState after)
		{
			await _lavalinkClient.VoiceStateUpdateAsync(new VoiceStateUpdate {
				ChannelID = after.VoiceChannel?.Id,
				GuildID = after.VoiceChannel?.Guild.Id ?? before.VoiceChannel.Guild.Id,
				SessionID = after.VoiceSessionId,
				UserID = user.Id,
				Deaf = after.IsDeafened,
				Mute = after.IsMuted,
				Suppress = after.IsSuppressed,
				SelfDeaf = after.IsSelfDeafened,
				SelfMute = after.IsSelfMuted
			});
		}

		private async Task VoiceServerUpdate(SocketVoiceServer voiceServer)
		{
			await _lavalinkClient.VoiceServerUpdateAsync(new VoiceServerUpdate {
				Endpoint = voiceServer.Endpoint,
				GuildID = voiceServer.Guild.Id,
				Token = voiceServer.Token
			});
		}

		private Task InitLavalink()
		{
			_lavalinkClient = new LavalinkClient(new ClientOptions
			{
				UserID = _client.CurrentUser.Id,
				HostRest = "http://localhost:2333",
				HostWS = "ws://localhost:8060",
				Password = "youshallnotpass",
				UseLogging = true,
				LogLevel = LogLevel.Debug
			}, _client);

			_lavalinkClient.Start();

			return Task.CompletedTask;
		}

		private async Task InstallCommands()
		{
			_client.MessageReceived += HandleCommand;

			await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
		}

		private async Task HandleCommand(SocketMessage messageParam)
		{
			if (!(messageParam is SocketUserMessage message)) return;
			int argPos = 0;
			if (!(message.HasCharPrefix('.', ref argPos) || message.HasMentionPrefix(_client.CurrentUser, ref argPos))) return;
			var context = new SocketCommandContext(_client, message);
			var result = await _commands.ExecuteAsync(context, argPos, _services);
			if (!result.IsSuccess && result.Error != CommandError.UnknownCommand)
				await context.Channel.SendMessageAsync(result.ErrorReason);
		}
	}
	public abstract class WebsocketStorage
	{
		internal static SortedDictionary<int, Discord.Net.WebSockets.IWebSocketClient> storage = new SortedDictionary<int, Discord.Net.WebSockets.IWebSocketClient>();

		internal static Discord.Net.WebSockets.IWebSocketClient Instance()
		{
			Discord.Net.WebSockets.IWebSocketClient webSocketClient = Discord.Net.WebSockets.DefaultWebSocketProvider.Instance();
			storage.Add(storage.Count + 1, webSocketClient);
			return webSocketClient;
		}
	}
}
