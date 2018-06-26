using System.Collections.Generic;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using Lavalink.NET.Player;
using Lavalink.NET.Types;

namespace TestBot_DSharpPlus.Commands
{
	public class Commands : BaseCommandModule
    {
		[Command("hi")]
		public async Task Hi(CommandContext ctx)
		{
			await ctx.RespondAsync($"👋 Hi, {ctx.User.Mention}!");
		}

		[Command("join")]
		public async Task Join(CommandContext ctx)
		{
			Player player = Client._lavalinkClient.Players.GetPlayer(ctx.Guild.Id);
			await player.JoinAsync(ctx.Member.VoiceState.Channel.Id);
		}

		[Command("leave")]
		public async Task Leave(CommandContext ctx)
		{
			Player player = Client._lavalinkClient.Players.GetPlayer(ctx.Guild.Id);
			await player.LeaveAsync();
		}

		[Command("play")]
		public async Task Play(CommandContext ctx, string query)
		{
			Player player = Client._lavalinkClient.Players.GetPlayer(ctx.Guild.Id);
			List<Track> tracks = await Client._lavalinkClient.LoadTracksAsync(query);
			await player.PlayAsync(tracks[0]);
		}

		[Command("pause")]
		public async Task Pause(CommandContext ctx)
		{
			Player player = Client._lavalinkClient.Players.GetPlayer(ctx.Guild.Id);
			await player.PauseAsync();
		}

		[Command("resume")]
		public async Task Resume(CommandContext ctx)
		{
			Player player = Client._lavalinkClient.Players.GetPlayer(ctx.Guild.Id);
			await player.PauseAsync(false);
		}

		[Command("joinchannel")]
		public async Task JoinSpecificChannel(CommandContext ctx, DiscordChannel channel)
		{
			Player player = Client._lavalinkClient.Players.GetPlayer(ctx.Guild.Id);
			await player.JoinAsync(channel.Id);
		}
	}
}
