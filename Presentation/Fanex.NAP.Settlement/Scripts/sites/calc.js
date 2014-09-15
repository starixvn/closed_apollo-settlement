/*global DoSingleTicket, Site*/
var CalcPopup = (function ($) {
    "use strict";
    var totalFinalHomeScore = 0, totalFinalAwayScore = 0;
    var totalFinalHomeSet = 0, totalFinalAwaySet = 0;
    var totalHalfTimeHomeScore = 0, totalHalfTimeAwayScore = 0;
    var firstGoal = "first-goal";
    var lastGoal = "last-goal";
    var firstLastGoal = "first-last-goal";
    var halfTimeFirstGoal = 'ht-first-goal';
    var halfTimeLastGoal = 'ht-last-goal';
    var halfTimeFirstLastGoal = 'ht-first-last-goal';
    return {
        isInputScoreDirectly: false,
        // Soccer
        soccerAddEventOnTextBox: function () {
            // Set Focus for textbox
            //   $("#HtHomeScore").focus();

            $("#GoalRecord").keydown(function () {
                return CalcPopup.goalRecordOnKeydown(arguments[0]);
            });

            $("#GoalRecord").keyup(function () {
                CalcPopup.goalRecordOnKeyup(arguments[0]);
            });

            // Only input numbers in TextBox
            $("#HtHomeScore, #FinalHomeScore, #HtAwayScore, #FinalAwayScore").keypress(function () {
                $("#errMsg").hide();
                return Site.onKeypress(arguments[0]);
            });
        },

        goalRecordOnKeydown: function (event) {
            // A:65, 97 | h:72, 104 | back:8| move left:37| move right:39| delete:46 | semi-colon: 186
            var charCode = (event.which) ? event.which : event.keyCode;
            var evt = event ? event : window.event;
            var obj = evt.currentTarget ? evt.currentTarget : evt.srcElement;
            var contentTextBox = $("#" + obj.id).val();

            if ((charCode !== 86) && (charCode === 65 || charCode === 72 || charCode === 104 || charCode === 8 ||
                 charCode === 35 || charCode === 36 || charCode === 37 || charCode === 39 || charCode === 46 ||
                 ((charCode === 186 || charCode === 59) && contentTextBox.indexOf(';') < 0) || charCode === 9)) {
                return true;
            }

            return false;
        },

        goalRecordOnKeyup: function (event) {
            var evt = event ? event : window.event;
            var obj = evt.currentTarget ? evt.currentTarget : evt.srcElement;
            var contentTextBox = $("#" + obj.id).val();
            var homeIcon = $('#home-icon');
            var awayIcon = $('#away-icon');
            var halfTimeHomeIcon = $('#ht-home-icon');
            var halfTimeAwayIcon = $('#ht-away-icon');

            var finalHomeScore = $('#FinalHomeScore').val();
            var finalAwayScore = $('#FinalAwayScore').val();
            var htHomeScore = $('#HtHomeScore').val();
            var htAwayScore = $('#HtAwayScore').val();

            homeIcon.removeClass();
            awayIcon.removeClass();
            halfTimeHomeIcon.removeClass();
            halfTimeAwayIcon.removeClass();

            contentTextBox = contentTextBox.toLowerCase();
            if (contentTextBox !== "" && contentTextBox !== ";") {
                if (contentTextBox.indexOf(';') > -1) {
                    CalcPopup.displayIconWithSemicolon(contentTextBox, homeIcon, awayIcon, halfTimeHomeIcon, halfTimeAwayIcon,
                                                       htHomeScore, htAwayScore, finalHomeScore, finalAwayScore);
                } else {
                    CalcPopup.displayIconWithOutSemicolon(contentTextBox, homeIcon, awayIcon, finalHomeScore, finalAwayScore);
                }
            }

            this.displayGoalRecord(contentTextBox);
        },

        displayIconWithSemicolon: function (contentTextBox, homeIcon, awayIcon, halfTimeHomeIcon, halfTimeAwayIcon,
                                            htHomeScore, htAwayScore, finalHomeScore, finalAwayScore) {
            var haftTimeGoalRecord = contentTextBox.slice(0, contentTextBox.indexOf(';'));
            var goalRecord = contentTextBox.replace(';', '');
            var isExistHTFGLG = $('#IsExistHtFgLg').val().toLowerCase() === 'true' ? true : false;
            var isExistFgLg = $('#IsExistFgLg').val().toLowerCase() === 'true' ? true : false;

            if (haftTimeGoalRecord.length > 0 && isExistHTFGLG && htHomeScore.length > 0 && htAwayScore.length > 0) {
                CalcPopup.displayIconWithSemicolonForHalfTime(haftTimeGoalRecord, halfTimeHomeIcon, halfTimeAwayIcon);
            }

            if (isExistFgLg) {
                CalcPopup.displayIconWithSemicolonForFullTime(goalRecord, homeIcon, awayIcon, finalHomeScore, finalAwayScore);
            }
        },

        displayIconWithSemicolonForHalfTime: function (haftTimeGoalRecord, halfTimeHomeIcon, halfTimeAwayIcon) {
            if (haftTimeGoalRecord.length === 1) {
                if (haftTimeGoalRecord.substring(0, 1) === "h") {
                    halfTimeHomeIcon.addClass(halfTimeFirstLastGoal);
                } else if (haftTimeGoalRecord.substring(0, 1) === "a") {
                    halfTimeAwayIcon.addClass(halfTimeFirstLastGoal);
                }
            } else {
                var contentLength = haftTimeGoalRecord.length;

                if (haftTimeGoalRecord.substring(0, 1) === "h" && haftTimeGoalRecord.substring(contentLength - 1, contentLength) === "h") {
                    halfTimeHomeIcon.addClass(halfTimeFirstLastGoal);
                } else if (haftTimeGoalRecord.substring(0, 1) === "a" && haftTimeGoalRecord.substring(contentLength - 1, contentLength) === "a") {
                    halfTimeAwayIcon.addClass(halfTimeFirstLastGoal);
                } else if (haftTimeGoalRecord.substring(0, 1) === "a" && haftTimeGoalRecord.substring(contentLength - 1, contentLength) === "h") {
                    halfTimeHomeIcon.addClass(halfTimeLastGoal);
                    halfTimeAwayIcon.addClass(halfTimeFirstGoal);
                } else if (haftTimeGoalRecord.substring(0, 1) === "h" && haftTimeGoalRecord.substring(contentLength - 1, contentLength) === "a") {
                    halfTimeHomeIcon.addClass(halfTimeFirstGoal);
                    halfTimeAwayIcon.addClass(halfTimeLastGoal);
                }
            }
        },

        displayIconWithSemicolonForFullTime: function (goalRecord, homeIcon, awayIcon, finalHomeScore, finalAwayScore) {
            if (goalRecord.length === 1) {
                if (goalRecord.substring(0, 1) === "h" && finalHomeScore.length > 0 && finalAwayScore.length > 0) {
                    homeIcon.addClass(firstLastGoal);
                } else if (goalRecord.substring(0, 1) === "a" && finalHomeScore.length > 0 && finalAwayScore.length > 0) {
                    awayIcon.addClass(firstLastGoal);
                } else if (goalRecord.substring(0, 1) === "h") {
                    homeIcon.addClass(firstGoal);
                } else if (goalRecord.substring(0, 1) === "a") {
                    awayIcon.addClass(firstGoal);
                }
            } else {
                var contentLength = goalRecord.length;

                if (goalRecord.substring(0, 1) === "h" && goalRecord.substring(contentLength - 1, contentLength) === "h" &&
                         finalHomeScore.length > 0 && finalAwayScore.length > 0) {
                    homeIcon.addClass(firstLastGoal);
                } else if (goalRecord.substring(0, 1) === "a" && goalRecord.substring(contentLength - 1, contentLength) === "a" &&
                               finalHomeScore.length > 0 && finalAwayScore.length > 0) {
                    awayIcon.addClass(firstLastGoal);
                } else if (goalRecord.substring(0, 1) === "a" && goalRecord.substring(contentLength - 1, contentLength) === "h" &&
                               finalHomeScore.length > 0 && finalAwayScore.length > 0) {
                    homeIcon.addClass(lastGoal);
                    awayIcon.addClass(firstGoal);
                } else if (goalRecord.substring(0, 1) === "h" && goalRecord.substring(contentLength - 1, contentLength) === "a" &&
                               finalHomeScore.length > 0 && finalAwayScore.length > 0) {
                    homeIcon.addClass(firstGoal);
                    awayIcon.addClass(lastGoal);
                } else if (goalRecord.substring(0, 1) === "h") {
                    homeIcon.addClass(firstGoal);
                } else if (goalRecord.substring(0, 1) === "a") {
                    awayIcon.addClass(firstGoal);
                }
            }
        },

        displayIconWithOutSemicolon: function (contentTextBox, homeIcon, awayIcon, finalHomeScore, finalAwayScore) {
            var isExistFgLg = $('#IsExistFgLg').val().toLowerCase() === 'true' ? true : false;

            if (isExistFgLg) {
                if (contentTextBox.length === 1) {
                    CalcPopup.displayIconWithOutSemicolonInOneChar(contentTextBox, homeIcon, awayIcon);
                }
                else {
                    CalcPopup.displayIconWithOutSemicolonInManyChar(contentTextBox, homeIcon, awayIcon, finalHomeScore, finalAwayScore);
                }
            }
        },

        displayIconWithOutSemicolonInOneChar: function (contentTextBox, homeIcon, awayIcon) {
            if (contentTextBox.substring(0, 1) === "h") {
                homeIcon.addClass(firstGoal);
            } else if (contentTextBox.substring(0, 1) === "a") {
                awayIcon.addClass(firstGoal);
            }
        },

        displayIconWithOutSemicolonInManyChar: function (contentTextBox, homeIcon, awayIcon, finalHomeScore, finalAwayScore) {
            var contentLength = contentTextBox.length;

            if (contentTextBox.substring(0, 1) === "h" && contentTextBox.substring(contentLength - 1, contentLength) === "h" &&
                         finalHomeScore.length > 0 && finalAwayScore.length > 0) {
                homeIcon.addClass(firstLastGoal);
            } else if (contentTextBox.substring(0, 1) === "a" && contentTextBox.substring(contentLength - 1, contentLength) === "a" &&
                               finalHomeScore.length > 0 && finalAwayScore.length > 0) {
                awayIcon.addClass(firstLastGoal);
            } else if (contentTextBox.substring(0, 1) === "a" && contentTextBox.substring(contentLength - 1, contentLength) === "h" &&
                               finalHomeScore.length > 0 && finalAwayScore.length > 0) {
                homeIcon.addClass(lastGoal);
                awayIcon.addClass(firstGoal);
            } else if (contentTextBox.substring(0, 1) === "h" && contentTextBox.substring(contentLength - 1, contentLength) === "a" &&
                               finalHomeScore.length > 0 && finalAwayScore.length > 0) {
                homeIcon.addClass(firstGoal);
                awayIcon.addClass(lastGoal);
            } else if (contentTextBox.substring(0, 1) === "h") {
                homeIcon.addClass(firstGoal);
            } else if (contentTextBox.substring(0, 1) === "a") {
                awayIcon.addClass(firstGoal);
            }
        },

        addGoalRecordRow: function (goalRecord) {
            var homeScore = 0, awayScore = 0, row, name, stopAddingHalfTimeRecord = false;
            var isExistHtFglg = $('#IsExistHtFgLg').val().toLowerCase() === 'true' ? true : false;

            for (var goal = 0; goal < goalRecord.length; goal++) {
                if (goalRecord[goal] === ';') {
                    stopAddingHalfTimeRecord = true;
                } else {
                    homeScore += 'h' === goalRecord[goal] ? 1 : 0;
                    awayScore += 'a' === goalRecord[goal] ? 1 : 0;
                    name = 'h' === goalRecord[goal] ? "Home" : "Away";

                    row = "<div class='goal-row'><div class='goal-score'> {0} - {1} </div>" + "		<div>{2}</div>" + "</div>";
                    row = row.replace("{0}", homeScore);
                    row = row.replace("{1}", awayScore);
                    row = row.replace("{2}", name);

                    $(".goal-record .content").append(row);

                    if (!stopAddingHalfTimeRecord && isExistHtFglg && goalRecord.indexOf(';') > -1) {
                        $(".ht-goal-record .content").append(row);
                    }
                }
            }
        },

        displayGoalRecord: function (value) {
            if (value === "" || value === ';') {
                $(".goal-record").html("");
                $(".ht-goal-record").html("");
            } else {
                var isExistHtFglg = $('#IsExistHtFgLg').val().toLowerCase() === 'true' ? true : false;
                var hasHalfTimeRecord = false;

                if (value.indexOf(';') < 0 && isExistHtFglg) {
                    hasHalfTimeRecord = true;
                } else if (value.slice(0, value.indexOf(';')).length > 0 && isExistHtFglg) {
                    hasHalfTimeRecord = true;
                } else {
                    hasHalfTimeRecord = false;
                }

                if (typeof ($(".goal-record .content").val()) === 'undefined') {
                    var header = "<div class='header'>FT Goal Record</div>";
                    header += " <div class='content'></div>";
                    $(".goal-record").append(header);
                }

                if (typeof ($(".ht-goal-record .content").val()) === 'undefined' && value.indexOf(';') > -1) {
                    var htHeader = "<div class='header'>HT Goal Record</div>";
                    htHeader += " <div class='content'></div>";
                    $(".ht-goal-record").append(htHeader);
                }

                if (hasHalfTimeRecord && value.indexOf(';') > -1) {
                    $(".ht-goal-record .content").html("");
                } else {
                    $(".ht-goal-record").html("");
                }

                $(".goal-record .content").html("");

                CalcPopup.addGoalRecordRow(value);
            }
        },

        // IceHockey
        iceHockeyScoreDirect: function (val) {
            for (var i = 1; i < 4; i++) {
                var homeId = "TextBoxHome" + i + "Q";
                var awayId = "TextBoxAway" + i + "Q";
                var homeValue = val ? '' : $('#' + homeId + '_hidden').val();
                var awayValue = val ? '' : $('#' + awayId + '_hidden').val();
                this.disableAndReLoadTextBox(homeId, val, homeValue);
                this.disableAndReLoadTextBox(awayId, val, awayValue);
            }

            this.disableAndReLoadTextBox('TextBoxHomeOT', val, val ? '' : $('#TextBoxHomeOT' + '_hidden').val());
            this.disableAndReLoadTextBox('TextBoxAwayOT', val, val ? '' : $('#TextBoxAwayOT' + '_hidden').val());
            this.disableAndReLoadTextBox('FinalHomeScore', !val, $('#FinalHomeScore' + '_hidden').val());
            this.disableAndReLoadTextBox('FinalAwayScore', !val, $('#FinalAwayScore' + '_hidden').val());

            // Set Focus for textbox
            if (!val) {
                //   $("#Home1Q").focus();
                CalcPopup.isInputScoreDirectly = false;
            }
            else {
                //  $("#FinalHomeScore").focus();
                CalcPopup.isInputScoreDirectly = true;
            }
        },

        iceHockeyAddEventOnTextBox: function (numberTextBox) {
            if ($(".act-set-directly-icehockey").length > 0) {
                for (var i = 1; i < numberTextBox; i++) {
                    this.eventOnTextBox("TextBoxHome" + i + "Q", "CalcPopup.onKeyupIceHockey()");
                    this.eventOnTextBox("TextBoxAway" + i + "Q", "CalcPopup.onKeyupIceHockey()");
                }

                this.eventOnTextBox("TextBoxHomeOT", "CalcPopup.onKeyupIceHockey()");
                this.eventOnTextBox("TextBoxAwayOT", "CalcPopup.onKeyupIceHockey()");
            }
            // Set Focus for textbox
            // $("#Home1Q").focus();
        },

        onKeyupIceHockey: function () {
            var numberTextBox = 4;
            this.setTotalScore(numberTextBox);

            totalFinalHomeScore += ($("#TextBoxHomeOT").val() === '') ? 0 : parseInt($("#TextBoxHomeOT").val(), 10);
            totalFinalAwayScore += ($("#TextBoxAwayOT").val() === '') ? 0 : parseInt($("#TextBoxAwayOT").val(), 10);

            $("#FinalHomeScore").attr("value", totalFinalHomeScore);
            $("#FinalAwayScore").attr("value", totalFinalAwayScore);

            // Hide message when input data
            $("#errMsg").hide();
        },

        // Set focus in textbox for Billiard sport
        billiardOnTextBox: function () {
            // $("#FinalHomeScore").focus();
        },

        // Tennis
        tennisScoreDirect: function (val) {
            for (var i = 1; i < 6; i++) {
                var homeId = "TextBoxHome" + i + "Q";
                var awayId = "TextBoxAway" + i + "Q";
                var homeValue = val ? '' : $('#' + homeId + '_hidden').val();
                var awayValue = val ? '' : $('#' + awayId + '_hidden').val();
                this.disableAndReLoadTextBox(homeId, val, homeValue);
                this.disableAndReLoadTextBox(awayId, val, awayValue);
            }

            this.disableAndReLoadTextBox('HomeTotalGames', true, val ? '' : $('#HomeTotalGames' + '_hidden').val());
            this.disableAndReLoadTextBox('AwayTotalGames', true, val ? '' : $('#AwayTotalGames' + '_hidden').val());

            this.disableAndReLoadTextBox('FinalHomeScore', !val, $('#FinalHomeScore' + '_hidden').val());
            this.disableAndReLoadTextBox('FinalAwayScore', !val, $('#FinalAwayScore' + '_hidden').val());

            // Set Focus for textbox
            if (!val) {
                //  $("#Home1Q").focus();
                CalcPopup.isInputScoreDirectly = false;
            }
            else {
                // $("#FinalHomeScore").focus();
                CalcPopup.isInputScoreDirectly = true;
            }
        },

        tennisAddEventOnTextBox: function (numberTextBox) {
            if ($(".act-set-directly-tennis").length > 0) {
                for (var i = 1; i < numberTextBox; i++) {
                    this.eventOnTextBox("TextBoxHome" + i + "Q", "CalcPopup.onKeyupTennis()");
                    this.eventOnTextBox("TextBoxAway" + i + "Q", "CalcPopup.onKeyupTennis()");
                }
            }
            // Set Focus for textbox
            //  $("#Home1Q").focus();
        },

        onKeyupTennis: function () {
            var numberTextBox = 6;
            this.setTotalScore(numberTextBox);

            $("#HomeTotalGames").attr("value", totalFinalHomeScore);
            $("#AwayTotalGames").attr("value", totalFinalAwayScore);

            $("#FinalHomeScore").attr("value", totalFinalHomeSet);
            $("#FinalAwayScore").attr("value", totalFinalAwaySet);
            // Hide message when input data
            $("#errMsg").hide();
        },

        //Basketball
        basketballScoreDirect: function (val) {
            for (var i = 1; i < 5; i++) {
                var homeId = "TextBoxHome" + i + "Q";
                var awayId = "TextBoxAway" + i + "Q";
                var homeValue = val ? '' : $('#' + homeId + '_hidden').val();
                var awayValue = val ? '' : $('#' + awayId + '_hidden').val();
                this.disableAndReLoadTextBox(homeId, val, homeValue);
                this.disableAndReLoadTextBox(awayId, val, awayValue);
            }

            this.disableAndReLoadTextBox('TextBoxHomeOT', val, val ? '' : $('#TextBoxHomeOT_hidden').val());
            this.disableAndReLoadTextBox('TextBoxAwayOT', val, val ? '' : $('#TextBoxAwayOT_hidden').val());
            this.disableAndReLoadTextBox('HtHomeScore', !val, $('#HtHomeScore_hidden').val());
            this.disableAndReLoadTextBox('HtAwayScore', !val, $('#HtAwayScore_hidden').val());
            this.disableAndReLoadTextBox('FinalHomeScore', !val, $('#FinalHomeScore_hidden').val());
            this.disableAndReLoadTextBox('FinalAwayScore', !val, $('#TextBoxFinalAwayScore_hidden').val());

            if (!val) {
                //   $("#Home1Q").focus();
                CalcPopup.isInputScoreDirectly = false;
            }
            else {
                //  $("#HtHomeScore").focus();
                CalcPopup.isInputScoreDirectly = true;
            }
        },
        basketballAddEventOnTextBox: function (numberTextBox) {
            if ($(".act-set-directly-basketball").length > 0) {
                for (var i = 1; i < numberTextBox; i++) {
                    this.eventOnTextBox("TextBoxHome" + i + "Q", "CalcPopup.onKeyupBasketball()");
                    this.eventOnTextBox("TextBoxAway" + i + "Q", "CalcPopup.onKeyupBasketball()");
                }

                this.eventOnTextBox("TextBoxHomeOT", "CalcPopup.onKeyupBasketball()");
                this.eventOnTextBox("TextBoxAwayOT", "CalcPopup.onKeyupBasketball()");
            }
        },

        onKeyupBasketball: function () {
            var numberTextBox = 5;
            this.setTotalScore(numberTextBox, 2);

            $("#HtHomeScore").attr("value", totalHalfTimeHomeScore);
            $("#HtAwayScore").attr("value", totalHalfTimeAwayScore);

            totalFinalHomeScore += ($("#TextBoxHomeOT").val() === '') ? 0 : parseInt($("#TextBoxHomeOT").val(), 10);
            totalFinalAwayScore += ($("#TextBoxAwayOT").val() === '') ? 0 : parseInt($("#TextBoxAwayOT").val(), 10);

            $("#FinalHomeScore").attr("value", totalFinalHomeScore);
            $("#FinalAwayScore").attr("value", totalFinalAwayScore);

            // Hide message when input data
            $("#errMsg").hide();
        },

        //Baseball
        baseballScoreDirect: function (val) {
            for (var i = 1; i < 10; i++) {
                var homeId = "TextBoxHome" + i + "Q";
                var awayId = "TextBoxAway" + i + "Q";
                var homeValue = val ? '' : $('#' + homeId + '_hidden').val();
                var awayValue = val ? '' : $('#' + awayId + '_hidden').val();
                this.disableAndReLoadTextBox(homeId, val, homeValue);
                this.disableAndReLoadTextBox(awayId, val, awayValue);
            }

            this.disableAndReLoadTextBox('TextBoxHomeEI', val, val ? '' : $('#TextBoxHomeEI' + '_hidden').val());
            this.disableAndReLoadTextBox('TextBoxAwayEI', val, val ? '' : $('#TextBoxAwayEI' + '_hidden').val());
            this.disableAndReLoadTextBox('HtHomeScore', !val, $('#HtHomeScore' + '_hidden').val());
            this.disableAndReLoadTextBox('HtAwayScore', !val, $('#HtAwayScore' + '_hidden').val());
            this.disableAndReLoadTextBox('FinalHomeScore', !val, $('#FinalHomeScore' + '_hidden').val());
            this.disableAndReLoadTextBox('FinalAwayScore', !val, $('#FinalAwayScore' + '_hidden').val());

            // Set Focus for textbox
            if (!val) {
                // $("#Home1Q").focus();
                CalcPopup.isInputScoreDirectly = false;
            }
            else {
                //  $("#HtHomeScore").focus();
                CalcPopup.isInputScoreDirectly = true;
            }
        },
        baseballAddEventOnTextBox: function (numberTextBox) {
            if ($(".act-set-directly-baseball").length > 0) {
                for (var i = 1; i < numberTextBox; i++) {
                    this.eventOnTextBox("TextBoxHome" + i + "Q", "CalcPopup.onKeyupBaseball()");
                    this.eventOnTextBox("TextBoxAway" + i + "Q", "CalcPopup.onKeyupBaseball()");
                }

                this.eventOnTextBox("TextBoxHomeEI", "CalcPopup.onKeyupBaseball()");
                this.eventOnTextBox("TextBoxAwayEI", "CalcPopup.onKeyupBaseball()");
            }
            // Set Focus for textbox
            //$("#Home1Q").focus();
        },

        onKeyupBaseball: function () {
            var numberTextBox = 10;
            this.setTotalScore(numberTextBox, 5);

            $("#HtHomeScore").attr("value", totalHalfTimeHomeScore);
            $("#HtAwayScore").attr("value", totalHalfTimeAwayScore);

            totalFinalHomeScore += ($("#TextBoxHomeEI").val() === '') ? 0 : parseInt($("#TextBoxHomeEI").val(), 10);
            totalFinalAwayScore += ($("#TextBoxAwayEI").val() === '') ? 0 : parseInt($("#TextBoxAwayEI").val(), 10);

            $("#FinalHomeScore").attr("value", totalFinalHomeScore);
            $("#FinalAwayScore").attr("value", totalFinalAwayScore);

            // Hide message when input data
            $("#errMsg").hide();
        },

        // Utility: Set Total Score
        setTotalScore: function (numberTextBox, step) {
            totalFinalHomeScore = 0;
            totalFinalAwayScore = 0;
            totalFinalHomeSet = 0;
            totalFinalAwaySet = 0;
            for (var i = 1; i < numberTextBox; i++) {
                var homeScore = $("#TextBoxHome" + i + "Q").val();
                var awayScore = $("#TextBoxAway" + i + "Q").val();
                homeScore = (homeScore === '') ? '0' : homeScore;
                awayScore = (awayScore === '') ? '0' : awayScore;

                homeScore = (/[^0-9]/.test(homeScore)) ? 0 : parseInt(homeScore, 10);
                awayScore = (/[^0-9]/.test(awayScore)) ? 0 : parseInt(awayScore, 10);

                totalFinalHomeSet = (homeScore > awayScore) ? (totalFinalHomeSet += 1) : totalFinalHomeSet;
                totalFinalAwaySet = (homeScore < awayScore) ? (totalFinalAwaySet += 1) : totalFinalAwaySet;

                totalFinalHomeScore += homeScore;
                totalFinalAwayScore += awayScore;

                // Use on Basketball, Baseball sports.(Basketball i=2, Baseball i=5)
                if ((typeof (step) !== 'undefined') && (i === step)) {
                    totalHalfTimeHomeScore = totalFinalHomeScore;
                    totalHalfTimeAwayScore = totalFinalAwayScore;
                }
            }
        },

        // Utility: Disable and Empty TextBox
        disableAndEmptyTextBox: function (id, val) {
            if (typeof ($("#" + id).val()) !== 'undefined') {
                $("#" + id).attr("disabled", val);
                $("#" + id).attr("value", "");
            }
        },

        // Utility: Disable and re-load value for TextBox
        disableAndReLoadTextBox: function (id, disabled, value) {
            if (typeof ($("#" + id).val()) !== 'undefined') {
                $("#" + id).attr("disabled", disabled);
                $("#" + id).attr("value", value);
            }
        },

        // Utility: Add event keyup and keypress
        eventOnTextBox: function (id, functionName) {
            $("#" + id).keyup(function () {
                /*jslint evil: true */
                eval(functionName);
            });
            $("#" + id).keypress(function () {
                return Site.onKeypress(arguments[0]);
            });
        },

        bindEventAllSport: function () {
            // Set focus in textbox for Billiard sport
            CalcPopup.billiardOnTextBox();

            // Event click on checkbox IceHockey
            $('.act-set-directly-icehockey').click(function () {
                CalcPopup.iceHockeyScoreDirect($(this).is(':checked'));
            });

            // Event click on checkbox Tennis
            $('.act-set-directly-tennis').click(function () {
                CalcPopup.tennisScoreDirect($(this).is(':checked'));
            });

            // Event click on checkbox Basketball
            $('.act-set-directly-basketball').click(function () {
                CalcPopup.basketballScoreDirect($(this).is(':checked'));
            });

            // Event click on checkbox Baseball
            $('.act-set-directly-baseball').click(function () {
                CalcPopup.baseballScoreDirect($(this).is(':checked'));
            });

            // Event click on button CalculateHT
            $('.act-calc-ht').click(function () {
                DoSingleTicket.doCalculate(Type.CalculateHalfTime);
            });

            // Event click on button CalculateFT
            $('.act-calc-ft').click(function () {
                DoSingleTicket.doCalculate(Type.CalculateFullTime);
            });

            // Event click on button CalculateFGNG
            $('.act-calc-all').click(function () {
                DoSingleTicket.doCalculate(Type.CalculateAll);
            });

            // Event click on Calculate Fisrt Goal No Goal
            $('.act-calc-fgng').click(function () {
                DoSingleTicket.doCalculate(Type.CalculateFgNg);
            });

            // Event click on button UnCalculate
            $('.act-uncalc').click(function () {
                DoSingleTicket.doCalculate(Type.UnCalculate);
            });

            // Check exist control set directly and add dynamic event in textbox for Soccer sport
            CalcPopup.soccerAddEventOnTextBox();

            // Check exist control set directly and add dynamic event in textbox for IceHocky sport
            CalcPopup.iceHockeyAddEventOnTextBox(4);

            // Check exist control set directly and add dynamic event in textbox for Tennis sport
            CalcPopup.tennisAddEventOnTextBox(6);

            // Check exist control set directly and add dynamic event in textbox for Basketball sport
            CalcPopup.basketballAddEventOnTextBox(5);

            // Check exist control set directly and add dynamic event in textbox for Baseball sport
            CalcPopup.baseballAddEventOnTextBox(10);
        },

        init: function () {
            Site.unBlockUI();
            this.bindEventAllSport();
        }
    };
} (jQuery));