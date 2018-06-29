using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Lavalink.NET;
using Lavalink.NET.Types;

namespace Testbot_Discord.Net.Music
{
	class LavalinkClient : Lavalink.NET.Client
	{
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
					SocketChannel channel = _client.GetChannel((ulong) packet.DiscordVoicePacket.ChannelID);

					if (!(channel is IAudioChannel voicechannel)) throw new Exception("Wrong channel type.");

					await voicechannel.ConnectAsync(false, false, true);
				} else
				{
					ulong channelID = (ulong) Client.Lavalink.Players.GetPlayer(packet.DiscordVoicePacket.GuildID).ChannelID;

					SocketChannel channel = _client.GetChannel(channelID);

					if (!(channel is IAudioChannel voicechannel)) throw new Exception("Wrong channel type.");

					await voicechannel.DisconnectAsync();
				}
			}
		}
	}
}
