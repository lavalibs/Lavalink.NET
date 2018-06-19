using System;
using System.Threading.Tasks;
using Discord.WebSocket;
using Lavalink.NET;
using Newtonsoft.Json.Linq;

namespace Testbot_Discord.Net.Util
{
	class LavalinkClient : Lavalink.NET.Client
	{
		DiscordSocketClient _client;

		public LavalinkClient(ClientOptions options, DiscordSocketClient client) 
			: base(options)
		{
			_client = client;
		}

		public override Task SendAsync(ulong guildID, string packetJSON)
		{
			if (_client.GetGuild(guildID) != null)
			{
				dynamic json = JObject.Parse(packetJSON);

				SocketChannel channel = _client.GetChannel(Convert.ToUInt64(json.d.channel_id));

				if (!(channel is SocketVoiceChannel voicechannel)) return Task.FromException(new Exception("Wrong channel type."));

				voicechannel.ConnectAsync(false, false, true);
			}

			return Task.CompletedTask;
		}
	}
}
