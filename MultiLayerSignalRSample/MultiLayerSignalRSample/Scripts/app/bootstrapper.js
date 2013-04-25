/// <reference path="../../require.js" />
define('bootstrapper',
    ['signalr', 'config', 'binder', 'vm', 'connection-boot', 'model'],
    function (conn, config, binder, vm, connBoot, model) {

        function run() {
            console.log('inside bootstrapper.js');
            vm.chatMessages.addNewMessage('tugberk', 'Hey Dude!!', false);
            connBoot.init();
            console.log(model);
        }

        return {
            run: run
        };
    });