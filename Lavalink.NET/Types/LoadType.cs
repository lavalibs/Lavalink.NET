namespace Lavalink.NET.Types
{
	/// <summary>
	/// Additionally, in every /loadtracks response, a loadType property is returned which can be used to judge the response from Lavalink properly.
	/// </summary>
	public enum LoadType
	{
		/// <summary>
		/// Returned when a single track is loaded.
		/// </summary>
		TRACK_LOADED,
		
		/// <summary>
		/// Returned when a playlist is loaded.
		/// </summary>
		PLAYLIST_LOADED,
		
		/// <summary>
		/// Returned when a search result is made (i.e ytsearch: some song).
		/// </summary>
		SEARCH_RESULT,
		
		/// <summary>
		/// Returned if no matches/sources could be found for a given identifier.
		/// </summary>
		NO_MATCHES,
		
		/// <summary>
		/// Returned if Lavaplayer failed to load something for some reason.
		/// </summary>
		LOAD_FAILED
	}
}