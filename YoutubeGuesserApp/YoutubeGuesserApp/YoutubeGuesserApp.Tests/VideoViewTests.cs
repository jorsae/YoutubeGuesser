using Microsoft.VisualStudio.TestTools.UnitTesting;
using YoutubeGuesserApp.Model;

namespace YoutubeGuesserApp.Tests
{
    [TestClass]
    public class VideoViewTests
    {
        [TestMethod]
        public void Assert_VideoView_ctor_is_correct()
        {
            string videoId = "asasf";
            long viewCount = 909210251;
            VideoView videoView = new VideoView(videoId, viewCount);

            Assert.AreEqual(videoView.VideoId, videoId);
            Assert.AreEqual(videoView.ViewCount, viewCount);
        }
    }
}