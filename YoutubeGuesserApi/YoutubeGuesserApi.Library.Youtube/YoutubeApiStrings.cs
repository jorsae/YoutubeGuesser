namespace YoutubeGuesserApi.Library.Youtube
{
    public class YoutubeApiStrings
    {
        public static string GetVideoStatistics(string videoId, string youtubeApiKey)
        {
            return $"https://www.googleapis.com/youtube/v3/videos?&id={videoId}&key={youtubeApiKey}&part=statistics";
        }

        /**
         * relevanceLanguage=en, this can still show videos in other languages, if they are highly relevant to the search term!
         */
        public static string GetVideoBySearch(string search, string youtubeApiKey, int maxResults = 1, string order = "viewCount")
        {
            return $"https://www.googleapis.com/youtube/v3/search?q={search}&key={youtubeApiKey}&maxResults={maxResults}&order={order}&part=snippet&type=video&relevanceLanguage=en";
        }

        public static string GetVideoRelatedToVideoId(string videoId, string youtubeApiKey, int maxResults = 1, string order = "relevance")
        {
            return $"https://www.googleapis.com/youtube/v3/search?0={videoId}&key={youtubeApiKey}&maxResults={maxResults}&order={order}&part=snippet&type=video";
        }

        public static string GetThumbnailByVideoId(string videoId)
        {
            return $"https://i.ytimg.com/vi/{videoId}/maxresdefault.jpg";
        }

        public static string GetTrendingVideo(string regionCode, string youtubeApiKey, int maxResults = 50, string pageToken = null)
        {
            maxResults = (maxResults > 50) ? 50 : maxResults;
            if(pageToken == null)
            {
                return $"https://www.googleapis.com/youtube/v3/videos?regionCode={regionCode}&key={youtubeApiKey}&maxResults={maxResults}&part=snippet,statistics&chart=mostPopular";
            }
            return $"https://www.googleapis.com/youtube/v3/videos?regionCode={regionCode}&key={youtubeApiKey}&maxResults={maxResults}&pageToken={pageToken}&part=snippet,statistics&chart=mostPopular";
        }
    }
}