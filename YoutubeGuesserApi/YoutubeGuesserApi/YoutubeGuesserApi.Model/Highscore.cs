using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using YoutubeGuesserApi.Library.Utility;

namespace YoutubeGuesserApi.Model
{
    public class Highscore
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        private int _Id;
        private string _Alias;
        private int _Score;
        private string _Location;
        private DateTime _DateAdded;

        public int Id { get => _Id; set => _Id = value; }

        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Use letters only please")]
        public string Alias { get => _Alias; set => _Alias = value; }
        public int Score { get => _Score; set => _Score = value; }
        public string Location { get => _Location; set => _Location = value; }
        public DateTime DateAdded { get => _DateAdded; set => _DateAdded = value; }

        public Highscore(string alias, int score, string location)
        {
            Alias = alias;
            Score = score;
            Location = location;
            DateAdded = DateTime.Now;
        }
    }
}