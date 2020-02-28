using Microsoft.AspNetCore.Mvc;

namespace YoutubeGuesserApp.Controllers
{
    [Route("error")]
    public class ErrorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Route("404")]
        public ActionResult PageNotFound()
        {
            return View();
        }
    }
}