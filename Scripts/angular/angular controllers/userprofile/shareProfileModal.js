//MODAL CONTROLLER ============================================MODAL CONTROLLER
(function () {
    "use strict";

    angular.module(APPNAME)
        .controller('modalController', ModalController);

    //  $uibModalInstance is coming from the UI Bootstrap library and is a reference to the modal window itself so we can work with it.
    //  items is the array passed in from the main controller above through the resolve property:
    ModalController.$inject = ['$scope', '$baseController', '$userService', '$uibModalInstance', 'selectedUser']

    function ModalController(
        $scope
      , $baseController
      , $userService
      , $uibModalInstance
      , selectedUser) {

        var vm = this;

        vm.myUser = selectedUser;
        vm.message = {};
        vm.shareForm = null;
        vm.showShareErrors = false;

        vm.submitShareProfile = _submitShareProfile;
        vm.shareSuccess = _shareSuccess;
        vm.cancelShare = _cancelShare;
        vm.ajaxError = _ajaxError;

        $baseController.merge(vm, $baseController);
        vm.$scope = $scope;
        vm.$uibModalInstance = $uibModalInstance;
        vm.$userService = $userService;


        function _submitShareProfile() {
            vm.showShareErrors = true;

            if (vm.shareForm.$valid) {
                vm.message.UserId = vm.myUser.userId;
                vm.$userService.socialEmail(vm.message, vm.shareSuccess, vm.ajaxError);
            }
            else {
                console.log("form submitted with invalid data")
            }
        }

        function _shareSuccess() {
            vm.$uibModalInstance.close();
        }

        function _ajaxError(jqXhr) {
            console.log("Share Profile Error");
        }

        function _cancelShare() {
            vm.$uibModalInstance.dismiss('cancel');
        };
    }
})();