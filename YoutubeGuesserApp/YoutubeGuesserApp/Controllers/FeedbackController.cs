using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using YoutubeGuesserApp.Api;
using YoutubeGuesserApp.Model;

namespace YoutubeGuesserApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbackController : Controller
    {
        private YoutubeGuesserApi ygApi = new YoutubeGuesserApi();

        [HttpPost]
        public async Task<ActionResult<bool>> PostUserDataAsync(Feedback feedback)
        {
            bool success = await ygApi.PostFeedbackAsync(feedback);
            return Ok(success);
        }
    }
}