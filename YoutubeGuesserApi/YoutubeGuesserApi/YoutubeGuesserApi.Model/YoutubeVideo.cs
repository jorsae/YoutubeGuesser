using System;
using Newtonsoft.Json;

namespace YoutubeGuesserApi.Model
{
    public class YoutubeVideo : VideoView
    {
        private string _Title;
        private string _ChannelName;
        private DateTime _LastUpdated;

        public string Title { get => _Title; set => _Title = value; }
        public string ChannelName { get => _ChannelName; set => _ChannelName = value; }
        [JsonIgnore]
        public DateTime LastUpdated { get => _LastUpdated; set => _LastUpdated = value; }

        public YoutubeVideo(string videoId, string title, string channelName, long viewCount = -1) 
                            : base(videoId, viewCount)
        {
            VideoId = videoId;
            Title = title;
            ChannelName = channelName;
            ViewCount = viewCount;
            LastUpdated = DateTime.Now;
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
            return $"VideoId: {VideoId}, Channel: {ChannelName} | Title: {Title}, ViewCount: {ViewCount}, Thumbnail: {GetThumbnail()}";
        }

        public override bool ShouldSerializeViewCount()
        {
            return false;
        }
    }
}