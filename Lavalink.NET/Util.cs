using System;
using System.Security.Cryptography;

namespace Lavalink.NET
{
	/// <summary>
	/// Util functions used in Lavalink.NET
	/// </summary>
	public static class Util
	{
		private static RNGCryptoServiceProvider RNG { get; } = new RNGCryptoServiceProvider();
		
		/// <summary>
		/// Generates a random string with a specified length
		/// </summary>
		/// <param name="length">the length of the byte array</param>
		/// <returns>Random String</returns>
		public static string RandomString(int length)
		{
			var bytes = new byte[length];
			RNG.GetBytes(bytes);
			return Convert.ToBase64String(bytes);
		}
		
		/// <summary>
		/// Utility Method to Compare Lavalink Nodes by there CPU Usage
		/// </summary>
		/// <param name="a">Node A</param>
		/// <param name="b">Node B</param>
		/// <returns>int</returns>
		public static int CompareCPU(LavalinkNode a, LavalinkNode b)
		{
			if (a.LavalinkStats == null || b.LavalinkStats == null || !b.Connected) return -1;
			return (int) (a.LavalinkStats.CPU.SystemLoad / a.LavalinkStats.CPU.Cores - (b.LavalinkStats.CPU.SystemLoad - b.LavalinkStats.CPU.Cores));
		}
	}
}