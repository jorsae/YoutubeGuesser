using Microsoft.VisualStudio.TestTools.UnitTesting;
using YoutubeGuesserApp.Model;

namespace YoutubeGuesserApp.Tests
{
    [TestClass]
    public class YoutubeGuesserApiTests
    {
        private static YoutubeVideo youtubeVideo;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            youtubeVideo = new YoutubeVideo("pNVPJeYZXT8", "title", "channelName");
        }

        [DataTestMethod]
        [DataRow(ThumbnailQuality.Low, "https://i.ytimg.com/vi/pNVPJeYZXT8/default.jpg")]
        [DataRow(ThumbnailQuality.Medium, "https://i.ytimg.com/vi/pNVPJeYZXT8/mqdefault.jpg")]
        [DataRow(ThumbnailQuality.High, "https://i.ytimg.com/vi/pNVPJeYZXT8/hqdefault.jpg")]
        [DataRow(ThumbnailQuality.MaxResolution, "https://i.ytimg.com/vi/pNVPJeYZXT8/maxresdefault.jpg")]
        public void Assert_GetThumbnail_is_correct(ThumbnailQuality thumbnailQuality, string expected)
        {
            string thumbnailUrl = youtubeVideo.GetThumbnail(thumbnailQuality);
            Assert.AreEqual(thumbnailUrl, expected);
        }

        [TestMethod]
        public void Assert_GetThumbnail_default_is_correct()
        {
            string expected = "https://i.ytimg.com/vi/pNVPJeYZXT8/hqdefault.jpg";
            string thumbnailUrl = youtubeVideo.GetThumbnail();
            Assert.AreEqual(thumbnailUrl, expected);
        }
    }
}