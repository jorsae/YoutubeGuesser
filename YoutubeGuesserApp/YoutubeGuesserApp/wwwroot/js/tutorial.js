/**
 * This file holds all js needed for the tutorial
 */

var tutorialStep = 0; // Part of the tutorial the user is on
var showingTutorial = false; // If the tutorial is showing to the user

addTutorialEvents();

/**
 * Sets up all click events
 */
function addTutorialEvents() {
    // When the user clicks the 'how to play' button, it starts the tutorial
    var tutorialButton = document.getElementById("howToPlay");
    tutorialButton.onclick = startTutorial;

    // When the user click on the exit button, close the tutorial
    var tutorialButton = document.getElementById("exitTutorial");
    tutorialButton.onclick = exitTutorial;

    // Text Dialog confirmation button is reset to close the text dialog
    var tutorialTextConfirmation = document.getElementById("tutorialTextConfirmation");
    tutorialTextConfirmation.onclick = closeDialog;

    // Hooks up events for clicking on the tutorial images
    var tutorialImage1 = document.getElementById("tutorialImage1");
    var tutorialImage2 = document.getElementById("tutorialImage2");
    tutorialImage1.onclick = clickTutorialImage;
    tutorialImage2.onclick = clickTutorialImage;
}

/**
 * Starts the tutorial
 * */
function startTutorial() {
    // Hides/Displays thumbnails
    document.getElementById("videoImage1").classList.add("hidden");;
    document.getElementById("videoImage2").classList.add("hidden");;
    document.getElementById("tutorialImage1").classList.remove("hidden");
    document.getElementById("tutorialImage2").classList.remove("hidden");

    // Hides/Displays video titles
    document.getElementById("videoTitle1").classList.add("hidden");;
    document.getElementById("videoTitle2").classList.add("hidden");;
    document.getElementById("tutorialTitle1").classList.remove("hidden");
    document.getElementById("tutorialTitle2").classList.remove("hidden");

    // Hides/Displays how to play / exit buttons
    document.getElementById("howToPlay").classList.add("hidden");
    document.getElementById("exitTutorial").classList.remove("hidden");

    // Hides/Displays score
    document.getElementById("score").classList.add("hidden");
    document.getElementById("tutorialScore").classList.remove("hidden");

    tutorialStep1();
}

/**
 * function that progresses the tutorial
 * */
function clickTutorialImage() {
    if (tutorialStep === 1) {
        tutorialStep2();
    }
    if (tutorialStep === 2) {
        tutorialStep3();
    }
}

/**
 * The 1st step of the tutorial.
 * This is called right after the tutorial is started
 * */
function tutorialStep1() {
    var tutorialText = document.getElementById("tutorialText");
    tutorialText.innerHTML = "The goal is to guess the YouTube video with the most views. <br />";
    tutorialText.innerHTML += "You can see the title and the thumbnail for each video.<br />";
    tutorialText.innerHTML += "Click on a thumbnail to make your choice!";

    document.getElementById("tutorialTextDiv").classList.remove("hidden");
}

/**
 * The 2nd step of the tutorial
 * */
function tutorialStep2() {
    tutorialImagePulsate(false);

    // Set the text to the user
    var tutorialText = document.getElementById("tutorialText");
    tutorialText.innerHTML = "You get 1 point for guessing correctly! <br />";
    tutorialText.innerHTML += "Let's see if you can guess correctly this time too!";

    // Sets the text dialog visible
    var tutorialTextDiv = document.getElementById("tutorialTextDiv");
    tutorialTextDiv.classList.remove("hidden");

    // Swap tutorial images
    var tutorialImage1 = document.getElementById("tutorialImage1");
    var tutorialImage2 = document.getElementById("tutorialImage2");
    tutorialImage1.setAttribute("src", "/images/tutorial_image3.jpg");
    tutorialImage2.setAttribute("src", "/images/tutorial_image4.jpg");

    // Change the title of the images
    var tutorialTitle1 = document.getElementById("tutorialTitle1");
    var tutorialTitle2 = document.getElementById("tutorialTitle2");
    tutorialTitle1.innerText = "Booooring";
    tutorialTitle2.innerText = "😋";

    // Change score
    var tutorialScore = document.getElementById("tutorialScore");
    tutorialScore.innerHTML = "Score: 1";
}

/**
 * The 3rd step of the tutorial
 * */
function tutorialStep3() {
    // Stop the pulsating tutorial images
    tutorialImagePulsate(false);

    // Set the text to the user
    var tutorialText = document.getElementById("tutorialText");
    tutorialText.innerHTML = "That's it for the introduction.<br />";
    tutorialText.innerHTML += "Now it's time to try a real game!";

    // Sets the text dialog visible
    var tutorialTextDiv = document.getElementById("tutorialTextDiv");
    tutorialTextDiv.classList.remove("hidden");

    // Change score
    var tutorialScore = document.getElementById("tutorialScore");
    tutorialScore.innerHTML = "Score: 2";

    // Set the tutorial button to go back to the main game
    var tutorialTextConfirmation = document.getElementById("tutorialTextConfirmation");
    tutorialTextConfirmation.onclick = exitTutorial;
    tutorialTextConfirmation.innerText = "Back to the game";
}

/**
 * Closes the text dialog
 * */
function closeDialog() {
    var tutorialTextConfirmation = document.getElementById("tutorialTextDiv");
    tutorialTextDiv.classList.add("hidden");

    switch (tutorialStep) {
        default:
        case 0:
            tutorialImagePulsate();
            break
        case 1:
            tutorialImagePulsate();
            break;
        case 2:
            tutorialStep3();
            break;
    }
    tutorialStep++;
}

/**
 * Helper function that adds or removes pulsating effect on the tutorial images
 * @param {bool} pulsate
 */
function tutorialImagePulsate(pulsate = true) {
    var tutorialImage1= document.getElementById("tutorialImage1");
    var tutorialImage2 = document.getElementById("tutorialImage2");
    if (pulsate) {
        tutorialImage1.classList.add("pulsating-border");
        tutorialImage2.classList.add("pulsating-border");
    }
    else {
        tutorialImage1.classList.remove("pulsating-border");
        tutorialImage2.classList.remove("pulsating-border");
    }
}

/**
 * Exits and resets the tutorial back to it's starting state
 * */
function exitTutorial() {
    // Stops tutorial images from pulsating
    tutorialImagePulsate(false);

    // Reset tutorial progress
    tutorialStep = 0;

    // Hides/Displays thumbnails
    document.getElementById("videoImage1").classList.remove("hidden");;
    document.getElementById("videoImage2").classList.remove("hidden");;
    document.getElementById("tutorialImage1").classList.add("hidden");
    document.getElementById("tutorialImage2").classList.add("hidden");

    // Hides/Displays video titles
    document.getElementById("videoTitle1").classList.remove("hidden");;
    document.getElementById("videoTitle2").classList.remove("hidden");;
    document.getElementById("tutorialTitle1").classList.add("hidden");
    document.getElementById("tutorialTitle2").classList.add("hidden");

    // Hides/Displays how to play / exit buttons
    document.getElementById("howToPlay").classList.remove("hidden");
    document.getElementById("exitTutorial").classList.add("hidden");

    // Hides/Displays score
    document.getElementById("score").classList.remove("hidden");
    document.getElementById("tutorialScore").classList.add("hidden");

    // hides the tutorial dialog
    document.getElementById("tutorialTextDiv").classList.add("hidden");

    // Change the tutorial YouTube vidoe thumbnails
    var tutorialImage1 = document.getElementById("tutorialImage1");
    var tutorialImage2 = document.getElementById("tutorialImage2");
    tutorialImage1.setAttribute("src", "/images/tutorial_image1.jpg");
    tutorialImage2.setAttribute("src", "/images/tutorial_image2.jpg");

    // Change the tutorial video titles
    var tutorialTitle1 = document.getElementById("tutorialTitle1");
    var tutorialTitle2 = document.getElementById("tutorialTitle2");
    tutorialTitle1.innerText = "Click me!";
    tutorialTitle2.innerText = "Don't click me!";

    // Change the tutorialScore back
    var tutorialScore = document.getElementById("tutorialScore");
    tutorialScore.innerHTML = "Score: 0";

    // Text Dialog confirmation button is reset to close the text dialog
    var tutorialTextConfirmation = document.getElementById("tutorialTextConfirmation");
    tutorialTextConfirmation.onclick = closeDialog;
}