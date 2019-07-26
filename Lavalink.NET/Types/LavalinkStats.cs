using Newtonsoft.Json;

namespace Lavalink.NET.Types
{
	/// <summary>
	/// Stats about a Lavalink Node
	/// </summary>
	public class LavalinkStats
	{
		/// <summary>
		/// Players on this Node
		/// </summary>
		[JsonProperty("players")]
		public int Players { get; set; }
		
		/// <summary>
		/// Playing Players on this Node
		/// </summary>
		[JsonProperty("playingPlayers")]
		public int PlayingPlayers { get; set; }
		
		/// <summary>
		/// the Uptime of this Node
		/// </summary>
		[JsonProperty("uptime")]
		public long Uptime { get; set; }
		
		/// <summary>
		/// Information about the Memory on the Host of this node
		/// </summary>
		[JsonProperty("memory")]
		public LavalinkMemory Memory { get; set; }
		
		/// <summary>
		/// Information about the CPU usage on the Host of this node
		/// </summary>
		[JsonProperty("cpu")]
		public LavalinkCPU CPU { get; set; }
		
		/// <summary>
		/// Information about the Frames Sent/Nulled/Deficit of the Node
		/// </summary>
		[JsonProperty("frameStats")]
		public LavalinkFrame FrameStats { get; set; }
	}
}