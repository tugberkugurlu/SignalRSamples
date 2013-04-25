/// <reference path="../../require.js" />
define('bootstrapper',
    ['signalr', 'config', 'binder', 'vm', 'connection-boot'],
    function (conn, config, binder, vm, connBoot) {

        function run() {
            console.log('inside bootstrapper.js');
            vm.chatMessages.addNewMessage('tugberk', 'Hey Dude!!', false);
            connBoot.init();
        }

        return {
            run: run
        };
    });