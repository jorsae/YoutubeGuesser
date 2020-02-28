using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using YoutubeGuesserApp.Api;
using YoutubeGuesserApp.Library;
using YoutubeGuesserApp.Model;

namespace YoutubeGuesserApp.Controllers
{
    public class YoutubeGuesserController : Controller
    {
        private readonly YoutubeGuesserApi youtubeGuesserApi = new YoutubeGuesserApi();
        private readonly Utility utility = new Utility();

        public IActionResult Index()
        {
            // Only reset guesses if the SESSION_CORRECT_GUESSES_KEY flag is set.
            int? gameOver = HttpContext.Session.GetInt32(Constants.SESSION_GAME_OVER_KEY);
            if(gameOver == 1)
                HttpContext.Session.SetInt32(Constants.SESSION_CORRECT_GUESSES_KEY, 0);
            HttpContext.Session.SetInt32(Constants.SESSION_GAME_OVER_KEY, 0);
            HttpContext.Session.SetInt32(Constants.SESSION_SUBMITTED_TO_HIGHSCORES_KEY, 0);

            int? score = HttpContext.Session.GetInt32(Constants.SESSION_CORRECT_GUESSES_KEY);
            score = (score == null) ? 0 : score;
            return View(score);
        }

        public async Task<ActionResult> Highscore()
        {
            List<Highscore> highscores = await youtubeGuesserApi.GetHighscoreAsync(10);
            return View(highscores);
        }

        [HttpPost]
        public IActionResult GameOver(string encodedData)
        {
            byte[] data = Convert.FromBase64String(encodedData);
            string decodedString = Encoding.UTF8.GetString(data);
            List<YoutubeVideoView> gameHistory = JsonConvert.DeserializeObject<List<YoutubeVideoView>>(decodedString);
            foreach(YoutubeVideoView videoView in gameHistory)
                videoView.Title = HttpUtility.HtmlDecode(videoView.Title);
            LimitYoutubeVideoViews(gameHistory);
            utility.ReverseGameHistory(gameHistory);

            int? guessesCorrect = HttpContext.Session.GetInt32(Constants.SESSION_CORRECT_GUESSES_KEY);
            if (guessesCorrect == null)
                guessesCorrect = 0;

            ViewBag.correctGuesses = guessesCorrect;
            ViewBag.tweetMessage = $"I just got {guessesCorrect} points on #ViewGuessr!&url=https://viewguessr.com";

            return View(gameHistory);
        }

        public static IPAddress GetRemoteIPAddress(HttpContext context, bool allowForwarded = true)
        {
            if (allowForwarded)
            {
                string header = (context.Request.Headers["CF-Connecting-IP"].FirstOrDefault() ?? context.Request.Headers["X-Forwarded-For"].FirstOrDefault());
                if (IPAddress.TryParse(header, out IPAddress ip))
                {
                    return ip;
                }
            }
            return context.Connection.RemoteIpAddress;
        }

        private void LimitYoutubeVideoViews(List<YoutubeVideoView> youtubeVideos)
        {
            if(youtubeVideos.Count > Constants.MAXIMUM_VIDEO_HISTORY_LENGTH)
            {
                youtubeVideos.RemoveRange(0, youtubeVideos.Count - Constants.MAXIMUM_VIDEO_HISTORY_LENGTH);
            }
        }

        private void ReverseYoutubeVideoViews(List<YoutubeVideoView> youtubeVideos)
        {
            youtubeVideos.Reverse();
            for(int i = youtubeVideos.Count - 1; i >= 1; i-=2)
            {
                YoutubeVideoView temp = youtubeVideos[i];
                youtubeVideos[i] = youtubeVideos[i - 1];
                youtubeVideos[i - 1] = temp;
            }
        }
    }
}