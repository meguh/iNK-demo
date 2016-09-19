(function () {
    "use strict";

    angular.module(APPNAME)
        .controller('calendarController', CalendarController);

    CalendarController.$inject = ['$scope', '$baseController', '$userEventsService', '$eventService'];

    function CalendarController(
      $scope
    , $baseController
    , $userEventsService
    , $eventService) {

        var vm = this;

        vm.SearchString = null;

        vm.$userEventsService = $userEventsService;
        vm.$eventService = $eventService;
        vm.$scope = $scope;
        vm.pageNumber = 1;
        vm.pageSize = 5;
        vm.totalItems = 0;

        vm.getAllEvents = _getAllEvents;
        vm.getAllEventsError = _getAllEventsError;
        vm.getUserEvents = _getUserEvents;
        vm.getUserEventsError = _getUserEventsError;
        vm.pageChangeHandler = _pageChangeHandler;
        vm.itemsPerPageHandler = _itemsPerPageHandler;
        vm.search = _search;

        vm.notify = vm.$eventService.getNotifier($scope);

        render();

        function render() {

            var payload = {};
            payload.PageSize = vm.pageSize;
            payload.PageNumber = vm.pageNumber;
            payload.SearchString = vm.SearchString;

            console.log(payload);

            vm.$eventService.discover(payload, vm.getAllEvents, vm.getAllEventsError);
            vm.$userEventsService.get(vm.getUserEvents, vm.getUserEventsError);
        }

        function _getUserEvents(data) {
            vm.notify(function () {
                vm.itemrino = data.items;
            });
            console.log(vm.itemrino);
        }

        function _getAllEvents(data) {
            vm.notify(function () {
                vm.items = data.items;
            });
            console.log(vm.items)
        }

        function _getAllEventsError(jqXhr, error) {
            console.error(error);
        }

        function _search() {
            console.log("test search", vm.SearchString);

            render();
        }

        function _pageChangeHandler(num) {
            console.log("change page -> ", num);

            vm.pageNumber = num;
            render();
        }

        function _itemsPerPageHandler(num) {
            console.log(num);

            vm.pageSize = num;
            render();
        }

        function _getUserEventsError(jqXhr, error) {
            console.error(error);
        }
        return vm;
    }
})();