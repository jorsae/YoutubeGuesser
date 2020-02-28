namespace YoutubeGuesserApp.Model
{
    public class VideoView
    {
        private string _VideoId;
        private long _ViewCount;

        public string VideoId { get => _VideoId; set => _VideoId = value; }
        public long ViewCount { get => _ViewCount; set => _ViewCount = value; }

        public VideoView(string videoId, long viewCount)
        {
            VideoId = videoId;
            ViewCount = viewCount;
        }
    }
}