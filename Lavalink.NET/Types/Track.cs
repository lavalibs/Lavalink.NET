using Newtonsoft.Json;

namespace Lavalink.NET.Types
{
	public class TrackInfo
	{
		[JsonProperty("identifier")]
		public string Identifier { get; set; }
		[JsonProperty("isSeekable")]
		public bool Seekable { get; set; }
		[JsonProperty("author")]
		public string Author { get; set; }
		[JsonProperty("length")]
		public int Length { get; set; }
		[JsonProperty("isStream")]
		public bool Stream { get; set; }
		[JsonProperty("position")]
		public int Position { get; set; }
		[JsonProperty("title")]
		public string Title { get; set; }
		[JsonProperty("uri")]
		public string URL { get; set; }
	}

    public class Track
    {
		[JsonProperty("track")]
		public string TrackString { get; set; }
		[JsonProperty("info")]
		public TrackInfo TrackInfo { get; set; }
    }
}
