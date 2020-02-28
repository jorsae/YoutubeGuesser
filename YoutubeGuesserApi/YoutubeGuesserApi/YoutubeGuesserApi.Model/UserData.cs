using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using YoutubeGuesserApi.Library.Utility;

namespace YoutubeGuesserApi.Model
{
    public class UserData
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        private int _Id;
        private string _Browser;
        private string _OperatingSystem;
        private string _IpAddress;
        private string _Location;
        private string _Language;
        private string _Referrer;
        private bool _IsMobile;
        private DateTime _DateAdded;

        public int Id { get => _Id; set => _Id = value; }
        public string Browser { get => _Browser; set => _Browser = value; }
        public string OperatingSystem { get => _OperatingSystem; set => _OperatingSystem = value; }
        public string IpAddress { get => _IpAddress; set => _IpAddress = value; }
        public string Location { get => _Location; set => _Location = value; }
        public string Language { get => _Language; set => _Language = value; }
        public string Referrer { get => _Referrer; set => _Referrer = value; }
        public bool IsMobile { get => _IsMobile; set => _IsMobile = value; }
        public DateTime DateAdded { get => _DateAdded; set => _DateAdded = value; }

        public UserData(string browser, string operatingSystem, string ipAddress, string location, string language, string referrer, bool isMobile)
        {
            Browser = browser;
            OperatingSystem = operatingSystem;
            IpAddress = ipAddress;
            Location = location;
            Language = language;
            Referrer = referrer;
            IsMobile = isMobile;
            DateAdded = DateTime.Now;
        }
    }
}