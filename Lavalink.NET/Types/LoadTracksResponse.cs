using Newtonsoft.Json;

namespace Lavalink.NET.Types
{
	/// <summary>
	///  Response from the /loadtracks endpoint
	/// </summary>
	public class LoadTracksResponse
	{
		/// <summary>
		/// The LoadType of this Response
		/// </summary>
		[JsonProperty("loadType")]
		public LoadType LoadType { get; set; }
		
		/// <summary>
		/// The Playlist Information if this Load is from a Playlist
		/// </summary>
		[JsonProperty("playlistInfo")]
		public PlaylistInfo PlaylistInfo { get; set; }
		
		/// <summary>
		/// The Tracks Loaded, if any
		/// </summary>
		[JsonProperty("tracks")]
		public Track[] Tracks { get; set; }
	}
}