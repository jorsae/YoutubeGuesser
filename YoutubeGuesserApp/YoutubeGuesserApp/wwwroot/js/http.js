/**
 * This file holds functions for sending GET and POST request.
 * It's important that this file is linked before other files that
 * utilize those funcctions, else it will throw an error. As it
 * thinks those functions does not exist.
 */

/**
 * Sends http GET request to given url and callbacks on a given callback
 * @param {string} url
 * @param {function} callback
 * @param {bool} async if the request should be asynchronous or not
 */
function getRequest(url, callback, async = true) {
    var xhttp = new XMLHttpRequest();
    xhttp.onreadystatechange = function () {
        if (this.readyState === 4) {
            callback(xhttp);
        }
    }
    xhttp.open("GET", url, async);
    xhttp.send()
}

/**
 * Sends http POST request to given url and callbacks on a given callback
 * @param {string} url
 * @param {object} object that's sent with the request
 * @param {function} callback with the XMLHttpRequest object
 * @param {bool} async if the request should be asynchronous or not
 */
function postRequest(url, object, callback, async = true) {
    var xhttp = new XMLHttpRequest();
    xhttp.onreadystatechange = function () {
        if (this.readyState === 4) {
            callback(xhttp);
        }
    }
    xhttp.open("POST", url, async);
    xhttp.setRequestHeader('Content-type', 'application/json');
    if (object == null || object == undefined)
        xhttp.send();
    else
        xhttp.send(object);
}