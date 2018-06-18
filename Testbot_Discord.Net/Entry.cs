
namespace Testbot_Discord.Net
{
	class Entry
	{
		static void Main(string[] args)
			=> new Client().MainAsync().GetAwaiter().GetResult();
	}
}
