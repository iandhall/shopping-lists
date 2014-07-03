"use strict";

// Sets  RequestVerificationToken header field with value from hidden input before posting. See AntiForgeryAttribute.cs
function post(actionUrl, postData, context, success, error) {

    var settings, headers = {};
    headers.__RequestVerificationToken = $("input[name=__RequestVerificationToken]").val();

    settings = {
        type: "POST",
        url: actionUrl,
        dataType: "json",
        contentType: "application/json",
        data: JSON.stringify(postData),
        headers: headers
    };
    if (context !== undefined) {
        settings.context = context;
    }
    if (success !== undefined) {
        settings.success = success;
    }
    if (error !== undefined) {
        settings.error = error;
    }

    $.ajax(settings);
}
