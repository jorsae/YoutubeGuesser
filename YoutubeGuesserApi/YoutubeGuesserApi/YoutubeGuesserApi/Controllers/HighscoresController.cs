using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YoutubeGuesserApi.DataAccess;
using YoutubeGuesserApi.Model;

namespace YoutubeGuesserApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HighscoresController : Controller
    {
        private readonly DatabaseContext _context;

        public HighscoresController(DatabaseContext context)
        {
            _context = context;
        }

        [HttpGet("{count}")]
        public ActionResult GetHighscores(int count)
        {
            if (count <= 0)
                return BadRequest();

            List<Highscore> highscores = _context.Highscores.OrderByDescending(highscore => highscore.Score)
                                                            .Take(count).ToList();

            return Ok(highscores);
        }

        [HttpPost]
        public async Task<ActionResult> PostHighscoreAsync(Highscore highscore)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (highscore == null)
                return BadRequest();

            if (highscore.Id != 0)
                return BadRequest();

            // Alias contains profanity
            if (GetProfanity(highscore.Alias))
                return StatusCode(StatusCodes.Status403Forbidden);

            highscore.DateAdded = DateTime.Now;
            _context.Highscores.Add(highscore);

            bool savedChanges = await SaveChangesDbAsync();
            if (savedChanges)
                return Ok();
            else
                return StatusCode(StatusCodes.Status500InternalServerError);
        }

        private bool GetProfanity(string word)
        {
            word = word.ToLower();

            if (_context.Profanities.Any(w => word.Contains(w.Word)))
                return true;
            else
                return false;
        }

        private async Task<bool> SaveChangesDbAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}