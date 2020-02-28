namespace YoutubeGuesserApp.Model
{
    public class ViewCountResult
    {
        private bool _Correct;
        private int _CorrectGuesses;
        private VideoView _VideoGuessed;
        private VideoView _VideoNotGuessed;

        public bool Correct { get => _Correct; set => _Correct = value; }
        public int CorrectGuesses { get => _CorrectGuesses; set => _CorrectGuesses = value; }
        public VideoView VideoGuessed { get => _VideoGuessed; set => _VideoGuessed = value; }
        public VideoView VideoNotGuessed { get => _VideoNotGuessed; set => _VideoNotGuessed = value; }

        public ViewCountResult()
        {
            Correct = false;
            CorrectGuesses = 0;
            VideoGuessed = null;
            VideoNotGuessed = null;
        }

        public ViewCountResult(bool correct, int correctGuesses, VideoView videoGuessed, VideoView videoNotGuessed)
        {
            Correct = correct;
            CorrectGuesses = correctGuesses;
            VideoGuessed = videoGuessed;
            VideoNotGuessed = videoNotGuessed;
        }
    }
}