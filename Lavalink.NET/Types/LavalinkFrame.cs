using Newtonsoft.Json;

namespace Lavalink.NET.Types
{
	/// <summary>
	/// Information about the Frames Sent/Nulled/Deficit of the Lavalink Server
	/// </summary>
	public class LavalinkFrame
	{
		/// <summary>
		/// Average Frames sent per Minute
		/// </summary>
		[JsonProperty("sent")]
		public int Sent { get; set; }
		
		/// <summary>
		/// Average Frames nulled per Minute
		/// </summary>
		[JsonProperty("nulled")]
		public int Nulled { get; set; }
		
		/// <summary>
		/// Average Frames deficit per Minute
		/// </summary>
		[JsonProperty("deficit")]
		public int Deficit { get; set; }
	}
}