/*global CalcPopup, Languages, Site*/
var DoSingleTicket = (function ($) {
    "use strict";
    return {
        // Check input value
        checkInputValue: function (sportId) {
            switch (sportId) {
                case 1:     // soccer
                case 7:     // billiard
                case 10:    // golf
                case 11:    // motorsports
                    return true;

                case 2:     // basketball
                case 3:     // football
                case 26:    // rugby
                case 32:    // netball
                    return Site.isValidScore(['Home1Q', 'Home2Q', 'Home3Q', 'Home4Q',
                        'Away1Q', 'Away2Q', 'Away3Q', 'Away4Q']);

                case 4:     // ice hockey
                case 28:    // field hockey
                    return Site.isValidScore(['Home1Q', 'Home2Q', 'Home3Q', 'Away1Q',
                        'Away2Q', 'Away3Q']);

                case 5:     // tennis
                    return Site.isValidScore(['Home1Q', 'Home2Q', 'Home3Q', 'Home4Q',
                        'Home5Q', 'Away1Q', 'Away2Q', 'Away3Q', 'Away4Q',
                        'Away5Q']);

                case 8:     // baseball
                    return Site.isValidScore(['Home1Q', 'Home2Q', 'Home3Q', 'Home4Q',
                        'Home5Q', 'Home6Q', 'Home7Q', 'Home8Q', 'Home9Q',
                        'Away1Q', 'Away2Q', 'Away3Q', 'Away4Q', 'Away5Q',
                        'Away6Q', 'Away7Q', 'Away8Q', 'Away9Q']);

                case 6:     // volleyball
                case 9:     // badminton
                    return Site.isValidScore(['Home1Q', 'Home2Q', 'Home3Q', 'Home4Q',
                        'Home5Q', 'Away1Q', 'Away2Q', 'Away3Q', 'Away4Q',
                        'Away5Q']);

                default:
                    return true;
            }
        },

        // is exist first goal and last goal
        checkValueGoalRecord: function (type, sportId) {
            // only check for soccer and FT or ALL
            if (sportId === 1) {
                if (type === Type.CalculateAll || type === Type.CalculateFullTime || type === Type.CalculateFgNg) {
                    return DoSingleTicket.isGoalRecordExist(type);
                } else if (type === Type.CalculateHalfTime) {
                    return DoSingleTicket.isHalfTimeGoalRecordConsist();
                }
            }

            return true;
        },

        isHalfTimeGoalRecordConsist: function () {
            var isExistHtFgLg = $('#IsExistHtFgLg').val().toLowerCase() === 'true' ? true : false;
            if (isExistHtFgLg) {
                var goalRecord = $('#GoalRecord').val().toLowerCase();
                var halfTimeGoalRecord = goalRecord.indexOf(';') < 0 ? goalRecord : goalRecord.slice(0, goalRecord.indexOf(';'));

                var homeH = ($('#HtHomeScore').val() === "" || $('#HtHomeScore').val() === 0) ? 0 : parseInt($('#HtHomeScore').val());
                var awayH = ($('#HtAwayScore').val() === "" || $('#HtAwayScore').val() === 0) ? 0 : parseInt($('#HtAwayScore').val());
                var isLive = $('#IsLive').val() === 'True';

                if (isLive) {
                    if ((homeH + awayH) !== halfTimeGoalRecord.length || Site.replaceAll(halfTimeGoalRecord, 'a', '').length !== homeH) {
                        Site.showErrorMessage(Languages.HalfTimeGoalRecordNotConsistent);
                        return false;
                    }
                } else {
                    if (homeH < 0 || awayH < 0) {
                        Site.showErrorMessage(Languages.HalfTimeGoalRecordNotConsistent);
                        return false;
                    }

                    if (homeH + awayH !== halfTimeGoalRecord.length) {
                        Site.showErrorMessage(Languages.HalfTimeGoalRecordNotConsistent);
                        return false;
                    }

                    if (halfTimeGoalRecord.length > 0) {
                        var goalRecordTrimH = Site.replaceAll(halfTimeGoalRecord, 'h', '');
                        var goalRecordTrimA = Site.replaceAll(halfTimeGoalRecord, 'a', '');

                        if (goalRecordTrimA.length !== homeH || goalRecordTrimH.length !== awayH) {
                            Site.showErrorMessage(Languages.HalfTimeGoalRecordNotConsistent);
                            return false;
                        }
                    }
                }
            }

            return true;
        },
        isSuspendHalfTime: function (goalRecord) {
            var homeH = $('#HtHomeScore').val();
            var awayH = $('#HtAwayScore').val();
            var hasSemicolon = goalRecord.indexOf(";") > -1 ? true : false;
            var isAlreadyRefundHT = $('#IsAlreadyRefundHT').val();

            if ((homeH !== "" || awayH !== "" || hasSemicolon)  && !isAlreadyRefundHT) {
                return false;
            }

            return true;
        },
        isGoalRecordExist: function (type) {
            var goalRecord = $('#GoalRecord').val().toLowerCase();
            var fullTimeGoalRecord = goalRecord.replace(';', '');

            var homeF = ($('#FinalHomeScore').val() === "" || $('#FinalHomeScore').val() === 0) ? 0 : parseInt($('#FinalHomeScore').val());
            var awayF = ($('#FinalAwayScore').val() === "" || $('#FinalAwayScore').val() === 0) ? 0 : parseInt($('#FinalAwayScore').val());
            var homeH = ($('#HtHomeScore').val() === "" || $('#HtHomeScore').val() === 0) ? 0 : parseInt($('#HtHomeScore').val());
            var awayH = ($('#HtAwayScore').val() === "" || $('#HtAwayScore').val() === 0) ? 0 : parseInt($('#HtAwayScore').val());

            if (type === Type.CalculateFgNg) {
                if (homeH > homeF || awayH > awayF) {
                    homeF = homeH;
                    awayF = awayH;
                }
            }
            var isLive = $('#IsLive').val() === 'True';
            var isExistFgLg = $('#IsExistFgLg').val() === 'True';

            if (!DoSingleTicket.isSuspendHalfTime(goalRecord)) {
                if (!DoSingleTicket.isHalfTimeGoalRecordConsist()) {
                    return false;
                }
            }

            if (isExistFgLg) {
                if (isLive) {
                    if ((homeF + awayF) !== fullTimeGoalRecord.length || Site.replaceAll(fullTimeGoalRecord, 'a', '').length !== homeF) {
                        Site.showErrorMessage(Languages.GoalRecordNotConsistent);
                        return false;
                    }
                } else {
                    if (homeF < 0 || awayF < 0) {
                        Site.showErrorMessage(Languages.GoalRecordNotConsistent);
                        return false;
                    }

                    if (homeF + awayF !== fullTimeGoalRecord.length) {
                        Site.showErrorMessage(Languages.GoalRecordNotConsistent);
                        return false;
                    }

                    if (fullTimeGoalRecord.length > 0) {
                        var goalRecordTrimH = Site.replaceAll(fullTimeGoalRecord, 'h', '');
                        var goalRecordTrimA = Site.replaceAll(fullTimeGoalRecord, 'a', '');

                        if (goalRecordTrimA.length !== homeF || goalRecordTrimH.length !== awayF) {
                            Site.showErrorMessage(Languages.GoalRecordNotConsistent);
                            return false;
                        }
                    }
                }
            }
            return true;
        },

        // Do calculation
        doCalculate: function (type) {
            var sportId = parseInt($("#SportId").val());

            // Exist first goal and last goal
            if (!DoSingleTicket.checkValueGoalRecord(type, sportId)) {
                return false;
            }

            var messageFirst = "";
            var messageSecond = "";

            switch (type) {
                case Type.CalculateHalfTime:
                    if (!CalcPopup.isInputScoreDirectly && !DoSingleTicket.checkInputValue(sportId)) {
                        $("#messageBox").show();
                        return false;
                    }
                    messageFirst = Languages.Warning + Languages.DoubleCheckCalculateHalfTime;
                    messageSecond = Languages.CheckAgainCalculateHalfTime;
                    break;
                case Type.CalculateFullTime:
                    if (!CalcPopup.isInputScoreDirectly && !DoSingleTicket.checkInputValue(sportId)) {
                        $("#messageBox").show();
                        return false;
                    }
                    messageFirst = Languages.Warning + Languages.DoubleCheckCalculateFullTime;
                    messageSecond = Languages.CheckAgainCalculateFullTime;
                    break;
                case Type.CalculateAll:
                    if (!CalcPopup.isInputScoreDirectly && !DoSingleTicket.checkInputValue(sportId)) {
                        $("#messageBox").show();
                        return false;
                    }
                    messageFirst = Languages.Warning + Languages.DoubleCheckCalculateAll;
                    messageSecond = Languages.CheckAgainCalculateAll;
                    break;

                case Type.CalculateFgNg:
                    messageFirst = Languages.CalculateFirstGoalAndNoGoal;
                    break;
                case Type.UnCalculate:
                    messageFirst = Languages.Warning + Languages.DoubleCheckUnCalculate;
                    messageSecond = Languages.CheckAgainUnCalculate;
                    break;
            }
            $("#messageBox").hide();
            confirm(messageFirst, function () {
                if (messageSecond.length > 1) {
                    confirm(messageSecond, function () {
                        DoSingleTicket.doCalculateSubmit(type);
                        return true;
                    });
                    return false;
                }
                else {
                    DoSingleTicket.doCalculateSubmit(type);
                    return true;
                }
            });
            return false;
        },

        // Do Calculate
        doCalculateSubmit: function (type) {
            var url;
            $('#ActionType').val(type);

            if (type === Type.UnCalculate) {
                url = "Sportsbook/UnCalculate";
            } else {
                url = "Sportsbook/Calculate";
            }
            Site.blockUI();
            $.ajax({
                type: 'POST',
                async: true,
                url: Site.getAbsoluteUrl(url),
                data: $("#frm-calculate").serialize(),
                success: function (result) {
                    DoSingleTicket.resultCalculate(result);
                },
                error: function () {
                    alert(Languages.SystemError);
                },
                complete: function () {
                    Site.unBlockUI();
                }
            });
        },

        // Do Refund
        doRefundSubmit: function (type) {
            var url;
            $('#ActionType').val(type);
            url = "Sportsbook/Refund";

            Site.blockUI();
            $.ajax({
                type: 'POST',
                async: true,
                url: Site.getAbsoluteUrl(url),
                data: $("#frm-calculate").serialize(),
                success: function (result) {
                    DoSingleTicket.resultCalculate(result);
                },
                error: function () {
                    alert(Languages.SystemError);
                },
                complete: function () {
                    Site.unBlockUI();
                }
            });
        },

        resultCalculate: function (result) {
            if (!result.IsSuccessful) {
                Site.showErrorMessage(result.ErrorMessage);
            }
            else {
                parent.DialogBox.closePopup();
                parent.Calculate.submitForm();
            }
        }
    };
} (jQuery));