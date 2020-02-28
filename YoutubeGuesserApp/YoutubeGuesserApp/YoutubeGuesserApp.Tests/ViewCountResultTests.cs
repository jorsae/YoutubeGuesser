using Microsoft.VisualStudio.TestTools.UnitTesting;
using YoutubeGuesserApp.Model;

namespace YoutubeGuesserApp.Tests
{
    [TestClass]
    public class ViewCountResultTests
    {
        [TestMethod]
        public void Assert_ViewCountResult_ctor_is_correct()
        {
            ViewCountResult viewCountResult = new ViewCountResult();

            Assert.AreEqual(viewCountResult.Correct, false);
            Assert.AreEqual(viewCountResult.CorrectGuesses, 0);
            Assert.AreEqual(viewCountResult.VideoGuessed, null);
            Assert.AreEqual(viewCountResult.VideoNotGuessed, null);
        }

        [TestMethod]
        public void Assert_ViewCountResult_ctor_with_values_is_correct()
        {
            bool correct = true;
            int correctGuesses = 152;
            VideoView videoView = new VideoView("videoId", 125639876);

            ViewCountResult viewCountResult = new ViewCountResult(correct, correctGuesses, videoView, videoView);

            Assert.AreEqual(viewCountResult.Correct, correct);
            Assert.AreEqual(viewCountResult.CorrectGuesses, correctGuesses);
            Assert.AreEqual(viewCountResult.VideoGuessed.VideoId, videoView.VideoId);
            Assert.AreEqual(viewCountResult.VideoGuessed.ViewCount, videoView.ViewCount);
            Assert.AreEqual(viewCountResult.VideoNotGuessed.VideoId, videoView.VideoId);
            Assert.AreEqual(viewCountResult.VideoNotGuessed.ViewCount, videoView.ViewCount);
        }
    }
}