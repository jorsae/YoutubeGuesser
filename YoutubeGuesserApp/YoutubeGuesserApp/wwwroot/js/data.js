/**
 * This file holds all js needed to send user data to API.
 * This needs the http.js file.
 * The http.js file also needs to be referenced before this file.
 * If not it will not recognize function calls from those files.
 */

const cookieName = "_data";
const cookieValue = true;
const cookieDurationDays = 365;

// Start harvest function
harvest();

function harvest() {
    var cookieValue = getHarvestCookie(cookieName);
    if (cookieValue) {
        return;
    }

    var operatingSystem = getOS();
    var browser = getBrowser();
    var language = navigator.language;
    var isMobile = detectMobile();
    var referrer = getReferrer();
    var userData = {
        'OperatingSystem': operatingSystem,
        'browser': browser,
        'language': language,
        'isMobile': isMobile,
        'referrer': referrer
    }

    jsonData = JSON.stringify(userData);
    postRequest("/api/harvest", jsonData, callbackHarvest);
}

function callbackHarvest(response) {
    if (response.responseText === "true") {
        setHarvestCookie(cookieName, cookieValue);
    }
}

function setHarvestCookie(name, value) {
    var date = new Date();
    date.setTime(date.getTime() + (cookieDurationDays * 24 * 60 * 60 * 1000));
    var expires = "; expires=" + date.toUTCString();
    document.cookie = name + "=" + (value || "") + expires + "; path=/";
}
function getHarvestCookie(name) {
    var nameEQ = name + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') c = c.substring(1, c.length);
        if (c.indexOf(nameEQ) == 0) return c.substring(nameEQ.length, c.length);
    }
    return null;
}
function eraseCookie(name) {
    document.cookie = name + '=; Max-Age=-99999999;';
}

function getReferrer() {
    var referrer = document.referrer;
    if (referrer.indexOf(location.protocol + "//" + location.host) !== 0)
        return referrer;
    return "";
}

function detectMobile() {
    if (navigator.userAgent.match(/Android/i)
        || navigator.userAgent.match(/webOS/i)
        || navigator.userAgent.match(/iPhone/i)
        || navigator.userAgent.match(/iPad/i)
        || navigator.userAgent.match(/iPod/i)
        || navigator.userAgent.match(/BlackBerry/i)
        || navigator.userAgent.match(/Windows Phone/i)
    ) {
        return true;
    }
    else {
        return false;
    }
}

function getOS() {
    if (window.navigator.userAgent.indexOf("Windows NT 10.0") != -1) return "Windows 10";
    if (window.navigator.userAgent.indexOf("Windows NT 6.2") != -1) return "Windows 8";
    if (window.navigator.userAgent.indexOf("Windows NT 6.1") != -1) return "Windows 7";
    if (window.navigator.userAgent.indexOf("Windows NT 6.0") != -1) return "Windows Vista";
    if (window.navigator.userAgent.indexOf("Windows NT 5.1") != -1) return "Windows XP";
    if (window.navigator.userAgent.indexOf("Windows NT 5.0") != -1) return "Windows 2000";
    if (window.navigator.userAgent.indexOf("Mac") != -1) return "Mac/iOS";
    if (window.navigator.userAgent.indexOf("X11") != -1) return "UNIX";
    if (window.navigator.userAgent.indexOf("Linux") != -1) return "Linux";
    return "Unknown";
}

function getBrowser() {
    browser = getBrowserByDuckTyping();
    if (browser === "Unknown") {
        return getBrowserByUserAgent();
    }
    else {
        return browser;
    }
}

/*
 * Code taken and modified from:
 * https://stackoverflow.com/questions/9847580/how-to-detect-safari-chrome-ie-firefox-and-opera-browser/9851769
 * This uses duck-typing. Fallback, if it doesn't find any is by the user-agent.
 */
function getBrowserByDuckTyping() {
    // Opera 8.0+
    if ((!!window.opr && !!opr.addons) || !!window.opera || navigator.userAgent.indexOf(' OPR/') >= 0) {
        return "Opera";
    }

    // Firefox 1.0+
    if (typeof InstallTrigger !== 'undefined') {
        return "Firefox";
    }

    // Safari
    if (/constructor/i.test(window.HTMLElement) ||
        (function (p) { return p.toString() === "[object SafariRemoteNotification]"; })(!window['safari']
            || (typeof safari !== 'undefined' && safari.pushNotification))) {
        return "Safari";
    }

    // Internet Explorer 6-11
    if (/*@cc_on!@*/false || !!document.documentMode) {
        return "IE";
    }

    if (!!window.StyleMedia) {
        return "Edge";
    }

    // Chrome 1 - 71
    if (!!window.chrome && (!!window.chrome.webstore || !!window.chrome.runtime)) {
        return "Chrome";
    }

    return "Unknown";
}

function getBrowserByUserAgent() {
    if ((navigator.userAgent.indexOf("Opera") || navigator.userAgent.indexOf('OPR')) != -1) {
        return "Opera";
    }
    else if (navigator.userAgent.indexOf("Chrome") != -1) {
        return "Chrome";
    }
    else if (navigator.userAgent.indexOf("Safari") != -1) {
        return "Safari";
    }
    else if (navigator.userAgent.indexOf("Firefox") != -1) {
        return "Firefox";
    }
    //IF IE > 10
    else if ((navigator.userAgent.indexOf("MSIE") != -1) || (!!document.documentMode == true)) {
        return "IE";
    }
    else {
        return "Unknown";
    }
}