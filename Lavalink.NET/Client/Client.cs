using System.IO;
using System.Net;
using System.Threading.Tasks;
using Lavalink.NET.Types;
using Newtonsoft.Json;

namespace Lavalink.NET
{
	public class ClientOptions
	{
		public string HostRest { get; set; }
		public string HostWS { get; set; }
		public string Password { get; set; }
		public string UserID { get; set; }

		public ClientOptions SetHostRest(string value)
		{
			HostRest = value;
			return this;
		}

		public ClientOptions SetHostWS(string value)
		{
			HostWS = value;
			return this;
		}

		public ClientOptions SetPassword(string value)
		{
			Password = value;
			return this;
		}

		public ClientOptions SetUserID(string value)
		{
			UserID = value;
			return this;
		}

		public ClientOptions SetUserID(ulong value)
		{
			UserID = value.ToString();
			return this;
		}
	}


	public abstract class Client
	{
		public Websocket.Websocket Websocket { get; }
		public Player.PlayerStore PlayerStore;

		private readonly ClientOptions _config;

		public Client(ClientOptions options)
		{
			_config = options;
			Websocket = new Websocket.Websocket(
				new Websocket.WebsocketOptions()
					.SetHost(_config.HostWS)
					.SetPassword(_config.Password)
					.SetUserID(_config.UserID)
			);
			PlayerStore = new Player.PlayerStore(this);
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

		public abstract Task Send(string guildID, string packetJSON);
	}
}
