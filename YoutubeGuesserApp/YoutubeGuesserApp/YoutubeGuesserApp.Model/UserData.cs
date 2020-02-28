using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YoutubeGuesserApp.Model
{
    public class UserData
    {
        private string _Browser;
        private string _OperatingSystem;
        private string _IpAddress;
        private string _Language;
        private string _Referrer;
        private bool _IsMobile;

        public string Browser { get => _Browser; set => _Browser = value; }
        public string OperatingSystem { get => _OperatingSystem; set => _OperatingSystem = value; }
        public string IpAddress { get => _IpAddress; set => _IpAddress = value; }
        public string Language { get => _Language; set => _Language = value; }
        public string Referrer { get => _Referrer; set => _Referrer = value; }
        public bool IsMobile { get => _IsMobile; set => _IsMobile = value; }

        public UserData(string browser, string operatingSystem, string ipAddress, string language, string referrer, bool isMobile)
        {
            Browser = browser;
            OperatingSystem = operatingSystem;
            IpAddress = ipAddress;
            Language = language;
            Referrer = referrer;
            IsMobile = isMobile;
        }
    }
}