using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YoutubeGuesserApi.DataAccess;
using YoutubeGuesserApi.Model;

namespace YoutubeGuesserApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfanitiesController : Controller
    {
        private readonly DatabaseContext _context;

        public ProfanitiesController(DatabaseContext context)
        {
            _context = context;
        }

        [HttpGet("{alias}")]
        public ActionResult GetIsProfanity(string alias)
        {
            if (string.IsNullOrEmpty(alias))
                return BadRequest(false);

            // if true contains profanity, so we have to inverse the result
            int profanities = (from prof in _context.Profanities
                             where EF.Functions.Like(alias.ToLower(), $"%{prof.Word}%")
                             select prof).Count();

            if (profanities > 0)
                return Ok(true);
            else
                return Ok(false);
        }

        [HttpPost("{word}")]
        public async Task<ActionResult> PostProfanityAsync(string word)
        {
            if (string.IsNullOrEmpty(word))
                return BadRequest();

            word = word.ToLower();
            // Delete all unnecessary entities. e.g. word is "fuck", delete "fucker" from profanities.
            // As if it's marked for fucker, it's going to be marked by fuck and therefore no longer necessary.
            List<Profanity> profanities = _context.Profanities.Where(w => w.Word.Contains(word)).ToList();
            foreach(Profanity profanity in profanities)
            {
                _context.Profanities.Remove(profanity);
            }

            if (_context.Profanities.Any(w => word.Contains(w.Word)))
                return StatusCode(StatusCodes.Status409Conflict);

            _context.Profanities.Add(new Profanity(word));

            bool savedChanges = await SaveChangesDbAsync();
            if (savedChanges)
                return Ok();
            else
                return StatusCode(StatusCodes.Status500InternalServerError);
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