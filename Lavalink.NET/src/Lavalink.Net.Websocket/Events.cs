using System;
using System.Net.WebSockets;

namespace Lavalink.NET.Websocket
{
    public class DebugEventArgs : EventArgs
	{
		public string Message { get; }

		public DebugEventArgs(string input) 
			=> Message = input;
	}

	public class MessageEventArgs : EventArgs
	{
		public string Message { get; }

		public MessageEventArgs(string input)
			=> Message = input;
	}

	public class ErrorEventArgs : EventArgs
	{
		public string Message { get; }

		public ErrorEventArgs(string input)
			=> Message = input;
	}

	public class CloseEventArgs : EventArgs
	{
		public WebSocketCloseStatus? Status { get; }
		public string Reason { get; }

		public CloseEventArgs(WebSocketCloseStatus? code, string reason)
		{
			Status = code;
			Reason = reason;
		}
	}

	public delegate void DebugEventHandler(object sender, DebugEventArgs e);
	public delegate void MessageEventHandler(object sender, MessageEventArgs e);
	public delegate void ErrorEventHandler(object sender, ErrorEventArgs e);
	public delegate void CloseEventHandler(object sender, CloseEventArgs e);
}