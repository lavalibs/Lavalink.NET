namespace Lavalink.NET.Types
{
	/// <summary>
	/// The Player Status
	/// </summary>
	public enum PlayerStatus
	{
		/// <summary>
		/// The Player was instantiated
		/// </summary>
		INSTANTIATED,
		
		/// <summary>
		/// The Player is Playing
		/// </summary>
		PLAYING,
		
		/// <summary>
		/// The Player is Paused
		/// </summary>
		PAUSED,
		
		/// <summary>
		/// The Player ended playing a Track
		/// </summary>
		ENDED,
		
		/// <summary>
		/// The Player errored
		/// </summary>
		ERRORED,
		
		/// <summary>
		/// The Player got Stuck
		/// </summary>
		STUCK,
		
		/// <summary>
		/// The Player Status is Unknown
		/// </summary>
		UNKNOWN,
	}
}