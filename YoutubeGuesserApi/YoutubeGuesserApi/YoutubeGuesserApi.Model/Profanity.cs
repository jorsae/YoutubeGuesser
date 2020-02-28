using System;
using System.ComponentModel.DataAnnotations;
using YoutubeGuesserApi.Library.Utility;

namespace YoutubeGuesserApi.Model
{
    public class Profanity
    {
        private string _Word;
        private DateTime _DateAdded;

        [Key]
        public string Word { get => _Word; set => _Word = value; }
        public DateTime DateAdded { get => _DateAdded; set => _DateAdded = value; }

        public Profanity(string word)
        {
            Word = word;
            DateAdded = DateTime.Now;
        }

        public Profanity(string word, DateTime dateAdded)
        {
            Word = word;
            DateAdded = dateAdded;
        }
    }
}
