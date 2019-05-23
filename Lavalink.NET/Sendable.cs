using System;

namespace Lavalink.NET
{
	/// <summary>
	/// Holds Data which should be send over WebSocket
	/// </summary>
	public class Sendable
	{
		/// <summary>
		/// Event called on Success
		/// </summary>
		public event EventHandler Success;

		/// <summary>
		/// Event called on Error
		/// </summary>
		public event EventHandler<Exception> Error;

		/// <summary>
		/// the Packet to send
		/// </summary>
		public object Packet { get; }

		/// <summary>
		/// Creates a new instance of Sendable
		/// </summary>
		/// <param name="packet"></param>
		public Sendable(object packet)
		{
			Packet = packet;
		}

		/// <summary>
		/// Emits either Success or Error event
		/// </summary>
		/// <param name="exception">optional, the exception encountered</param>
		/// <exception cref="ArgumentOutOfRangeException">If the type is out of range of the enum</exception>
		public void Emit(Exception exception = null)
		{
			if (exception == null) Success?.Invoke(this, null);
			else Error?.Invoke(this, exception);
		}
	}
}