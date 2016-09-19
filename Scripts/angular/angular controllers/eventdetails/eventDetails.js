(function () {
    "use strict";

    angular.module(APPNAME)
        .controller('eventDetailsController', eventController);

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
        //geolocation
        vm.geocoder = null;
        vm.map = null;
        vm.addressId = null;
        vm.geocodeResponse = null;
        vm.eventId = $('#userIdFromModel').val();


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
        vm.receiveInvitedConnections = _receiveInvitedConnections;
        vm.isDisabled = _isDisabled;
        vm.inviteSuccess = _inviteSuccess;
        vm.responseCircle = _responseCircle;
        vm.pushAttendees = _pushAttendees;
        vm.renderLocation = _renderLocation;


        //-- this line to simulate inheritance
        $baseController.merge(vm, $baseController);

        //this is a wrapper for our small dependency on $scope
        vm.notify = vm.$eventService.getNotifier($scope);

        vm.tabs = [
                    { link: '#/event/' + vm.eventId, label: 'Attendees' },
                    { link: '#/discussion/' + vm.eventId, label: 'Discussion' }
        ];

        vm.selectedTab = vm.tabs[0];


        function _tabClass(tab) {
            if (vm.selectedTab == tab) {
                return "active";
            } else {
                return "";
            }
        }



        function _setSelectedTab(tab) {
            console.log("set selected tab", tab);
            vm.selectedTab = tab;
        }



        initialize();



        function initialize() {

            console.log("initialize has fired");
            vm.geocoder = new google.maps.Geocoder();
            var latlng = new google.maps.LatLng(-34.397, 150.644);
            var mapOptions = {
                zoom: 8,
                center: latlng
            }
            vm.map = new google.maps.Map($('#map-canvas')[0], mapOptions)

            vm.eventId = $('#userIdFromModel').val();
            console.log("this is vm.eventId", vm.eventId);
            vm.$eventService.getById(vm.eventId, vm.receiveItems, vm.onEventError);

        }



        function _receiveItems(data) {
            //this receives the data and calls the special
            //notify method that will trigger ng to refresh UI
            vm.notify(function () {
                vm.item = data.item;
                if (vm.item.location) {
                    _renderLocation();
                }
            });
        }


        function _onEventError(jqXhr, error) {
            console.error(error);
        }

        //not needed? switch from tabs to angular routing
        //function _onTabChange(tabId) {
        //    if (tabId == 3) {
        //        vm.$eventService.getAttendeesByEvent(vm.eventId, 1, vm.receiveAttendees, vm.onEventError);
        //    }
        //    else if (tabId == 4) {
        //        vm.$eventInvitesService.getEventInvitedConnects(vm.eventId, vm.receiveInvitedConnections, vm.onEventError);
        //    }
        //    if (tabId == 2) {
        //        //geocode initialize
        //        vm.geocoder = new google.maps.Geocoder();
        //        var latlng = new google.maps.LatLng(-34.397, 150.644);
        //        var mapOptions = {
        //            zoom: 8,
        //            center: latlng
        //        }
        //        vm.map = new google.maps.Map($('#map-canvas')[0], mapOptions)
        //        vm.renderLocation();
        //    }
        //    else {
        //        console.log('you are on tab ' + tabId);
        //    }
        //}

        function _receiveAttendees(data) {
            vm.notify(function () {
                vm.attendees = data.items;
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


        //geolocation functions
        function _renderLocation() {

            var addressString = vm.item.location.address + " " + vm.item.location.city + " " + vm.item.location.state + " " + vm.item.location.zipCode;
            _codeAddress(addressString);
        }


        function _codeAddress(address) {
            vm.geocoder.geocode({ 'address': address }, _onCodeAddress);
        }


        function _onCodeAddress(results, status) {
            vm.notify(function () {
                vm.geocodeResponse = JSON.stringify(results, null, "     ");
            });

            if (status == google.maps.GeocoderStatus.OK) {

                var geometry = results[0].geometry;
                var loc = geometry.location;

                console.log("got location data from API", loc);

                vm.map.setCenter(loc);

                var marker = new google.maps.Marker({
                    map: vm.map,
                    position: loc
                });

                if (geometry.viewport)
                    vm.map.fitBounds(geometry.viewport);

                var lat = loc.lat();
                var lon = loc.lng();

                console.log("found coordinates in reply -> (%s, %s)", lat, lon);

            } else {
                alert('Geocode was not successful for the following reason: ' + status);
            }
        }


    }
})();