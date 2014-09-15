/**
* Override javascript confirm() and wrap it into a jQuery-UI Dialog box
*
* Source: stackoverflow.com
* Edit by:Alex.Nguyen
* @depends $.getOrCreateDialog
*
* @param { String } the alert message
* @param { String/Object } the confirm callback
* @param { Object } jQuery Dialog box options
*/
/*jshint strict:false */
/*exported confirm*/
function confirm(message, callback, options) {
    var defaults = {
        modal: true,
        title: document.title,
        resizable: false,
        width: 400,
        buttons: {
            Ok: function () {
                $(this).dialog('close');

                try {
                    return (typeof callback === 'string') ?
                    window.location.href = callback :
                    callback();
                }
                catch (e)
                { }
            },
            Cancel: function () {
                $(this).dialog('close');
                return false;
            }
        },
        dialogClass: 'modal-shadow confirm-dialog'
    };

    var $confirm = $.getOrCreateDialog('confirm');
    // set message
    $("p", $confirm).html(message);
    // init dialog
    $confirm.dialog($.extend({}, defaults, options));
}

function alert(message, options) {
    var defaults = {
        modal: true,
        title: document.title,
        resizable: false,
        buttons: {
            Ok: function () {
                $(this).dialog('close');
            }
        },
        dialogClass: 'modal-shadow error-dialog'
    };

    var $alert = $.getOrCreateDialog('alert');
    // set message
    $("p", $alert).html(message);
    // init dialog
    $alert.dialog($.extend({}, defaults, options));
}
/*exported alert*/
function alert(message, callback, options) {
    var defaults = {
        modal: true,
        title: document.title,
        resizable: false,
        buttons: {
            Ok: function () {
                try {
                    $(this).dialog('close');
                    callback();
                }
                catch (e) {
                }
            }
        },
        dialogClass: 'modal-shadow error-dialog'
    };

    var $alert = $.getOrCreateDialog('alert');
    // set message
    $("p", $alert).html(message);
    // init dialog
    $alert.dialog($.extend({}, defaults, options));
}