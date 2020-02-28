/**
 * Handles the conversion of YouTube thumbnail to embedded YouTube video in the Game History
 * Also handles displaying tooltip onclick regarding the alias
 */

/* Display Alias tooltip of "No profanity/advertisement" onclick */
$("#aliasHighscore").tooltip({ show: onclick });


var previousEmbeddedVideoId = null;

/**
 * Converts a thumbnail image to an embedded youtube video
 * @param {string} videoId
 */
function playVideo(videoId) {
    var imgThumbnail = document.getElementById("img:" + videoId);
    var imgParent = imgThumbnail.parentNode; // class="video-image"
    var parent = imgParent.parentNode; // class="video"

    parent.style.width = parent.getBoundingClientRect().width + "px";
    parent.style.height = parent.getBoundingClientRect().height + "px";

    if (previousEmbeddedVideoId != null) {
        var oldThumbnail = document.getElementById("img:" + previousEmbeddedVideoId);
        oldThumbnail.classList.remove("video-thumbnail-hidden");

        var oldIframe = document.getElementById("iframe:" + previousEmbeddedVideoId);
        oldThumbnail.parentNode.removeChild(oldIframe);
    }
    previousEmbeddedVideoId = videoId;

    // Create iframe for embedded video
    const ytVideo = "https://www.youtube.com/embed/" + videoId + "?autoplay=1";
    var iframe = document.createElement("iframe");
    iframe.setAttribute("rel", "nofollow");
    iframe.setAttribute("src", ytVideo);
    iframe.setAttribute("frameborder", "0");
    iframe.setAttribute("id", "iframe:" + videoId);
    iframe.setAttribute("allow", "autoplay; encrypted-media");
    iframe.setAttribute("allowfullscreen", '');
    iframe.width = imgThumbnail.clientWidth;
    iframe.height = imgThumbnail.clientHeight;

    //imgParent.removeChild(imgThumbnail);
    imgThumbnail.classList.add("video-thumbnail-hidden"); // Makes this thumbnail hidden
    imgParent.appendChild(iframe);
}