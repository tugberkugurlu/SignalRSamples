/// <reference path="../jquery-1.6.4.js" />
/// <reference path="../jquery.signalR-0.5.2.js" />
/// <reference path="../jquery-ui-1.8.20.js" />

$(function () {

    var hub = $.connection.moveShape,
        $shape = $("#shape");

        $.extend(hub, {
            shapeMoved: function (cid, x, y) {
                if (cid !== $.connection.hub.id) {
                    $shape.css({ left: x, top: y });
                }
            }
        });

        $.connection.hub.start().done(function () {

            $shape.draggable({
                drag: function () {
                    hub.moveShape(this.offsetLeft, this.offsetTop || 0);
                }
            });
        });

});