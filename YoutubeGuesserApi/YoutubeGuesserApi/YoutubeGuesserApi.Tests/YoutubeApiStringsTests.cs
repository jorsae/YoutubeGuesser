using Microsoft.VisualStudio.TestTools.UnitTesting;
using YoutubeGuesserApi.Library.Youtube;

namespace YoutubeGuesserApi.Tests
{
    [TestClass]
    public class YoutubeApiStringsTests
    {
        private static string videoId;
        private static string youtubeApiKey;
        private static int maxResults;
        private static string order;
        private static string search;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            videoId = "videoId";
            youtubeApiKey = "apiKey";
            maxResults = 2;
            order = "viewCount";
            search = "searchTerm";
        }

        [TestMethod]
        public void Assert_GetVideoStatistics_is_correct()
        {
            string expected = $"https://www.googleapis.com/youtube/v3/videos?&id={videoId}&key={youtubeApiKey}&part=statistics";

            Assert.AreEqual(expected, YoutubeApiStrings.GetVideoStatistics(videoId, youtubeApiKey));
        }

        [TestMethod]
        public void Assert_GetVideoBySearch_is_correct()
        {
            string expected = $"https://www.googleapis.com/youtube/v3/search?q={search}&key={youtubeApiKey}&maxResults=1&order=viewCount&part=snippet&type=video&relevanceLanguage=en";

            Assert.AreEqual(expected, YoutubeApiStrings.GetVideoBySearch(search, youtubeApiKey));
        }

        [TestMethod]
        public void Assert_GetVideoBySearch_with_overloads_is_correct()
        {
            string expected = $"https://www.googleapis.com/youtube/v3/search?q={search}&key={youtubeApiKey}&maxResults={maxResults}&order={order}&part=snippet&type=video&relevanceLanguage=en";

            Assert.AreEqual(expected, YoutubeApiStrings.GetVideoBySearch(search, youtubeApiKey, maxResults, order));
        }

        [TestMethod]
        public void Assert_GetVideoRelatedToVideoId_is_correct()
        {
            string expected = $"https://www.googleapis.com/youtube/v3/search?0={videoId}&key={youtubeApiKey}&maxResults=1&order=relevance&part=snippet&type=video";
            Assert.AreEqual(expected, YoutubeApiStrings.GetVideoRelatedToVideoId(videoId, youtubeApiKey));
        }

        [TestMethod]
        public void Assert_GetVideoRelatedToVideoId_with_overloads_is_correct()
        {
            string expected = $"https://www.googleapis.com/youtube/v3/search?0={videoId}&key={youtubeApiKey}&maxResults={maxResults}&order={order}&part=snippet&type=video";
            Assert.AreEqual(expected, YoutubeApiStrings.GetVideoRelatedToVideoId(videoId, youtubeApiKey, maxResults, order));
        }

        [TestMethod]
        public void Assert_GetTrendingVideo_page_token_is_null()
        {
            string expected = "https://www.googleapis.com/youtube/v3/videos?regionCode=regionCode&key=apiKey&maxResults=2&part=snippet,statistics&chart=mostPopular";
            string trendingVideo = YoutubeApiStrings.GetTrendingVideo("regionCode", "apiKey", 2);
            Assert.AreEqual(expected, trendingVideo);
        }

        [TestMethod]
        public void Assert_GetTrendingVideo_page_token_is_not_null()
        {
            string expected = "https://www.googleapis.com/youtube/v3/videos?regionCode=regionCode&key=apiKey&maxResults=2&pageToken=token&part=snippet,statistics&chart=mostPopular";
            string trendingVideo = YoutubeApiStrings.GetTrendingVideo("regionCode", "apiKey", 2, "token");
            Assert.AreEqual(expected, trendingVideo);
        }

        [TestMethod]
        public void Assert_GetTrendingVideo_max_result()
        {
            string expected = "https://www.googleapis.com/youtube/v3/videos?regionCode=regionCode&key=apiKey&maxResults=50&part=snippet,statistics&chart=mostPopular";
            string trendingVideo = YoutubeApiStrings.GetTrendingVideo("regionCode", "apiKey", maxResults: 500);
            Assert.AreEqual(expected, trendingVideo);
        }
    }
}