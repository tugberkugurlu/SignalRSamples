/// <reference path="../require.js" />
define('vm', ['vm.chatMessages', 'vm.users'], function (chatMessagesViewModel, usersViewModel) {

    return {
        chatMessages: chatMessagesViewModel,
        users: usersViewModel
    };
});