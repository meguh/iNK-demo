//Angular controller factory
(function () {
    "use strict";

    angular.module(APPNAME)
        .controller('eventDetailsDiscussionController', DiscussionController);

    DiscussionController.$inject = ['$scope', '$baseController', '$discussionService', '$msgThreadService'];

    function DiscussionController(
        $scope
        , $baseController
        , $discussionService
        , $msgThreadService) {

        var vm = this; //this points to a new {}, attach properties to object
        vm.discs = null; //ajax calls will attach here
        vm.newDisc = {};
        vm.newDisc.threadId = vm.threadId;

        vm.threadInfo = null;
        vm.newThreadInfo = {};
        vm.newThreadInfo.threadId = vm.threadId;


        //attach dependency injections to the vm
        vm.$discussionService = $discussionService;
        vm.$msgThreadService = $msgThreadService;
        vm.$scope = $scope;

        //bindable functions
        vm.onGetDiscSuccess = _onGetDiscSuccess; //success handler
        vm.onDiscError = _onDiscError; //error handler
        vm.postAnswer = _postAnswer; //add new thread fn, which will run 'insert' ajax call
        vm.loadDisc = _loadDisc; //get all discussions for thread
        vm.onPostAnswerSuccess = _onPostAnswerSuccess;
        vm.onGetThreadInfoSuccess = _onGetThreadInfoSuccess //grab ThreadInfo



        //-- this line to simulate inheritance
        $baseController.merge(vm, $baseController);
        //grab threadId from url -- must be after $baseController.merge(vm, $baseController);
        vm.setUpCurrentRequest(vm);
        vm.threadId = vm.currentRequest.params.eventId;

        //this is a wrapper for our small dependency on $scope
        vm.notify = vm.$discussionService.getNotifier($scope);
        vm.notify = vm.$msgThreadService.getNotifier($scope);

        //startUp function
        render();


        console.log("eventsDiscussion route", vm.currentRequest);


        function render() {
            console.log("render/startUp has fired");

            _loadDisc();
        };




        function _onPostAnswerSuccess(data) {
            //this receives the data and calls the special notify method that will trigger ng to refresh UI
            console.log("_onPostAnswerSuccess has fired");
            vm.notify(function () {
                vm.newDisc = {};
                vm.newDisc.threadId = vm.threadId;
                console.log("this is threadId on postAnswerSucc ", vm.newDisc.threadId);
                _loadDisc();
            });
        };



        function _onGetDiscSuccess(data) {
            //this receives the data and calls the special notify method that will trigger ng to refresh UI
            console.log("_onGetDiscSuccess has fired");
            vm.notify(function () {
                console.log("main payload", data);
                vm.discs = data.items;
                vm.totalItems = data.totalItems;
            });
        };



        function _onDiscError(jqXhr, error) {
            console.error(error);
        };


        function _postAnswer() {
            console.log("postAnswer fn has fired");
            vm.newDisc.threadId = vm.threadId;
            console.log("vm.newDisc is ", vm.newDisc);
            vm.$discussionService.postAnswer(vm.newDisc, _onPostAnswerSuccess, _onDiscError); //insert ajax call
        };


        function _onGetThreadInfoSuccess(data) {
            console.log("_onGetThreadInfoSuccess has fired");
            vm.notify(function () {
                vm.threadInfo = data.item;
                console.log("this is vm.threadInfo", vm.threadInfo);


            });

        };



        function _loadDisc() {
            console.log("_loadDisc has been called");
            console.log("this is vm.threadId", vm.threadId);

            var payload = {

                threadId: vm.threadId
            }

            //get discussion by threadId
            vm.$discussionService.getByThreadId(payload, _onGetDiscSuccess, _onDiscError);

            //get threadInfo for table heading
            vm.$msgThreadService.getThreadInfoById(vm.threadId, _onGetThreadInfoSuccess, _onDiscError);

        };



    }

})();