/**
 * This file holds all js needed for the feedback functionality.
 * This includes hooking up click events and sending the http request.
 * This needs the http.js file AND the data.js file.
 * Both of those files also needs to be referenced before this file.
 * If not it will not recognize function calls from those files.
 */


// Set register when script loads
registerClickEvent();

/* Events for opening and closing feedbackModal */
function registerClickEvent() {
    var modal = document.getElementById("feedbackModal");
    var feedbackModal = document.getElementById("feedbackModalButton");

    feedbackModal.onclick = function () {
        modal.style.display = "block";
    }

    // 'X' button on the modal
    var feedbackCloseModal = document.getElementById("feedbackModalClose");
    feedbackCloseModal.onclick = function () {
        modal.style.display = "none";
    }

    // When the user clicks anywhere outside of the modal, close it
    window.addEventListener('click', function (event) {
        if (event.target == modal) {
            modal.style.display = "none";
        }
    });
}

function submitFeedback() {
    var title = document.getElementById("feedbackTitle").value;
    var email = document.getElementById("feedbackEmail").value;
    var body = document.getElementById("feedbackBody").value;
    var browser = getBrowser();
    var os = getOS();

    var feedback = {
        "Title": title,
        "Email": email,
        "Body": body,
        "Browser": browser,
        "OperatingSystem" : os
    };
    var feedbackJson = JSON.stringify(feedback);

    // Clear the input fields
    document.getElementById("feedbackForm").reset();

    var submitButton = document.getElementById("feedbackSubmit");
    submitButton.style = "border: solid 4px blue;";

    postRequest("/api/feedback", feedbackJson, callbackSubmitFeedback);

    // feedback inputs are in a form. return false, keeps the form action from happening
    return false;
}

function callbackSubmitFeedback(response) {
    // .modal-content set border to green/red
    var submitButton = document.getElementById("feedbackSubmit");

    if (response.responseText == "true") {
        submitButton.disabled = true;
        submitButton.value = "Thank you for your feedback!";
        submitButton.style = "border: solid 4px green;";
    }
    else {
        submitButton.value = "Something went wrong";
        submitButton.style = "border: solid 4px red;";
    }
}