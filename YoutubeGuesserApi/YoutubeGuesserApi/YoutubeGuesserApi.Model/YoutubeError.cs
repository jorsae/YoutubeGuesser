namespace YoutubeGuesserApi.Model
{
    public class YoutubeError
    {
        private int _StatusCode;
        private string _Domain;
        private string _Reason;
        private string _Message;

        public int StatusCode { get => _StatusCode; set => _StatusCode = value; }
        public string Domain { get => _Domain; set => _Domain = value; }
        public string Reason { get => _Reason; set => _Reason = value; }
        public string Message { get => _Message; set => _Message = value; }

        public YoutubeError(int statusCode, string domain, string reason, string message)
        {
            StatusCode = statusCode;
            Domain = domain;
            Reason = reason;
            Message = message;
        }
    }
}