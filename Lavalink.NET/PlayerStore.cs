using System.Collections.Concurrent;

namespace Lavalink.NET
{
	/// <summary>
	/// The Player Store for a Lavalink Node	
	/// </summary>
	public class PlayerStore : ConcurrentDictionary<long, Player>
	{
		/// <summary>
		/// The LavalinkNode this PlayerStore is for
		/// </summary>
		private LavalinkNode Node { get; }
		
		/// <summary>
		/// Creates a new PlayerStore instance
		/// </summary>
		/// <param name="node">The LavalinkNode this PlayerStore is for</param>
		public PlayerStore(LavalinkNode node) 
			=> Node = node;

		/// <summary>
		/// Gets or Creates a Player
		/// </summary>
		/// <param name="key">The GuildID of this player</param>
		/// <returns>Player</returns>
		public Player Get(long key)
		{
			if (TryGetValue(key, out var player)) return player;
			player = new Player(Node, key);
			this[key] = player;
			return player;
		}
	}
}