using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using YoutubeGuesserApp.Api;
using YoutubeGuesserApp.Model;
using YoutubeGuesserApp.Models;

namespace YoutubeGuesserApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HarvestController : Controller
    {
        private readonly YoutubeGuesserApi ygApi = new YoutubeGuesserApi();

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult<bool>> PostUserDataAsync(UserData userData)
        {
            userData.IpAddress = ServerUtility.GetIpAddress(Request, HttpContext);

            bool success = await ygApi.PostUserDataAsync(userData);
            return Ok(success);
        }
    }
}