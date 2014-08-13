/*global Site*/
var DialogBox = (function ($) {
    "use strict";
    return {
        dialogOpt: {
            position: { my: "top+50%", at: "center top", of: window },
            modal: true,
            width: 'auto',
            resizable: false
        },
        currentOpenDialog: null,
        openDialog: function (target, title, options) {
            options = options || {};
            if (title) {
                options.title = title;
            }
            if (target instanceof jQuery === false) {
                target = $(target);
            }

            DialogBox.currentOpenDialog = target.dialog($.extend({}, DialogBox.dialogOpt, options));

            var iframeLoading = target.find('iframe').get(0);
            if (iframeLoading) {
                iframeLoading.contentWindow.onload = function () {
                    Site.unBlockUI();
                };
            }

            return DialogBox.currentOpenDialog;
        },
        openDialogFrame: function (url, title, options, singleFrame) {
            var frameHtml = '<iframe src="' + url + '" width="100%" height="99%" frameborder="0" border="0"></iframe>';

            if (typeof singleFrame === 'undefined') {
                singleFrame = true; // not allow multiple instances of dialog by default
            }
            if (singleFrame) { // only one instance of dialog can be opened
                var frame = $("#dialog-modal");

                if (frame.length > 0) {
                    frame.html(frameHtml);
                } else {
                    frame = '<div id="dialog-modal">' + frameHtml + '</div>';
                }
                return DialogBox.openDialog(frame, title, options);
            }

            // multiple instances of dialog can be opened
            return DialogBox.openDialog('<div>' + frameHtml + '</div>', title, options);
        },

        closePopup: function () {
            $("#dialog-modal").dialog("close");
        }
    };
} (jQuery));