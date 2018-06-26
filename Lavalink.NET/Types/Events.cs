using System;
using System.Net.WebSockets;
using Newtonsoft.Json;

namespace Lavalink.NET.Types
{
	/// <summary>
	/// Argument for the Debug Event.
	/// </summary>
    public class DebugEventArgs : EventArgs
	{
		/// <summary>
		/// The string containing the actuall Message.
		/// </summary>
		public string Message { get; internal set; }

		/// <summary>
		/// Creates a new DebugEventArgs instance.
		/// </summary>
		/// <param name="input">The Debug Message.</param>
		public DebugEventArgs(string input) 
			=> Message = input;
	}

	/// <summary>
	/// Argument for the Message Event.
	/// </summary>
	public class MessageEventArgs : EventArgs
	{
		/// <summary>
		/// Parsed message from Lavalink.
		/// </summary>
		public dynamic Message { get; internal set; }

		/// <summary>
		/// Creates a new MessageEventArgs instance.
		/// </summary>
		/// <param name="input">The Message from Lavalink.</param>
		public MessageEventArgs(dynamic input)
			=> Message = input;
	}

	/// <summary>
	/// Argument for the Error Event.
	/// </summary>
	public class ErrorEventArgs : EventArgs
	{
		/// <summary>
		/// The Exeption this Client encountered.
		/// </summary>
		public Exception Error { get; internal set; }

		/// <summary>
		/// Creates a new ErrorEventArgs instance.
		/// </summary>
		/// <param name="input">The Exception the client encountered.</param>
		public ErrorEventArgs(Exception input)
			=> Error = input;
	}

	/// <summary>
	/// Argument for the CloseEvent.
	/// </summary>
	public class CloseEventArgs : EventArgs
	{
		/// <summary>
		/// The Websocket Close code.
		/// </summary>
		public WebSocketCloseStatus? Status { get; internal set; }
		/// <summary>
		/// The Reason of the Disconnect.
		/// </summary>
		public string Reason { get; internal set; }

		/// <summary>
		/// Creates a new CloseEventArgs instance.
		/// </summary>
		/// <param name="code">The Websocket Close code.</param>
		/// <param name="reason">The Reason of the Disconnect.</param>
		public CloseEventArgs(WebSocketCloseStatus? code, string reason)
		{
			Status = code;
			Reason = reason;
		}
	}

	/// <summary>
	/// Base TrackEventArgs class, all event args for TrackEnding events extend this class.
	/// </summary>
	public class TrackEventArgs : EventArgs
	{
		/// <summary>
		/// The OP code for this Lavalink event.
		/// </summary>
		[JsonProperty("op")]
		public string OP { get; set; }

		/// <summary>
		/// The track what was playing.
		/// </summary>
		[JsonProperty("track")]
		public string Track { get; set; }

		/// <summary>
		/// The Guild ID of the player.
		/// </summary>
		[JsonProperty("guildId")]
		public string GuildID { get; set; }

		/// <summary>
		/// The type of event.
		/// </summary>
		[JsonProperty("type")]
		public string Type { get; set; }
	}

	/// <summary>
	/// TrackArgs for the TrackEnd event.
	/// </summary>
	public class TrackEndEventArgs : TrackEventArgs
	{
		/// <summary>
		/// The reason why this Track ended.
		/// </summary>
		[JsonProperty("reason")]
		public string Reason { get; set; }
	}

	/// <summary>
	/// TrackArgs for the TrackExeption event.
	/// </summary>
	public class TrackExceptionEventArgs : TrackEventArgs
	{
		/// <summary>
		/// The error string what the Player encountered while trying to play a song.
		/// </summary>
		[JsonProperty("error")]
		public string Error { get; set; }
	}

	/// <summary>
	/// TrackArgs for the TrackStuck event.
	/// </summary>
	public class TrackStuckEventArgs : TrackEventArgs
	{
		/// <summary>
		/// The timestamp this Track got stuck at.
		/// </summary>
		[JsonProperty("thresholdMs")]
		public long ThresholdMS { get; set; }
	}

	/// <summary>
	/// ConnectionFailedArgs for the ConnectionFailedEvent.
	/// </summary>
	public class ConnectionFailedArgs : EventArgs
	{
		/// <summary>
		/// The Exeption that occoured on the Websocket Connection.
		/// </summary>
		public Exception Exception { get; set; }

		/// <summary>
		/// Creates a new ConnectionFailedArgs instance.
		/// </summary>
		/// <param name="error">The Exeption that occoured on the Websocket Connection</param>
		public ConnectionFailedArgs(Exception error) 
		{
			Exception = error;
		}
	}

	/// <summary>
	/// Memory class for the StatsEvent.
	/// </summary>
	public class MemoryInfo
	{
		/// <summary>
		/// Memory that can be reserved for Lavalink.
		/// </summary>
		[JsonProperty("reservable")]
		public ushort Reservable { get; set; }

		/// <summary>
		/// Memory that is used by Lavalink
		/// </summary>
		[JsonProperty("used")]
		public ushort Used { get; set; }

		/// <summary>
		/// Memory that is free.
		/// </summary>
		[JsonProperty("free")]
		public ushort Free { get; set; }

		/// <summary>
		/// Memory that is allocated.
		/// </summary>
		[JsonProperty("allocated")]
		public ushort Allocated { get; set; }
	}

	/// <summary>
	/// CPU class for the StatsEvent.
	/// </summary>
	public class CPUInfo
	{
		/// <summary>
		/// The cores that Lavalink can make use of.
		/// </summary>
		[JsonProperty("cores")]
		public int Cores { get; set; }

		/// <summary>
		/// The complete load of the System.
		/// </summary>
		[JsonProperty("systemLoad")]
		public float SystemLoad { get; set; }

		/// <summary>
		/// The load of Lavalink on the System.
		/// </summary>
		[JsonProperty("lavalinkLoad")]
		public float LavalinkLoad { get; set; }
	}

	/// <summary>
	/// The args
	/// </summary>
	public class StatsEventArgs : EventArgs
	{
		/// <summary>
		/// The current playing players for this Client.
		/// </summary>
		[JsonProperty("playingPlayers")]
		public int PlayingPlayers { get; set; }

		/// <summary>
		/// The amount of players for this client in Lavalink.
		/// </summary>
		[JsonProperty("players")]
		public int Players { get; set; }

		/// <summary>
		/// The uptime in ms.
		/// </summary>
		[JsonProperty("uptime")]
		public ushort Uptime { get; set; }

		/// <summary>
		/// The OPCode for this Lavalink Message.
		/// </summary>
		[JsonProperty("op")]
		public string OPCode { get; set; }

		/// <summary>
		/// Infos about the memory usage of Lavalink.
		/// </summary>
		[JsonProperty("memory")]
		public MemoryInfo Memory { get; set; }

		/// <summary>
		/// Infos about the CPU usage of Lavalink.
		/// </summary>
		[JsonProperty("cpu")]
		public CPUInfo CPU { get; set; }
	}

	/// <summary>
	/// The Event for emitting Debug messages.
	/// </summary>
	/// <param name="sender">the sender object.</param>
	/// <param name="e">The Arguments.</param>
	public delegate void DebugEvent(object sender, DebugEventArgs e);

	/// <summary>
	/// The Event for emitting Stats Events from Lavalink.
	/// </summary>
	/// <param name="sender">the sender object.</param>
	/// <param name="e">The Arguments.</param>
	public delegate void StatsEvent(object sender, StatsEventArgs e);

	/// <summary>
	/// The Event for emitting Messages from Lavalink.
	/// </summary>
	/// <param name="sender">the sender object.</param>
	/// <param name="e">The Arguments.</param>
	public delegate void MessageEvent(object sender, MessageEventArgs e);

	/// <summary>
	/// The Event for emitting Errors from the Client.
	/// </summary>
	/// <param name="sender">the sender object.</param>
	/// <param name="e">The Arguments.</param>
	public delegate void ErrorEvent(object sender, ErrorEventArgs e);

	/// <summary>
	/// The Event for emitting Close events from the Websocket of this Client.
	/// </summary>
	/// <param name="sender">the sender object.</param>
	/// <param name="e">The Arguments.</param>
	public delegate void CloseEvent(object sender, CloseEventArgs e);

	/// <summary>
	/// The Event for emitting ConnectionFailed errors from the websocket of this Client.
	/// </summary>
	/// <param name="sender">the sender object.</param>
	/// <param name="e">The Arguments.</param>
	public delegate void ConnectionFailedEvent(object sender, ConnectionFailedArgs e);

	/// <summary>
	/// The Event for emitting TrackEndEvents from Players.
	/// </summary>
	/// <param name="sender">the sender object.</param>
	/// <param name="e">The Arguments.</param>
	public delegate void TrackEndEvent(object sender, TrackEndEventArgs e);

	/// <summary>
	/// The Event for emitting TrackExceptionEvent from Players.
	/// </summary>
	/// <param name="sender">the sender object.</param>
	/// <param name="e">The Arguments.</param>
	public delegate void TrackExceptionEvent(object sender, TrackExceptionEventArgs e);

	/// <summary>
	/// The Event for emitting TrackStuckEvent from Players.
	/// </summary>
	/// <param name="sender">the sender object.</param>
	/// <param name="e">The Arguments.</param>
	public delegate void TrackStuckEvent(object sender, TrackStuckEventArgs e);
}