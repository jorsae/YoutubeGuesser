using System.Collections.Generic;
using YoutubeGuesserApp.Model;

namespace YoutubeGuesserApp.Library
{
    public class Utility
    {
        public void ReverseGameHistory(List<YoutubeVideoView> youtubeVideos)
        {
            youtubeVideos.Reverse();
            for (int i = youtubeVideos.Count - 1; i >= 1; i -= 2)
            {
                YoutubeVideoView temp = youtubeVideos[i];
                youtubeVideos[i] = youtubeVideos[i - 1];
                youtubeVideos[i - 1] = temp;
            }
        }
    }
}
