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
    public class HighscoresControllerTests
    {
        private static DatabaseContext _context;
        private static HighscoresController highscoresController;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            _context = new DatabaseContext("Server=(localdb)\\mssqllocaldb;Database=YoutubeGuesserApiDB_Tests;Trusted_Connection=True;MultipleActiveResultSets=true");
            _context.Profanities.Add(new Profanity("fuck"));
            _context.Highscores.Add(new Highscore("alias", 5, "location"));
            _context.Highscores.Add(new Highscore("alias2", 3, "location"));
            _context.Highscores.Add(new Highscore("alias3", 6, "location"));
            _context.SaveChanges();
            highscoresController = new HighscoresController(_context);
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            _context.Highscores.RemoveRange(_context.Highscores);
            _context.Profanities.RemoveRange(_context.Profanities);
            _context.SaveChanges();
        }

        [TestMethod]
        public async Task Assert_PostHighscoreAsync_is_correct()
        {
            StatusCodeResult result = (StatusCodeResult) await highscoresController.PostHighscoreAsync(new Highscore("alias", 3, "location"));
            Assert.AreEqual(200, result.StatusCode);
        }

        [TestMethod]
        public async Task Assert_PostHighscoreAsync_highscore_is_null()
        {
            StatusCodeResult result = (StatusCodeResult) await highscoresController.PostHighscoreAsync(null);
            Assert.AreEqual(400, result.StatusCode);
        }

        [TestMethod]
        public async Task Assert_PostHighscoreAsync_manually_set_id()
        {
            Highscore highscore = new Highscore("alias", 3, "location")
            {
                Id = 9001
            };

            StatusCodeResult result = (StatusCodeResult) await highscoresController.PostHighscoreAsync(highscore);
            Assert.AreEqual(400, result.StatusCode);
        }

        [DataTestMethod]
        [DataRow(403, "afucka")]
        [DataRow(403, "fuck")]
        [DataRow(403, "fuckfuck")]
        public async Task Assert_PostHighscoreAsync_profanity_name(int expected, string alias)
        {
            StatusCodeResult result = (StatusCodeResult)await highscoresController.PostHighscoreAsync(new Highscore(alias, 300, "location"));
            Assert.AreEqual(expected, result.StatusCode);
        }

        [DataTestMethod]
        [DataRow(1, 1)]
        [DataRow(2, 2)]
        public void Assert_GetHighscores_is_correct(int expected, int count)
        {
            ActionResult result = highscoresController.GetHighscores(count);
            var okResult = result as OkObjectResult;
            List<Highscore> highscores = okResult.Value as List<Highscore>;

            Assert.AreEqual(expected, highscores.Count);
        }

        [DataTestMethod]
        [DataRow(400, 0)]
        [DataRow(400, -5)]
        [DataRow(400, -10000)]
        public void Assert_GetHighscores_zero_or_negative_count(int expected, int count)
        {
            StatusCodeResult result = (StatusCodeResult) highscoresController.GetHighscores(count);
            Assert.AreEqual(expected, result.StatusCode);
        }
    }
}