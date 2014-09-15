/*global Site*/
/*jshint -W066*/
/*jshint strict:false*/
var Recheckin = (function ($) {
    return {
        time: null,
        root: null,
        kickedOutMsg: null,
        run: null,
        init: function () {
            Recheckin.time = 8000;
            Recheckin.kickedOutMsg = 'You have logged in from another session or your login session has expired!';
            Recheckin.systemError = 'Oops... you are here because of a system error, please notify the administrators';
        },

        reCheckin: function () {
            if (Recheckin.run !== null) {
                Recheckin.run.abort();
            }
            Recheckin.run = $.ajax({
                url: Site.getAbsoluteUrl("UserLogin/Login/ReCheckLogin"),
                type: "POST",
                dataType: "json",
                success: function (result) {
                    if (result === "1") {
                        alert(Recheckin.kickedOutMsg, function () {
                            location.href = Site.getAbsoluteUrl("UserLogin/Login/LogOut");
                        });
                    }
                    if (result === "-1") {
                        window.location.href = Site.getAbsoluteUrl("UserLogin/Common/Offline");
                    }
                },
                error: function (request, status) {
                    if (status === "abort") {
                        return;
                    }
                }
            });
        },

        closeBrowser: function () {
            var index = window;
            while (index !== index.parent) // Jump out
            {
                index = index.parent;
            }
            index.close();
        }
    };
} (jQuery));

/*jshint ignore:start */
var recheckinId = null;
$(document).ready(function () {
    Recheckin.init();
    if (recheckinId != null) {
        clearInterval(recheckinId);
    }
    recheckinId = setInterval("Recheckin.reCheckin()", Recheckin.time);
});
/*jshint ignore:end */