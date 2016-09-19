(function () {
    "use strict";

    angular.module(APPNAME)
        .controller('eventsDetailsAttendeesController', eventController);

    eventController.$inject = ['$scope', '$baseController', "$eventService", "$usersService", "$userEventsService", "$connectionsService", "$eventInvitesService"];

    function eventController(
        $scope
        , $baseController
        , $eventService
        , $usersService
        , $userEventsService
        , $connectionsService
        , $eventInvitesService) {

        //  controllerAs with vm syntax: https://github.com/johnpapa/angular-styleguide#style-y032
        var vm = this;//this points to a new {}
        vm.items = null;
        vm.attendees = null;
        vm.input = null;
        vm.inviteList = null;
        vm.query = null;
        vm.invitedConnections = null;


        //attach dependency injections to the vm
        vm.$eventService = $eventService;
        vm.$usersService = $usersService;
        vm.$userEventsService = $userEventsService;
        vm.$connectionsService = $connectionsService;
        vm.$eventInvitesService = $eventInvitesService;
        vm.$scope = $scope;

        //  bindable members (functions) always go up top while the "meat" of the functions go below: https://github.com/johnpapa/angular-styleguide#style-y033
        vm.receiveItems = _receiveItems;
        vm.onEventError = _onEventError;
        vm.receiveAttendees = _receiveAttendees;
        vm.rsvp = _rsvp;
        vm.rsvpSuccess = _rsvpSuccess;
        vm.receiveInviteList = _receiveInviteList;
        vm.invitePost = _invitePost;
        vm.receiveInvitedConnections = _receiveInvitedConnections; //drop down
        vm.isDisabled = _isDisabled;
        vm.inviteSuccess = _inviteSuccess;
        vm.responseCircle = _responseCircle;
        vm.pushAttendees = _pushAttendees;
        vm.loadAttendees = _loadAttendees;


        //-- this line to simulate inheritance
        $baseController.merge(vm, $baseController);
        //grab threadId from url -- must be after $baseController.merge(vm, $baseController);
        vm.setUpCurrentRequest(vm);
        vm.eventId = vm.currentRequest.params.eventId;

        //this is a wrapper for our small dependency on $scope
        vm.notify = vm.$eventService.getNotifier($scope);
        vm.notify = vm.$eventService.getNotifier($scope);
        vm.notify = vm.$usersService.getNotifier($scope);
        vm.notify = vm.$userEventsService.getNotifier($scope);
        vm.notify = vm.$connectionsService.getNotifier($scope);
        vm.notify = vm.$eventInvitesService.getNotifier($scope);

        initialize();

        function initialize() {

            console.log("attendees tab initialize has fired");

            vm.$eventService.getById(vm.eventId, vm.receiveItems, vm.onEventError);
            _loadAttendees();

        }

        function _receiveItems(data) {

            console.log("getById on success has fired");

            //this receives the data and calls the special
            //notify method that will trigger ng to refresh UI
            vm.notify(function () {
                vm.item = data.item;

                console.log("this is vm.item", vm.item);
            });
        }


        function _onEventError(jqXhr, error) {
            console.error(error);
        }


        function _loadAttendees() {

            console.log("function _loadAttendees() has been called");

            vm.$eventService.getAttendeesByEvent(vm.eventId, 1, vm.receiveAttendees, vm.onEventError);

            vm.$eventInvitesService.getEventInvitedConnects(vm.eventId, vm.receiveInvitedConnections, vm.onEventError);
        }

        function _receiveAttendees(data) {

            console.log("_receiveAttendees has fired");

            vm.notify(function () {
                vm.attendees = data.items;

                console.log("this is vm.attendees", vm.attendees);
            });
        }

        function _rsvp(rsvpStatus) {
            var payload = {}
            payload.RsvpStatus = rsvpStatus;
            payload.EventId = vm.eventId;

            vm.$userEventsService.post(payload, vm.rsvpSuccess, vm.onEventError);
        }

        function _rsvpSuccess() {
            vm.$eventService.getById(vm.eventId, vm.receiveItems, vm.onEventError);
        }


        function _receiveInviteList(data) {
            vm.notify(function () {
                vm.inviteList = data.items;
            });
        }

        function _invitePost() {

            var payload = {};
            payload.inviteeId = vm.input;
            payload.eventId = vm.eventId;
            console.log(vm.input);

            vm.$eventInvitesService.insert(payload, vm.inviteSuccess, vm.onEventError);

        }

        function _receiveInvitedConnections(data) {
            vm.notify(function () {
                vm.invitedConnections = data.items;
            });
            sabio.services.eventInvites.alreadyInvited = [];

            vm.$eventService.getAttendeesByEvent(vm.eventId, 1, vm.pushAttendees, vm.onEventError);

            for (var i = 0; i < data.items.length; i++) {
                sabio.services.eventInvites.alreadyInvited.push(data.items[i].userId);
                console.log("called from the receiveinvitedconnex" + sabio.services.eventInvites.alreadyInvited)
            };

            vm.$connectionsService.getMyConnections(vm.query, vm.receiveInviteList, vm.onEventError);
        }


        function _pushAttendees(data) {
            var attendeeList = data.items;
            for (var j = 0; j < attendeeList.length; j++) {
                sabio.services.eventInvites.alreadyInvited.push(attendeeList[j].userId);
                console.log("called from the pushattendees" + sabio.services.eventInvites.alreadyInvited)
            }
        }

        function _isDisabled(userId) {
            for (var i = 0; i < sabio.services.eventInvites.alreadyInvited.length; i++) {
                if (sabio.services.eventInvites.alreadyInvited[i] == userId) {
                    return true;
                }
            }
            return false;
        }


        function _inviteSuccess() {
            vm.$alertService.success("good job young grasshopper");
            vm.$eventInvitesService.getEventInvitedConnects(vm.eventId, vm.receiveInvitedConnections, vm.onEventError);
        }



        function _responseCircle(int) {
            if (int == 0) {
                return "circle-info"
            }
            else if (int == 1) {
                return "circle-success"
            }
            else if (int == 2) {
                return "circle-danger"
            }
            else if (int == 3) {
                return "circle-warning"
            }
        }
       


    }
})();