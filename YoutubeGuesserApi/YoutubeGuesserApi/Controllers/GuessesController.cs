using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YoutubeGuesserApi.DataAccess;
using YoutubeGuesserApi.Library.Utility;

namespace YoutubeGuesserApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GuessesController : Controller
    {
        private readonly DatabaseContext _context;
        public GuessesController(DatabaseContext context)
        {
            _context = context;
        }

        [HttpGet("statistics")]
        public ActionResult GetGuessesLastDays(IFormCollection data)
        {
            if (data == null)
                return BadRequest();

            string password = data["password"];
            if (password != Constants.ADMIN_API_PASSWORD)
                return Unauthorized();

            int.TryParse(data["days"], out int days);
            if (days < 0)
                return BadRequest();

            // Get stat for each individual day
            List<dynamic> guessesStats = new List<dynamic>();
            for (int i = 0; i < days + 1; i++)
            {
                DateTime dateTime = DateTime.Now.Date.AddDays(-i);
                int correctGuesses = _context.Guesses.Where(g => g.Date.Date == dateTime).Sum(g => g.CorrectGuesses);
                int wrongGuesses = _context.Guesses.Where(g => g.Date.Date == dateTime).Sum(g => g.WrongGuesses);
                int total = correctGuesses + wrongGuesses;

                var guessesStat = new { Total = total, CorrectGuesses = correctGuesses, WrongGuesses = wrongGuesses, Date = dateTime, DaysAgo = i };
                guessesStats.Add(guessesStat);
            }

            // Get stat total for all days combined
            DateTime dateTimeAllDays = DateTime.Now.Date.AddDays(-days);
            int totalCorrectGuesses = _context.Guesses.Where(g => g.Date.Date >= dateTimeAllDays).Sum(g => g.CorrectGuesses);
            int totalWrongGuesses = _context.Guesses.Where(g => g.Date.Date >= dateTimeAllDays).Sum(g => g.WrongGuesses);
            int totalGuesses = totalCorrectGuesses + totalWrongGuesses;

            var totalStats = new
            {
                TotalDays = days,
                TotalGuesses = totalGuesses,
                TotalCorrectGuesses = totalCorrectGuesses,
                TotalWrongGuesses = totalWrongGuesses,
                GuessesPerDay = guessesStats
            };
            return Ok(totalStats);
        }
    }
}