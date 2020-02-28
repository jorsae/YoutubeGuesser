using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YoutubeGuesserApp.Api;
using YoutubeGuesserApp.Library;
using YoutubeGuesserApp.Model;
using YoutubeGuesserApp.Models;

namespace YoutubeGuesserApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HighscoreApiController : Controller
    {
        private readonly YoutubeGuesserApi ygApi = new YoutubeGuesserApi();

        [HttpPost]
        public async Task<ActionResult> PostHighscoreAsync([FromBody]string alias)
        {
            int? submittedHighscore = HttpContext.Session.GetInt32(Constants.SESSION_SUBMITTED_TO_HIGHSCORES_KEY);
            var responseMessage = new Dictionary<string, string>
            {
                { "errorMessage", "Something went wrong" }
            };

            if (submittedHighscore == 1)
            {
                responseMessage["errorMessage"] = "Highscore have already been submitted";
                return BadRequest(responseMessage);
            }

            bool result = alias.All(Char.IsLetterOrDigit);
            if (!result)
            {
                responseMessage["errorMessage"] = "Alias contains non alphanumeric values";
                return BadRequest(responseMessage);
            }

            if (alias == null || alias == String.Empty)
            {
                responseMessage["errorMessage"] = "You have to write an alias";
                return BadRequest(responseMessage);
            }

            bool isProfanity = await ygApi.IsAliasProfanityAsync(alias);
            if (isProfanity)
            {
                responseMessage["errorMessage"] = "Alias contains profanity";
                return BadRequest(responseMessage);
            }

            int? guessesCorrect = HttpContext.Session.GetInt32(Constants.SESSION_CORRECT_GUESSES_KEY);
            if (guessesCorrect == null)
                guessesCorrect = 0;

            string ip = ServerUtility.GetIpAddress(Request, HttpContext);
            string location = await ygApi.GetLocationByIpAsync(ip);

            Highscore highscoreEntry = new Highscore(alias, (int)guessesCorrect, location);

            bool highscorePosted = await ygApi.PostHighscoreAsync(highscoreEntry);
            if (highscorePosted)
                HttpContext.Session.SetInt32(Constants.SESSION_SUBMITTED_TO_HIGHSCORES_KEY, 1);
            return Ok(highscorePosted);
        }
    }
}