using Newtonsoft.Json;

namespace Lavalink.NET.Types
{
	/// <summary>
	/// Memory information about the Host of the Lavalink Server
	/// </summary>
	public class LavalinkMemory
	{
		/// <summary>
		/// Free Memory on the Host of the Lavalink Server
		/// </summary>
		[JsonProperty("free")]
		public long Free { get; set; }
		
		/// <summary>
		/// Used Memory on the Host of the Lavalink Server
		/// </summary>
		[JsonProperty("used")]
		public long Used { get; set; }
		
		/// <summary>
		/// Allocated Memory on the Host of the Lavalink Server
		/// </summary>
		[JsonProperty("allocated")]
		public long Allocated { get; set; }
		
		/// <summary>
		/// Reserved Memory on the Host of the Lavalink Server
		/// </summary>
		[JsonProperty("reservable")]
		public long Reservable { get; set; }
	}
}