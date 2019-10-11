using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Lavalink.NET.Types;
using Spectacles.NET.Types;

namespace Lavalink.NET
{
	/// <summary>
	/// A LavalinkCluster is a Cluster of Lavalink Nodes.
	/// </summary>
	public abstract class LavalinkCluster
	{
		/// <summary>
		/// Event which fires when we receive an Event from a Lavalink Node of this Cluster
		/// </summary>
		public event EventHandler<MessageEventArgs> Event;
		
		/// <summary>
		/// Event Which fires on Stats Update from a Lavalink Node of this Cluster
		/// </summary>
		public event EventHandler<StatsEventArgs> Stats;

		/// <summary>
		/// Event which fires on Logs from a Lavalink Node of this Cluster
		/// </summary>
		public event EventHandler<LogEventArgs> Logs;
		
		/// <summary>
		/// The Lavalink Nodes of this Cluster
		/// </summary>
		public List<LavalinkNode> LavalinkNodes { get; } = new List<LavalinkNode>();
		
		/// <summary>
		/// The HttpClient of this Lavalink Cluster
		/// </summary>
		public HttpClient HttpClient { get; }

		/// <summary>
		/// Creates a Lavalink Cluster from LavalinkNodeOptions
		/// </summary>
		/// <param name="nodeOptions">The Node Options to create this Cluster with</param>
		protected LavalinkCluster(IEnumerable<LavalinkNodeOptions> nodeOptions)
		{
			foreach (var options in nodeOptions) LavalinkNodes.Add(new LavalinkNode(this, options));
			HttpClient = new HttpClient();
		}

		/// <summary>
		/// Creates a Lavalink Cluster from LavalinkNodeOptions and an existing HttpClient
		/// </summary>
		/// <param name="nodeOptions">The Node Options to create this Cluster with</param>
		/// <param name="httpClient">The HttpClient to use for this Cluster</param>
		protected LavalinkCluster(IEnumerable<LavalinkNodeOptions> nodeOptions, HttpClient httpClient)
		{
			foreach (var option in nodeOptions) LavalinkNodes.Add(new LavalinkNode(this, option));
			HttpClient = httpClient;
		}

		/// <summary>
		/// Connects all Lavalink Nodes in this Cluster
		/// </summary>
		/// <returns>Task</returns>
		public Task ConnectAsync()
			=> Task.WhenAll(LavalinkNodes.Select(node => node.ConnectAsync()));

		/// <summary>
		/// Loads Track(s) from the least used Lavalink Node by query
		/// </summary>
		/// <param name="query">The query to search by</param>
		/// <returns>Task resolving with a LoadTracksResponse</returns>
		public Task<LoadTracksResponse> LoadTrackAsync(string query)
		{
			var list = new List<LavalinkNode>(LavalinkNodes);
			list.Sort(Util.CompareCPU);
			return list.ElementAt(0).LoadTracksAsync(query);
		}

		/// <summary>
		/// Sorts the LavalinkNodes by CPU usage &amp; provided Filter
		/// </summary>
		/// <param name="guildID">The GuildID to sort for</param>
		/// <returns>List of Nodes</returns>
		public List<LavalinkNode> Sort(long guildID)
		{
			var list = new List<LavalinkNode>(LavalinkNodes);
			var enumerable = list.Where(node => node.Connected && Filter(node, guildID));
			list = enumerable.ToList();
			list.Sort(Util.CompareCPU);
			return list;
		}
		
		/// <summary>
		/// Gets the Node corresponding for a GuildID, takes the <see cref="Filter"/> into account
		/// </summary>
		/// <param name="guildID">The GuildID to get the Node for</param>
		/// <returns>LavalinkNode</returns>
		/// <exception cref="Exception">thrown if no node could be found because of the filter</exception>
		public LavalinkNode GetNode(long guildID)
		{
			var node = LavalinkNodes.Find(lavalinkNode => lavalinkNode.Players.ContainsKey(guildID)) ?? Sort(guildID).Find(lavalinkNode => Filter(lavalinkNode, guildID));
			if (node != null) return node;
			throw new Exception("unable to find appropriate node; please check your filter");
		}

		/// <summary>
		/// Checks if this Cluster contains a Player, by GuildID
		/// </summary>
		/// <param name="guildID">The GuildID to check for</param>
		/// <returns>bool</returns>
		public bool HasPlayer(long guildID)
			=> LavalinkNodes.Any(node => node.Players.ContainsKey(guildID));

		/// <summary>
		/// Gets a Player from this Cluster by GuildID
		/// </summary>
		/// <param name="guildID">The GuildID to get by</param>
		/// <returns>Player</returns>
		public Player GetPlayer(long guildID)
			=> GetNode(guildID).Players.Get(guildID);

		/// <summary>
		/// Sends the VoiceStateUpdate to the corresponding Lavalink Node
		/// </summary>
		/// <param name="state">The updated VoiceState</param>
		/// <returns>Task</returns>
		public Task VoiceStateUpdateAsync(VoiceState state)
			=> GetNode(long.Parse(state.GuildId)).VoiceStateUpdateAsync(state);

		/// <summary>
		/// Sends the VoiceServerUpdate to the corresponding Lavalink Node
		/// </summary>
		/// <param name="server">The updated VoiceServer payload</param>
		/// <returns>Task</returns>
		public Task VoiceServerUpdateAsync(VoiceServerUpdatePayload server)
			=> GetNode(long.Parse(server.GuildId)).VoiceServerUpdateAsync(server);

		internal void EmitEvent(EventType type, object data)
		{
			switch (type)
			{
				case EventType.EVENT:
					Event?.Invoke(this, (MessageEventArgs) data);
					break;
				case EventType.STATS:
					Stats?.Invoke(this, (StatsEventArgs) data);
					break;
				case EventType.LOGS:
					Logs?.Invoke(this, (LogEventArgs) data);
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(type), type, null);
			}
		}
		
		/// <summary>
		/// Abstract SendAsync method that should forward VoiceStateDispatches to the Discord API
		/// </summary>
		/// <param name="guildID"></param>
		/// <param name="packet"></param>
		/// <returns></returns>
		public abstract Task SendAsync(long guildID, UpdateVoiceStateDispatch packet);
		
		/// <summary>
		/// Abstract Filter method that should be used to determine if a Node is appropriate for a Player
		/// </summary>
		/// <param name="node">The Node to test</param>
		/// <param name="guildID">The GuildID to test on this Node</param>
		/// <returns>bool</returns>
		protected abstract bool Filter(LavalinkNode node, long guildID);
	}
}