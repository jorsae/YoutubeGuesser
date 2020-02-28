using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using YoutubeGuesserApi.DataAccess;
using YoutubeGuesserApi.Library.Utility;
using YoutubeGuesserApi.Model;

namespace YoutubeGuesserApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HarvestsController : Controller
    {
        private readonly DatabaseContext _context;

        public HarvestsController(DatabaseContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult> PostUserDataAsync(UserData userData)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (userData == null)
                return BadRequest();

            if (userData.Id != 0)
                return BadRequest();

            userData.Location = GetLocationFromIp(userData.IpAddress);

            _context.Add(userData);
            bool savedChanges = await SaveChangesDbAsync();
            if (savedChanges)
                return Ok();
            else
                return StatusCode(StatusCodes.Status500InternalServerError);
        }

        [HttpGet("geo/{ip}")]
        public ActionResult GetGeoLocation(string ip)
        {
            string location = GetLocationFromIp(ip);
            if(location != null)
                return Ok(location);
            else
                return StatusCode(StatusCodes.Status500InternalServerError);
        }

        [HttpPost("device/")]
        public ActionResult GetDevices(IFormCollection data)
        {
            if (data == null)
                return BadRequest();

            string password = data["password"];
            if (password != Constants.ADMIN_API_PASSWORD)
                return Unauthorized();

            int mobile = _context.UserData.Count(d => d.IsMobile == true);
            int pc = _context.UserData.Count(d => d.IsMobile == false);
            int total = mobile + pc;

            Dictionary<string, int> devices = new Dictionary<string, int>
            {
                { "total", total },
                { "pc", pc },
                { "mobile", mobile }
            };

            return Ok(devices);
        }

        [HttpPost("distinct/")]
        public ActionResult GetDistinctType(IFormCollection data)
        {
            if (data == null)
                return BadRequest();

            string password = data["password"];
            string type = data["type"];
            if (password != Constants.ADMIN_API_PASSWORD)
                return Unauthorized();

            if (type == null)
                return BadRequest();

            type = type.ToLower();
            switch (type)
            {
                case "os":
                    return Ok(GetDistinctOperatingSystem());
                case "location":
                    return Ok(GetDistinctLocation());
                case "browser":
                    return Ok(GetDistinctBrowser());
                default:
                    return BadRequest();
            }
        }

        private Dictionary<string, int> GetDistinctOperatingSystem()
        {
            dynamic data = _context.UserData.GroupBy(ud => ud.OperatingSystem)
                            .Select(g => new
                            {
                                OperatingSystem = g.Key,
                                Frequency = g.Select(l => l.OperatingSystem).Count()
                            }).ToList();
            Dictionary<string, int> dict = new Dictionary<string, int>();
            for (int i = 0; i < data.Count; i++)
            {
                string key = data[i].OperatingSystem ?? "Unknown";
                dict.Add(key, data[i].Frequency);
            }
            return dict;
        }

        private Dictionary<string, int> GetDistinctBrowser()
        {
            dynamic data = _context.UserData.GroupBy(ud => ud.Browser)
                            .Select(g => new
                            {
                                Browser = g.Key,
                                Frequency = g.Select(l => l.Browser).Count()
                            }).ToList();
            Dictionary<string, int> dict = new Dictionary<string, int>();
            for (int i = 0; i < data.Count; i++)
            {
                string key = data[i].Browser ?? "Unknown";
                dict.Add(key, data[i].Frequency);
            }
            return dict;
        }

        private Dictionary<string, int> GetDistinctLocation()
        {
            dynamic data = _context.UserData.GroupBy(ud => ud.Location)
                            .Select(g => new
                            {
                                Location = g.Key,
                                Frequency = g.Select(l => l.Location).Count()
                            }).ToList();
            Dictionary<string, int> dict = new Dictionary<string, int>();
            for(int i = 0; i < data.Count; i++)
            {
                string key = data[i].Location ?? "Unknown";
                dict.Add(key, data[i].Frequency);
            }
            return dict;
        }

        private string GetLocationFromIp(string ip)
        {
            try
            {
                string url = GetIpGeoLocationUrl(ip);
                using (var wc = new WebClient())
                {
                    string rawResponse = wc.DownloadString(url);
                    dynamic response = JsonConvert.DeserializeObject(rawResponse);

                    return response.country_name;
                }
            }
            catch
            {
                return Constants.DEFAULT_UNKNOWN_USER_DATA;
            }
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

        private string GetIpGeoLocationUrl(string ip)
        {
            return $"https://api.ipgeolocation.io/ipgeo?apiKey={Constants.IPGEOLOCATION_API_KEY}&ip={ip}";
        }
    }
}