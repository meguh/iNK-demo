(function () {
    "use strict";

    angular.module(APPNAME)
        .controller('eventDetailsDiscussionThreadsController', MsgThreadController);

    MsgThreadController.$inject = ['$scope', '$baseController', '$msgThreadService'];

    function MsgThreadController(
        $scope
        , $baseController
        , $msgThreadService) {

        var vm = this; //this points to a new {}, attach properties to object
        vm.threads = {}; //ajax calls will attach here
        vm.newThread = {};
        vm.newThread.eventId = $('#userIdFromModel').val();


        //attach dependency injections to the vm
        vm.$msgThreadService = $msgThreadService;
        vm.$scope = $scope;

        //bindable functions
        vm.onMsgThreadSuccess = _onMsgThreadSuccess; //success handler
        vm.onMsgThreadError = _onMsgThreadError; //error handler
        vm.addThread = _addThread //add new thread fn, which will run 'insert' ajax call
        vm.loadThreads = _loadThreads //get all Threads
        vm.onAddThreadSuccess = _onAddThreadSuccess


        //-- this line to simulate inheritance
        $baseController.merge(vm, $baseController);

        //this is a wrapper for our small dependency on $scope
        vm.notify = vm.$msgThreadService.getNotifier($scope);

        //startUp function
        render();

        function render() {
            console.log("render/startUp has fired");
            _loadThreads();
        };


        function _onAddThreadSuccess(data) {
            //this receives the data and calls the special notify method that will trigger ng to refresh UI
            console.log("_onMsgThreadSuccess has fired");
            vm.notify(function () {
                vm.newThread = {};
                vm.newThread.eventId = $('#userIdFromModel').val();
                _loadThreads();
                vm.$systemEventService.broadcast('refresh');


            });
        };



        function _onMsgThreadSuccess(data) {
            //this receives the data and calls the special notify method that will trigger ng to refresh UI
            console.log("_onMsgThreadSuccess has fired");
            vm.notify(function () {
                vm.threads = data.items;
                vm.threads.eventId = $('#userIdFromModel').val();
                console.log('this is vm.threads', vm.threads);

            });
        };


        function _onMsgThreadError(jqXhr, error) {
            console.error(error);
        };



        function _addThread() {
            console.log("addThread fn has fired");
            vm.$msgThreadService.postEventMsgThread(vm.newThread, _onAddThreadSuccess, _onMsgThreadError); //insert ajax call

        };


        function _loadThreads() {
            console.log("_loadThreads has been called");

            var payload = {

                eventId: $('#userIdFromModel').val()
            }

        vm.$msgThreadService.getThreadsByEventId(payload, _onMsgThreadSuccess, _onMsgThreadError);
            console.log("this is payload", payload);
        };





    }

})();