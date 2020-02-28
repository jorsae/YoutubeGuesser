namespace YoutubeGuesserApp.Model
{
    public class Feedback
    {
        private string _Title;
        private string _Email;
        private string _Body;
        private string _Browser;
        private string _OperatingSystem;

        public string Title { get => _Title; set => _Title = value; }
        public string Email { get => _Email; set => _Email = value; }
        public string Body { get => _Body; set => _Body = value; }
        public string Browser { get => _Browser; set => _Browser = value; }
        public string OperatingSystem { get => _OperatingSystem; set => _OperatingSystem = value; }

        public Feedback(string title, string email, string body, string browser, string operatingSystem)
        {
            Title = title;
            Email = email;
            Body = body;
            Browser = browser;
            OperatingSystem = operatingSystem;
        }
    }
}