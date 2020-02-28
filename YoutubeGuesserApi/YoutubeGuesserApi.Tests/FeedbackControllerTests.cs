using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using YoutubeGuesserApi.Controllers;
using YoutubeGuesserApi.DataAccess;
using YoutubeGuesserApi.Model;

namespace YoutubeGuesserApi.Tests
{

    [TestClass]
    public class FeedbackControllerTests
    {
        private static DatabaseContext _context;
        private static FeedbackController feedbackController;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            _context = new DatabaseContext("Server=(localdb)\\mssqllocaldb;Database=YoutubeGuesserApiDB_Tests;Trusted_Connection=True;MultipleActiveResultSets=true");
            feedbackController = new FeedbackController(_context);
            Feedback fb = new Feedback("a", "s", "d", "d", "a")
            {
                IsRead = true
            };
            _context.Feedback.Add(fb);
            _context.Feedback.Add(new Feedback("a", "s", "d", "d", "a"));
            _context.SaveChanges();
        }

        [TestCleanup]
        public void ClassCleanup()
        {
            _context.Feedback.RemoveRange(_context.Feedback);
            _context.SaveChanges();
        }

        [TestMethod]
        public async Task Assert_PostFeedbackAsync_is_correct()
        {
            StatusCodeResult result = (StatusCodeResult) await feedbackController.PostFeedbackAsync(new Feedback("a", "s", "d", "d", "a"));
            Assert.AreEqual(200, result.StatusCode);
        }

        [TestMethod]
        public async Task Assert_PostFeedbackAsync_feedback_is_null()
        {
            StatusCodeResult result = (StatusCodeResult) await feedbackController.PostFeedbackAsync(null);
            Assert.AreEqual(400, result.StatusCode);
        }

        [TestMethod]
        public async Task Assert_PostFeedbackAsync_manually_set_id()
        {
            Feedback feedback2 = new Feedback("a", "s", "d", "d", "a")
            {
                Id = 9001
            };
            StatusCodeResult result = (StatusCodeResult) await feedbackController.PostFeedbackAsync(feedback2);
            Assert.AreEqual(400, result.StatusCode);
        }

        [TestMethod]
        public async Task Assert_GetAllFeedback_is_correct()
        {
            ActionResult result = await feedbackController.GetAllFeedback();
            var okResult = result as OkObjectResult;
            List<Feedback> feedback = okResult.Value as List<Feedback>;
            
            Assert.AreEqual(1, feedback.Count);
        }

        [TestMethod]
        public async Task Assert_GetAllFeedback_marked_IsRead()
        {
            ActionResult result = await feedbackController.GetAllFeedback();
            var okResult = result as OkObjectResult;
            List<Feedback> feedback = okResult.Value as List<Feedback>;

            foreach(Feedback f in feedback)
            {
                Assert.AreEqual(true, f.IsRead);
            }
        }
    }
}