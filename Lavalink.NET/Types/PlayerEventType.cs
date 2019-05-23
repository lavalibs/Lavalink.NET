namespace Lavalink.NET.Types
{
	/// <summary>
	/// Types for Player Events.
	/// </summary>
	public enum PlayerEventType
	{
		/// <summary>
		/// Track Ended
		/// </summary>
		TrackEndEvent,
		
		/// <summary>
		/// Track encountered Exception
		/// </summary>
		TrackExceptionEvent,
		
		/// <summary>
		/// Track Stuck, you might want to play a new Track at this point
		/// </summary>
		TrackStuckEvent
	}
}