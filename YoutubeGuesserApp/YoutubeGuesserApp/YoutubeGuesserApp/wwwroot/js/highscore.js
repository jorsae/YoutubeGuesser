/**
 * This file holds all js needed for submitting a highscore.
 * This needs the http.js file.
 * The http.js file also needs to be referenced before this file.
 * If not it will not recognize function calls from those files.
 */

var submitButton = document.getElementById("submitHighscore");
submitButton.addEventListener("click", submitHighscore);

function submitHighscore() {
    submitButton.disabled = true;

    var alias = "\"" + document.getElementById("aliasHighscore").value + "\"";

    // Remove 2px off the animation size. This is to account for border at the top and bottom of the button.
    const loadingAnimationSize = submitButton.offsetHeight - 2;
    var animation = document.getElementById("highscoreLoadingAnimation");

    if (animation !== null) {
        animation.style.width = loadingAnimationSize + "px";
        animation.style.height = loadingAnimationSize + "px";
        animation.classList.remove("hidden");
    }

    postRequest("/api/highscoreapi", alias, callbackSubmitHighscore);
}

function callbackSubmitHighscore(response) {
    var highscoreSubmitInfo = document.getElementById("highscoreSubmitInfo");

    if (response.responseText == "true" && response.status === 200) {
        const linkText = "<a href=\"/YoutubeGuesser/Highscore\">Highscores</a>";
        highscoreSubmitInfo.innerHTML = "Your highscore was submitted. See " + linkText + ".";
    }
    else {
        submitButton.innerText = "Retry";
        var responseMessage = JSON.parse(response.responseText);
        highscoreSubmitInfo.innerHTML = responseMessage.errorMessage;

        submitButton.disabled = false;
    }

    // Hide the animation again
    var animation = document.getElementById("highscoreLoadingAnimation");
    animation.classList.add("hidden");
}

function validate(evt) {
    var theEvent = evt || window.event;
    var key = theEvent.charCode;
    if (48 <= key && key <= 57)
        return true;
    if (65 <= key && key <= 90)
        return true;
    if (97 <= key && key <= 122)
        return true;
    theEvent.preventDefault();
}