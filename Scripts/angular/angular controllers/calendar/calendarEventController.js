(function () {

    angular.module(APPNAME) //you will need to declare your module with the dependencies ['mwl.calendar', 'ui.bootstrap', 'ngAnimate']
    .controller('calendarEventController', CalendarEventController);

    CalendarEventController.$inject = ['$baseController', 'moment', 'calendarConfig', '$userEventsService', '$scope'];


    function CalendarEventController(
        $baseController
        , moment
        , calendarConfig
        , $userEventsService
        , $scope) {

        var vm = this;

        $alertService = alert;
        vm.$userEventsService = $userEventsService;
        vm.$scope = $scope;

        vm.calendarView = 'month';
        vm.today = moment().format("MM/DD/YYYY");

        console.log('today', vm.today);

        vm.getUserEvents = _getUserEvents;
        vm.getUserEventsError = _getUserEventsError;



        vm.notify = vm.$userEventsService.getNotifier($scope);

        render();

        function render() {
            vm.$userEventsService.get(vm.getUserEvents, vm.getUserEventsError);
        }

        function _getUserEvents(data) {

            var evt = data.items;


            var processed = [];
            if (evt && evt.length) {

                for (i = 0; i < evt.length; i++) {
                    var eventDetails = evt[i];

                    var eventObjDetails = {
                        title: eventDetails.title,
                        color: 'blue',//calendarConfig.colorTypes.warning,
                        startAt: eventDetails.start,
                        endsAt: eventDetails.end,
                        draggable: true,
                        resizable: true,
                        actions: actions
                    }
                    processed.push(eventObjDetails);
                }
            }


            vm.notify(function () {
                vm.userEvents = processed;

                console.log("processed events", vm.userEvents);
            });

        }

        function _getUserEventsError(jqXhr, error) {
            console.error(error);
        }
        //These variables MUST be set as a minimum for the calendar to work

        var actions = [{
            label: '<i class=\'glyphicon glyphicon-pencil\'></i>',
            onClick: function (args) {
                alert.show('Edited', args.calendarEvent);
            }
        }, {
            label: '<i class=\'glyphicon glyphicon-remove\'></i>',
            onClick: function (args) {
                alert.show('Deleted', args.calendarEvent);
            }
        }];

        vm.isCellOpen = true;

        vm.isCellOpen = true;

        vm.addEvent = function () {
            vm.events.push({
                title: 'New event',
                startsAt: moment().startOf('day').toDate(),
                endsAt: moment().endOf('day').toDate(),
                color: calendarConfig.colorTypes.important,
                draggable: true,
                resizable: true
            });
        };

        vm.eventClicked = function (event) {
            alert.show('Clicked', event);
        };

        vm.eventEdited = function (event) {
            alert.show('Edited', event);
        };

        vm.eventDeleted = function (event) {
            alert.show('Deleted', event);
        };

        vm.eventTimesChanged = function (event) {
            alert.show('Dropped or resized', event);
        };

        vm.toggle = function ($event, field, event) {
            $event.preventDefault();
            $event.stopPropagation();
            event[field] = !event[field];
        };


        return vm;
    }

})();