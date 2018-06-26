using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord.Audio;
using Discord.WebSocket;
using Lavalink.NET;
using Lavalink.NET.Types;

namespace Testbot_Discord.Net.Util
{
	class LavalinkClient : Lavalink.NET.Client
	{
		public readonly Dictionary<ulong, IAudioClient> AudioClientStore = new Dictionary<ulong, IAudioClient>();
		private DiscordSocketClient _client;

		public LavalinkClient(ClientOptions options, DiscordSocketClient client) 
			: base(options)
		{
			_client = client;
		}

		public override async Task SendAsync(DiscordOP4Packet packet)
		{
			if (_client.GetGuild(packet.DiscordVoicePacket.GuildID) != null) {
				if (packet.DiscordVoicePacket.ChannelID != null) {
					SocketChannel channel = _client.GetChannel(packet.DiscordVoicePacket.ChannelID.Value);

					if (!(channel is SocketVoiceChannel voicechannel)) throw new Exception("Wrong channel type.");

					IAudioClient audioclient = await voicechannel.ConnectAsync(false, false, true);
					AudioClientStore.Add(packet.DiscordVoicePacket.GuildID, audioclient);
				} else
				{
					AudioClientStore.TryGetValue(packet.DiscordVoicePacket.ChannelID.Value, out IAudioClient audioClient);
					await audioClient.StopAsync();
					AudioClientStore.Remove(packet.DiscordVoicePacket.GuildID);
				}
			}
		}
	}
}
