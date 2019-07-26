using System;
using System.Collections.Generic;
using System.Net;
using WS.NET;

namespace Lavalink.NET
{
	/// <summary>
	/// Websocket Options for the Websocket Client
	/// </summary>
	public class WebsocketOptions : IWebSocketOptions
	{
		/// <summary>
		/// The Headers of this Websocket Connection
		/// </summary>
		public IEnumerable<Tuple<string, string>> Headers { get; set; }
		
		/// <summary>
		/// The Proxy of this Websocket Connection (if any)
		/// </summary>
		public IWebProxy Proxy { get; set; }
	}
}