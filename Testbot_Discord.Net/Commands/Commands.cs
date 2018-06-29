using System.Collections.Generic;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using Lavalink.NET.Player;
using Lavalink.NET.Types;

namespace Testbot_Discord.Net.Commands
{
    public class Commands : ModuleBase<SocketCommandContext>
    {
		[Command("hi")]
		public async Task Hi()
		{
			await Context.Channel.SendMessageAsync($"👋 Hi, {Context.User.Mention}!");
		}

		[Command("join")]
		public async Task Join()
		{
			Player player = Client.Lavalink.Players.GetPlayer(Context.Guild.Id);
			await player.JoinAsync(Context.Guild.GetUser(Context.User.Id).VoiceChannel.Id);
		}

		[Command("leave")]
		public async Task Leave()
		{
			Player player = Client.Lavalink.Players.GetPlayer(Context.Guild.Id);
			await player.LeaveAsync();
		}

		[Command("play")]
		public async Task Play([Remainder] string query)
		{
			Player player = Client.Lavalink.Players.GetPlayer(Context.Guild.Id);
			List<Track> tracks = await Client.Lavalink.LoadTracksAsync(query);
			await player.PlayAsync(tracks[0]);
		}

		[Command("pause")]
		public async Task Pause()
		{
			Player player = Client.Lavalink.Players.GetPlayer(Context.Guild.Id);
			await player.PauseAsync();
		}

		[Command("resume")]
		public async Task Resume()
		{
			Player player = Client.Lavalink.Players.GetPlayer(Context.Guild.Id);
			await player.PauseAsync(false);
		}

		[Command("joinchannel")]
		public async Task JoinSpecificChannel(SocketChannel channel)
		{
			Player player = Client.Lavalink.Players.GetPlayer(Context.Guild.Id);
			await player.JoinAsync(channel.Id);
		}
	}
}
