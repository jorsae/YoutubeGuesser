using Microsoft.AspNetCore.Http;

namespace YoutubeGuesserApp.Models
{
    public class ServerUtility
    {
        public static string GetIpAddress(HttpRequest request, HttpContext context)
        {
            // Fetch the user's ip address
            string ip = request.Headers["X-Client-IP"];
            if (string.IsNullOrEmpty(ip))
            {
                ip = context.Connection.RemoteIpAddress.ToString();
            }
            return ip;
        }
    }
}