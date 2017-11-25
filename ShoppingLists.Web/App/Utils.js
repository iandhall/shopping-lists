"use strict";

var utils = function () {
    function onAjaxFailure() {
        console.log(e);
        bootbox.alert(e);
    }

    return {
        onAjaxFailure: onAjaxFailure
    }
}();