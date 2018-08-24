using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lavalink.NET.Websocket
{
	internal class WebsocketOptions
	{
		internal string Host { get; set; }
		internal string Password { get; set; }
		internal string UserID { get; set; }
		internal string ShardCount { get; set; }

		internal WebsocketOptions(string host, string password, ulong userID, int shardCount)
		{
			Host = host ?? throw new ArgumentNullException(nameof(host));
			Password = password ?? throw new ArgumentNullException(nameof(password));
			UserID = userID.ToString();
			ShardCount = shardCount.ToString();
		}
	}

	internal class Websocket
	{
		internal event Func<Exception, Task> ConnectionFailed;
		internal event Func<string, Task> Message;
		internal event Func<Task> Ready;
		internal event Func<WebSocketCloseStatus, string, Task> Close;
		internal event Func<Exception, Task> Error;
		internal event Func<string, Task> Debug;

		private const int ReceiveChunkSize = 1024;
		private const int SendChunkSize = 1024;

		private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
		private readonly CancellationToken _cancellationToken;
		private readonly ClientWebSocket _ws;
		private readonly Uri _uri;

		internal Websocket(WebsocketOptions options)
		{
			_ws = new ClientWebSocket();
			_ws.Options.KeepAliveInterval = TimeSpan.FromSeconds(20);
			_ws.Options.SetRequestHeader("Authorization", options.Password);
			_ws.Options.SetRequestHeader("Num-Shards", options.ShardCount);
			_ws.Options.SetRequestHeader("User-Id", options.UserID);
			_uri = new Uri(options.Host);
			_cancellationToken = _cancellationTokenSource.Token;
		}

		internal async void ConnectAsync()
		{
			try
			{
				await _ws.ConnectAsync(_uri, _cancellationToken);
			}
			catch (Exception e)
			{
				ConnectionFailed?.Invoke(e);
				return;
			}
			Debug?.Invoke("Websocket Connection succesfully established");
			Ready?.Invoke();
			StartListen();
		}

		internal async Task SendMessageAsync(string message)
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
							Close?.Invoke((WebSocketCloseStatus) _ws.CloseStatus, _ws.CloseStatusDescription);
						}
						else
						{
							string str = Encoding.UTF8.GetString(buffer, 0, result.Count);
							stringResult.Append(str);
						}

					} while (!result.EndOfMessage);
					if (stringResult.ToString().Length > 0) ThreadPool.QueueUserWorkItem((Object stateInfo) => InvokeMessageEvent(stringResult.ToString()));


				}
			}
			catch (Exception)
			{
				Close?.Invoke((WebSocketCloseStatus) _ws.CloseStatus, _ws.CloseStatusDescription);
			}
			finally
			{
				_ws.Dispose();
			}
		}

		private async void InvokeMessageEvent(string args)
		{
			try
			{
				await Message?.Invoke(args);
			}
			catch (Exception e)
			{
				Error?.Invoke(e);
			}
		}
	}
}