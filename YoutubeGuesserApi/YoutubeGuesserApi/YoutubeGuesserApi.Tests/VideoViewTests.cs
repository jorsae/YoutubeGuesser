using Microsoft.VisualStudio.TestTools.UnitTesting;
using YoutubeGuesserApi.Model;

namespace YoutubeGuesserApi.Tests
{
    [TestClass]
    public class VideoViewTests
    {
        private static YoutubeVideo youtubeVideo;

        [ClassInitialize]
        public static void ClassInitalize(TestContext context)
        {
            youtubeVideo = new YoutubeVideo("pNVPJeYZXT8", "title", "channelName", viewCount: 5912521);
        }

        [TestMethod]
        public void Assert_VideoView_ctor_YoutubeVideo_is_correct()
        {
            VideoView videoView = new VideoView(youtubeVideo.VideoId, youtubeVideo.ViewCount);
            Assert.AreEqual(videoView.VideoId, youtubeVideo.VideoId);
            Assert.AreEqual(videoView.ViewCount, youtubeVideo.ViewCount);
        }

        [TestMethod]
        public void Assert_VideoView_ctor_is_correct()
        {
            string videoId = "asdas";
            long viewCount = 4510257192851;

            VideoView videoView = new VideoView(videoId, viewCount);
            Assert.AreEqual(videoView.VideoId, videoId);
            Assert.AreEqual(videoView.ViewCount, viewCount);
        }
    }
}