using Newtonsoft.Json;

namespace Lavalink.NET.Types
{
	/// <summary>
	/// Information about a Playlist
	/// </summary>
	public class PlaylistInfo
	{
		/// <summary>
		/// The Name of the Playlist.
		/// </summary>
		[JsonProperty("name")]
		public string Name { get; set; }
		
		/// <summary>
		/// The Selected Track of this Playlist (if any)
		/// </summary>
		[JsonProperty("selectTrack")]
		public int SelectedTrack { get; set; }
	}
}