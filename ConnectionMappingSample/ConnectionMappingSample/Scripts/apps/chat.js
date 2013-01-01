(function () {

    function Message(from, msg) {
        this.from = ko.observable(from);
        this.message = ko.observable(msg);
    }

    var viewModel = {
        messages: ko.observableArray([]),
        users: ko.observableArray([])
    };

    $(function () {

        var chatHub = $.connection.chatHub,
            loginHub = $.connection.login,
            $sendBtn = $('#btnSend'),
            $msgTxt = $('#txtMsg');

        // turn the logging on for demo purposes
        $.connection.hub.logging = true;

        chatHub.client.received = function (message) {
            viewModel.messages.push(new Message(message.sender, message.message));
        };

        chatHub.client.userConnected = function (username) {
            viewModel.users.push(username);
        };

        chatHub.client.userDisconnected = function (username) {
            viewModel.users.pop(username);
        };

        // $.connection.hub.starting(callback)
        // there is also $.connection.hub.(received|stateChanged|error|disconnected|connectionSlow|reconnected)

        // $($.connection.hub).bind($.signalR.events.onStart, callback)

        // $.connection.hub.error(function () {
        //     console.log("foo");
        // });

        startConnection();

        ko.applyBindings(viewModel);

        // helper functions

        function startConnection() {

            // forcing longPolling to make the login thing work. DDTAH (don't do this at home)!
            $.connection.hub.start({ transport: 'longPolling' }).done(function () {

                toggleInputs(false);
                bindClickEvents();

                chatHub.server.getConnectedUsers().done(function (users) {
                    $.each(users, function (i, username) {
                        viewModel.users.push(username);
                    });
                });

            }).fail(function (err) {

                console.log(err);
            });
        }

        function bindClickEvents() {

            $sendBtn.click(function (e) {

                var msgValue = $msgTxt.val();
                if (msgValue !== null && msgValue.length > 0) {

                    chatHub.server.send(msgValue).fail(function (err) {
                        console.log('Send method failed: ' + err);
                    });
                }
                e.preventDefault();
            });
        }

        function toggleInputs(status) {

            $sendBtn.prop('disabled', status);
            $msgTxt.prop('disabled', status);
        }
    });
}());