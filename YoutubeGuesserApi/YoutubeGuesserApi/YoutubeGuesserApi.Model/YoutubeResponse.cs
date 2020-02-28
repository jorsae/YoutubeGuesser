using System.Net;

namespace YoutubeGuesserApi.Model
{
    public class YoutubeResponse
    {
        private HttpStatusCode _StatusCode;
        private string _Content;

        public HttpStatusCode StatusCode { get => _StatusCode; set => _StatusCode = value; }
        public string Content { get => _Content; set => _Content = value; }

        public YoutubeResponse(HttpStatusCode statusCode, string content)
        {
            StatusCode = statusCode;
            Content = content;
        }
    }
}