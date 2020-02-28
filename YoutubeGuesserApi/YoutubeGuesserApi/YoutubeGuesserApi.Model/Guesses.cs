using System;
using System.ComponentModel.DataAnnotations;

namespace YoutubeGuesserApi.Model
{
    public class Guesses
    {
        private DateTime _Date;
        private int _CorrectGuesses;
        private int _WrongGuesses;
        [Key]
        public DateTime Date { get => _Date; set => _Date = value; }
        public int CorrectGuesses { get => _CorrectGuesses; set => _CorrectGuesses = value; }
        public int WrongGuesses { get => _WrongGuesses; set => _WrongGuesses = value; }

        public Guesses()
        {
            CorrectGuesses = 0;
            WrongGuesses = 0;
            Date = DateTime.Now.Date;
        }
    }
}