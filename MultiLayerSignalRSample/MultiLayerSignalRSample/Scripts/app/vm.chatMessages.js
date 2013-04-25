/// <reference path="../require.js" />
define('vm.chatMessages', ['ko'], function(ko) {

    var
        Message = function (from, msg, isPrivate) {
            this.from = ko.observable(from);
            this.message = ko.observable(msg);
            this.isPrivate = ko.observable(isPrivate);
        },
        messages = ko.observableArray([]),
        addNewMessage = function (from, msg, isPrivate) {
            messages.push(new Message(from, msg, isPrivate));
        };

    return {
        messages: messages,
        addNewMessage: addNewMessage
    };
});