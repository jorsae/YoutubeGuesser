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
    public class FeedbackController : Controller
    {
        private readonly DatabaseContext _context;

        public FeedbackController(DatabaseContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllFeedback()
        {
            List<Feedback> feedback = _context.Feedback.Where(f => f.IsRead == false).ToList();
            foreach(Feedback fb in feedback)
            {
                fb.IsRead = true;
                _context.Entry(fb).State = EntityState.Modified;
            }
            
            await SaveChangesDbAsync();
            return Ok(feedback);
        }

        [HttpPost]
        public async Task<ActionResult> PostFeedbackAsync(Feedback feedback)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (feedback == null)
                return BadRequest();

            if (feedback.Id != 0)
                return BadRequest();

            if (string.IsNullOrEmpty(feedback.Title) || string.IsNullOrEmpty(feedback.Body))
                return BadRequest();

            _context.Feedback.Add(feedback);
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