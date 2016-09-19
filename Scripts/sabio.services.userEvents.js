sabio.services.userEvents = sabio.services.userEvents || {};


sabio.services.userEvents.get = function (onSuccess, onError) {

    var url = "/api/user-events";
    var settings = {
        cache: false
    , contentType: "application/x-www-form-urlencoded; charset=UTF-8"
    , dataType: "json"
    , success: onSuccess
    , error: onError
    , type: "GET"
    };

    $.ajax(url, settings);
};

sabio.services.userEvents.post = function (payload, onSuccess, onError) {
    
    var url = "/api/user-events";
    var settings = {
        cache: false
    , contentType: "application/x-www-form-urlencoded; charset=UTF-8"
    , dataType: "json"
    , data: payload
    , success: onSuccess
    , error: onError
    , type: "Post"
    };

    $.ajax(url, settings);
};

sabio.services.userEvents.delete = function (id, onSuccess, onError) {

    var url = "/api/user-events/" +id;
    var settings = {
        cache: false
    , contentType: "application/x-www-form-urlencoded; charset=UTF-8"
    , dataType: "json"
    , success: onSuccess
    , error: onError
    , type: "DELETE"
    };

    $.ajax(url, settings);
};

sabio.services.userEvents.updateEvent = function (id, data, onSuccess, onError) {

    var url = "/api/user-events/" + id;
    var settings = {
        cache: false
    , contentType: "application/x-www-form-urlencoded; charset=UTF-8"
    , data: data
    , dataType: "json"
    , success: onSuccess
    , error: onError
    , type: "PUT"
    };

    $.ajax(url, settings);
};

