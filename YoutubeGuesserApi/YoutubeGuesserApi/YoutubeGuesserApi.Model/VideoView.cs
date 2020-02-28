using System.ComponentModel.DataAnnotations;

namespace YoutubeGuesserApi.Model
{
    public class VideoView
    {
        private string _VideoId;
        private long _ViewCount;

        [Key]
        public string VideoId { get => _VideoId; set => _VideoId = value; }
        public long ViewCount { get => _ViewCount; set => _ViewCount = value; }

        public VideoView(string videoId, long viewCount = -1)
        {
            VideoId = videoId;
            ViewCount = viewCount;
        }

        public VideoView(YoutubeVideo youtubeVideo)
        {
            this.VideoId = youtubeVideo.VideoId;
            this.ViewCount = youtubeVideo.ViewCount;
        }

        public virtual bool ShouldSerializeViewCount()
        {
            return true;
        }
    }
}
