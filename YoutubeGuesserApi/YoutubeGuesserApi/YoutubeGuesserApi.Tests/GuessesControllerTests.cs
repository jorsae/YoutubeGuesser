using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using YoutubeGuesserApi.Controllers;
using YoutubeGuesserApi.DataAccess;
using YoutubeGuesserApi.Model;
using YoutubeGuesserApi.Library.Utility;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace YoutubeGuesserApi.Tests
{
    [TestClass]
    public class GuessesControllerTests
    {
        private static DatabaseContext _context;
        private static GuessesController guessesController;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            _context = new DatabaseContext("Server=(localdb)\\mssqllocaldb;Database=YoutubeGuesserApiDB_Tests;Trusted_Connection=True;MultipleActiveResultSets=true");
            guessesController = new GuessesController(_context);

            DateTime currentDay = DateTime.Now.Date;
            _context.Add(new Guesses() { Date = currentDay,             CorrectGuesses = 5, WrongGuesses = 2 });
            _context.Add(new Guesses() { Date = currentDay.AddDays(-1), CorrectGuesses = 1, WrongGuesses = 2 });
            _context.Add(new Guesses() { Date = currentDay.AddDays(-2), CorrectGuesses = 3, WrongGuesses = 4 });
            _context.Add(new Guesses() { Date = currentDay.AddDays(-3), CorrectGuesses = 5, WrongGuesses = 10 });
            _context.Add(new Guesses() { Date = currentDay.AddDays(-4), CorrectGuesses = 10, WrongGuesses = 10 });
            _context.Add(new Guesses() { Date = currentDay.AddDays(-5), CorrectGuesses = 10, WrongGuesses = 10 });
            _context.SaveChanges();

        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            _context.Guesses.RemoveRange(_context.Guesses);
            _context.SaveChanges();
        }

        /*
        [DataTestMethod]
        [DataRow(0, 5, 2)]
        [DataRow(1, 6, 4)]
        [DataRow(2, 9, 8)]
        [DataRow(3, 14, 18)]
        public void Assert_GetGuessesLastDays_is_correct(int days, int expectedCorrect, int expectedWrong)
        {
            var fc = new FormCollection(new Dictionary<string, StringValues>
            {
                { "password", Constants.ADMIN_API_PASSWORD },
                { "days", days.ToString() }
            });
            ActionResult actionResult = guessesController.GetGuessesLastDays(fc);
            string data = actionResult.ToString();
            dynamic result = JObject.Parse(data);

            int a = result.TotalGuesses;

            Assert.AreEqual(result.TotalGuesses, expectedCorrect + expectedWrong);
            Assert.AreEqual(result.guessesPerDay[days].correctGuesses, expectedCorrect);
            Assert.AreEqual(result.guessesPerDay[days].wrongGuesses, expectedWrong);
        }*/

        [DataTestMethod]
        [DataRow(-1)]
        [DataRow(-100)]
        public void Assert_GetGuessesLastDays_negative_input(int days)
        {
            var fc = new FormCollection(new Dictionary<string, StringValues>
            {
                { "password", Constants.ADMIN_API_PASSWORD },
                { "days", days.ToString() }
            });
            StatusCodeResult result = (StatusCodeResult) guessesController.GetGuessesLastDays(fc);
            Assert.AreEqual(400, result.StatusCode);
        }
    }
}