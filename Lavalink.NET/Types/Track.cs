namespace Lavalink.NET.Types
{
	public class TrackInfo
	{
		public string identifier { get; set; }
		public bool isSeekable { get; set; }
		public string author { get; set; }
		public int length { get; set; }
		public bool isStream { get; set; }
		public int position { get; set; }
		public string title { get; set; }
		public string uri { get; set; }
	}

    public class Track
    {
		public string track { get; }
		public TrackInfo info { get; }
    }
}
