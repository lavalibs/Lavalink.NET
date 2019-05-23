using Newtonsoft.Json;

namespace Lavalink.NET.Types
{
	/// <summary>
	/// Information about the CPU Usage on the Lavalink Host
	/// </summary>
	public class LavalinkCPU
	{
		/// <summary>
		/// The Amount of Cores the Host of the Lavalink Server has
		/// </summary>
		[JsonProperty("cores")]
		public int Cores { get; set; }
		
		/// <summary>
		/// The System load of the Host of the Lavalink Server
		/// </summary>
		[JsonProperty("systemLoad")]
		public double SystemLoad { get; set; }
		
		/// <summary>
		/// The Lavalink load it has on the Host
		/// </summary>
		[JsonProperty("lavalinkLoad")]
		public double LavalinkLoad { get; set; }
	}
}