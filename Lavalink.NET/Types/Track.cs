using Newtonsoft.Json;

namespace Lavalink.NET.Types
{
	/// <summary>
	/// The actuall info about the Track.
	/// </summary>
	public class TrackInfo
	{
		/// <summary>
		/// The identifier of this Track.
		/// </summary>
		[JsonProperty("identifier")]
		public string Identifier { get; set; }

		/// <summary>
		/// if this track is seekable.
		/// </summary>
		[JsonProperty("isSeekable")]
		public bool Seekable { get; set; }

		/// <summary>
		/// The author of this channel.
		/// </summary>
		[JsonProperty("author")]
		public string Author { get; set; }

		/// <summary>
		/// The Length of this Track.
		/// </summary>
		[JsonProperty("length")]
		public int Length { get; set; }

		/// <summary>
		/// A boolean to indicate if this Track is a stream.
		/// </summary>
		[JsonProperty("isStream")]
		public bool Stream { get; set; }

		/// <summary>
		/// The Position of this Track.
		/// </summary>
		[JsonProperty("position")]
		public int Position { get; set; }

		/// <summary>
		/// The Title of this Track.
		/// </summary>
		[JsonProperty("title")]
		public string Title { get; set; }

		/// <summary>
		/// The URL of this Track.
		/// </summary>
		[JsonProperty("uri")]
		public string URL { get; set; }
	}

	/// <summary>
	/// The Track with all the needed information from Lavalink.
	/// </summary>
    public class Track
    {
		/// <summary>
		/// The TrackString that lavalink sent/needs to play Songs.
		/// </summary>
		[JsonProperty("track")]
		public string TrackString { get; set; }

		/// <summary>
		/// The actuall info about this Track.
		/// </summary>
		[JsonProperty("info")]
		public TrackInfo TrackInfo { get; set; }
    }
}
