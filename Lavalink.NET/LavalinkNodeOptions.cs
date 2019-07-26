using System.Collections.Generic;

namespace Lavalink.NET
{
	/// <summary>
	/// Options of a Lavalink Node.
	/// </summary>
	public class LavalinkNodeOptions
	{
		/// <summary>
		/// Host for the Rest API, includes protocol, hostname and port.
		/// </summary>
		public string HostRest { get; set; }

		/// <summary>
		/// Host for the Websocket API, includes protocol, hostname and port.
		/// </summary>
		public string HostWS { get; set; }

		/// <summary>
		/// Password for your lavalink node.
		/// </summary>
		public string Password { get; set; }

		/// <summary>
		/// The UserID of your Bot User.
		/// </summary>
		public long UserID { get; set; }

		/// <summary>
		/// The ShardCount of your Bot.
		/// </summary>
		public int ShardCount { get; set; } = 1;
		
		/// <summary>
		/// Tags for this Lavalink Node (useful for region, id etc.)
		/// </summary>
		public IEnumerable<string> Tags { get; set; }
	}
}