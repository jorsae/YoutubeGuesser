using System.ComponentModel.DataAnnotations;

namespace YoutubeGuesserApp.Model
{
    public class YoutubeVideo
    {
        private string _VideoId;
        private string _Title;
        private string _ChannelName;

        [Key]
        public string VideoId { get => _VideoId; set => _VideoId = value; }
        public string Title { get => _Title; set => _Title = value; }
        public string ChannelName { get => _ChannelName; set => _ChannelName = value; }

        public YoutubeVideo(string videoId, string title, string channelName)
        {
            VideoId = videoId;
            Title = title;
            ChannelName = channelName;
        }

        /*
         * thumbnailQuality is listed as such:
         * 0 - default (lowest quality)
         * 1 - medium
         * 2 - high
         * 3 - maxres (highest quality)
         */
        public string GetThumbnail(ThumbnailQuality thumbnailQuality = ThumbnailQuality.High)
        {
            switch (thumbnailQuality)
            {
                default:
                case ThumbnailQuality.High:
                    return $"https://i.ytimg.com/vi/{this.VideoId}/hqdefault.jpg";
                case ThumbnailQuality.Low:
                    return $"https://i.ytimg.com/vi/{this.VideoId}/default.jpg";
                case ThumbnailQuality.Medium:
                    return $"https://i.ytimg.com/vi/{this.VideoId}/mqdefault.jpg";
                case ThumbnailQuality.MaxResolution:
                    return $"https://i.ytimg.com/vi/{this.VideoId}/maxresdefault.jpg";
            }
        }

        public override string ToString()
        {
            return $"VideoId: {VideoId}, Channel: {ChannelName} | Title: {Title}, Thumbnail: {GetThumbnail()}";
        }
    }
}