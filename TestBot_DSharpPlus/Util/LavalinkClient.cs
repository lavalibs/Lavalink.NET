using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Net.WebSocket;
using Lavalink.NET;
using Lavalink.NET.Types;
using Newtonsoft.Json;

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

		public override Task SendAsync(DiscordOP4Packet packet)
		{
			if (_client.Guilds.ContainsKey(packet.DiscordVoicePacket.GuildID))
			{
				WebsocketStorage.storage.TryGetValue(1, out BaseWebSocketClient ws);
				ws.SendMessage(JsonConvert.SerializeObject(packet));
			}
			return Task.CompletedTask;
		}
	}
}
