/// <reference path="../../require.js" />
(function() {
    var root = this;

    define3rdPartyModules();
    loadPluginsAndBoot();

    function define3rdPartyModules() {
        
        // These are already loaded via bundles.
        // Just register them through require.js
        define('jquery', [], function () { return root.jQuery; });
        define('ko', [], function () { return root.ko; });
        define('signalr', [], function () { return root.jQuery.connection; });
        define('amplify', [], function () { return root.amplify; });
        define('infuser', [], function () { return root.infuser; });
        define('moment', [], function () { return root.moment; });
        define('sammy', [], function () { return root.Sammy; });
        define('toastr', [], function () { return root.toastr; });
        define('underscore', [], function () { return root._; });
    }
    
    function loadPluginsAndBoot() {

        // Plugins must be loaded after jQuery and Knockout, 
        // since they depend on them.
        requirejs(['ko.debug.helpers'], boot);
    }

    function boot() {
        require(['bootstrapper'], function(bs) { bs.run(); });
    }
}());