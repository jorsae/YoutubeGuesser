using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YoutubeGuesserApi.Model
{
    public class Feedback
    {
        private int _Id;
        private string _Title;
        private string _Email;
        private string _Body;
        private string _Browser;
        private string _OperatingSystem;
        private bool _IsRead;
        private DateTime _DateAdded;

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get => _Id; set => _Id = value; }
        public string Title { get => _Title; set => _Title = value; }
        public string Email { get => _Email; set => _Email = value; }
        public string Body { get => _Body; set => _Body = value; }
        public string Browser { get => _Browser; set => _Browser = value; }
        public string OperatingSystem { get => _OperatingSystem; set => _OperatingSystem = value; }
        public bool IsRead { get => _IsRead; set => _IsRead = value; }
        public DateTime DateAdded { get => _DateAdded; set => _DateAdded = value; }

        public Feedback(string title, string email, string body, string browser, string operatingSystem)
        {
            Title = title;
            Email = email;
            Body = body;
            Browser = browser;
            OperatingSystem = operatingSystem;
            IsRead = false;
            DateAdded = DateTime.Now;
        }
    }
}