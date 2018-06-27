using System;
using Newtonsoft.Json;

namespace Lavalink.NET.Types
{
	/// <summary>
	/// Base TrackEventArgs class, all event args for TrackEnding events extend this class.
	/// </summary>
	public class TrackEvent
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
	public class TrackEndEvent : TrackEvent
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
	public class TrackExceptionEvent : TrackEvent
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
	public class TrackStuckEvent : TrackEvent
	{
		/// <summary>
		/// The timestamp this Track got stuck at.
		/// </summary>
		[JsonProperty("thresholdMs")]
		public long ThresholdMS { get; set; }
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
	/// The Stats class for the Lavalink Stats event
	/// </summary>
	public class Stats
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
}