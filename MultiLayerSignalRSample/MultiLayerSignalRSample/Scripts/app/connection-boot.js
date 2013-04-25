/// <reference path="../require.js" />
define('connection-boot', ['vm.chatMessages'], function (chatMessages) {

    return {
        init: function () {
            console.log('Messages length: ' + chatMessages.messages().length);
        }
    };
});