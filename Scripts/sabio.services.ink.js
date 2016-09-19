sabio.services.ink = sabio.services.ink || {};

sabio.services.ink.GetUserProfile = function (userId, onAjaxSuccess, onAjaxError) {
    var url = "/api/Users/" + userId;
    var settings = {
        cache: false
            , contentType: "application/x-www-form-urlencoded; charset=UTF-8"
            , dataType: "json"
            , success: onAjaxSuccess
            , error: onAjaxError
            , type: "GET"
    };

    $.ajax(url, settings);

}

sabio.services.ink.socialEmail = function (data, onSuccess, onError) {
    var url = "/api/social/email";

    var settings = {
        cache: false
        , contentType: "application/x-www-form-urlencoded; charset=UTF-8"
        , data: data
        , dataType: "json"
        , success: onSuccess
        , error: onError
        , type: "POST"
    };
    $.ajax(url, settings);

}



