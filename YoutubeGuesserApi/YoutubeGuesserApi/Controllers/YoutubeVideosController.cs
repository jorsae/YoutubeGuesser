using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YoutubeGuesserApi.DataAccess;
using YoutubeGuesserApi.Library.Utility;
using YoutubeGuesserApi.Library.Youtube;
using YoutubeGuesserApi.Model;

namespace YoutubeGuesserApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class YoutubeVideosController : Controller
    {
        private readonly DatabaseContext _context;
        private readonly Random rnd;
        private readonly YoutubeApi youtubeApi;
        private readonly Parser parser;

        private static DateTime dateTimeRateLimited = new DateTime();

        public YoutubeVideosController(DatabaseContext context)
        {
            _context = context;
            rnd = new Random();
            parser = new Parser();
            youtubeApi = new YoutubeApi();
        }

        [HttpGet]
        public async Task<ActionResult> GetAsync()
        {
            List<YoutubeVideo> videos = await _context.YoutubeVideos.Where(yt => yt.LastUpdated >= DateTime.Now.AddDays(-1)) // Get all videos >= 1 day old
                                                                    .OrderByDescending(yt => yt.LastUpdated)
                                                                    .Take(Constants.TOP_VIDEO_VIEWS_SELECTION) // Get the 1000 most popular
                                                                    .OrderByDescending(x => rnd.Next())
                                                                    .Take(2) // pick 2 randomly among them
                                                                    .ToListAsync();
            if(videos.Count == 2)
            {
                return Ok(videos);
            }
            return Ok(await _context.YoutubeVideos.OrderByDescending(yt => yt.LastUpdated)// Get the 1000 most popular videos
                                                        .Take(Constants.TOP_VIDEO_VIEWS_SELECTION)
                                                        .OrderByDescending(x => rnd.Next())
                                                        .Take(2) // pick 2 randomly among them
                                                        .ToListAsync());
        }

        [HttpGet("videoid/{videoId}")]
        public async Task<ActionResult> GetVideoByVideoIdAsync(string videoId)
        {
            if (string.IsNullOrEmpty(videoId))
                return BadRequest();

            YoutubeVideo video = await _context.YoutubeVideos.FindAsync(videoId);
            return Ok(video);
        }


        [HttpGet("{videoIdGuessed}/{videoIdNotGuessed}")]
        [Produces("application/json")]
        public async Task<ActionResult> CompareViewCountAsync(string videoIdGuessed, string videoIdNotGuessed)
        {
            if (string.IsNullOrEmpty(videoIdGuessed) || string.IsNullOrEmpty(videoIdNotGuessed))
                return BadRequest();

            if (videoIdGuessed == videoIdNotGuessed)
                return BadRequest();

            // Try to get Viewcount from database
            YoutubeVideo video1 = await _context.YoutubeVideos.FindAsync(videoIdGuessed);
            YoutubeVideo video2 = await _context.YoutubeVideos.FindAsync(videoIdNotGuessed);
            if (video1 == null || video2 == null)
                return BadRequest();

            List<VideoView> videoViews = new List<VideoView>
            {
                new VideoView(video1),
                new VideoView(video2)
            };

            await AddGuessesStatisticsAsync(videoViews[0].ViewCount, videoViews[1].ViewCount);

            return Ok(videoViews);
        }

        private async Task AddGuessesStatisticsAsync(long selectedViewCount, long notSelectedViewCount, bool save=true)
        {
            Guesses guesses = await _context.Guesses.FindAsync(DateTime.Now.Date); // DateTime.Now.Date removes hours/minutes/seconds
            bool guessesModified = true;
            if (guesses == null)
            {
                guessesModified = false;
                guesses = new Guesses();
            }

            if (selectedViewCount >= notSelectedViewCount)
                guesses.CorrectGuesses++;
            else
                guesses.WrongGuesses++;

            if (guessesModified)
            {
                _context.Entry(guesses).State = EntityState.Modified;
            }
            else
            {
                _context.Guesses.Add(guesses);
            }

            if (save)
            {
                await SaveChangesDbAsync();
            }
        }

        /**
         * Fetches all trending videos for all countries listed Constants.TRENDING_COUNTRY_CODES
         * Also fetches top 50 relevant videos to all trending videos
         * */
        [HttpGet("fetch/trending")]
        public async Task<ActionResult> FetchYoutubeVideosAsync()
        {
            if (dateTimeRateLimited == DateTime.Now.Date)
                return BadRequest("rate-limited");

            int totalVideosAdded = 0;

            foreach(string country in Constants.TRENDING_COUNTRY_CODES)
            {
                // Finds and adds all the trending videos
                List<YoutubeVideo> trendingVideos = new List<YoutubeVideo>();
                string nextPageToken = null;
                do
                {
                    string url = YoutubeApiStrings.GetTrendingVideo(country, Constants.YOUTUBE_API_KEY, pageToken: nextPageToken);
                    YoutubeResponse response = await youtubeApi.GetRequest(url);
                    if(response.StatusCode != HttpStatusCode.OK)
                    {
                        dateTimeRateLimited = DateTime.Now.Date;
                        YoutubeError error = parser.ParseYoutubeError(response.Content);
                        return BadRequest(error);
                    }
                    nextPageToken = parser.ParseTokenAndVideoCount(response.Content, out int videoCount);
                    trendingVideos.AddRange(parser.ParseYoutubeVideosFromTrending(response.Content, videoCount));
                } while (nextPageToken != null);

                await AddYoutubeVideosDatabaseAsync(trendingVideos);
                await SaveChangesDbAsync();
                totalVideosAdded += trendingVideos.Count;

                // Find and adds all videos related to the trending videos
                List<YoutubeVideo> relatedVideos = new List<YoutubeVideo>();
                foreach(YoutubeVideo video in trendingVideos)
                {
                    string url = YoutubeApiStrings.GetVideoRelatedToVideoId(video.VideoId, Constants.YOUTUBE_API_KEY, maxResults: 50);
                    YoutubeResponse response = await youtubeApi.GetRequest(url);
                    if(response.StatusCode != HttpStatusCode.OK)
                    {
                        dateTimeRateLimited = DateTime.Now.Date;
                        YoutubeError error = parser.ParseYoutubeError(response.Content);
                        return BadRequest(error);
                    }
                    relatedVideos.AddRange(parser.ParseYoutubeVideosFromSearch(response.Content, 50));
                }

                await AddYoutubeVideosDatabaseAsync(relatedVideos);
                await SaveChangesDbAsync();
                totalVideosAdded += relatedVideos.Count;
            }

            return Ok(totalVideosAdded);
        }

        [HttpGet("fetch/filler")]
        public async Task<ActionResult> GetFillerVideos()
        {
            if (dateTimeRateLimited == DateTime.Now.Date)
                return BadRequest("rate-limited");

            int totalVideosAdded = 0;

            // Fetch all videos by search, while not rate limited
            while (dateTimeRateLimited != DateTime.Now.Date)
            {
                string url = YoutubeApiStrings.GetVideoBySearch(RandomString(3), Constants.YOUTUBE_API_KEY, maxResults: 50);
                YoutubeResponse response = await youtubeApi.GetRequest(url);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    dateTimeRateLimited = DateTime.Now.Date;
                    // TODO: Fetch and log this YoutubeError message
                    YoutubeError error = parser.ParseYoutubeError(response.Content);
                    return BadRequest(error);
                }
                parser.ParseTokenAndVideoCount(response.Content, out int videoCount);
                List<YoutubeVideo> videos = parser.ParseYoutubeVideosFromSearch(response.Content, videoCount);

                for (int i = 0; i < videos.Count; i++)
                {
                    string urlStats = YoutubeApiStrings.GetVideoStatistics(videos[i].VideoId, Constants.YOUTUBE_API_KEY);
                    long viewCount = await GetYoutubeVideoViewCountFromApiAsync(urlStats);
                    if (viewCount == -1)
                        break;
                    videos[i].ViewCount = viewCount;
                }
                List<YoutubeVideo> validVideos = videos.Where(yt => yt.ViewCount != -1).ToList();

                totalVideosAdded += validVideos.Count;
                await AddYoutubeVideosDatabaseAsync(validVideos);
                await SaveChangesDbAsync();
            }
            return Ok(totalVideosAdded);
        }

        /**
         * Deletes vides older than 29days. This must be done, to ensure compliance with YouTube's API policy.
         * Also deletes videos with less than Constants.FILTER_YOUTUBE_VIDEOS_VIEWCOUNT views.
         * They should not be in here in the first place, but more of a double-security filter.
         * */
        [HttpGet("deleteoldvideos")]
        public async Task<ActionResult> DeleteOldVideosAsync()
        {
            // Delete videos that are older than Constants.MAX_DAYS_STORAGE_YOUTUBE_VIDEO days.
            // This must be done, to ensure you comply with YouTube's API policy.
            DateTime deletionDate = DateTime.Now.Date.AddDays(-(Constants.MAX_DAYS_STORAGE_YOUTUBE_VIDEO + 1));
            List<YoutubeVideo> youtubeVideos = await _context.YoutubeVideos.Where(yt => yt.LastUpdated <= deletionDate).ToListAsync();
            _context.YoutubeVideos.RemoveRange(youtubeVideos);

            // Delete YouTube videos with less than Constants.FILTER_YOUTUBE_VIDEOS_VIEWCOUNT views
            List<YoutubeVideo> videos = await _context.YoutubeVideos.Where(yt => yt.ViewCount < Constants.FILTER_YOUTUBE_VIDEOS_VIEWCOUNT).ToListAsync();
            _context.YoutubeVideos.RemoveRange(videos);

            await SaveChangesDbAsync();
            int totalDeletedVideos = youtubeVideos.Count + videos.Count;
            return Ok(totalDeletedVideos);
        }

        /**
         * Password requred function
         * */
        [HttpGet("statistics")]
        public async Task<ActionResult> GetStatisticsAsync(IFormCollection data)
        {
            if (data == null)
                return BadRequest();

            string password = data["password"];
            if (password != Constants.ADMIN_API_PASSWORD)
                return Unauthorized();

            var info = await (from yt in _context.YoutubeVideos
                              group yt by 0 into g // any constant would work
                              select new
                              {
                                  Total = g.Count(),
                                  MinViewCount = g.Min(yt => yt.ViewCount),
                                  MaxViewCount = g.Max(yt => yt.ViewCount),
                                  AverageViewCount = g.Average(yt => yt.ViewCount),
                              }).FirstOrDefaultAsync();

            Dictionary<string, object> stats = new Dictionary<string, object>
            {
                { "Total", info.Total },
                { "MinViewCount", info.MinViewCount },
                { "MaxViewCount", info.MaxViewCount },
                { "AverageViewCount", info.AverageViewCount }
            };

            int videosAddedLastWeek = _context.YoutubeVideos.Count(yt => yt.LastUpdated >= DateTime.Now.AddDays(-7));
            stats.Add("VideosAddedLastWeek", videosAddedLastWeek);
            return Ok(stats);
        }

        private string RandomString(int length)
        {
            return new string(Enumerable.Repeat(Constants.CHARACTER_SEARCH, length)
                .Select(c => c[rnd.Next(c.Length)]).ToArray());
        }

        private async Task<long> GetYoutubeVideoViewCountFromApiAsync(string apiUrl)
        {
            YoutubeResponse response = await youtubeApi.GetRequest(apiUrl);
            return parser.ParseYoutubeVideoViewCount(response.Content);
        }

        private async Task AddYoutubeVideosDatabaseAsync(List<YoutubeVideo> youtubeVideos)
        {
            List<YoutubeVideo> validVideos = youtubeVideos.Where(yt => yt.ViewCount >= Constants.FILTER_YOUTUBE_VIDEOS_VIEWCOUNT).ToList();
            foreach (YoutubeVideo video in validVideos)
                await AddYoutubeVideoDatabaseAsync(video);
        }

        private async Task AddYoutubeVideoDatabaseAsync(YoutubeVideo youtubeVideo)
        {
            YoutubeVideo video = await _context.YoutubeVideos.FindAsync(youtubeVideo.VideoId);
            if (video == null)
            {
                _context.Add(youtubeVideo);
            }
            else
            {
                video.ChannelName = youtubeVideo.ChannelName;
                video.Title = youtubeVideo.Title;
                video.ViewCount = youtubeVideo.ViewCount;
                video.LastUpdated = youtubeVideo.LastUpdated;
                _context.Entry(video).State = EntityState.Modified;
            }
        }

        private async Task<bool> SaveChangesDbAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}