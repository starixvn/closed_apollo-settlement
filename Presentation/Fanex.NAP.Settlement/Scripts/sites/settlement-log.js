/*global DialogBox, Site, Languages, Validators*/

var SettlementLog = (function ($) {
    "use strict";
    return {
        init: function () {
            Site.unBlockUI();
            SettlementLog.FromDate = $("#FromDate");
            SettlementLog.ToDate = $("#ToDate");

            SettlementLog.initDate(SettlementLog.FromDate);
            SettlementLog.initDate(SettlementLog.ToDate);

            // Event click to get list settlement
            $('#SubmitButton').click(function () {
                SettlementLog.submitForm();
            });

            Site.onEnter(SettlementLog.submitForm);

            // Event Click on ip
            $(".action-ip-info").live("click", SettlementLog.viewIpInfo);
        },

        initDate: function (id) {
            SettlementLog.refreshDate(id);
            id.datepicker({
                showOn: 'both',
                buttonImage: Site.getAbsoluteUrl("Content/sites/images/calendar.png"),
                buttonText: "Show Date Picker",
                buttonImageOnly: true,
                changeMonth: true,
                changeYear: true,
                dateFormat: 'yy-mm-dd'
            });
        },
        refreshDate: function (id) {
            var date;
            if (id === SettlementLog.FromDate) {
                date = $("#OriginalFromDate").val();
            }
            else if (id === SettlementLog.ToDate) {
                date = $("#OriginalToDate").val();
            }
            id.val(date);
        },
        // Event Submit
        submitForm: function () {
            if ($("#MatchCode").val() === "") {
                Validators.show("MatchCode", Languages.InputMatchCode, true);
                return false;
            }
            Site.blockUI();
            $('#SettlementLogForm').submit();
            Site.unBlockUI();
            return true;
        },
        //Collapse, Expand match log
        displayMatchInfoBlock: function (id) {
            var matchinfo = document.getElementById("matchBlock" + id);
            var btnRight = document.getElementById("btnRight" + id);

            if (matchinfo.className.length === 0) {
                matchinfo.className = "hide";

                $(btnRight).removeClass("btn-right").addClass("btn-right-up");
            }
            else {
                matchinfo.className = '';

                $(btnRight).removeClass("btn-right-up").addClass("btn-right");
            }
        },
        viewIpInfo: function () {
            var ip = $(this).attr("ip");
            var title = 'IP Info';
            var url = Site.getAbsoluteUrl("Settlement/Log/ViewIpInfo?Ip=" + ip);
            DialogBox.openDialogFrame(url, title, { width: 300, height: 370, modal: false }, true);
            DialogBox.openPopup(url, title);
        }
    };
})(jQuery);