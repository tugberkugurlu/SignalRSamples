define('config', ['infuser'], function(infuser) {

    var
        toasts = {
            changesPending: 'Please save or cancel your changes before leaving the page.',
            errorSavingData: 'Data could not be saved. Please check the logs.',
            errorGettingData: 'Could not retrieve data.  Please check the logs.',
            invalidRoute: 'Cannot navigate. Invalid route',
            retreivedData: 'Data retrieved successfully',
            savedData: 'Data saved successfully'
        },
        viewIds = {
            messages: '#messages-view'
        },
        configureExternalTemplates = function () {
            infuser.defaults.templatePrefix = "_";
            infuser.defaults.templateSuffix = ".tmpl.html";
            infuser.defaults.templateUrl = "/Tmpl";
        },
        init = function () {
            configureExternalTemplates();
        };

    init();

    return {
        viewIds: viewIds,
        toasts: toasts
    };
});