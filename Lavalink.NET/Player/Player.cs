using System;
using System.Threading.Tasks;
using Lavalink.NET.Types;
using Newtonsoft.Json;

namespace Lavalink.NET.Player
{
	/// <summary>
	/// The Player class to use.
	/// </summary>
	public class Player
    {
		/// <summary>
		/// Event to call on Track Ending.
		/// </summary>
		public event Func<TrackEndEvent, Task> End;

		/// <summary>
		/// Event to call on Track Exception.
		/// </summary>
		public event Func<TrackExceptionEvent, Task> Exception;

		/// <summary>
		/// Event to call on Track Stuck.
		/// </summary>
		public event Func<TrackStuckEvent, Task> Stuck;

		/// <summary>
		/// The GuildID of this player.
		/// </summary>
		public ulong GuildID { get; private set; }

		/// <summary>
		/// The current Status of this player.
		/// </summary>
		public Status Status { get; private set; } = Status.INSTANTIATED;

		/// <summary>
		/// Boolean representing if this Player is connected to an Channel.
		/// </summary>
		public bool Connected { get; private set; } = false;

		/// <summary>
		/// The Channel ID which this Player is currently connected to, if this player isn't connected this is null.
		/// </summary>
		public ulong? ChannelID { get; private set; } = null;

		/// <summary>
		/// The position of the Player from the current playing song, this is -1 when there is no Song playing.
		/// </summary>
		public long Position
		{
			get {
				if (Status == Status.PLAYING) { return _position; }
				else return -1;
			}

			internal set { _position = value; }
		}

		/// <summary>
		/// Holds the value for the Position.
		/// </summary>
		private long _position;
		
		/// <summary>
		/// Client instance that created this player.
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

			End += PlayerEndEvent;
			Exception += PlayerExeptionEvent;
			Stuck += PlayerStuckEvent;
		}

		/// <summary>
		/// Method to Join a VoiceChannel with your bot.
		/// </summary>
		/// <param name="channelID"> The Channel ID of the channel to join. </param>
		/// <param name="mute"> should this player join muted. </param>
		/// <param name="deaf"> should this player join deafen. </param>
		/// <returns> Task resolving with void. </returns>
		public async Task JoinAsync(ulong channelID, bool mute = false, bool deaf = false)
		{
			await _client.SendAsync(new DiscordOP4Packet(GuildID, channelID, mute, deaf));

			ChannelID = channelID;

			Connected = true;
		}

		/// <summary>
		/// Method to Leave VoiceChannel on this Server.
		/// </summary>
		/// <returns> Task resolving with void. </returns>
		public async Task LeaveAsync()
		{
			await _client.SendAsync(new DiscordOP4Packet(GuildID, null, false, false));

			ChannelID = null;

			Connected = false;
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
			await _client.Websocket.SendMessageAsync(JsonConvert.SerializeObject(new PlayPacket
			{
				OPCode = "play",
				GuildID = GuildID.ToString(),
				Track = track,
				StartTime = start.ToString(),
				EndTime = end.ToString()
			}));

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
			await _client.Websocket.SendMessageAsync(JsonConvert.SerializeObject(new PlayPacket {
				OPCode = "play",
				GuildID = GuildID.ToString(),
				Track = track.TrackString,
				StartTime = start.ToString(),
				EndTime = end.ToString()
			}));

			Status = Status.PLAYING;
		}

		/// <summary>
		/// Method to pause/resume the player
		/// </summary>
		/// <param name="pause"> boolean </param>
		/// <returns> Task resolving with void. </returns>
		public async Task PauseAsync(bool pause = true)
		{
			await _client.Websocket.SendMessageAsync(JsonConvert.SerializeObject(new PausePacket {
				OPCode = "pause",
				Pause = pause,
				GuildID = GuildID.ToString()
			}));

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
			return _client.Websocket.SendMessageAsync(JsonConvert.SerializeObject(new SeekPacket
			{
				OPCode = "seek",
				GuildID = GuildID.ToString(),
				Position = position
			}));
		}

		/// <summary>
		/// Method to set the Volume
		/// </summary>
		/// <param name="volume"> the volume to set</param>
		/// <returns> Task resolving with void. </returns>
		public Task SetVolumeAsync(int volume)
		{
			return _client.Websocket.SendMessageAsync(JsonConvert.SerializeObject(new VolumePacket
			{
				OPCode = "volume",
				GuildID = GuildID.ToString(),
				Volume = volume
			}));
		}

		/// <summary>
		/// Method to call on Discord Voice Updates
		/// </summary>
		/// <param name="sessionID"> The SessionID </param>
		/// <param name="voiceEvent"> The VoiceEvent </param>
		/// <returns> Task resolving with void. </returns>
		public Task VoiceUpdateAsync(string sessionID, VoiceServerUpdate voiceEvent)
		{
			return _client.Websocket.SendMessageAsync(JsonConvert.SerializeObject(new VoiceUpdatePacket
			{
				OPCode = "voiceUpdate",
				SessionID = sessionID,
				UpdateEvent = voiceEvent,
				GuildID = GuildID.ToString()
			}));
		}

		internal void EmitEvent(dynamic lavalinkEvent)
		{
			switch (lavalinkEvent.type)
			{
				case "TrackEndEvent":
					End(JsonConvert.DeserializeObject<TrackEndEvent>(lavalinkEvent));
					break;
				case "TrackExeptionEvent":
					Exception(JsonConvert.DeserializeObject<TrackExceptionEvent>(lavalinkEvent));
					break;
				case "TrackStuckEvent":
					Stuck(JsonConvert.DeserializeObject<TrackStuckEvent>(lavalinkEvent));
					break;
			}
		}

		private Task PlayerEndEvent(TrackEndEvent e)
		{
			Status = Status.ENDED;

			return Task.CompletedTask;
		}

		private Task PlayerExeptionEvent(TrackExceptionEvent e)
		{
			Status = Status.ERRORED;

			return Task.CompletedTask;
		}

		private Task PlayerStuckEvent(TrackStuckEvent e)
		{
			Status = Status.STUCK;

			return Task.CompletedTask;
		}
    }
}
