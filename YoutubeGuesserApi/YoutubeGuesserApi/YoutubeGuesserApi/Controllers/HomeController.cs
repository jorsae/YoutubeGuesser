using System;
using Microsoft.AspNetCore.Mvc;

namespace YoutubeGuesserApi.Controllers
{
    [Route("")]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return Ok(DateTime.Now.ToString());
        }
    }
}