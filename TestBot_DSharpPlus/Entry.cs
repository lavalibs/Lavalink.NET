using System;
using System.Collections.Generic;
using System.Text;

namespace TestBot
{
	class Entry
	{
		static void Main(string[] args)
		{
			new Client().MainAsync(args).ConfigureAwait(false).GetAwaiter().GetResult();
		}
	}
}
