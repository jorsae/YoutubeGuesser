namespace YoutubeGuesserApp.Model
{
    public class YoutubeVideoView : YoutubeVideo
    {
        private long _ViewCount;
        public long ViewCount { get => _ViewCount; set => _ViewCount = value; }

        public YoutubeVideoView(string videoId, string title, string channelName, long viewCount)
                            : base (videoId, title, channelName)
        {
            ViewCount = viewCount;
        }
    }
}