
(function () {
    "use strict";

    angular.module(APPNAME)
        .factory('$userEventsService', userEventsServiceFactory);

    userEventsServiceFactory.$inject = ['$baseService', '$sabio'];

    function userEventsServiceFactory($baseService, $sabio) {
        var aSabioServiceObject = sabio.services.userEvents;
        var newService = $baseService.merge(true, {}, aSabioServiceObject, $baseService);

        return newService;
    }
})();