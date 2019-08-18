using System.Net.Http;
using System.Threading.Tasks;

namespace Lavalink.NET.Extensions
{
	/// <summary>
	/// Extension Methods of HttpClient
	/// </summary>
	public static class HttpClientExtension
	{
		/// <summary>
		/// Helper Method to send a HttpRequest and throws an exception if the StatusCode is not a Success Code
		/// </summary>
		/// <param name="client">The HttpClient this request will be send with</param>
		/// <param name="request">The request to send</param>
		/// <returns>HttpResponse</returns>
		public static async Task<HttpResponseMessage> SendAndConfirmAsync(this HttpClient client, HttpRequestMessage request)
		{
			var res = await client.SendAsync(request);
			res.EnsureSuccessStatusCode();
			return res;
		} 
	}
}