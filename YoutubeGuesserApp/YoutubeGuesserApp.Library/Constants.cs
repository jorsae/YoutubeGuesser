namespace YoutubeGuesserApp.Library
{
    public class Constants
    {
        public const string BASEURL = "https://youtubeguesserapi.azurewebsites.net/api/";
        public const string CHARACTER_SEARCH = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        public const int YOUTUBE_VIDEO_SEARCH_LENGTH = 3;
        public const int MAXIMUM_VIDEO_HISTORY_LENGTH = 50;

        public const string SESSION_CORRECT_GUESSES_KEY = "SESSION_CORRECT_GUESSES_KEY";
        public const string SESSION_GAME_OVER_KEY = "SESSION_GAME_OVER_KEY";
        public const string SESSION_SUBMITTED_TO_HIGHSCORES_KEY = "SESSION_SUBMITTED_TO_HIGHSCORES_KEY";
        public const string SESSION_LAST_YOUTUBEVIDEOS = "SESSION_LAST_YOUTUBEVIDEOS";

        public const string DEFAULT_UNKNOWN_LOCATION = "Unknown";
    }
}