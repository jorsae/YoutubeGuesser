using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using YoutubeGuesserApi.Library.Youtube;
using YoutubeGuesserApi.Model;

namespace YoutubeGuesserApi.Tests
{
    [TestClass]
    public class ParserTests
    {
        private static Parser parser;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            parser = new Parser();
        }

        private string GetTextFromFile(string path)
        {
            return File.ReadAllText($"../../../TestData/{path}");
        }

        [TestMethod]
        public void Assert_ParseYoutubeVideo_is_correct()
        {
            YoutubeVideo expectedYoutubeVideo = new YoutubeVideo("2cXDgFwE13g", "Skrillex - First Of The Year (Equinox) [Official Music Video]", "Skrillex");

            string jsonData = GetTextFromFile("VideoSearchResponse.json");
            YoutubeVideo yt = parser.ParseYoutubeVideo(jsonData);
            Assert.AreEqual(yt.Title, expectedYoutubeVideo.Title);
            Assert.AreEqual(yt.ViewCount, expectedYoutubeVideo.ViewCount);
            Assert.AreEqual(yt.ChannelName, expectedYoutubeVideo.ChannelName);
            Assert.AreEqual(yt.VideoId, expectedYoutubeVideo.VideoId);
        }

        [TestMethod]
        public void Assert_ParseYoutubeVideo_25_results_is_correct()
        {
            string jsonData = GetTextFromFile("VideoSearchResponse_25_results.json");
            YoutubeVideo yt = parser.ParseYoutubeVideo(jsonData);
            Assert.IsNotNull(yt.VideoId);
            Assert.IsNotNull(yt.Title);
            Assert.IsNotNull(yt.ChannelName);
        }

        [TestMethod]
        public void Assert_ParseYoutubeVideo_invalid_json_is_null()
        {
            YoutubeVideo yt = parser.ParseYoutubeVideo("blahblah");
            Assert.IsNull(yt);
        }

        [TestMethod]
        public void Assert_ParseYoutubeVideo_valid_json_is_null()
        {
            YoutubeVideo yt = parser.ParseYoutubeVideo("{ 'name':'John', 'age':30, 'car':null }");
            Assert.IsNull(yt);
        }

        [TestMethod]
        public void Assert_ParseYoutubeVideoViewCount_is_correct()
        {
            long expectedViewCount = 412739213;

            string jsonData = GetTextFromFile("GetVideoBySearchResponse.json");
            long viewCount = parser.ParseYoutubeVideoViewCount(jsonData);

            Assert.AreEqual(viewCount, expectedViewCount);
        }

        [TestMethod]
        public void Assert_ParseYoutubeVideoViewCount_invalid_json_is_negative_one()
        {
            long viewCount = parser.ParseYoutubeVideoViewCount("blahblah");
            Assert.AreEqual(viewCount, -1);
        }

        [TestMethod]
        public void Assert_ParseYoutubeVideoViewCount_valid_json_is_null()
        {
            long viewCount = parser.ParseYoutubeVideoViewCount("{ 'name':'John', 'age':30, 'car':null }");
            Assert.AreEqual(viewCount, -1);
        }

        [TestMethod]
        public void Assert_ParseTokenAndVideoCount_from_trending_is_correct()
        {
            string jsonData = GetTextFromFile("TrendingResult.json");
            string nextPageToken = parser.ParseTokenAndVideoCount(jsonData, out int videos);

            Assert.AreEqual(50, videos);
            Assert.AreEqual("CDIQAA", nextPageToken);
        }

        [TestMethod]
        public void Assert_ParseTokenAndVideoCount_from_search_is_correct()
        {
            string jsonData = GetTextFromFile("VideoSearchResponse_25_results.json");
            string nextPageToken = parser.ParseTokenAndVideoCount(jsonData, out int videoCount);

            Assert.AreEqual("CBkQAA", nextPageToken);
            Assert.AreEqual(25, videoCount);
        }

        [TestMethod]
        public void Assert_ParseTokenAndVideoCount_no_nextPageToken()
        {
            string jsonData = GetTextFromFile("TrendingResult_no_nextPageToken.json");
            string nextPageToken = parser.ParseTokenAndVideoCount(jsonData, out int videos);

            Assert.AreEqual(50, videos);
            Assert.IsNull(nextPageToken);
        }

        [TestMethod]
        public void Assert_ParseTokenAndVideoCount_no_json()
        {
            string nextPageToken = parser.ParseTokenAndVideoCount("", out int videos);
            Assert.AreEqual(-1, videos);
            Assert.IsNull(nextPageToken);
        }

        [TestMethod]
        public void Assert_ParseYoutubeVideosFromTrending_is_correct()
        {
            string jsonData = GetTextFromFile("TrendingResult.json");
            parser.ParseTokenAndVideoCount(jsonData, out int videos);
            List<YoutubeVideo> youtubeVideos = parser.ParseYoutubeVideosFromTrending(jsonData, videos);

            Assert.AreEqual(videos, youtubeVideos.Count);
        }

        [DataTestMethod]
        [DataRow(0, "")]
        [DataRow(0, null)]
        [DataRow(0, "asdasd")]
        public void Assert_ParseYoutubeVideosFromTrending_invalid_json(int expected, string jsonData)
        {
            List<YoutubeVideo> youtubeVideos = parser.ParseYoutubeVideosFromTrending(jsonData, 50);
            Assert.AreEqual(expected, youtubeVideos.Count);
        }

        [TestMethod]
        public void Assert_ParseYoutubeVideosFromTrending_wrong_videoCount()
        {
            string jsonData = GetTextFromFile("TrendingResult.json");
            parser.ParseTokenAndVideoCount(jsonData, out int videoCount);
            int wrongVideos = videoCount + 10;
            List<YoutubeVideo> youtubeVideos = parser.ParseYoutubeVideosFromTrending(jsonData, wrongVideos);

            Assert.AreEqual(videoCount, youtubeVideos.Count);
        }

        [TestMethod]
        public void Assert_ParseYoutubeVideosFromSearch_is_correct()
        {
            string jsonData = GetTextFromFile("VideoSearchResponse_25_results.json");
            parser.ParseTokenAndVideoCount(jsonData, out int videoCount);
            List<YoutubeVideo> youtubeVideos = parser.ParseYoutubeVideosFromSearch(jsonData, videoCount);

            Assert.AreEqual(25, youtubeVideos.Count);
        }

        [DataTestMethod]
        [DataRow(0, "")]
        [DataRow(0, null)]
        [DataRow(0, "asdasd")]
        public void Assert_ParseYoutubeVideosFromSearch_invalid_json(int expected, string jsonData)
        {
            List<YoutubeVideo> youtubeVideos = parser.ParseYoutubeVideosFromSearch(jsonData, 50);
            Assert.AreEqual(expected, youtubeVideos.Count);
        }

        [TestMethod]
        public void Assert_ParseYoutubeVideosFromSearch_wrong_videoCount()
        {
            string jsonData = GetTextFromFile("VideoSearchResponse_25_results.json");
            parser.ParseTokenAndVideoCount(jsonData, out int videoCount);
            int wrongVideoCount = videoCount + 10;
            List<YoutubeVideo> youtubeVideos = parser.ParseYoutubeVideosFromSearch(jsonData, wrongVideoCount);

            Assert.AreEqual(25, youtubeVideos.Count);
        }

        [TestMethod]
        public void Assert_ParseYoutubeError_is_correct()
        {
            string messageExpected = "The request cannot be completed because you have exceeded your \u003ca href=\"/youtube/v3/getting-started#quota\"\u003equota\u003c/a\u003e.";

            string jsonData = GetTextFromFile("QuotaReached.json");
            YoutubeError youtubeError = parser.ParseYoutubeError(jsonData);

            Assert.AreEqual(403, youtubeError.StatusCode);
            Assert.AreEqual("youtube.quota", youtubeError.Domain);
            Assert.AreEqual("quotaExceeded", youtubeError.Reason);
            Assert.AreEqual(messageExpected, youtubeError.Message);
        }
    }
}