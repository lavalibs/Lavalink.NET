using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Lavalink.NET.Types
{
	/// <summary>
	/// Status enum for players.
	/// </summary>
	public enum Status
	{
		INSTANTIATED,
		PLAYING,
		PAUSED,
		ENDED,
		ERRORED,
		STUCK
	}

	/// <summary>
	/// The Player class to use.
	/// </summary>
	public class Player
    {
		/// <summary>
		/// Event to call on Track Ending.
		/// </summary>
		public event TrackEndEvent End;

		/// <summary>
		/// Event to call on Track Exeption.
		/// </summary>
		public event TrackExceptionEvent Exeption;

		/// <summary>
		/// Event to call on Track Stuck.
		/// </summary>
		public event TrackStuckEvent Stuck;

		/// <summary>
		/// The GuildID of this player.
		/// </summary>
		public ulong GuildID { get; }
		/// <summary>
		/// The current Status od this player.
		/// </summary>
		public Status Status { get; set; }
		
		/// <summary>
		/// Client instance of this player.
		/// </summary>
		private Client _client;

		/// <summary>
		/// Constructor of the Player class.
		/// </summary>
		/// <param name="client"> The Client of this player. </param>
		/// <param name="guildID"> The GuildID of this player. </param>
		public Player(Client client, ulong guildID)
		{
			_client = client;
			GuildID = guildID;
			Status = Status.INSTANTIATED;

			End += PlayerEndEvent;
			Exeption += PlayerExeptionEvent;
			Stuck += PlayerStuckEvent;
		}

		/// <summary>
		/// Method to Join a VoiceChannel with your bot.
		/// </summary>
		/// <param name="channelID"> The Channel ID of the channel to join. </param>
		/// <param name="mute"> should this player join muted. </param>
		/// <param name="deaf"> should this player join deafen. </param>
		/// <returns> Task resolving with void. </returns>
		public Task JoinAsync(string channelID, bool mute = false, bool deaf = false)
		{
			return _client.SendAsync(GuildID, JsonConvert.SerializeObject(new DiscordOP4Packet(new DiscordVoicePacket(GuildID.ToString(), channelID, mute, deaf))));
		}

		/// <summary>
		/// Method to Leave VoiceChannel on this Server.
		/// </summary>
		/// <returns> Task resolving with void. </returns>
		public Task LeaveAsync()
		{
			return _client.SendAsync(GuildID, JsonConvert.SerializeObject(new DiscordOP4Packet(new DiscordVoicePacket(GuildID.ToString(), null, false, false))));
		}

		/// <summary>
		/// Method to let the player start a track
		/// </summary>
		/// <param name="track"> Track string of the track to play. </param>
		/// <param name="start"> optional: start time of the track</param>
		/// <param name="end"> optional: end time of the track</param>
		/// <returns> Task resolving with void. </returns>
		public async Task PlayAsync(string track, int? start = 0, int? end = 0)
		{
			await _client.Websocket.SendMessage(JsonConvert.SerializeObject(new PlayPacket("play", GuildID.ToString(), track, start.ToString(), end.ToString())));

			Status = Status.PLAYING;
		}

		/// <summary>
		/// Method to let the player start a track
		/// </summary>
		/// <param name="track"> Track instance of the track to play. </param>
		/// <param name="start"> optional: start time of the track</param>
		/// <param name="end"> optional: end time of the track</param>
		/// <returns> Task resolving with void. </returns>
		public async Task PlayAsync(Track track, int? start = 0, int? end = 0)
		{
			await _client.Websocket.SendMessage(JsonConvert.SerializeObject(new PlayPacket("play", GuildID.ToString(), track.TrackString, start.ToString(), end.ToString())));

			Status = Status.PLAYING;
		}

		/// <summary>
		/// Method to pause/resume the player
		/// </summary>
		/// <param name="paused"> boolean </param>
		/// <returns> Task resolving with void. </returns>
		public async Task PauseAsync(bool pause = true)
		{
			await _client.Websocket.SendMessage(JsonConvert.SerializeObject(new PausePacket("pause", GuildID.ToString(), pause)));

			if (pause)
			{
				Status = Status.PAUSED;
			} else
			{
				Status = Status.PLAYING;
			}
		}

		/// <summary>
		/// Method to Seek to a specified position.
		/// </summary>
		/// <param name="position"> position to seek to</param>
		/// <returns> Task resolving with void. </returns>
		public Task SeekAsync(int position)
		{
			return _client.Websocket.SendMessage(JsonConvert.SerializeObject(new SeekPacket("seek", GuildID.ToString(), position)));
		}

		/// <summary>
		/// Method to set the Volume
		/// </summary>
		/// <param name="volume"> the volume to set</param>
		/// <returns> Task resolving with void. </returns>
		public Task SetVolumeAsync(int volume)
		{
			return _client.Websocket.SendMessage(JsonConvert.SerializeObject(new VolumePacket("volume", GuildID.ToString(), volume)));
		}

		/// <summary>
		/// Method to call on Discord Voice Updates
		/// </summary>
		/// <param name="sessionID"> The SessionID </param>
		/// <param name="voiceEvent"> The VoiceEvent </param>
		/// <returns> Task resolving with void. </returns>
		public Task VoiceUpdateAsync(string sessionID, VoiceServerUpdate voiceEvent)
		{
			return _client.Websocket.SendMessage(JsonConvert.SerializeObject(new VoiceUpdatePacket("voiceUpdate", GuildID.ToString(), sessionID, voiceEvent)));
		}

		internal void EmitEvent(dynamic lavalinkEvent)
		{
			switch (lavalinkEvent.type)
			{
				case "TrackEndEvent":
					End(this, new TrackEndEventArgs(lavalinkEvent.op, lavalinkEvent.track, lavalinkEvent.guildId, lavalinkEvent.type, lavalinkEvent.reason));
					break;
				case "TrackExeptionEvent":
					Exeption(this, new TrackExceptionEventArgs(lavalinkEvent.op, lavalinkEvent.type, lavalinkEvent.guildId, lavalinkEvent.type, lavalinkEvent.error));
					break;
				case "TrackStuckEvent":
					Stuck(this, new TrackStuckEventArgs(lavalinkEvent.op, lavalinkEvent.track, lavalinkEvent.guildId, lavalinkEvent.type, lavalinkEvent.thresholdMs));
					break;
			}
		}

		private void PlayerEndEvent(object sender, TrackEndEventArgs e)
		{
			Status = Status.ENDED;
		}

		private void PlayerExeptionEvent(object sender, TrackExceptionEventArgs e)
		{
			Status = Status.ERRORED;
		}

		private void PlayerStuckEvent(object sender, TrackStuckEventArgs e)
		{
			Status = Status.STUCK;
		}
    }
}
