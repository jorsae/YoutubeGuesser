using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using YoutubeGuesserApp.Library;
using YoutubeGuesserApp.Model;

namespace YoutubeGuesserApp.Api
{
    public class YoutubeGuesserApi
    {
        private readonly HttpClient client = new HttpClient();

        public async Task<List<YoutubeVideo>> GetVideosAsync()
        {
            string url = $"{Constants.BASEURL}youtubevideos/";
            HttpResponseMessage response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<List<YoutubeVideo>>();
            }
            return null;
        }

        public async Task<YoutubeVideo> GetVideoByVideoIdAsync(string videoId)
        {
            string url = $"{Constants.BASEURL}youtubevideos/videoid/{videoId}";
            HttpResponseMessage response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<YoutubeVideo>();
            }
            return null;
        }

        public async Task<List<VideoView>> GetVideosViewCountAsync(string videoId1, string videoId2)
        {
            string url = $"{Constants.BASEURL}youtubevideos/{videoId1}/{videoId2}";
            HttpResponseMessage response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<List<VideoView>>();
            }
            return null;
        }

        public async Task<bool> PostUserDataAsync(UserData userData)
        {
            string url = $"{Constants.BASEURL}harvests/";
            HttpResponseMessage response = await client.PostAsJsonAsync<UserData>(url, userData);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> IsAliasProfanityAsync(string username)
        {
            string url = $"{Constants.BASEURL}profanities/{username}";
            HttpResponseMessage response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<bool>();
            }
            return true;
        }

        public async Task<string> GetLocationByIpAsync(string ipAddress)
        {
            string url = $"{Constants.BASEURL}harvests/geo/{ipAddress}";
            HttpResponseMessage response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            return Constants.DEFAULT_UNKNOWN_LOCATION;
        }

        public async Task<bool> PostHighscoreAsync(Highscore highscore)
        {
            string url = $"{Constants.BASEURL}highscores/";
            HttpResponseMessage response = await client.PostAsJsonAsync<Highscore>(url, highscore);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }

        public async Task<List<Highscore>> GetHighscoreAsync(int count)
        {
            string url = $"{Constants.BASEURL}highscores/{count}";
            HttpResponseMessage response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<List<Highscore>>();
            }
            return null;
        }

        public async Task<bool> PostFeedbackAsync(Feedback feedback)
        {
            string url = $"{Constants.BASEURL}feedback/";
            HttpResponseMessage response = await client.PostAsJsonAsync<Feedback>(url, feedback);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }
    }
}