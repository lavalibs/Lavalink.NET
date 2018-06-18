using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Lavalink.NET.Types;

namespace Lavalink.NET.Websocket
{
	public class WebsocketOptions
	{
		public string Host { get; set; }
		public string Password { get; set; }
		public string UserID { get; set; }

		public WebsocketOptions(string host, string password, string userID)
		{
			Host = host ?? throw new ArgumentNullException(nameof(host));
			Password = password ?? throw new ArgumentNullException(nameof(password));
			UserID = userID ?? throw new ArgumentNullException(nameof(userID));
		}
	}

	public class Websocket
	{
		public event ConnectionFailedEvent ConnectionFailed;
		public event MessageEvent Message;
		public event EventHandler Ready;
		public event CloseEvent Close;
		public event DebugEvent Debug;

		private const int ReceiveChunkSize = 1024;
		private const int SendChunkSize = 1024;

		private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
		private readonly CancellationToken _cancellationToken;
		private readonly ClientWebSocket _ws;
		private readonly Uri _uri;

		public Websocket(WebsocketOptions options)
		{
			_ws = new ClientWebSocket();
			_ws.Options.KeepAliveInterval = TimeSpan.FromSeconds(20);
			_ws.Options.SetRequestHeader("Authorization", options.Password);
			_ws.Options.SetRequestHeader("Num-Shards", "1");
			_ws.Options.SetRequestHeader("User-Id", options.UserID);
			_uri = new Uri(options.Host);
			_cancellationToken = _cancellationTokenSource.Token;
		}

		public Task Connect()
		{
			return ConnectAsync();
		}

		public Task SendMessage(string message)
		{
			return SendMessageAsync(message);
		}

		private async Task SendMessageAsync(string message)
		{
			if (_ws.State != WebSocketState.Open)
			{
				throw new Exception("Connection is not open.");
			}

			byte[] messageBuffer = Encoding.UTF8.GetBytes(message);
			int messagesCount = (int) Math.Ceiling((double) messageBuffer.Length / SendChunkSize);

			for (var i = 0; i < messagesCount; i++)
			{
				int offset = (SendChunkSize * i);
				int count = SendChunkSize;
				bool lastMessage = ((i + 1) == messagesCount);

				if ((count * (i + 1)) > messageBuffer.Length)
				{
					count = messageBuffer.Length - offset;
				}

				await _ws.SendAsync(new ArraySegment<byte>(messageBuffer, offset, count), WebSocketMessageType.Text, lastMessage, _cancellationToken);
			}
		}

		private async Task ConnectAsync()
		{
			try
			{
				await _ws.ConnectAsync(_uri, _cancellationToken);
			} catch (Exception e)
			{
				ConnectionFailed?.Invoke(this, new ConnectionFailedArgs(e));
			}
			Debug?.Invoke(this, new DebugEventArgs("Websocket Connection succesfully established"));
			Ready?.Invoke(this, new EventArgs());
			StartListen();
		}

		private async void StartListen()
		{
			byte[] buffer = new byte[ReceiveChunkSize];

			try
			{
				while (_ws.State == WebSocketState.Open)
				{
					StringBuilder stringResult = new StringBuilder();

					WebSocketReceiveResult result;
					do
					{
						result = await _ws.ReceiveAsync(new ArraySegment<byte>(buffer), _cancellationToken);

						if (result.MessageType == WebSocketMessageType.Close)
						{
							await _ws.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
							Close?.Invoke(this, new CloseEventArgs(_ws.CloseStatus, _ws.CloseStatusDescription));
						}
						else
						{
							string str = Encoding.UTF8.GetString(buffer, 0, result.Count);
							stringResult.Append(str);
						}

					} while (!result.EndOfMessage);

					if(stringResult.ToString().Length > 0) Message(this, new MessageEventArgs(stringResult.ToString()));
				}
			}
			catch (Exception)
			{
				Close?.Invoke(this, new CloseEventArgs(_ws.CloseStatus, _ws.CloseStatusDescription));
			}
			finally
			{
				_ws.Dispose();
			}
		}
	}
}