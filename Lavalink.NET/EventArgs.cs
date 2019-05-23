using System;
using Lavalink.NET.Types;
using Newtonsoft.Json.Linq;

namespace Lavalink.NET
{
	/// <summary>
	/// EventArgs for Message Event
	/// </summary>
	public class MessageEventArgs : EventArgs
	{
		/// <summary>
		/// The Node where the Event occured
		/// </summary>
		public LavalinkNode Node { get; }
		
		/// <summary>
		/// The JObject of the Event
		/// </summary>
		public JObject Event { get; }

		/// <summary>
		/// Creates a new MessageEventArgs instance
		/// </summary>
		/// <param name="node">The Node where the Event occured</param>
		/// <param name="message">The JObject of the Event</param>
		public MessageEventArgs(LavalinkNode node, JObject message)
		{
			Node = node;
			Event = message;
		}
	}

	/// <summary>
	/// EventArgs for Stats Event
	/// </summary>
	public class StatsEventArgs : EventArgs
	{
		/// <summary>
		/// The Node where the Event occured
		/// </summary>
		public LavalinkNode Node { get; }
		
		/// <summary>
		/// The new Stats of the Node
		/// </summary>
		public LavalinkStats Stats { get; }

		/// <summary>
		/// Creates a new StatsEventArgs instance
		/// </summary>
		/// <param name="node">The Node where the Event occured</param>
		/// <param name="stats">The new Stats of the Node</param>
		public StatsEventArgs(LavalinkNode node, LavalinkStats stats)
		{
			Node = node;
			Stats = stats;
		}
	}

	/// <summary>
	/// 
	/// </summary>
	public class LogEventArgs : EventArgs
	{
		/// <summary>
		/// The Node where the Event occured, if this is null it comes from the Cluster
		/// </summary>
		public LavalinkNode Node { get; }
		
		/// <summary>
		/// The LogLevel of this Event
		/// </summary>
		public LogLevel LogLevel { get; }
		
		/// <summary>
		/// The Message of this Log
		/// </summary>
		public string Message { get; }

		/// <summary>
		/// Creates a new StatsEventArgs instance 
		/// </summary>
		/// <param name="node">The Node where the Event occured, if this is null it comes from the Cluster</param>
		/// <param name="logLevel">The LogLevel of this Event</param>
		/// <param name="message">The Message of this Log</param>
		public LogEventArgs(LavalinkNode node, LogLevel logLevel, string message)
		{
			Node = node;
			LogLevel = logLevel;
			Message = message;
		}
	}
}