namespace YoutubeGuesserApi.Library.Utility
{
    public class Constants
    {
        public const string YOUTUBE_API_KEY = @"";
        public const int MAX_RESULTS_FROM_YOUTUBE_API = 25;
        public const int MAX_DAYS_STORAGE_YOUTUBE_VIDEO = 30;

        public const string IPGEOLOCATION_API_KEY = @"";

        public const string ADMIN_API_PASSWORD = "";

        public const string DEFAULT_UNKNOWN_USER_DATA = "Unknown";

        public const int TOP_VIDEO_VIEWS_SELECTION = 1000;
        public const int FILTER_YOUTUBE_VIDEOS_VIEWCOUNT = 1000000;

        // ISO 3166-1 alpha-2 country code. That we fetch the trending videos from
        public static readonly string[] TRENDING_COUNTRY_CODES = new string[] { "US", "GB", "AU", "CA" };

        public const string CHARACTER_SEARCH = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    }
}