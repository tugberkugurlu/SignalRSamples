/// <reference path="../../require.js" />
define('bootstrapper', [], function () {

    function run() {
        console.log('inside bootstrapper.js');
    }

    return {
        run: run
    };
});