/**
 * This file holds all js needed for keeping track of game history.
 * It also manages and converts youtube thumbnails to embedded youtube videos.
 */

var videoHistory = [];

/**
 * Adds a video to the videoHistory
 * @param {YouTubeVideo} video
 */
function addVideo(video) {
    videoHistory.push(video);
}

/**
 * Updates a YouTube video's view count
 * @param {int} videoView
 */
function addViewCount(videoView) {
    for (var i = videoHistory.length - 1; i >= 0; i--) {
        if (videoHistory[i].videoId == videoView.videoId) {
            videoHistory[i].viewCount = videoView.viewCount;
            break;
        }
    }
}