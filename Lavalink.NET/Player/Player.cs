using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Lavalink.NET.Player
{
	public enum Status
	{
		INSTANTIATED,
		PLAYING,
		PAUSED,
		ENDED,
		ERRORED,
	}

	public class DiscordVoicePacket
	{
		#pragma warning disable IDE1006
		public string guild_id { get; set; }
		public string channel_id { get; set; }
		public bool self_mute { get; set; }
		public bool self_deaf { get; set; }
		#pragma warning restore IDE1006

		public DiscordVoicePacket(string guildID, string channelID, bool selfMute, bool selfDeaf)
		{
			guild_id = guildID;
			channel_id = channelID;
			self_mute = selfMute;
			self_deaf = selfDeaf;
		}
	}

	public class DiscordOP4Packet
	{
		#pragma warning disable IDE1006
		public int op { get; set; }
		public DiscordVoicePacket d { get; set; }
		#pragma warning restore IDE1006

		public DiscordOP4Packet(DiscordVoicePacket packet)
		{
			op = 4;
			d = packet;
		}
	}

	public class Player
    {
		public string GuildID { get; }
		public Status Status { get; }

		private Client _client;

		public Player(Client client, string guildID)
		{
			_client = client;
			GuildID = guildID;
			Status = Status.INSTANTIATED;
		}

		public Task JoinAsync(string channelID, bool mute = false, bool deaf = false)
		{
			return _client.Send(GuildID, JsonConvert.SerializeObject(new DiscordOP4Packet(new DiscordVoicePacket(GuildID, channelID, mute, deaf))));
		}

		public Task LeaveAsync()
		{
			return _client.Send(GuildID, JsonConvert.SerializeObject(new DiscordOP4Packet(new DiscordVoicePacket(GuildID, null, false, false))));
		}

		public Task SendAsync(string op, object body)
		{
			return _client.Websocket.SendMessage(JsonConvert.SerializeObject(body));
		}
    }
}
