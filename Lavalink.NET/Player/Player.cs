using System.Threading.Tasks;
using Lavalink.NET.Types;
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
		STUCK
	}

	public class Player
    {
		public event TrackEndEvent TrackEndEvent;
		public event TrackExceptionEvent TrackExceptionEvent;
		public event TrackStuckEvent TrackStuckEvent;

		public string GuildID { get; }
		public Status Status { get; set; }

		private Client _client;

		public Player(Client client, string guildID)
		{
			_client = client;
			GuildID = guildID;
			Status = Status.INSTANTIATED;
		}

		public Task JoinAsync(string channelID, bool mute = false, bool deaf = false)
		{
			return _client.SendAsync(GuildID, JsonConvert.SerializeObject(new DiscordOP4Packet(new DiscordVoicePacket(GuildID, channelID, mute, deaf))));
		}

		public Task LeaveAsync()
		{
			return _client.SendAsync(GuildID, JsonConvert.SerializeObject(new DiscordOP4Packet(new DiscordVoicePacket(GuildID, null, false, false))));
		}

		public async Task PlayAsync(string track, int? start = 0, int? end = 0)
		{
			await _client.Websocket.SendMessage(JsonConvert.SerializeObject(new PlayPacket("play", GuildID, track, start.ToString(), end.ToString())));

			Status = Status.PLAYING;
		}

		public async Task PlayAsync(Track track, int? start = 0, int? end = 0)
		{
			await _client.Websocket.SendMessage(JsonConvert.SerializeObject(new PlayPacket("play", GuildID, track.TrackString, start.ToString(), end.ToString())));

			Status = Status.PLAYING;
		}

		public async Task PauseAsync(bool paused = true)
		{
			await _client.Websocket.SendMessage(JsonConvert.SerializeObject(new PausePacket("pause", GuildID, paused)));

			Status = Status.PAUSED;
		}

		public Task SeekAsync(int position)
		{
			return _client.Websocket.SendMessage(JsonConvert.SerializeObject(new SeekPacket("seek", GuildID, position)));
		}

		public Task SetVolumeAsync(int volume)
		{
			return _client.Websocket.SendMessage(JsonConvert.SerializeObject(new VolumePacket("volume", GuildID, volume)));
		}

		public Task VoiceUpdateAsync(string sessionID, VoiceServerUpdate voiceEvent)
		{
			return _client.Websocket.SendMessage(JsonConvert.SerializeObject(new VoiceUpdatePacket("voiceUpdate", GuildID, sessionID, voiceEvent)));
		}
    }
}
