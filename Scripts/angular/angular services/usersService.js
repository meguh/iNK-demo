
(function () {
    "use strict";

    angular.module(APPNAME)
        .factory('$usersService', usersServiceFactory);

    usersServiceFactory.$inject = ['$baseService', '$sabio'];

    function usersServiceFactory($baseService, $sabio) {
        var aSabioServiceObject = sabio.services.users;
        var newService = $baseService.merge(true, {}, aSabioServiceObject, $baseService);

        return newService;
    }
})();