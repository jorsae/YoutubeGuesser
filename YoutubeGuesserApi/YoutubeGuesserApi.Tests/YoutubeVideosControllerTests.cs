using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using YoutubeGuesserApi.Controllers;
using YoutubeGuesserApi.DataAccess;
using YoutubeGuesserApi.Model;

namespace YoutubeGuesserApi.Tests
{
    [TestClass]
    public class YoutubeVideosControllerTests
    {
        private static DatabaseContext _context;
        private static YoutubeVideosController videosController;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            _context = new DatabaseContext("Server=(localdb)\\mssqllocaldb;Database=YoutubeGuesserApiDB_Tests;Trusted_Connection=True;MultipleActiveResultSets=true");
            videosController = new YoutubeVideosController(_context);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _context.YoutubeVideos.RemoveRange(_context.YoutubeVideos);
            _context.Guesses.RemoveRange(_context.Guesses);
            _context.SaveChanges();
        }

        [TestInitialize]
        public void TestInitalize()
        {
            _context.YoutubeVideos.RemoveRange(_context.YoutubeVideos);
            YoutubeVideo yt = new YoutubeVideo("id1", "title1", "channel1", 100)
            {
                LastUpdated = new DateTime()
            };
            _context.YoutubeVideos.Add(yt);
            _context.YoutubeVideos.Add(new YoutubeVideo("id2", "title2", "channel2", 200));
            _context.YoutubeVideos.Add(new YoutubeVideo("id3", "title3", "channel3", 300));
            _context.SaveChanges();
        }

        [DataTestMethod]
        public async Task Assert_GetVideos_is_correct()
        {
            ActionResult actionResult = await videosController.GetAsync();
            var okResult = actionResult as OkObjectResult;
            List<YoutubeVideo> videos = okResult.Value as List<YoutubeVideo>;

            Assert.IsTrue(videos.Count == 2);
        }

        [DataTestMethod]
        [DataRow("id1", "id2")]
        [DataRow("id2", "id1")]
        public async Task Assert_CompareViewCount_is_correct(string videoId1, string videoId2)
        {
            ActionResult actionResult = await videosController.CompareViewCountAsync(videoId1, videoId2);
            var okResult = actionResult as OkObjectResult;
            List<VideoView> videos = okResult.Value as List<VideoView>;

            VideoView id1 = videos.Where(v => v.VideoId == "id1").FirstOrDefault();
            VideoView id2 = videos.Where(v => v.VideoId == "id2").FirstOrDefault();
            Assert.IsTrue(id2.ViewCount > id1.ViewCount);
        }

        [DataTestMethod]
        [DataRow("asdasdasd", "asdaspfaspfas")]
        [DataRow("id1", "id1")]
        [DataRow("", "")]
        [DataRow(null, null)]
        [DataRow("id1", "")]
        [DataRow("id1", null)]
        [DataRow("", "id1")]
        [DataRow(null, "id1")]
        public async Task Assert_CompareViewCount_videoId_is_false(string videoId1, string videoId2)
        {
            StatusCodeResult result = (StatusCodeResult) await videosController.CompareViewCountAsync(videoId1, videoId2);
            Assert.AreEqual(400, result.StatusCode);
        }

        [TestMethod]
        public async Task Assert_GetVideoByVideoId_is_correct()
        {
            ActionResult actionResult = await videosController.GetVideoByVideoIdAsync("id1");
            var okResult = actionResult as OkObjectResult;
            YoutubeVideo video = okResult.Value as YoutubeVideo;
            Assert.AreEqual("id1", video.VideoId);
            Assert.AreEqual("title1", video.Title);
            Assert.AreEqual("channel1", video.ChannelName);
        }

        [TestMethod]
        public async Task Assert_GetVideoByVideoId_videoid_is_null()
        {
            StatusCodeResult result = (StatusCodeResult) await videosController.GetVideoByVideoIdAsync(null);
            Assert.AreEqual(400, result.StatusCode);
        }

        [TestMethod]
        public async Task Assert_GetVideoByVideoId_videoid_is_invalid()
        {
            ActionResult actionResult = await videosController.GetVideoByVideoIdAsync("invalid videoid");
            var okResult = actionResult as OkObjectResult;
            YoutubeVideo video = okResult.Value as YoutubeVideo;
            Assert.IsNull(video);
        }

        [TestMethod]
        public async Task Assert_DeleteOldVideos_is_correct()
        {
            ActionResult actionResult = await videosController.DeleteOldVideosAsync();
            var okResult = actionResult as OkObjectResult;
            int videosDeleted = (int) okResult.Value;
            Assert.AreEqual(4, videosDeleted);
        }
    }
}
 