using System;
using System.Text;
using System.Threading.Tasks;
using Discord.WebSocket;
using Lavalink.NET;

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

		public override async Task SendAsync(ulong guildID, string packetJSON)
		{
			if (_client.GetGuild(guildID) != null)
			{
				WebsocketStorage.storage.TryGetValue(1, out Discord.Net.WebSockets.IWebSocketClient websocket);
				byte[] bytes = Encoding.BigEndianUnicode.GetBytes(packetJSON);
				await websocket.SendAsync(bytes, 0, 1, true);
			}
		}
	}
}
