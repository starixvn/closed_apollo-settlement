/*global Languages*/
/*jshint strict:false */
/*jshint unused:false*/
var Site = (function ($) {
    return {
        unBlockUI: function () {
            $('div.block-body').hide();
            $('div.loading').hide();
            $.unblockUI();
        },

        blockUI: function () {
            $('div.block-body').show();
            $('div.loading').show();
            $.blockUI({
                message: "",
                timeout: 30000
            });
        },
        // Get screen resolution show scroll max-height
        // header is height header and filter
        resize: function (header) {
            var winW = 1024, winH = 860;
            if (window.innerWidth && window.innerHeight) {
                winW = window.innerWidth;
                winH = window.innerHeight;
            }
            else {
                winH = $(window).height(); // For IE
            }

            // 273 is equal menu, filter, header
            $(".content-scroll").css('max-height', winH - header);
        },
        replaceAll: function (str, regexp, newstr) {
            /*jslint evil: true */
            return str.replace(eval('/' + regexp + '/g'), newstr);
        },

        // Validation Score
        isValidScore: function (arrItem) {
            var len = arrItem.length;
            if (len < 1) {
                return true;
            }
            for (var i = 0; i < len; i++) {
                if ($('#' + arrItem[i]).val()) {
                    return true;
                }
            }
            return false;
        },

        // Allow only numbers in TextBox
        onKeypress: function (event) {
            var charCode = (event.which) ? event.which : event.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }

            return true;
        },
        // Register function on enter keypress for all textbox
        onEnter: function (fn) {
            if (typeof fn === typeof (Function)) {
                // bind event submit form to input with type tetxtbox
                $("input[type=text]").on("keyup", function (evt) {
                    var keyCode = evt ? (evt.which ? evt.which : evt.keyCode) : event.keyCode;
                    // if user press enter
                    if (keyCode === 13) {
                        fn.apply();
                    }
                });
            }
        },

        // Show message
        showErrorMessage: function (msg) {
            $("#messageBox").show();
            $('#messageInfo').html(msg);
        }
    };
})(jQuery);

$.ajaxSetup({
    statusCode: {
        403: function () {
            return alert(Languages.NoPermission);
        }
    }
});

$(document).ready(function () {
    $("ul.subnav").parent().append("<span></span>"); //Only shows drop down trigger when js is enabled (Adds empty span tag after ul.subnav*)

    $("ul.topnav li.has-child div").click(function () {
        $(this).parent().find("ul.subnav").show().slideDown(); //Drop dow
    });
    $("ul.topnav li.has-child").hover(function () {
        $(this).find("ul.subnav").show(); //Drop down the subnav on click
        $(this).addClass("subhover"); //On hover over, add class "subhover"
    }, function () {	//On Hover Out
        $(this).removeClass("subhover"); //On hover out, remove class "subhover"
        $(this).find("ul.subnav").slideUp(); //When the mouse hovers out of the subnav, move it back
    });
});