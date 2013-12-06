var CalcPopup = (function ($) {
    var totalFinalHomeScore = 0, totalFinalAwayScore = 0;
    var totalFinalHomeSet = 0, totalFinalAwaySet = 0;
    var totalHalfTimeHomeScore = 0, totalHalfTimeAwayScore = 0;

    return {
        isInputScoreDirectly: false,
        // Soccer
        soccerAddEventOnTextBox: function () {
            // Set Focus for textbox
            //   $("#TextBoxHtHomeScore").focus();

            $("#GoalRecord").keydown(function () {
                return CalcPopup.goalRecordOnKeydown(arguments[0]);
            });

            $("#GoalRecord").keyup(function () {
                CalcPopup.goalRecordOnKeyup(arguments[0]);
            });

            // Only input numbers in TextBox
            $("#TextBoxHtHomeScore, #TextBoxFinalHomeScore, #TextBoxHtAwayScore, #TextBoxFinalAwayScore").keypress(function () {
                $("#errMsg").hide();
                return Site.onKeypress(arguments[0]);
            });
        },

        goalRecordOnKeydown: function (event) {
            // A:65, 97 | h:72, 104 | back:8| move left:37| move right:39| delete:46
            var charCode = (event.which) ? event.which : event.keyCode;
            if ((charCode != 86) && (charCode == 65 || charCode == 72 || charCode == 104 || charCode == 8 || charCode == 35 || charCode == 36 || charCode == 37 || charCode == 39 || charCode == 46 || charCode == 9))
                return true;
            return false;
        },

        goalRecordOnKeyup: function (event) {
            var evt = event ? event : window.event;
            var obj = evt.currentTarget ? evt.currentTarget : evt.srcElement;
            var contentTextBox = $("#" + obj.id).val();

            var firstGoal = "first-goal";
            var lastGoal = "last-goal";
            var firstLastGoal = "first-last-goal";
            $(".match-away").removeClass(firstGoal).removeClass(lastGoal).removeClass(firstLastGoal);
            $(".match-home").removeClass(firstGoal).removeClass(lastGoal).removeClass(firstLastGoal);

            contentTextBox = contentTextBox.toLowerCase();
            // Change Icon first goal and last goal
            // If value is empty then remove icon
            if (contentTextBox != "") {
                // Check first goal
                if ($('#TextBoxFinalHomeScore').val() == '' && $('#TextBoxFinalAwayScore').val() == '') {
                    //The system does NOT show the L icon because the Last Goal cannot be determined yet at this time.
                    if (contentTextBox.substring(0, 1) === "h") {
                        $(".match-home").addClass(firstGoal);
                    }
                    else if (contentTextBox.substring(0, 1) === "a") {
                        $(".match-away").addClass(firstGoal);
                    }
                }
                else if (contentTextBox.substring(0, 1) === "h" && contentTextBox.substring(contentTextBox.length - 1, contentTextBox.length) === "h") {
                    $(".match-home").addClass(firstLastGoal);
                } else if (contentTextBox.substring(0, 1) === "a" && contentTextBox.substring(contentTextBox.length - 1, contentTextBox.length) === "a") {
                    $(".match-away").addClass(firstLastGoal);
                } else if (contentTextBox.substring(0, 1) === "a" && contentTextBox.substring(contentTextBox.length - 1, contentTextBox.length) === "h") {
                    $(".match-away").addClass(firstGoal);
                    $(".match-home").addClass(lastGoal);
                } else if (contentTextBox.substring(0, 1) === "h" && contentTextBox.substring(contentTextBox.length - 1, contentTextBox.length) === "a") {
                    $(".match-home").addClass(firstGoal);
                    $(".match-away").addClass(lastGoal);
                }
            }

            // Display goal record data on Grid
            this.displayGoalRecord(contentTextBox);
        },

        // check content Goal Record on textbox, after that show data on grid
        displayGoalRecord: function (value) {
            // If value is empty then remove grid
            if (value === "") {
                $(".calc-action-goal-record").html("");
            }
            else {
                var homeScore,
                awayScore,
                row,
                name;

                homeScore = 0;
                awayScore = 0;

                if (typeof ($(".goal-record-content").val()) == 'undefined') {
                    var header = "<div class='goal-record-header'>Goal Record</div>";
                    header += " <div class='goal-record-content'></div>";
                    $(".calc-action-goal-record").append(header);
                }
                $(".goal-record-content").html("");

                for (var goal = 0; goal < value.length; goal++) {
                    homeScore += 'h' == value[goal] ? 1 : 0;
                    awayScore += 'a' == value[goal] ? 1 : 0;
                    name = 'h' == value[goal] ? "Home" : "Away";

                    row = "<div class='goal-record-row'><div class='goal-record-score'> {0} - {1} </div>" + "		<div>{2}</div>" + "</div>";

                    row = row.replace("{0}", homeScore);
                    row = row.replace("{1}", awayScore);
                    row = row.replace("{2}", name);
                    $(".goal-record-content").append(row);
                }
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
            this.disableAndReLoadTextBox('TextBoxFinalHomeScore', !val, $('#TextBoxFinalHomeScore' + '_hidden').val());
            this.disableAndReLoadTextBox('TextBoxFinalAwayScore', !val, $('#TextBoxFinalAwayScore' + '_hidden').val());

            // Set Focus for textbox
            if (!val) {
                //   $("#TextBoxHome1Q").focus();
                CalcPopup.isInputScoreDirectly = false;
            }
            else {
                //  $("#TextBoxFinalHomeScore").focus();
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
            // $("#TextBoxHome1Q").focus();
        },

        onKeyupIceHockey: function () {
            var numberTextBox = 4;
            this.setTotalScore(numberTextBox);

            totalFinalHomeScore += ($("#TextBoxHomeOT").val() == '') ? 0 : parseInt($("#TextBoxHomeOT").val(), 10);
            totalFinalAwayScore += ($("#TextBoxAwayOT").val() == '') ? 0 : parseInt($("#TextBoxAwayOT").val(), 10);

            $("#TextBoxFinalHomeScore").attr("value", totalFinalHomeScore);
            $("#TextBoxFinalAwayScore").attr("value", totalFinalAwayScore);
            // Hide message when input data
            $("#errMsg").hide();
        },

        // Set focus in textbox for Billiard sport
        billiardOnTextBox: function () {
            // $("#TextBoxFinalHomeScore").focus();
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

            this.disableAndReLoadTextBox('TextBoxHomeTotalGames', true, val ? '' : $('#TextBoxHomeTotalGames' + '_hidden').val());
            this.disableAndReLoadTextBox('TextBoxAwayTotalGames', true, val ? '' : $('#TextBoxAwayTotalGames' + '_hidden').val());

            this.disableAndReLoadTextBox('TextBoxFinalHomeScore', !val, $('#TextBoxFinalHomeScore' + '_hidden').val());
            this.disableAndReLoadTextBox('TextBoxFinalAwayScore', !val, $('#TextBoxFinalAwayScore' + '_hidden').val());

            // Set Focus for textbox
            if (!val) {
                //  $("#TextBoxHome1Q").focus();
                CalcPopup.isInputScoreDirectly = false;
            }
            else {
                // $("#TextBoxFinalHomeScore").focus();
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
            //  $("#TextBoxHome1Q").focus();
        },

        onKeyupTennis: function () {
            var numberTextBox = 6;
            this.setTotalScore(numberTextBox);

            $("#TextBoxHomeTotalGames").attr("value", totalFinalHomeScore);
            $("#TextBoxAwayTotalGames").attr("value", totalFinalAwayScore);

            $("#TextBoxFinalHomeScore").attr("value", totalFinalHomeSet);
            $("#TextBoxFinalAwayScore").attr("value", totalFinalAwaySet);
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
            this.disableAndReLoadTextBox('TextBoxHtHomeScore', !val, $('#TextBoxHtHomeScore_hidden').val());
            this.disableAndReLoadTextBox('TextBoxHtAwayScore', !val, $('#TextBoxHtAwayScore_hidden').val());
            this.disableAndReLoadTextBox('TextBoxFinalHomeScore', !val, $('#TextBoxFinalHomeScore_hidden').val());
            this.disableAndReLoadTextBox('TextBoxFinalAwayScore', !val, $('#TextBoxFinalAwayScore_hidden').val());

            if (!val) {
                //   $("#TextBoxHome1Q").focus();
                CalcPopup.isInputScoreDirectly = false;
            }
            else {
                //  $("#TextBoxHtHomeScore").focus();
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

            $("#TextBoxHtHomeScore").attr("value", totalHalfTimeHomeScore);
            $("#TextBoxHtAwayScore").attr("value", totalHalfTimeAwayScore);

            totalFinalHomeScore += ($("#TextBoxHomeOT").val() == '') ? 0 : parseInt($("#TextBoxHomeOT").val(), 10);
            totalFinalAwayScore += ($("#TextBoxAwayOT").val() == '') ? 0 : parseInt($("#TextBoxAwayOT").val(), 10);

            $("#TextBoxFinalHomeScore").attr("value", totalFinalHomeScore);
            $("#TextBoxFinalAwayScore").attr("value", totalFinalAwayScore);
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
            this.disableAndReLoadTextBox('TextBoxHtHomeScore', !val, $('#TextBoxHtHomeScore' + '_hidden').val());
            this.disableAndReLoadTextBox('TextBoxHtAwayScore', !val, $('#TextBoxHtAwayScore' + '_hidden').val());
            this.disableAndReLoadTextBox('TextBoxFinalHomeScore', !val, $('#TextBoxFinalHomeScore' + '_hidden').val());
            this.disableAndReLoadTextBox('TextBoxFinalAwayScore', !val, $('#TextBoxFinalAwayScore' + '_hidden').val());

            // Set Focus for textbox
            if (!val) {
                // $("#TextBoxHome1Q").focus();
                CalcPopup.isInputScoreDirectly = false;
            }
            else {
                //  $("#TextBoxHtHomeScore").focus();
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
            //$("#TextBoxHome1Q").focus();
        },

        onKeyupBaseball: function () {
            var numberTextBox = 10;
            this.setTotalScore(numberTextBox, 5);

            $("#TextBoxHtHomeScore").attr("value", totalHalfTimeHomeScore);
            $("#TextBoxHtAwayScore").attr("value", totalHalfTimeAwayScore);

            totalFinalHomeScore += ($("#TextBoxHomeEI").val() == '') ? 0 : parseInt($("#TextBoxHomeEI").val(), 10);
            totalFinalAwayScore += ($("#TextBoxAwayEI").val() == '') ? 0 : parseInt($("#TextBoxAwayEI").val(), 10);

            $("#TextBoxFinalHomeScore").attr("value", totalFinalHomeScore);
            $("#TextBoxFinalAwayScore").attr("value", totalFinalAwayScore);
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
                if (homeScore == '')
                    homeScore = '0';
                if (awayScore == '')
                    awayScore = '0';

                homeScore = (/[^0-9]/.test(homeScore)) ? 0 : parseInt(homeScore, 10);
                awayScore = (/[^0-9]/.test(awayScore)) ? 0 : parseInt(awayScore, 10);

                if (homeScore > awayScore) {
                    totalFinalHomeSet++;
                }
                else if (homeScore < awayScore) {
                    totalFinalAwaySet++;
                }

                totalFinalHomeScore += homeScore;
                totalFinalAwayScore += awayScore;

                // Use on Basketball, Baseball sports.(Basketball i=2, Baseball i=5)
                if ((typeof (step) !== 'undefined')) {
                    if (i == step) {
                        totalHalfTimeHomeScore = totalFinalHomeScore;
                        totalHalfTimeAwayScore = totalFinalAwayScore;
                    }
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

            $('.act-calc-fgng').click(function () {
                DoSingleTicket.doCalculate(Type.CalculateFgNg);
            });

            // Event click on button CalculateFGNG
            $('.act-calc-all').click(function () {
                DoSingleTicket.doCalculate(Type.CalculateAll);
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