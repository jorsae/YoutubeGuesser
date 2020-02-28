using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YoutubeGuesserApp.Api;
using YoutubeGuesserApp.Library;
using YoutubeGuesserApp.Model;

namespace YoutubeGuesserApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideosApiController : Controller
    {
        private readonly YoutubeGuesserApi ygApi = new YoutubeGuesserApi();
        private readonly Random rnd = new Random();

        [HttpGet]
        public async Task<ActionResult<List<YoutubeVideo>>> GetYoutubeVideosAsync()
        {
            List<YoutubeVideo> videoResult = new List<YoutubeVideo>();
            string videoSession = HttpContext.Session.GetString(Constants.SESSION_LAST_YOUTUBEVIDEOS);

            if(!string.IsNullOrEmpty(videoSession)) {
                string[] videoIds = videoSession.Split(',');
                if (videoIds.Length <= 0)
                    return StatusCode(StatusCodes.Status500InternalServerError);
                foreach(string videoId in videoIds)
                {
                    videoResult.Add(await ygApi.GetVideoByVideoIdAsync(videoId));
                }
                string sessionVideos = String.Join(",", videoResult.Select(v => v.VideoId));
                HttpContext.Session.SetString(Constants.SESSION_LAST_YOUTUBEVIDEOS, sessionVideos);
                return Ok(videoResult);
            }

            videoResult = await ygApi.GetVideosAsync();
            if (videoResult == null)
                return StatusCode(StatusCodes.Status500InternalServerError);
            else
            {
                string sessionVideos = String.Join(",", videoResult.Select(v => v.VideoId));
                HttpContext.Session.SetString(Constants.SESSION_LAST_YOUTUBEVIDEOS, sessionVideos);
                return Ok(videoResult);
            }
        }

        [HttpGet("{videoIdGuessed}/{videoIdNotGuessed}")]
        public async Task<ActionResult<ViewCountResult>> GetViewCountAsync(string videoIdGuessed, string videoIdNotGuessed)
        {
            // Make sure they are not empty
            if (string.IsNullOrEmpty(videoIdGuessed) || string.IsNullOrEmpty(videoIdNotGuessed))
                return BadRequest();

            // Make sure that the videoIds passed, are in their session
            string sessionLastYoutubeVideos = HttpContext.Session.GetString(Constants.SESSION_LAST_YOUTUBEVIDEOS);
            if(sessionLastYoutubeVideos == null)
            {
                return BadRequest();
            }
            string[] sessionVideos = sessionLastYoutubeVideos.Split(',');
            if(!sessionVideos.Any(videoIdGuessed.Contains) || !sessionVideos.Any(videoIdNotGuessed.Contains))
            {
                return BadRequest();
            }

            // Resets the videos that are linked to your session
            HttpContext.Session.SetString(Constants.SESSION_LAST_YOUTUBEVIDEOS, "");

            ViewCountResult viewCountResult = new ViewCountResult();
            List<VideoView> videoViews = await ygApi.GetVideosViewCountAsync(videoIdGuessed, videoIdNotGuessed);

            foreach (VideoView videoView in videoViews)
            {
                if (videoView.VideoId == videoIdGuessed)
                    viewCountResult.VideoGuessed = videoView;
                if (videoView.VideoId == videoIdNotGuessed)
                    viewCountResult.VideoNotGuessed = videoView;
            }

            int? correctGuesses = HttpContext.Session.GetInt32(Constants.SESSION_CORRECT_GUESSES_KEY);
            if (viewCountResult.VideoGuessed == null || viewCountResult.VideoNotGuessed == null)
                return Ok(viewCountResult);


            if(viewCountResult.VideoGuessed.ViewCount >= viewCountResult.VideoNotGuessed.ViewCount)
            {
                viewCountResult.Correct = true;
                if (correctGuesses == null)
                {
                    viewCountResult.CorrectGuesses = 1;
                    HttpContext.Session.SetInt32(Constants.SESSION_CORRECT_GUESSES_KEY, 1);
                }
                else
                {
                    correctGuesses++;
                    viewCountResult.CorrectGuesses = (int)correctGuesses;
                    HttpContext.Session.SetInt32(Constants.SESSION_CORRECT_GUESSES_KEY, (int)correctGuesses);
                }
            }
            else
            {
                HttpContext.Session.SetInt32(Constants.SESSION_GAME_OVER_KEY, 1);
                return Ok(viewCountResult);
            }

            // Check if they are trying to "cheat" the system, by not being redirected to the GameOver screen.
            int? gameOver = HttpContext.Session.GetInt32(Constants.SESSION_GAME_OVER_KEY);
            if (gameOver == 1)
            {
                viewCountResult.Correct = false;
                return Ok(viewCountResult);
            }
            return Ok(viewCountResult);
        }
    }
}