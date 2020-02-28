/**
 * This file holds all js that adds/edit or delete anything,
 * that is displayed to the user
 */

/**
 * Adjusts the size of the loading animation
 */
function setLoadingAnimationSize() {
    var startingPoint = (window.innerWidth > window.innerHeight) ? window.innerHeight : window.innerWidth;
    var loadingAnimationSize = startingPoint / 100 * 20;

    var animation = document.getElementById("loadingAnimation");
    if (animation !== null) {
        animation.style.width = loadingAnimationSize + "px";
        animation.style.height = loadingAnimationSize + "px";
    }
}

/**
 * Displays the correct/wrong image
 * @param {string videoId} winner
 * @param {string videoId} loser
 */
function resetVideos(viewCountResult) {
    try {
        var imageWinner = document.querySelectorAll('[videoid="' + viewCountResult.videoGuessed.videoId + '"]')[0];
        var imageLoser = document.querySelectorAll('[videoid="' + viewCountResult.videoNotGuessed.videoId + '"]')[0];
        imageWinner.classList.add("correct-image");
        imageLoser.classList.add("wrong-image");
    }
    catch (e) {
        console.log(e);
    }

    var scoreDiv = document.getElementById("score");
    scoreDiv.innerHTML = "Score: " + viewCountResult.correctGuesses;
}

function changeVideoInformation(videoNumber, video, otherVideoId) {
    // Change video title
    var videoTitle = document.getElementById("videoTitle" + videoNumber)
    videoTitle.innerHTML = video.title;

    // Change image
    var videoImage = document.getElementById("videoImage" + videoNumber);
    videoImage.classList = "";
    videoImage.setAttribute("src", "https://i.ytimg.com/vi/" + video.videoId + "/hqdefault.jpg");
    videoImage.setAttribute("alt", "Picture of YouTube video option " + videoNumber + ", with title \"" + video.title + "\"");
    videoImage.setAttribute("videoId", video.videoId);
    videoImage.onclick = function () { checkViews(video.videoId, otherVideoId) };
}