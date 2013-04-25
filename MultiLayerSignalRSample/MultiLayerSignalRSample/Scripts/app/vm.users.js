define('vm.users', ['ko'], function(ko) {

    var
        User = function (name) {
            
            var self = this;

            self.name = ko.observable(name);
            self.isPrivateChatUser = ko.observable(false);
            self.setAsPrivateChat = function (user) {
                self.isPrivateChatUser(true);
            };
        },
        
        users = ko.observableArray([]),
        addNewUser = function (name) {
            users.push(new User(name));
        };

    return {
        users: users,
        addNewUser: addNewUser
    };
});