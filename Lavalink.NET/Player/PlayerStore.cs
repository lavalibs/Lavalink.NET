using System.Collections.Generic;

namespace Lavalink.NET.Player
{
	/// <summary>
	/// Store of all Players
	/// </summary>
    public class PlayerStore : Dictionary<string, Player>
    {
		private Client _client;

		/// <summary>
		/// Constructor of PlayerStore
		/// </summary>
		/// <param name="client"> The Client to instanciate this Store for </param>
		public PlayerStore(Client client)
		{
			_client = client;
		}

		/// <summary>
		/// Method to Get Player and create and return new one if no Player is there.
		/// </summary>
		/// <param name="key"> GuildID of the Player </param>
		/// <returns> Player instance </returns>
		public Player GetPlayer(string key)
		{
			if (TryGetValue(key, out Player player))
			{
				return player;
			}
			else
			{
				player = new Player(_client, key);
				Add(key, player);
				return player;
			}
		}
    }
}
