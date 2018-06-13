using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Net.WebSocket;
using Lavalink.NET;

namespace TestBot.Util
{
	public class LavalinkClient : Lavalink.NET.Client
	{
		private DiscordClient _client;

		public LavalinkClient(ClientOptions options, DiscordClient client)
			: base(options)
		{
			_client = client;
		}

		public override Task SendAsync(ulong guildID, string packetJSON)
		{
			if (_client.Guilds.ContainsKey(guildID))
			{
				WebsocketStorage.storage.TryGetValue(1, out BaseWebSocketClient ws);
				ws.SendMessage(packetJSON);
			}
			return Task.CompletedTask;
		}
	}
}
