
(function () {
    "use strict";

    angular.module(APPNAME)
        .factory('$tagsService', tagsServiceFactory);

    tagsServiceFactory.$inject = ['$baseService', '$sabio'];

    function tagsServiceFactory($baseService, $sabio) {
        var aSabioServiceObject = sabio.services.tags;
        var newService = $baseService.merge(true, {}, aSabioServiceObject, $baseService);

        return newService;
    }
})();