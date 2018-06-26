using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;
using Discord.Audio;
using Discord.WebSocket;
using Lavalink.NET;
using Lavalink.NET.Types;

namespace Testbot_Discord.Net.Util
{
	class LavalinkClient : Lavalink.NET.Client
	{
		public readonly Dictionary<ulong, IAudioChannel> AudioClientStore = new Dictionary<ulong, IAudioChannel>();
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
					SocketChannel channel = _client.GetChannel(packet.DiscordVoicePacket.ChannelID ?? default(ulong));

					if (!(channel is IAudioChannel voicechannel)) throw new Exception("Wrong channel type.");

					await voicechannel.ConnectAsync(false, false, true);

					AudioClientStore.Add(packet.DiscordVoicePacket.GuildID, voicechannel);
				} else
				{
					AudioClientStore.TryGetValue(packet.DiscordVoicePacket.GuildID, out IAudioChannel voicechannel);
					await voicechannel.DisconnectAsync();
					AudioClientStore.Remove(packet.DiscordVoicePacket.GuildID);
				}
			}
		}
	}
}
