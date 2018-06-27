
using Newtonsoft.Json;

namespace Lavalink.NET.Types
{
	/// <summary>
	/// The update packet for VoiceServerUpdates.
	/// </summary>
	public class VoiceServerUpdate
	{
		/// <summary>
		/// The GuildID of the update as ulong.
		/// </summary>
		[JsonIgnore]
		public ulong GuildID { get; set; }

		/// <summary>
		/// The GuildID of the update as string.
		/// </summary>
		[JsonProperty("guild_id")]
		public string GuildIDString => GuildID.ToString();

		/// <summary>
		/// The VoiceToken of this VoiceServerUpdate.
		/// </summary>
		[JsonProperty("token")]
		public string Token { get; set; }

		/// <summary>
		/// The Endpoint for this new VoiceServer
		/// </summary>
		[JsonProperty("endpoint")]
		public string Endpoint { get; set; }
	}

	/// <summary>
	/// THe Update packet for VoiceStateUpdates.
	/// </summary>
	public class VoiceStateUpdate
	{
		/// <summary>
		/// The GuildID of the update as ulong.
		/// </summary>
		[JsonIgnore]
		public ulong GuildID { get; set; }

		/// <summary>
		/// The GuildID of the update as string.
		/// </summary>
		[JsonProperty("guild_id")]
		public string GuildIDString => GuildID.ToString();

		/// <summary>
		/// The ChannelID of the update as ulong.
		/// </summary>
		[JsonIgnore]
		public ulong? ChannelID { get; set; }

		/// <summary>
		/// The ChannelID of the update as String.
		/// </summary>
		[JsonProperty("channel_id")]
		public string ChannelIDString => ChannelID?.ToString();

		/// <summary>
		/// The User ID of the update as ulong.
		/// </summary>
		[JsonIgnore]
		public ulong UserID { get; set; }

		/// <summary>
		/// The User ID of the update.
		/// </summary>
		[JsonProperty("user_id")]
		public string UserIDString => UserID.ToString();

		/// <summary>
		/// The SessionID of the updated member.
		/// </summary>
		[JsonProperty("session_id")]
		public string SessionID { get; set; }

		/// <summary>
		/// Determines if the updated member is deafened.
		/// </summary>
		[JsonProperty("deaf")]
		public bool Deaf { get; set; }

		/// <summary>
		/// Determines if the updated member is muted.
		/// </summary>
		[JsonProperty("mute")]
		public bool Mute { get; set; }

		/// <summary>
		/// Determines if the User is deafened by himself.
		/// </summary>
		[JsonProperty("self_deaf")]
		public bool SelfDeaf { get; set; }

		/// <summary>
		/// Determines if the User is muted by himself.
		/// </summary>
		[JsonProperty("self_mute")]
		public bool SelfMute { get; set; }

		/// <summary>
		/// Determines if the User is suppressed.
		/// </summary>
		[JsonProperty("suppress")]
		public bool Suppress { get; set; }
	}

	/// <summary>
	/// The base class that all PlayerPackets extend.
	/// </summary>
	public class PlayerPacket
	{
		/// <summary>
		/// The OPCode of this PlayerPacket
		/// </summary>
		[JsonProperty("op")]
		public string OPCode { get; set; }

		/// <summary>
		/// The GuildID of this PlayerPacket
		/// </summary>
		[JsonProperty("guildId")]
		public string GuildID { get; set; }
	}

	/// <summary>
	/// The VoiceUpdatePacket for the Player.
	/// </summary>
	public class VoiceUpdatePacket : PlayerPacket
	{
		/// <summary>
		/// The SessionID of this VoiceUpdatePacket
		/// </summary>
		[JsonProperty("sessionId")]
		public string SessionID { get; set; }

		/// <summary>
		/// The UpdateEvent of this VoiceUpdatePacket
		/// </summary>
		[JsonProperty("event")]
		public VoiceServerUpdate UpdateEvent { get; set; }
	}

	/// <summary>
	/// The PlayPacket for the Player.
	/// </summary>
	public class PlayPacket : PlayerPacket
	{
		/// <summary>
		/// The Track string of this PlayPacket.
		/// </summary>
		[JsonProperty("track")]
		public string Track { get; set; }

		/// <summary>
		/// The Startime of this PlayPacket.
		/// </summary>
		[JsonProperty("startTime")]
		public string StartTime { get; set; }

		/// <summary>
		/// The Endtime of this PlayPacket.
		/// </summary>
		[JsonProperty("endTime")]
		public string EndTime { get; set; }
	}

	/// <summary>
	/// The PausePacket for the Player.
	/// </summary>
	public class PausePacket : PlayerPacket
	{
		/// <summary>
		/// Boolean indicating if the Player should be paused or not.
		/// </summary>
		[JsonProperty("pause")]
		public bool Pause { get; set; }
	}

	/// <summary>
	/// The SeekPacket for the Player.
	/// </summary>
	public class SeekPacket : PlayerPacket
	{
		/// <summary>
		/// The Position the track should seek to.
		/// </summary>
		[JsonProperty("position")]
		public int Position { get; set; }
	}

	/// <summary>
	/// The VolumePacket for the Player.
	/// </summary>
	public class VolumePacket : PlayerPacket
	{
		/// <summary>
		/// The volume to change to.
		/// </summary>
		[JsonProperty("volume")]
		public int Volume { get; set; }
	}
}
