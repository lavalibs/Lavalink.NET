using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Lavalink.NET.Types;
using Lavalink.NET.Player;
using Lavalink.NET.Websocket;

namespace Lavalink.NET
{
	/// <summary>
	/// ClientOptions to instanciate the Client with.
	/// </summary>
	public class ClientOptions
	{
		public string HostRest { get; set; }
		public string HostWS { get; set; }
		public string Password { get; set; }
		public string UserID { get; set; }

		/// <summary>
		/// Creates a new ClientOptions instance.
		/// </summary>
		/// <param name="hostRest"> Host for the Rest API, includes protocol, hostname and port. </param>
		/// <param name="hostWS"> Host for the Websocket API, includes protocol, hostname and port. </param>
		/// <param name="password"> Password for your lavalink node. </param>
		/// <param name="userID"> The UserID of your Bot User. </param>
		public ClientOptions(string hostRest, string hostWS, string password, string userID)
		{
			HostRest = hostRest ?? throw new ArgumentNullException(nameof(hostRest));
			HostWS = hostWS ?? throw new ArgumentNullException(nameof(hostWS));
			Password = password ?? throw new ArgumentNullException(nameof(password));
			UserID = userID ?? throw new ArgumentNullException(nameof(userID));
		}

		/// <summary>
		/// Creates a new ClientOptions instance.
		/// </summary>
		/// <param name="hostRest"> Host for the Rest API, includes protocol, hostname and port. </param>
		/// <param name="hostWS"> Host for the Websocket API, includes protocol, hostname and port. </param>
		/// <param name="password"> Password for your lavalink node. </param>
		/// <param name="userID"> The UserID of your Bot User. </param>
		public ClientOptions(string hostRest, string hostWS, string password, ushort userID)
		{
			HostRest = hostRest ?? throw new ArgumentNullException(nameof(hostRest));
			HostWS = hostWS ?? throw new ArgumentNullException(nameof(hostWS));
			Password = password ?? throw new ArgumentNullException(nameof(password));
			UserID = userID.ToString() ?? throw new ArgumentNullException(nameof(userID));
		}
	}

	/// <summary>
	/// Base Client to start interacting with Lavalink.
	/// </summary>
	public abstract class Client
	{
		public event Message Message;

		public PlayerStore Players { get; }

		internal Dictionary<string, string> VoiceStates = new Dictionary<string, string>();
		internal Dictionary<string, VoiceServerUpdate> VoiceServers = new Dictionary<string, VoiceServerUpdate>();
		internal Websocket.Websocket Websocket { get; }

		private readonly ClientOptions _config;

		/// <summary>
		/// Creates a new Client instance.
		/// </summary>
		/// <param name="options"> Options to instance the Client with. </param>
		public Client(ClientOptions options)
		{
			_config = options;
			Websocket = new Websocket.Websocket(new WebsocketOptions(_config.HostWS, _config.Password, _config.UserID));
			Players = new PlayerStore(this);

			Websocket.Message += WebsocketMessage;
		}

		/// <summary>
		/// Method to Connect to the Lavalink Websocket.
		/// </summary>
		/// <returns> Task resolving with void. </returns>
		public Task ConnectAsync()
		{
			return Websocket.Connect();
		}

		/// <summary>
		/// Method to Load tracks from the Lavalink Rest Api.
		/// </summary>
		/// <param name="query"> The Search Parameter for the track, eg. an URL, an local path or an string. </param>
		/// <returns> An Array of tracks the Rest API returns, can be empty if no result. </returns>
		public async Task<Track[]> LoadTracksAsync(string query)
		{
			HttpWebRequest request = (HttpWebRequest) WebRequest.Create(_config.HostRest + "/loadtracks?identifier=" + query);
			
			request.Headers.Add("Authorization", _config.Password);
			request.Headers.Add("Content-Type", "application/json");
			request.Headers.Add("Accept", "application/json=");

			request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

			using (HttpWebResponse response = (HttpWebResponse) await request.GetResponseAsync())
			using (Stream stream = response.GetResponseStream())
			using (StreamReader reader = new StreamReader(stream))
			{
				string json = await reader.ReadToEndAsync();
				return JsonConvert.DeserializeObject<Track[]>(json);
			}
		}

		/// <summary>
		/// Method to Load tracks from the Lavalink Rest Api.
		/// </summary>
		/// <param name="query"> The Search Parameter for the track, eg. an URL, an local path or an string. </param>
		/// <returns> An Array of tracks the Rest API returns, can be empty if no result was found. </returns>
		public async Task<Track[]> LoadTracksAsync(Uri query)
		{
			HttpWebRequest request = (HttpWebRequest) WebRequest.Create(_config.HostRest + "/loadtracks?identifier=" + query);
			request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

			using (HttpWebResponse response = (HttpWebResponse) await request.GetResponseAsync())
			using (Stream stream = response.GetResponseStream())
			using (StreamReader reader = new StreamReader(stream))
			{
				string json = await reader.ReadToEndAsync();
				return JsonConvert.DeserializeObject<Track[]>(json);
			}
		}

		/// <summary>
		/// Method to call on VoiceStateUpdates.
		/// </summary>
		/// <param name="packet"> VoiceStateUpdatePacket you get from the Discord API. </param>
		/// <returns> A Task resolving with a Bool what shows if the changes went through to the server. </returns>
		public Task<bool> VoiceStateUpdateAsync(VoiceStateUpdate packet)
		{
			if (packet.UserID != _config.UserID) return Task.FromResult(false);

			VoiceStates.TryAdd(packet.GuildID, packet.SessionID);

			return ConnectVoiceAsync(packet.GuildID);
		}

		/// <summary>
		/// Method to call on VoiceServerUpdate.
		/// </summary>
		/// <param name="packet"> VoiceServerUpdatePacket you get from the Discord API. </param>
		/// <returns> A Task resolving with a Bool what shows if the changes went through to the server. </returns>
		public Task<bool> VoiceServerUpdateAsync(VoiceServerUpdate packet)
		{
			VoiceServers.Add(packet.GuildID, packet);

			return ConnectVoiceAsync(packet.GuildID);
		}

		/// <summary>
		/// Abstracted method you need to implement yourself, this method should forward to the Discord Websocket.
		/// </summary>
		/// <param name="guildID"> the GuildID this packet belongs to. </param>
		/// <param name="packetJSON"> The actuall packet as string. </param>
		/// <returns> Task resolving with void. </returns>
		public abstract Task SendAsync(string guildID, string packetJSON);

		private async Task<bool> ConnectVoiceAsync(string guildID)
		{
			VoiceStates.TryGetValue(guildID, out string state);
			VoiceServers.TryGetValue(guildID, out VoiceServerUpdate voiceServerUpdate);

			if (state == null || voiceServerUpdate == null) return await Task.FromResult(false);

			await Players.GetPlayer(guildID).VoiceUpdateAsync(state, voiceServerUpdate);

			return await Task.FromResult(true);
		}

		private void WebsocketMessage(object sender, MessageEventArgs e)
		{
			dynamic lavalinkEvent = JObject.Parse(e.Message);

			Message(this, new ClientEventArgs(lavalinkEvent));

			if (lavalinkEvent.op == "event")
			{
				if (lavalinkEvent.guildId)
				{
					Player.Player player = Players.GetPlayer(lavalinkEvent.guildId);
					player.PlayerEventEmitter(this, new MessageEventArgs(e.Message));
				}
			}
		}
	}
}
