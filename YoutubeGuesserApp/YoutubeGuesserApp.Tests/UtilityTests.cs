using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using YoutubeGuesserApp.Library;
using YoutubeGuesserApp.Model;

namespace YoutubeGuesserApp.Tests
{
    [TestClass]
    public class UtilityTests
    {
        private static List<YoutubeVideoView> videoHistory;
        private static Utility utility;

        [ClassInitialize]
        public static void ClassInitalize(TestContext context)
        {
            videoHistory = new List<YoutubeVideoView>();
            utility = new Utility();
        }

        [TestInitialize]
        public void TestInitalize()
        {
            videoHistory.Clear();
        }

        [TestMethod]
        public void Assert_ReverseGameHistory_is_correct()
        {
            YoutubeVideoView videoView1 = new YoutubeVideoView("a", "a", "a", 5000);
            YoutubeVideoView videoView2 = new YoutubeVideoView("b", "b", "b", 5000);
            YoutubeVideoView videoView3 = new YoutubeVideoView("c", "c", "c", 5000);
            YoutubeVideoView videoView4 = new YoutubeVideoView("d", "d", "d", 5000);

            List<YoutubeVideoView> expectedHistory = new List<YoutubeVideoView>
            {
                videoView3,
                videoView4,
                videoView1,
                videoView2
            };

            videoHistory.Add(videoView1);
            videoHistory.Add(videoView2);
            videoHistory.Add(videoView3);
            videoHistory.Add(videoView4);
            utility.ReverseGameHistory(videoHistory);


            CollectionAssert.AreEqual(videoHistory, expectedHistory);
        }

        [TestMethod]
        public void Assert_ReverseGameHistory_odd_elements_is_correct()
        {
            YoutubeVideoView videoView1 = new YoutubeVideoView("a", "a", "a", 5000);
            YoutubeVideoView videoView2 = new YoutubeVideoView("b", "b", "b", 5000);
            YoutubeVideoView videoView3 = new YoutubeVideoView("c", "c", "c", 5000);

            List<YoutubeVideoView> expectedHistory = new List<YoutubeVideoView>
            {
                videoView3,
                videoView1,
                videoView2
            };

            videoHistory.Add(videoView1);
            videoHistory.Add(videoView2);
            videoHistory.Add(videoView3);
            utility.ReverseGameHistory(videoHistory);

            CollectionAssert.AreEqual(videoHistory, expectedHistory);
        }

        [TestMethod]
        public void Assert_ReverseGameHistory_no_elements_is_correct()
        {
            List<YoutubeVideoView> expectedHistory = new List<YoutubeVideoView>();
            utility.ReverseGameHistory(videoHistory);

            CollectionAssert.AreEqual(videoHistory, expectedHistory);
        }
    }
}