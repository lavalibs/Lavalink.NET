using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Lavalink.NET.Types;
using Newtonsoft.Json;
using System.Collections.Generic;
using Lavalink.NET.Player;

namespace Lavalink.NET
{
	public class ClientOptions
	{
		public string HostRest { get; set; }
		public string HostWS { get; set; }
		public string Password { get; set; }
		public string UserID { get; set; }

		public ClientOptions(string hostRest, string hostWS, string password, string userID)
		{
			HostRest = hostRest ?? throw new ArgumentNullException(nameof(hostRest));
			HostWS = hostWS ?? throw new ArgumentNullException(nameof(hostWS));
			Password = password ?? throw new ArgumentNullException(nameof(password));
			UserID = userID ?? throw new ArgumentNullException(nameof(userID));
		}

		public ClientOptions(string hostRest, string hostWS, string password, ushort userID)
		{
			HostRest = hostRest ?? throw new ArgumentNullException(nameof(hostRest));
			HostWS = hostWS ?? throw new ArgumentNullException(nameof(hostWS));
			Password = password ?? throw new ArgumentNullException(nameof(password));
			UserID = userID.ToString() ?? throw new ArgumentNullException(nameof(userID));
		}
	}


	public abstract class Client
	{
		public Websocket.Websocket Websocket { get; }
		public PlayerStore Players { get; }
		public Dictionary<string, string> VoiceStates = new Dictionary<string, string>();
		public Dictionary<string, VoiceServerUpdate> VoiceServers = new Dictionary<string, VoiceServerUpdate>();

		private readonly ClientOptions _config;

		public Client(ClientOptions options)
		{
			_config = options;
			Websocket = new Websocket.Websocket(new Websocket.WebsocketOptions(_config.HostWS, _config.Password, _config.UserID));
			Players = new PlayerStore(this);
		}

		public Task InitAsync()
		{
			return Websocket.Connect();
		}

		public async Task<Track[]> LoadTracksAsync(string query)
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

		public Task<bool> VoiceStateUpdateAsync(VoiceStateUpdate packet)
		{
			if (packet.UserID != _config.UserID) return Task.FromResult(false);

			VoiceStates.TryAdd(packet.GuildID, packet.SessionID);

			return ConnectAsync(packet.GuildID);
		}

		public Task<bool> VoiceServerUpdateAsync(VoiceServerUpdate packet)
		{
			VoiceServers.Add(packet.GuildID, packet);

			return ConnectAsync(packet.GuildID);
		}

		public abstract Task SendAsync(string guildID, string packetJSON);

		private async Task<bool> ConnectAsync(string guildID)
		{
			VoiceStates.TryGetValue(guildID, out string state);
			VoiceServers.TryGetValue(guildID, out VoiceServerUpdate voiceServerUpdate);

			if (state == null || voiceServerUpdate == null) return await Task.FromResult(false);

			await Players.GetPlayer(guildID).VoiceUpdateAsync(state, voiceServerUpdate);

			return await Task.FromResult(true);
		}
	}
}
