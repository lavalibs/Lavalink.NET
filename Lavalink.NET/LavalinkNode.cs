using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.WebSockets;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Lavalink.NET.Extensions;
using Lavalink.NET.Types;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Spectacles.NET.Types;
using WS.NET;

namespace Lavalink.NET
{
	/// <summary>
	/// A Lavalink Node
	/// </summary>
	public class LavalinkNode
	{
		/// <summary>
		/// Event which fires when we receive an Event from the Lavalink Node
		/// </summary>
		public event EventHandler<MessageEventArgs> Event;
		
		/// <summary>
		/// Event Which fires on Stats Update from the Lavalink Node
		/// </summary>
		public event EventHandler<StatsEventArgs> Stats;

		/// <summary>
		/// Event which fires on Logs for this Lavalink Node
		/// </summary>
		public event EventHandler<LogEventArgs> Logs;

		/// <summary>
		/// The Store for all VoiceStates.
		/// </summary>
		public Dictionary<long, string> VoiceStates { get; } = new Dictionary<long, string>();

		/// <summary>
		/// The Store for this nodes VoiceServers.
		/// </summary>
		public Dictionary<long, VoiceServerUpdatePayload> VoiceServers { get; } = new Dictionary<long, VoiceServerUpdatePayload>();

		/// <summary>
		/// The Store for this nodes Players
		/// </summary>
		public PlayerStore Players { get; }
		
		/// <summary>
		/// Latest Stats of this Lavalink Node
		/// </summary>
		public LavalinkStats LavalinkStats { get; private set; }

		/// <summary>
		/// The Function which forwards UpdateVoiceStateDispatch to the Discord WebSocket API
		/// </summary>
		public Func<long, UpdateVoiceStateDispatch, Task> DiscordSendFunction 
			=> Cluster != null ? Cluster.SendAsync : _discordSendFunction;

		/// <summary>
		/// The Tags of this Node
		/// </summary>
		public IEnumerable<string> Tags
			=> Options.Tags;

		/// <summary>
		/// The UserID of the Account of this Lavalink Connections
		/// </summary>
		public long UserID
			=> Options.UserID;

		/// <summary>
		/// The HTTPClient to use
		/// </summary>
		public HttpClient HttpClient
			=> Cluster?.HttpClient ?? _http;
		
		/// <summary>
		/// If this Node is Connected
		/// </summary>
		public bool Connected
			=> WebSocketClient?.Status == WebSocketState.Open;
		
		/// <summary>
		/// The ResumeKey of this Node, if any
		/// </summary>
		public string ResumeKey
		{
			get => Options.ResumeKey ?? _resumeKey;
			set => _resumeKey = value;
		}
		
		/// <summary>
		/// the WebSocketClient of this Node
		/// </summary>
		private WebSocketClient WebSocketClient { get; set; }

		/// <summary>
		/// The Options of this Node
		/// </summary>
		private LavalinkNodeOptions Options { get; }

		/// <summary>
		/// The Cluster which instantiated this Node, if any 
		/// </summary>
		private LavalinkCluster Cluster { get; }

		/// <summary>
		/// The Reconnect Delay of this Node, if any
		/// </summary>
		private int ReconnectDelay { get; set; }
		
		/// <summary>
		/// Queue for request done while Node WebSocket isn't connected
		/// </summary>
		private ConcurrentQueue<Sendable> Queue { get; } = new ConcurrentQueue<Sendable>();
		
		/// <summary>
		/// The HttpClient of this Node if no Cluster is used
		/// </summary>
		private readonly HttpClient _http;

		/// <summary>
		/// The DiscordSendFunction of this Node if no Cluster is used
		/// </summary>
		private readonly Func<long, UpdateVoiceStateDispatch, Task> _discordSendFunction;

		/// <summary>
		/// The created Resume Key if not provided in <see cref="LavalinkNodeOptions"/>
		/// </summary>
		private string _resumeKey;

		/// <summary>
		/// Creates a new LavalinkNode instance without a Cluster
		/// </summary>
		/// <param name="options">The options of this Node</param>
		/// <param name="discordSendFunction">The DiscordSendFunction for the OP4 Packets</param>
		/// <param name="client">Optional, the HttpClient to use for Rest Requests</param>
		public LavalinkNode(LavalinkNodeOptions options, Func<long, UpdateVoiceStateDispatch, Task> discordSendFunction, HttpClient client = null)
		{
			Options = options;
			ResumeKey = options.ResumeKey;
			Players = new PlayerStore(this);
			_discordSendFunction = discordSendFunction;
			_http = client ?? new HttpClient();
		}

		/// <summary>
		/// Creates a new LavalinkNode instance from a Cluster
		/// </summary>
		/// <param name="cluster">The LavalinkCluster which this Node is a part of</param>
		/// <param name="options">The options of this Node</param>
		public LavalinkNode(LavalinkCluster cluster, LavalinkNodeOptions options)
		{
			Cluster = cluster;
			Event += (sender, data) => Cluster.EmitEvent(EventType.EVENT, data);
			Stats += (sender, data) => Cluster.EmitEvent(EventType.STATS, data);
			Logs += (sender, data) => Cluster.EmitEvent(EventType.LOGS, data);
			
			Players = new PlayerStore(this);
			Options = options;
			ResumeKey = options.ResumeKey;
		}

		/// <summary>
		/// Connects this LavalinkNode via WebSocket to the Lavalink Server.
		/// </summary>
		/// <returns>Task</returns>
		public async Task ConnectAsync()
		{
			if (WebSocketClient != null)
			{
				WebSocketClient.Open -= OnOpen;
				WebSocketClient.Message -= OnMessage;
				WebSocketClient.Error -= OnError;
				WebSocketClient.Close -= OnClose;
				WebSocketClient.Dispose();
			}
			
			var headers = new List<Tuple<string, string>>
			{
				new Tuple<string, string>("Authorization", Options.Password),
				new Tuple<string, string>("Num-Shards", Options.ShardCount.ToString()),
				new Tuple<string, string>("User-Id", Options.UserID.ToString())
			};
			
			if (ResumeKey != null) headers.Add(new Tuple<string, string>("Resume-Key", ResumeKey));
			
			WebSocketClient = new WebSocketClient(Options.HostWS, new WebsocketOptions
			{
				Headers = headers
			});

			WebSocketClient.Open += OnOpen;
			WebSocketClient.Message += OnMessage;
			WebSocketClient.Error += OnError;
			WebSocketClient.Close += OnClose;

			try
			{ 
				await WebSocketClient.ConnectAsync();
				ReconnectDelay = 0;
			}
			catch (Exception e)
			{
				if (ReconnectDelay == 0)
				{
					ReconnectDelay = 1000;
				}
				else
				{
					ReconnectDelay *= 2;	
				}
				_log(LogLevel.ERROR, $"Websocket connection errored with {e.Message}, retrying in {TimeSpan.FromMilliseconds(ReconnectDelay).TotalSeconds} seconds...");
				await Task.Delay(ReconnectDelay);
				await ConnectAsync();
			}
		}

		/// <summary>
		/// Sends an Packet to the Lavalink Websocket if Connected, otherwise Queues it up
		/// </summary>
		/// <param name="packet">The packet to send</param>
		/// <returns>Task</returns>
		public async Task SendAsync(object packet)
		{
			if (Connected)
				try
				{
					await WebSocketClient.SendAsync(JsonConvert.SerializeObject(packet));
				}
				catch (ExternalException)
				{
					await QueuePacketUp(packet);
				}
			else
				await QueuePacketUp(packet);
		}

		/// <summary>
		/// Queues a Package up and returns a Task
		/// </summary>
		/// <param name="packet"></param>
		/// <returns></returns>
		private Task QueuePacketUp(object packet)
		{
			var tcs = new TaskCompletionSource<bool>();
			var sendable = new Sendable(packet);
			sendable.Success += (sender, args) => tcs.SetResult(true);
			sendable.Error += (sender, e) => tcs.SetException(e);
			Queue.Enqueue(sendable);
			return tcs.Task;
		}

		/// <summary>
		/// Loads Track(s) from Lavalink by query
		/// </summary>
		/// <param name="query">The query to search by</param>
		/// <returns>Task resolving with a LoadTracksResponse</returns>
		public async Task<LoadTracksResponse> LoadTracksAsync(string query)
		{
			var request = new HttpRequestMessage
			{
				Method = HttpMethod.Get,
				RequestUri = new Uri($"{Options.HostRest}/loadtracks?identifier={Uri.EscapeDataString(query)}")
			};
			request.Headers.Add("Authorization", Options.Password);
			request.Headers.Add("Accept", "application/json");

			var res = await HttpClient.SendAndConfirmAsync(request);
			return JsonConvert.DeserializeObject<LoadTracksResponse>(await res.Content.ReadAsStringAsync());
		}

		/// <summary>
		/// Called on VoiceStateUpdates
		/// </summary>
		/// <param name="state">The VoiceState of the Update</param>
		/// <returns>Task</returns>
		public Task VoiceStateUpdateAsync(VoiceState state)
		{
			if (long.Parse(state.UserID) != Options.UserID) return Task.CompletedTask;

			var id = long.Parse(state.GuildID);

			if (state.ChannelID == null) return Task.CompletedTask;
			var first = !VoiceStates.ContainsKey(id);
			VoiceStates[id] = state.SessionID;
			return TryConnection(id, first);

		}

		/// <summary>
		/// Called on VoiceServerUpdates, sends a VoiceUpdate if needed
		/// </summary>
		/// <param name="server">The VoiceServerUpdate payload</param>
		/// <returns>Task</returns>
		public Task VoiceServerUpdateAsync(VoiceServerUpdatePayload server)
		{
			var id = long.Parse(server.GuildID);
			VoiceServers[id] = server;
			return TryConnection(id);
		}

		/// <summary>
		/// Configures the Resume of the LavalinkSession
		/// </summary>
		/// <param name="timeout">The timeout to wait before the Session dies, in Seconds</param>
		/// <param name="key">The Key to Resume a session, you can use <see cref="Util.RandomString"/> to generate Random strings</param>
		/// <returns>Task</returns>
		public Task ConfigureResumeAsync(int timeout = 60, string key = null)
		{
			if (key == null) key = Util.RandomString(36);
			ResumeKey = key;

			return SendAsync(new ConfigureResumePacket
			{
				OPCode = "configureResuming",
				Key = key,
				Timeout = timeout
			});
		}

		/// <summary>
		/// Sends a Sendable to the Lavalink WebSocket
		/// </summary>
		/// <param name="data">The data to send</param>
		/// <returns>Task</returns>
		private async Task _sendAsync(Sendable data)
		{
			try
			{
				await WebSocketClient.SendAsync(JsonConvert.SerializeObject(data.Packet));
				data.Emit();
			}
			catch (Exception e)
			{
				data.Emit(e);
			}
		}

		/// <summary>
		/// Invokes a VoiceUpdate if needed
		/// </summary>
		/// <param name="guildID">The GuildID to try VoiceUpdate on.</param>
		/// <param name="first">Only used for VoiceStateUpdates, if this one is the first</param>
		/// <returns></returns>
		private async Task TryConnection(long guildID, bool? first = null)
		{
			VoiceStates.TryGetValue(guildID, out var state);
			VoiceServers.TryGetValue(guildID, out var server);
				
			if (state == null || server == null || first == false) return;
			await Players.Get(guildID).VoiceUpdateAsync(state, server);
		}
		
		/// <summary>
		/// Method Called when the WebSocket Opens Connections
		/// </summary>
		/// <param name="sender">The Sender of this Event</param>
		/// <param name="args">The EventArgs of this Event</param>
		private async void OnOpen(object sender, EventArgs args)
		{
			_log(LogLevel.DEBUG, "Websocket Connection Open");
			try
			{
				await Task.WhenAll(Queue.Select(_sendAsync));
				Queue.Clear();
				if (ResumeKey == null && Options.ResumeTimeout != null && Options.ResumeTimeout > 0) 
					await ConfigureResumeAsync((int) Options.ResumeTimeout);
			}
			catch (Exception e)
			{
				Logs?.Invoke(this, new LogEventArgs(this, LogLevel.ERROR, e.ToString()));
			}
		}
		
		/// <summary>
		/// Called when the WebSocket Connection receives a Message.
		/// </summary>
		/// <param name="sender">The Sender of this Event</param>
		/// <param name="str">The Message of this Event</param>
		private void OnMessage(object sender, string str)
		{
			var json = JsonConvert.DeserializeObject<BasePacket>(str);

			Player player;
			switch (json.OPCode)
			{
				case "event":
					var packet = JsonConvert.DeserializeObject<PlayerEventPacket>(str);
					player = Players.Get(long.Parse(packet.GuildID));
					player.EmitPlayerEvent(str, packet.EventType);
					Event?.Invoke(this, new MessageEventArgs(this, JObject.Parse(str)));
					break;

				case "stats":
					var stats = JsonConvert.DeserializeObject<LavalinkStats>(str);
					LavalinkStats = stats;
					Stats?.Invoke(this, new StatsEventArgs(this, stats));
					break;

				case "playerUpdate":
					var playerUpdate = JsonConvert.DeserializeObject<PlayerUpdatePacket>(str);
					player = Players.Get(long.Parse(playerUpdate.GuildID));
					player.EmitPlayerUpdate(playerUpdate);
					break;

				default:
					_log(LogLevel.WARN, $"Received unknown OPCode ${json.OPCode}");
					break;
			}
		}
		
		/// <summary>
		/// Called when the WebSocket Connection encounters an error.
		/// </summary>
		/// <param name="sender">The sender of the Event.</param>
		/// <param name="error">The Exception of this Event.</param>
		private void OnError(object sender, Exception error)
		{
			_log(LogLevel.WARN, $"Websocket encountered Exception {error.Message}, reconnecting...");
			ConnectAsync().ConfigureAwait(false);
		}
		
		/// <summary>
		/// Called when the WebSocket Connection closes.
		/// </summary>
		/// <param name="sender">The Sender of this Event</param>
		/// <param name="args">The WebSocketCloseEventArgs of this Event</param>
		private void OnClose(object sender, WebSocketCloseEventArgs args)
		{
			_log(LogLevel.WARN, $"Websocket disconnected with {args.CloseCode}: {args.Reason}");
			ConnectAsync().ConfigureAwait(false);
		}

		/// <summary>
		/// Helper method for logging.
		/// </summary>
		/// <param name="logLevel">The LogLevel of this Log</param>
		/// <param name="message">The Message of this Log</param>
		private void _log(LogLevel logLevel, string message)
			=> Logs?.Invoke(this, new LogEventArgs(this, logLevel, message));
	}
}