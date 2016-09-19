sabio.services.users = sabio.services.users || {};

sabio.services.users.getCurrent = function (onSuccess, onError) {
    var url = "/api/users/current"
    var settings = {
        cache: false
	, contentType: "application/x-www-form-urlencoded; charset=UTF-8"
	, dataType: "json"
	, success: onSuccess
	, error: onError
	, type: "GET"
    };

    $.ajax(url, settings);
}


sabio.services.users.serverUpdateProfile = function (data, onSuccess, onError) {
    var url = "/api/users";

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
};

// user update

sabio.services.users.update = function (data, onSuccess, onError) {
    var payload = data;
    var url = "/api/users/currentUser"
    var settings = {
        cache: false
	, contentType: "application/x-www-form-urlencoded; charset=UTF-8"
	, data: payload
	, dataType: "json"
	, success: onSuccess
	, error: onError
	, type: "PUT"
    };

    $.ajax(url, settings);

}

sabio.services.users.updateMedia = function (payload, onSuccess, onError) {
    var payload = { payload: payload };
    var url = "/api/users/media/";
    var settings = {
        cache: false
	, contentType: "application/x-www-form-urlencoded; charset=UTF-8"
	, data: payload
	, dataType: "json"
	, success: onSuccess
	, error: onError
	, type: "PUT"
    };

    $.ajax(url, settings);

}


sabio.services.users.setAvatar = function (avatarMediaId, onSuccess, onError) {
    var url = "/api/users/media"
    var settings = {
        cache: false
	, contentType: "application/x-www-form-urlencoded; charset=UTF-8"
	, dataType: "json"
    , data: { mediaId: avatarMediaId }
	, success: onSuccess
	, error: onError
	, type: "PUT"
    };

    $.ajax(url, settings);

}

sabio.services.users.setTheme = function (themeMediaId, onSuccess, onError) {
    var url = "/api/users/theme"
    var settings = {
        cache: false
	, contentType: "application/x-www-form-urlencoded; charset=UTF-8"
	, dataType: "json"
    , data: { mediaId: themeMediaId }
	, success: onSuccess
	, error: onError
	, type: "PUT"
    };

    $.ajax(url, settings);

}

sabio.services.users.getUsersByTagSlug = function (tagslug, onSuccess, onError) {

    var url = "/api/userbase/tag/" + tagslug;
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

sabio.services.users.tagsWithUser = function (onSuccess, onError) {

    var url = "/api/events/tag";
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

sabio.services.users.qrCodeReader = function (data, onSuccess, onError) {
    var url = "/api/media/QRMedia";

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
};

sabio.services.users.usersByLocation = function (lat, lon, distance, onSuccess, onError) {

    var url = "/api/events/" + lat + "/" + lon + "/" + distance;
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

sabio.services.users.discover = function (payload, onSuccess, onError) {

    var url = "/api/userbase/discover";
    var settings = {
        cache: false
    , contentType: "application/x-www-form-urlencoded; charset=UTF-8"
    , dataType: "json"
    , data: payload
    , success: onSuccess
    , error: onError
    , type: "GET"
    };

    $.ajax(url, settings);
};


sabio.services.users.updateCardTemplate = function (data, onSuccess, onError) {
    var payload = { CardTemplate: data }
    var url = "/api/users/cardtemplate"
    var settings = {
        cache: false
	, contentType: "application/x-www-form-urlencoded; charset=UTF-8"
	, data: payload
	, dataType: "json"
	, success: onSuccess
	, error: onError
	, type: "PUT"
    };

    $.ajax(url, settings);

}