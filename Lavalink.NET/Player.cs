using System;
using System.Threading.Tasks;
using Lavalink.NET.Types;
using Newtonsoft.Json;
using Spectacles.NET.Types;

namespace Lavalink.NET
{
	/// <summary>
	/// A Player of a Lavalink Node
	/// </summary>
	public class Player
	{
		/// <summary>
		/// Event which fires on Lavalink Player Updates
		/// </summary>
		public event EventHandler<PlayerUpdatePacket> PlayerUpdate;

		/// <summary>
		/// Event which fires on Lavalink TrackEndEvent
		/// </summary>
		public event EventHandler<PlayerEndEventPacket> End;

		/// <summary>
		/// Event which fires on Lavalink TrackExceptionEvent
		/// </summary>
		public event EventHandler<PlayerExceptionEventPacket> Exception;

		/// <summary>
		/// Event which fires on Lavalink TrackStuckEvent
		/// </summary>
		public event EventHandler<PlayerStuckEventPacket> Stuck; 
		
		/// <summary>
		/// The Lavalink Node of this Player
		/// </summary>
		public LavalinkNode Node { get; set; }
		
		/// <summary>
		/// The GuildID of this Playxer
		/// </summary>
		public long GuildID { get; }
		
		/// <summary>
		/// The last known position of this Player
		/// </summary>
		public long Position { get; set; }

		/// <summary>
		/// The VoiceServer Packet of this Player
		/// </summary>
		public VoiceServerUpdatePayload VoiceServer
			=> Node.VoiceServers.TryGetValue(GuildID, out var val) ? val : null;

		/// <summary>
		/// The VoiceState of this Player
		/// </summary>
		public VoiceState VoiceState
		{
			get
			{
				if (!Node.VoiceStates.TryGetValue(GuildID, out var sessionID)) return null;
				
				return new VoiceState
				{
					GuildID = GuildID.ToString(),
					UserID = Node.UserID.ToString(),
					SessionID = sessionID
				};
			}
		}

		/// <summary>
		/// Creates a new Player Instance
		/// </summary>
		/// <param name="node">The Node this Player was created on</param>
		/// <param name="guildID">The GuildID of this Player</param>
		public Player(LavalinkNode node, long guildID)
		{
			Node = node;
			GuildID = guildID;
		}

		/// <summary>
		/// Lets the Bot Join a Channel by sending an Packet to <see cref="LavalinkNode.DiscordSendFunction"/>
		/// </summary>
		/// <param name="channelID">The ID of the Channel to join</param>
		/// <param name="mute">if the bot should be muted</param>
		/// <param name="deaf">if the bot should be deafen</param>
		/// <returns>Task</returns>
		public Task JoinAsync(string channelID, bool mute = false, bool deaf = false)
			=> Node.DiscordSendFunction(GuildID, new UpdateVoiceStateDispatch
			{
				GuildID = GuildID.ToString(),
				ChannelID = channelID,
				SelfMute = mute,
				SelfDeaf = deaf
			});

		/// <summary>
		/// Lets the Bot Leave the Current Connected Channel by sending an Packet to <see cref="LavalinkNode.DiscordSendFunction"/>
		/// </summary>
		/// <returns>Task</returns>
		public Task LeaveAsync()
			=> Node.DiscordSendFunction(GuildID, new UpdateVoiceStateDispatch
			{
				GuildID = GuildID.ToString(),
				ChannelID = null,
				SelfMute = false,
				SelfDeaf = false
			});

		/// <summary>
		/// Starts to Play a Track on this Player
		/// </summary>
		/// <param name="track">The Track string to play</param>
		/// <param name="start">The Start time, defaults to 0</param>
		/// <param name="end">The End time, defaults to 0</param>
		/// <returns>Task</returns>
		public Task PlayAsync(string track, int? start = 0, int? end = 0)
			=> Node.SendAsync(new PlayPacket
			{
				OPCode = "play",
				GuildID = GuildID.ToString(),
				Track = track,
				StartTime = start.ToString(),
				EndTime = end.ToString()
			});
		
		/// <summary>
		/// Starts to Play a Track on this Player
		/// </summary>
		/// <param name="track">The Track to play</param>
		/// <param name="start">The Start time, defaults to 0</param>
		/// <param name="end">The End time, defaults to 0</param>
		/// <returns>Task</returns>
		public Task PlayAsync(Track track, int? start = 0, int? end = 0)
			=> Node.SendAsync(new PlayPacket
			{
				OPCode = "play",
				GuildID = GuildID.ToString(),
				Track = track.TrackString,
				StartTime = start.ToString(),
				EndTime = end.ToString()
			});

		/// <summary>
		/// Stops the Player
		/// </summary>
		/// <returns>Task</returns>
		public Task StopAsync()
			=> Node.SendAsync(new PlayerPacket()
			{
				OPCode = "stop",
				GuildID = GuildID.ToString()
			});

		/// <summary>
		/// Pauses this Player
		/// </summary>
		/// <returns>Task</returns>
		public Task PauseAsync()
			=> Node.SendAsync(new PausePacket
			{
				OPCode = "pause",
				GuildID = GuildID.ToString(),
				Pause = true
			});
		
		/// <summary>
		/// Resumes Playing on this Player
		/// </summary>
		/// <returns>Task</returns>
		public Task ResumeAsync()
			=> Node.SendAsync(new PausePacket
			{
				OPCode = "pause",
				GuildID = GuildID.ToString(),
				Pause = false
			});

		/// <summary>
		/// Seek to a position on this Player
		/// </summary>
		/// <param name="position">The position to seek to, in ms</param>
		/// <returns>Task</returns>
		public Task SeekAsync(long position)
			=> Node.SendAsync(new SeekPacket
			{
				OPCode = "seek",
				GuildID = GuildID.ToString(),
				Position = position
			});

		/// <summary>
		/// Set the Volume of this Player
		/// </summary>
		/// <param name="volume">The Volume to set the player to, Volume may range from 0 to 1000. 100 is default.</param>
		/// <returns>Task</returns>
		public Task SetVolumeAsync(int volume)
			=> Node.SendAsync(new VolumePacket
			{
				OPCode = "volume",
				GuildID = GuildID.ToString(),
				Volume = volume
			});

		/// <summary>
		/// 
		/// </summary>
		/// <returns>Task</returns>
		public Task DestroyAsync()
			=> Node.SendAsync(new PlayerPacket
			{
				OPCode = "destroy",
				GuildID = GuildID.ToString()
			});

		/// <summary>
		/// Moves this Player to another Lavalink Node
		/// </summary>
		/// <param name="node"></param>
		/// <returns></returns>
		/// <exception cref="Exception"></exception>
		public async Task MoveToAsync(LavalinkNode node)
		{
			if (node == Node) return;
			var voiceServer = VoiceServer;
			var voiceState = VoiceState;
			
			if (voiceServer == null || voiceState == null) throw new Exception("no voice state/server data to move");

			await DestroyAsync();
			await Task.WhenAll(node.VoiceServerUpdateAsync(voiceServer), node.VoiceStateUpdateAsync(voiceState));
			Node = node;
		}

		/// <summary>
		/// Sends an VoiceUpdate to Lavalink for this Player
		/// </summary>
		/// <param name="sessionID">The SessionID of this VoiceUpdate</param>
		/// <param name="event">The VoiceServer event of this VoiceUpdate</param>
		/// <returns>Task</returns>
		public Task VoiceUpdateAsync(string sessionID, VoiceServerUpdatePayload @event)
			=> Node.SendAsync(new VoiceUpdatePacket
			{
				OPCode = "voiceUpdate",
				GuildID = GuildID.ToString(),
				UpdateEvent = @event,
				SessionID = sessionID
			});

		internal void EmitPlayerEvent(string json, PlayerEventType type)
		{
			switch (type)
			{
				case PlayerEventType.TrackEndEvent:
					End?.Invoke(this, JsonConvert.DeserializeObject<PlayerEndEventPacket>(json));
					break;
				case PlayerEventType.TrackExceptionEvent:
					Exception?.Invoke(this, JsonConvert.DeserializeObject<PlayerExceptionEventPacket>(json));
					break;
				case PlayerEventType.TrackStuckEvent:
					Stuck?.Invoke(this, JsonConvert.DeserializeObject<PlayerStuckEventPacket>(json));
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(type), type, null);
			}
		}

		internal void EmitPlayerUpdate(PlayerUpdatePacket packet)
		{
			Position = packet.State.Position;
			PlayerUpdate?.Invoke(this, packet);
		}
	}
}