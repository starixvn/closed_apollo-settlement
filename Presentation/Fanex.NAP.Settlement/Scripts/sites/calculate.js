/// <reference path="../libraries/jquery.js" />
/// <reference path="../libraries/jquery-ui.js" />
/// <reference path="global.js" />

var Calculate = (function ($) {
    return {
        initialize: function () {
            Site.unBlockUI();
            Calculate.FromDate = $("#FromDate");
            Calculate.ToDate = $("#ToDate");
            Calculate.SportTypeId = $("#SportTypeId");
            Calculate.LeagueId =  $("#LeagueId");
            Calculate.StatusId = $("#StatusId");
            Calculate.initDate(Calculate.FromDate, true);
            Calculate.initDate(Calculate.ToDate, true);
            Calculate.initAutoComplete();
            Calculate.initSize();
            Calculate.initEvent();
            Calculate.initCheckBoxMultipleCalculate();

            Calculate.saveFilterParams();
            Site.resize(210);

            Site.onEnter(Calculate.submitForm);
        },

        initEvent: function () {
            // Disable status dropdown when check on Un-Calculated Game Only or Un-Calculated Parlay Only
            $("#filter-container").on("click", "#UnCalculate, #UncalculateMultiple", Calculate.disableStatus);

            // Event click to get list settlement
            $("#filter-container").on("click", "#SubmitButton", Calculate.submitForm);

            // Event click on Refund link
            $("#content-cotainer").on("click", ".act-refund, .rf", Calculate.refund);

            // Event Click on button Multiple Calculate
            $("#content-cotainer").on("click", ".act-calc-mix-parlay", Calculate.calculateMixParlay);

            // Event Click on reference action button
            $("#content-cotainer").on("click", ".act-parlay-ref", Calculate.calculateMixParlayRef);

            // Event Click on button calculate (act-calculate)
            // Event action from IBC (act-calc-ref)
            $("#content-cotainer").on("click", ".act-calculate, .act-calc-ref", Calculate.calculate);

            // Event Click on icon refresh league name
            $("#content-cotainer").on("click", ".act-refresh-league", Calculate.refreshLeague);

            // Event Click on button view match log
            $("#content-cotainer").on("click", ".act-match-log", Calculate.viewSettlementLog);

            // Event view log action from IBC
            $("#content-cotainer").on("click", ".act-view-log", Calculate.referenceActions);

            //Event Click on Auto Settlement button
            $('#filter-container').on('click', '.btn-auto-settle-all', Calculate.switchAutoSettleAll);

            // Event click to Auto Settlement
            $('#content-cotainer').on('click', '.act-auto-settle', Calculate.switchAutoSettleSingle);
        },
        initDate: function (id, isChange) {
            Calculate.refreshDate(id);
            id.datepicker({
                showOn: 'both',
                buttonImage: Site.getAbsoluteUrl("Content/sites/images/calendar.png"),
                buttonText: "Show Date Picker",
                buttonImageOnly: true,
                changeMonth: true,
                changeYear: true,
                dateFormat: 'yy-mm-dd',
                onSelect: function () {
                    if (isChange) {
                        Calculate.changeDate();
                    }
                }
            });
        },
        initAutoComplete: function () {
            if (typeof (Calculate.SportTypeId.val()) !== 'undefined' && Calculate.SportTypeId.is("select")) {
                // Default selected is Soccer
                Calculate.SportTypeId.val("1");

                Calculate.SportTypeId.combobox({
                    selected: function () {
                        Calculate.getLeagues();
                    }
                });
            }

            // Default selected status is All
            Calculate.StatusId.val("0");
            Calculate.LeagueId.combobox();
            Calculate.StatusId.combobox();
        },
        initSize: function () {
            $(window).resize(function () { Site.resize(210); });
        },
        refreshDate: function (id) {
            var date;
            if(id == Calculate.FromDate)
            {
                date = $("#OriginalFromDate").val();
            }
            else if(id == Calculate.ToDate)
            {
                date = $("#OriginalToDate").val();
            }
            id.val(date);
        },
        // Disable status
        disableStatus: function () {
            if ($("#UnCalculate").is(':checked') || $("#UncalculateMultiple").is(':checked')) {
               Calculate.StatusId.combobox('disable');
            } else {
                Calculate.StatusId.combobox('enable');
            }
        },
        // Event change Date
        changeDate: function () {
            // Hide tooltip
            Validators.hide();
            Calculate.getLeagues();
        },

        // Re-load Leagues when we choose sporttype
        getLeagues: function () {
            var params,
            fromDate,
            toDate,
            isOutright = false;

            fromDate = Calculate.FromDate.val();

            if (typeof (Calculate.ToDate.val()) !== 'undefined') {
                toDate = Calculate.ToDate.val();
            } else {
                toDate = fromDate;
            }

            if($("#IsOutright").length)
            {
                isOutright = true;
            }

            params =
            {
                "fromDate": fromDate,
                "toDate": toDate,
                "sportTypeId": Calculate.SportTypeId.val(),
                "isOutright": isOutright
            };

            params.showAll = true;

            $.ajax({
                type: 'POST',
                url: Site.getAbsoluteUrl("UserControl/LeaguesList"),
                contentType: "application/json",
                data: JSON.stringify(params),
                success: function (result) {
                    $("#league-div").html(result);
                    // For the same control select
                    $("#LeagueId").combobox();
                    Site.unBlockUI();
                }
            });
        },

        // Check if value is in select list
        isExistValueInSelect: function (id) {
            if ($("#" + id).length > 0) {
                var input = $("#input-" + id).attr("temp-text").replace(/\s/g, '');
                var exists = false;

                if (typeof (input) !== 'undefined') {
                    $("#" + id + " option").each(function () {
                        if ($.trim(this.text.toLowerCase().replace(/\s/g, '')) == $.trim(input.toLowerCase())) {
                            exists = true;
                        }
                    });
                } else {
                    return true;
                }

                if (!exists) {
                    return false;
                }
            }

            return true;
        },

        // Check value entered in select list
        checkAllFilterSelect: function () {
            var msg = Languages.ValueEnterOnList;

            if (!Calculate.isExistValueInSelect("SportTypeId")) {
                Validators.show("input-SportTypeId", msg);
                return false;
            }

            if (!Calculate.isExistValueInSelect("LeagueId")) {
                Validators.show("input-LeagueId", msg);
                return false;
            }

            if (!Calculate.isExistValueInSelect("StatusId")) {
                Validators.show("input-StatusId", msg);
                return false;
            }

            return true;
        },

        // Event Submit
        submitForm: function () {
            // Check all value filter select list
            if (!Calculate.checkAllFilterSelect()) {
                return false;
            }

            Site.blockUI();
            var formId = $('#calculate-form');
            Calculate.saveFilterParams();
            var params = JSON.parse($('#HiddenCriteria').val());

            $.ajax({
                type: "POST",
                url: formId.attr("action"),
                data: params,
                success: function (data) {
                    $("#content-cotainer").html(data);
                    Calculate.initCheckBoxMultipleCalculate();
                },
                error: function () {
                    alert(Languages.SystemError);
                },
                complete: function () {
                    Site.unBlockUI();
                }
            });
            return false;
        },

        // Refresh league name
        refreshLeague: function () {
            Site.blockUI();

            var self, url, params, sportTypeId, leagueId;
            self = this;

            // get the original params
            params = JSON.parse($('#HiddenCriteria').val());

            // modify the league property
            leagueId = $(self).attr("data-league-id");
            params['LeagueId'] = leagueId;
            params['IsReLoad'] = true;

            if (typeof ($("#overOnly").val()) !== 'undefined' && $("#overOnly").val() == 1) {
                url = Site.getAbsoluteUrl("Settlement/Sportsbook/OverOnly");
            } else if (typeof ($("#htOverOnly").val()) !== 'undefined' && $("#htOverOnly").val() == 1) {
                url = Site.getAbsoluteUrl("Settlement/Sportsbook/HtOverOnly");
            } else {
                url = Site.getAbsoluteUrl("Settlement/Sportsbook/Index");
            }

            $.ajax({
                type: 'POST',
                url: url,
                data: params,
                success: function (result) {
                    $(".row-" + leagueId).remove();
                    $(".league-item-" + leagueId).after(result);
                    Calculate.initCheckBoxMultipleCalculate();
                },
                error: function () {
                    alert(Languages.SystemError);
                },
                complete: function () {
                    Site.unBlockUI();
                }
            });
        },

        // Init CheckBox Multiple Calculate
        initCheckBoxMultipleCalculate: function () {
            var count, leagueCheckBoxList, $checkbox, leagueId;
            count = 0;
            leagueCheckBoxList = $("input[type=checkbox].league-chk");
            leagueCheckBoxList.each(function (index, checkbox) {
                $checkbox = $(checkbox);
                leagueId = $checkbox.attr("data-league-id");
                count = $("input.league-" + leagueId).filter(":not(:disabled)").length;
                $checkbox.attr("disabled", count <= 1);
            });

            count = $("input.match-chk").filter(":not(:disabled)").length;
            $("#MixParlayAllChk").attr("disabled", count === 0);

            // Unbind change event for all check box
            var allParlayCheckBox = $("input[type=checkbox].parlay-chk");
            allParlayCheckBox.unbind("change");

            // Set up the value 'disabled'  for check box league & check box all
            leagueCheckBoxList.each(function (index, checkbox) {
                $checkbox = $(checkbox);
                leagueId = $checkbox.attr("data-league-id");

                var leagueParlayCheckBox = $("input[type=checkbox].league-" + leagueId);
                leagueParlayCheckBox.checkboxgroup();
            });

            // Set up group box
            allParlayCheckBox.checkboxgroup();
        },

        // Alert Confirm action Calc Mix Parlay
        confirmActionMixParlay: function (matchIds, matchCodes) {
            var confirm = Languages.CalculateMultiple.replace("{0}", matchCodes);
            if (matchIds != '') {
                $("#dialog-confirm-calc-mp").dialog({
                    title: "Calculate Mix Parlay",
                    open: $("#msgContent").html(confirm),
                    modal: true,
                    resizable: false,
                    width: 500,
                    buttons: {
                        OK: function () {
                            DoMultipleTicket.doCalculateMixParlaySubmit(matchIds);
                            $(this).dialog("close");
                        },
                        Cancel: function () {
                            $(this).dialog("close");
                        }
                    }
                });
            }
        },

        // Event Click on button Multiple Calculate
        calculateMixParlay: function () {
            var matchIds, matchCodes;
            matchIds = matchCodes = "";

            var allParlayCheckBox = $("input[type=checkbox].match-chk");
            allParlayCheckBox.each(function (index, checkbox) {
                var $checkbox = $(checkbox);
                var id = $checkbox.attr("id");
                var matchCode = $checkbox.attr("data-matchcode");
                if ($(this).is(':checked')) {
                    var str = id.split('-');
                    if (matchIds == "") {
                        matchIds += str[1];
                        matchCodes += matchCode;
                    }
                    else {
                        matchIds += ", " + str[1];
                        matchCodes += ", " + matchCode;
                    }
                }
            });

            Calculate.confirmActionMixParlay(matchIds, matchCodes);
        },

        // Event Click on reference action button
        calculateMixParlayRef: function () {
            var tr = $(this).parents('tr');
            var matchId = tr.attr("data-match-id");
            var matchCode = tr.attr("data-match-code");

            var unsettle = $("input[name=CheckBoxCalc-" + matchId + "][type='checkbox']:disabled").length;

            var isCalcLink = $('#calc-' + matchId).hasClass('code-link act-calculate') ;
            if(!isCalcLink)
                return;

            if (unsettle > 0) {
                alert(Languages.CannotSettleParlay);
            }
            else {
                Calculate.confirmActionMixParlay(matchId, matchCode);
            }
        },

        // View log Popup
        viewSettlementLog: function () {
            var tr = $(this).parents('tr');
            var matchCode = tr.attr("data-match-code");
            var title = 'Settlement Log';
            var url = Site.getAbsoluteUrl('Settlement/Log/Popup?matchCode=' + matchCode + '&FromDate=' + Calculate.FromDate.val() + '&ToDate=' + Calculate.ToDate.val());
            //DialogBox.openPopup(url, title, false, 450, false);
             DialogBox.openDialogFrame(url, title,{ width: 1011, height: 450, modal: false }, true);
        },

        // View Popup Calculation or reference action link
        calculate: function () {
            var tr = $(this).parents('tr');
            var matchId = tr.attr("data-match-id");
            var sportId = tr.attr("data-sport-id");
            var title = 'Calculation';
            var logId, actionId;

            if ($(this).attr("data-action-id") !== undefined) {
                actionId = $(this).attr("data-action-id");
            }
            else {
                actionId = -1;
            }

            if ($(this).attr("data-log-id") !== undefined) {
                logId = $(this).attr("data-log-id");
            }
            else {
                logId = -1;
            }

            var isCalcLink = $('#calc-' + matchId).hasClass('code-link act-calculate') ;

            var url = Site.getAbsoluteUrl("Settlement/Sportsbook/Calc?sportId=" + sportId + "&matchId=" + matchId
                                        + "&actionId=" + actionId + "&logId=" + logId);
            if(isCalcLink){
				DialogBox.openDialogFrame(url, title,{ width: 780, height: 380, modal: true });
                //DialogBox.openPopup(url, title, true);
            }
        },

        // Open Model Dialog Refund
        refund: function () {
            var tr = $(this).parents('tr');
            var matchId = tr.attr("data-match-id");
            var sportId = tr.attr("data-sport-id");
            var title = 'Refund';
            var logId, actionId;

            if ($(this).attr("data-action-id") !== undefined) {
                actionId = $(this).attr("data-action-id");
            }
            else {
                actionId = -1;
            }

            if ($(this).attr("data-log-id") !== undefined) {
                logId = $(this).attr("data-log-id");
            }
            else {
                logId = -1;
            }
            var url = Site.getAbsoluteUrl("Settlement/Sportsbook/Refund?sportId=" + sportId + "&matchId=" + matchId + "&actionId=" + actionId + "&logId=" + logId);

            var isRefundLink = $('#refund-' + matchId).hasClass('act-refund code-link');
            if(isRefundLink){
				DialogBox.openDialogFrame(url, title,{ width: 780, height: 360, modal: true });
                //DialogBox.openPopup(url, title, true);
            }
        },

        // Reference Actions from IBC
        referenceActions: function () {
            var tr = $(this).parents('tr');
            var matchId = tr.attr('data-match-id');
            var sportId = tr.attr("data-sport-id");
            var title = 'Reference Action Log';
            var url = Site.getAbsoluteUrl("Settlement/Log/ReferenceActions?matchId=" + matchId + "&sportId=" + sportId);
            //DialogBox.openPopup(url, title, true);
            DialogBox.openDialogFrame(url, title,{ width: 720, height: 500, modal: false }, true);
        },

        // Save old filter paramaters
        saveFilterParams: function () {
            // save the original params
            var params = {};
            $("form input:checkbox, form > select").each(function () {
                params[this.id] = this.checked;
            });
            $("form input[type=text], form select, form input[type=hidden]").each(function () {
                params[this.id] = this.value == "" ? null : this.value;
            });
            // delete redundant properties
            delete params['OriginalFromDate'];
            delete params['OriginalToDate'];
            delete params['HiddenCriteria'];

            $('#HiddenCriteria').val(JSON.stringify(params));
        },

        processSwitchAutoSettle: function (isTurnOn, matchId, leagueId) {
            var autoSettleButton = $('.btn-auto-settle-all');
            var autoSettleSingleMatch = $('#auto-settle-' + matchId);
            var url = Site.getAbsoluteUrl("Settlement/AutoSettlement/UpdateStatusAll");
            var calcLink = null, refundLink = null, checkBoxCalcMP = null;
            if(matchId>0)
            {
                calcLink =  $('#calc-' + matchId);
                refundLink = $('#refund-' + matchId);
                checkBoxCalcMP = $('#CheckBoxCalc-' + matchId);
            }

            var canCalculate = $('#CanCalculate').val() == 'True';
            var canRefund = $('#CanRefund').val() == 'True';

            var param = {
                isTurnOn: isTurnOn, //0-off; 1-on
                matchId: matchId
            };

            Site.blockUI();

            $.ajax({
                type: "POST",
                url: url,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify(param),
                success: function (data) {
                    if (data.ErrorCode == 0) {
                        if (isTurnOn == 0) {//OFF
                            if (matchId == 0) {//All
                                autoSettleButton.removeClass('on');
                            }
                            else { //Single match
                                autoSettleSingleMatch.removeClass('on');
                                if(canCalculate){
                                    calcLink.addClass('code-link act-calculate');
                                }
                                if(canRefund){
                                    refundLink.addClass('act-refund code-link');
                                }

                                // Remove disable checkbox
                                if(checkBoxCalcMP.hasClass('match-chk'))
                                {
                                    checkBoxCalcMP.removeAttr("disabled");
                                    Calculate.initCheckBoxMultipleCalculate();
                                    $('#CheckBoxLeagueCalc-'+ leagueId).removeAttr("disabled");
                                }
                            }
                        }
                        else {//ON
                            if (matchId == 0) { //All
                                autoSettleButton.addClass('on');
                            }
                            else { //Single match
                                autoSettleSingleMatch.addClass('on');
                                if(canCalculate){
                                    calcLink.removeClass('code-link act-calculate');
                                }
                                if(canRefund){
                                    refundLink.removeClass('act-refund code-link');
                                }

                                // Remove disable checkbox
                                checkBoxCalcMP.attr('checked', false);
                                checkBoxCalcMP.attr("disabled", true);
                                Calculate.initCheckBoxMultipleCalculate();
                            }
                        }

                        if (matchId == 0 && $("div.head-table").length) {
                            Calculate.submitForm();
                        }
                    }
                    else{
                        alert(data.ErrorMessage);
                        if(data.ErrorCode == 150){
                            autoSettleButton.removeClass('on');
                            Calculate.submitForm();
                        }
                    }
                },
                error: function () {
                    alert(Languages.SystemError);
                },
                complete: function () {
                    Site.unBlockUI();
                }
            });
        },

        switchAutoSettleAll: function () {
            var canSetAutoSettlement = $('#CanSetAutoSettlement').val() == 'True';
            if (!canSetAutoSettlement) {
                alert('You are not authorized to use this function. Please contact the admin for help.');
                return;
            }

            var autoSettleButton = $('.btn-auto-settle-all');
            var isAutoSettleOn = autoSettleButton.hasClass('on');
            var title = 'Auto Settlement';
            var width = 600;
            if (isAutoSettleOn) {
                confirm(Languages.AutoSettlementAll_ON_to_OFF,   function () {
                    Calculate.processSwitchAutoSettle(0, 0, 0);
                }, {width:width, title: title});
            }
            else {
                confirm(Languages.AutoSettlementAll_OFF_to_ON,   function () {
                    Calculate.processSwitchAutoSettle(1, 0, 0);
                }, {width:width, title: title});
            }
        },

        switchAutoSettleSingle: function () {
            var tr = $(this).parents('tr');
            var matchId = tr.attr('data-match-id');
            var leagueId =  tr.attr('class').split(' ')[0].replace("row-","");

            var autoSettleButton = $('.btn-auto-settle-all');
            var isAutoSettleOn = autoSettleButton.hasClass('on');
            //Can switch ON/OFF for a match when and only when Global Switch is ON
            if(!isAutoSettleOn){
                return;
            }

            var canSetAutoSettlement = $('#CanSetAutoSettlement').val() == 'True';
            if (!canSetAutoSettlement) {
                alert('You are not authorized to use this function. Please contact the admin for help.');
                return;
            }

            var isAutoSettleOn = $(this).hasClass('on');
            var title = 'Auto Settlement';
            var width = 430;
            if (isAutoSettleOn) {
                confirm(Languages.AutoSettlementSingle_ON_to_OFF, function () {
                    Calculate.processSwitchAutoSettle(0, matchId, leagueId);
                }, {width:width, title: title});
            }
            else {
                confirm(Languages.AutoSettlementSingle_OFF_to_ON, function () {
                    Calculate.processSwitchAutoSettle(1, matchId, leagueId);
                }, {width:width, title: title});
            }
        },
    };
} (jQuery));