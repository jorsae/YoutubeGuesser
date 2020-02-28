 /**
 * This file is the starting point for the Youtube View guessing game.
 */

const getVideosUrl = "/api/videosapi"
var clickable = true;

/**
 * Starting point for the YouTube View guesser game
 */
window.onload = init;
function init() {
    setLoadingAnimationSize();
    getRequest(getVideosUrl, callbackGetVideos);
}

/**
 * Checks if the user guessed the right video
 * showingTutorial: variable from tutorial.js
 * tutorialToggle(): function from tutorial.js
 */
function checkViews(videoIdGuessed, videoIdNotGuessed) {
    if (showingTutorial) {
        tutorialToggle();
        return;
    }
    if (clickable) {
        clickable = false;
        var url = getVideosUrl + "/" + videoIdGuessed + "/" + videoIdNotGuessed;
        getRequest(url, callbackGetViews);
    }
}

/**
 * Callback for requesting videos for the user to guess
 * @param {any} response
 */
function callbackGetVideos(response) {
    // Clear loadingAnimation.
    var loadingAnimation = document.getElementById("loadingAnimation");
    loadingAnimation.style.display = "none";

    var obj = JSON.parse(response.responseText);
    if (obj.length !== 2) {
        console.log("Something went wrong");
        return;
    }

    // YoutubeVideo objects gotten from the api
    var video1 = obj[0];
    var video2 = obj[1];

    // Fills the <div with the content with the YouTubeVideo data
    changeVideoInformation(1, video1, video2.videoId);
    changeVideoInformation(2, video2, video1.videoId);

    // Add these video to the video history
    addVideo(video1);
    addVideo(video2);

    // <divs are now updated with new YouTube video, user is now allowed
    // to click a new video
    clickable = true;
}

/**
 * Callback for requesting checking views after the user guessed.
 * Resets the gui for videos afterwards
 * @param {any} response
 */
function callbackGetViews(response) {
    if (response.status === 400) {
        console.log("Cheater detected");
        return;
    }

    var viewCountResult = JSON.parse(response.responseText);

    // Add the viewcount to the video history
    addViewCount(viewCountResult.videoGuessed);
    addViewCount(viewCountResult.videoNotGuessed);

    // If the user made the wrong guess, this form will redirect them to the "Game Over" page
    if (!viewCountResult.correct) {
        var historyJson = JSON.stringify(videoHistory);
        var historyB64 = btoa(unescape(encodeURIComponent(historyJson)));
        var url = '/youtubeguesser/gameover';
        var form = $('<form action="' + url + '" method="post" style="display:none;">' +
            '<input type="text" name="encodedData" value="' + historyB64 + '" />' +
            '</form>');
        $('body').append(form);
        form.submit();
        return;
    }

    // Displays the correct/wrong video
    resetVideos(viewCountResult);

    // Queries for new videos, as the user made a correct guess
    getRequest(getVideosUrl, callbackGetVideos);
}