/// <reference path="../require.js" />
define('connection-boot', ['signalr', 'events'], function (conn, events) {

    var
        chatHub = conn.chatHub,

        init = function () {

            registerClientEvents();

            conn.hub.start()
                .done(function () {
                    events.connectionEstablished();
                    chatHub.server.getConnectedUsers().done(events.connectedUsersReceived);
                })
                .fail(events.connectionFailed);
        },

        registerClientEvents = function () {

            chatHub.client.received = events.messageReceived;
            chatHub.client.userConnected = events.newUserConnected;
            chatHub.client.userDisconnected = events.userDisconnected;
        };

    init();

    return {
    };
});