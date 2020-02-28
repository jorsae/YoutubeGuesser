using System.Net.Http;
using System.Threading.Tasks;
using YoutubeGuesserApi.Model;

namespace YoutubeGuesserApi.Library.Youtube
{
    public class YoutubeApi
    {
        private readonly HttpClient Client = new HttpClient();

        public async Task<YoutubeResponse> GetRequest(string url)
        {
            HttpResponseMessage response = await Client.GetAsync(url);
            string responseString = await response.Content.ReadAsStringAsync();
            return new YoutubeResponse(response.StatusCode, responseString);
        }
    }
}