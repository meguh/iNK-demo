sabio.services.admin = sabio.services.admin || {};


sabio.services.admin.getAll = function (payload, onAjaxSuccess, onAjaxError) {
    var url = "/api/users/";
    var settings = {
        cache: false
            , contentType: "application/x-www-form-urlencoded; charset=UTF-8"
            , dataType: "json"
            , data:payload
            , success: onAjaxSuccess
            , error: onAjaxError
            , type: "GET"
    };

    $.ajax(url, settings);
}


sabio.services.admin.update = function (id, data, onAjaxSuccess, onAjaxError) {
    var url = "/api/users/admin/" + id;
    var settings = {
        cache: false
        , contentType: "application/x-www-form-urlencoded; charset=UTF-8"
        , data: data
        , dataType: "json"
        , success: onAjaxSuccess
        , error: onAjaxError
        , type: "PUT"
    };
    $.ajax(url, settings);

}

sabio.services.admin.search = function (searchstring, onAjaxSuccess, onAjaxError) {
    var url = "/api/users/admin/search/" + searchstring;
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

