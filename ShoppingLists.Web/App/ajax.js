"use strict";

var ajax = function () {
    function getAjaxSettings(method, actionUrl, data, context) {
        var settings, headers = {};

        // Sets RequestVerificationToken header field with value from hidden input before posting. See AntiForgeryAttribute.cs
        headers.__RequestVerificationToken = $("input[name=__RequestVerificationToken]").val();

        settings = {
            type: method,
            url: actionUrl,
            dataType: "json",
            contentType: "application/json",
            headers: headers
        };
        if (data) {
            settings.data = JSON.stringify(data);
        }
        if (context) {
            settings.context = context;
        }
        return settings;
    }

    function del(actionUrl, data, context) {
        return $.ajax(getAjaxSettings("DELETE", actionUrl, data, context));
    }
    
    function get(actionUrl, context) {
        return $.ajax(getAjaxSettings("GET", actionUrl, null, context));
    }

    function post(actionUrl, data, context) {
        return $.ajax(getAjaxSettings("POST", actionUrl, data, context));
    }

    function put(actionUrl, data, context) {
        return $.ajax(getAjaxSettings("PUT", actionUrl, data, context));
    }

    return {
        del: del,
        get: get,
        post: post,
        put: put
    };
}();