using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using YoutubeGuesserApi.Controllers;
using YoutubeGuesserApi.DataAccess;
using YoutubeGuesserApi.Model;

namespace YoutubeGuesserApi.Tests
{
    [TestClass]
    public class ProfanitiesControllerTests
    {
        private static DatabaseContext _context;
        private static ProfanitiesController profanitiesController;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            _context = new DatabaseContext("Server=(localdb)\\mssqllocaldb;Database=YoutubeGuesserApiDB_Tests;Trusted_Connection=True;MultipleActiveResultSets=true");
            profanitiesController = new ProfanitiesController(_context);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _context.Profanities.RemoveRange(_context.Profanities);
            _context.SaveChanges();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _context.Profanities.RemoveRange(_context.Profanities);
            _context.Profanities.Add(new Profanity("fuck"));
            _context.SaveChanges();
        }

        [DataTestMethod]
        [DataRow(true, "afucka")]
        [DataRow(true, "fuckfuck")]
        [DataRow(true, "fuck")]
        [DataRow(false, "fuc")]
        [DataRow(false, "fuk")]
        public void Assert_GetIsProfanity(bool expected, string alias)
        {
            ActionResult actionResult = profanitiesController.GetIsProfanity(alias);
            var okResult = actionResult as OkObjectResult;
            bool result = (bool)okResult.Value;
            Assert.AreEqual(expected, result);
        }


        [DataTestMethod]
        [DataRow(400, null)]
        [DataRow(400, "")]
        public void Assert_GetIsProfanity_empty_alias(int expected, string alias)
        {
            ActionResult actionResult = profanitiesController.GetIsProfanity(alias);
            var badResult = actionResult as BadRequestObjectResult;
            Assert.AreEqual(expected, badResult.StatusCode);
        }

        [DataTestMethod]
        [DataRow(200, "pancakessssss")]
        [DataRow(200, "a")]
        [DataRow(400, "")]
        [DataRow(400, null)]
        public async Task Assert_PostProfanity_is_correct(int expected, string profanity)
        {
            StatusCodeResult result = (StatusCodeResult) await profanitiesController.PostProfanityAsync(profanity);
            Assert.AreEqual(expected, result.StatusCode);
        }
    }
}