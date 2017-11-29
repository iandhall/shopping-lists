"use strict";

var utils = function () {
    function onAjaxFailure(jqXHR, textStatus, errorThrown) {
        console.log(errorThrown);
        if (jqXHR.responseJSON && jqXHR.responseJSON.userMessage) {
            bootbox.alert(jqXHR.responseJSON.userMessage);
        } else {
            bootbox.alert("Uh-oh! Something went wrong... " + jqXHR.status + " " + errorThrown);
        }
    }

    return {
        onAjaxFailure: onAjaxFailure
    }
}();