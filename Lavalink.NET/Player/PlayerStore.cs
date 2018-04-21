using System.Collections.Generic;

namespace Lavalink.NET.Player
{
    public class PlayerStore : Dictionary<string, Player>
    {
		private Client _client;

		public PlayerStore(Client client)
		{
			_client = client;
		}

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
