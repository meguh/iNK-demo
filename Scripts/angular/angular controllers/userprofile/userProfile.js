(function () {
    "use strict";

    angular.module(APPNAME)
        .controller('mainController', MainController);

    MainController.$inject = ['$scope', '$baseController', '$userService', '$connectionsService', '$uibModal'];

    function MainController(
      $scope
      , $baseController
      , $userService
      , $connectionsService
      , $uibModal) {

        var vm = this;

        vm.disabled = false;
        vm.dataItem = {};
        vm.userId = $("#userId").val();
        vm.dataItem.cardTemplate = 'default';
        vm.notes = null;
        

        $baseController.merge(vm, $baseController);

        vm.receiveData = _receiveData;
        vm.onEmpError = _onEmpError;
        vm.addConnection = _addConnection;
        vm.openModal = _openModal;
        vm.notesPosted = _notesPosted;
        vm.notePostError = _notePostError;
        vm.submitNotes = submitNotes;
        vm.externalAccountIcon = _externalAccountIcon;

        vm.$scope = $scope;
        vm.$uibModal = $uibModal;
        vm.$userService = $userService;
        vm.$connectionsService = $connectionsService;
        vm.notify = vm.$connectionsService.getNotifier($scope);

        render();

        function render() {
            vm.$userService.GetUserProfile(vm.userId, vm.receiveData, vm.onEmpError);
        }

        function submitNotes() {

            var payload = {}

            payload.notes = vm.notes;
            payload.contactee = vm.userId;

            vm.$connectionsService.putNotes(payload, vm.notesPosted, vm.notePostError);
        }

        function _receiveData(data) {
            vm.notify(function () {
                vm.dataItem = data.item;
                if (data.item.connection != null) {
                    vm.notes = data.item.connection.notes;
                }

            });
        }

        function _onEmpError(jqXhr, error) {
            console.error(error);
        }

        function _connectionSuccess() {
            vm.$alertService.success("You are now connected - Ink", "Connection Success!");
        }

        function _addConnection() {
            var payload = {}
            payload.Contactee = vm.userId;
            vm.$connectionsService.insertConnection(payload, _connectionSuccess, vm.onEmpError);
        }

        function _notesPosted() {
            vm.$alertService.success("- Ink", "Notes Successfully Posted!");
        }

        function _notePostError() {
            vm.$alertService.error("- Ink", "Notes Not Posted");
        }

        function _externalAccountIcon(source) {
            switch (source) {
                case "Facebook":
                    return "fa-facebook";
                    break;
                case "Instagram":
                    return "fa-instagram";
                    break;
                case "Twitter":
                    return "fa-twitter";
                    break;
                case "Google":
                    return "fa-google";
                    break;
                case "Youtube":
                    return "fa-youtube";
                    break;
                case "Slack":
                    return "fa-slack";
                    break;
                case "LinkedIn":
                    return "fa-linkedin";
                    break;
                case "Pinterest":
                    return "fa-pinterest";
                    break;
                default:
                    return "";
                    break;
            }
        }

        function _openModal(data) {
            var modalInstance = vm.$uibModal.open({
                animation: true,
                templateUrl: '/Scripts/sabio/app/userProfile/templates/shareProfileModal.html',
                controller: 'modalController as mc',
                size: 'md',
                resolve: {
                    selectedUser: function () {
                        return data;
                    }
                }
            });

            modalInstance.result.then(function (selectedItem) {
                vm.modalSelected = selectedItem;                    //If the user closed the modal by clicking Save
            }, function () {
                console.log('Modal dismissed at: ' + new Date());   //If the user closed the modal by clicking cancel
            });
        }


    }

})();