using System;

namespace Lavalink.NET.Types
{
	public class ClientEventArgs : EventArgs
	{
		public dynamic Message { get; }

		public ClientEventArgs(dynamic input)
			=> Message = input;
	}

	public delegate void Message(object sender, ClientEventArgs e);
}
