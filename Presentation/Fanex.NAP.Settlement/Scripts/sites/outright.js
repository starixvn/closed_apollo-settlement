var OUTRIGHT = (function ($) {
    var countSelectTeam = 0;
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
            Calculate.saveFilterParams();
			OUTRIGHT.initEvent();
            Site.resize(200);
        },

        initEvent: function () {
            // Event click to get list settlement
            $("#filter-container").on("click", "#SubmitButton", OUTRIGHT.submitForm);
			$("#IsSettled").change(function(){
				OUTRIGHT.submitForm();
			});

			// Event click to Calculate button open Popup
			$("#content-cotainer").on("click", ".atc-cal-button", OUTRIGHT.openPopupCalculate);

            // Event click to Un-Calculate button open Popup
			$("#content-cotainer").on("click", ".atc-uncal-button", OUTRIGHT.openPopupUnCalculate);

			// Event click to Auto Settlement
			$("#content-cotainer").on("click", ".act-auto-settle", OUTRIGHT.autoSettlement);

			//Event Click on Auto Settlement button
            $('#filter-container').on('click', '.btn-auto-settle-all', OUTRIGHT.switchAutoSettleAll);

			 // Event click to View log action
			 $('#content-cotainer').on('click', '.act-view-log', OUTRIGHT.directlyPopupLog);
        },

        // Selected a team
        selectTeam: function()
        {
            if ($(this).is(":checked")) {
                    countSelectTeam++;
            }
            else {
                countSelectTeam--;
            }

            // Disable or Show UnCalculate button.
            if (countSelectTeam > 0) {
                $("#uncal").removeClass('calc-all-disabled');
                $("#uncal").addClass('act-uncalc calc-all-button');
            }
            else {
                $("#uncal").removeClass('act-uncalc calc-all-button');
                $("#uncal").addClass('calc-all-disabled');
            }
        },

        // Click unCalculate
		unCalculate: function()
		{
            Site.blockUI();
			var listSelectedTeam = "";
			var countDeadHeat = $("#countDeadHeat").val();
            var outrightIds = $("#orIds").val();
            var teamIds = $("#teamIds").val();

            $(".select-team").each(function () {
               listSelectedTeam += $(this).is(":checked")? "1,"+this.value+"|" : "0,0|";
            });

			var param = {
                outrightIds : outrightIds,
                listSelectedTeam : listSelectedTeam,
			    countDeadHeat : countDeadHeat,
                teamIds: teamIds
            };

			var url = Site.getAbsoluteUrl("Settlement/Outright/UnCalculate");

			$.ajax({
                type: "POST",
                url: url,
                data: param,
                success: function (result) {
						if (!result.IsSuccessful) {
							alert(result.ErrorMessage);
						}
						else {
							parent.DialogBox.closePopup();
							parent.Calculate.submitForm();
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

		// Click calculate
		calculate: function()
		{
			Site.blockUI();
			var orIds ="";
			var oldOrIds = $("#orIds").val();
			var orIdsFail = $("#orIdsFail").val();
			var teamIds = $("#teamIds").val();
            $("select").each(function () {
                orIds += this.id.replace("status-","") + "," + this.value + "|";
            });

			if(orIds == oldOrIds && orIdsFail== "")
			{
				parent.DialogBox.closePopup();
				parent.Calculate.submitForm();
				return;
			}

			var param = {
                outrightIds: orIds,
				oldOrIds: oldOrIds,
				outrightIdsFail: orIdsFail,
				teamIds: teamIds
            };

			var url = Site.getAbsoluteUrl("Settlement/Outright/Calculate");

			$.ajax({
                type: "POST",
                url: url,
                data: param,
                success: function (result) {
						if (!result.IsSuccessful) {
							alert(result.ErrorMessage);
						}
						else {
							parent.DialogBox.closePopup();
							parent.Calculate.submitForm();
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

		// Auto Settlement
		autoSettlement: function()
		{
			var leagueId = $(this).attr("data-league-id");
			OUTRIGHT.switchAutoSettleSingle(leagueId);
		},
		processSwitchAutoSettle: function (isTurnOn, leagueId) {
            var autoSettleButton = $('.btn-auto-settle-all');
            var autoSettleLeague = $('#auto-id-' + leagueId);
			var calId = $('#calc-' + leagueId);
            var unCalId = $('#uncalc-' + leagueId);
            var url, param;
			if(leagueId ==0)
			{
				url = Site.getAbsoluteUrl("Settlement/AutoSettlement/UpdateStatusAll");
				param = {
					isTurnOn: isTurnOn, //0-off; 1-on
					matchId: leagueId
				};
			}
			else
			{
				url = Site.getAbsoluteUrl("Settlement/Outright/UpdateFlagOutright");
				param = {
					isTurnOn: isTurnOn, //0-off; 1-on
					leagueId: leagueId
				};
			}

            Site.blockUI();

            $.ajax({
                type: "POST",
                url: url,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify(param),
                success: function (data) {
                    if (data.ErrorCode == 0) {
                        if (isTurnOn == 0) { // OFF
                            if (leagueId == 0) { // All
                                autoSettleButton.removeClass('on');
                            }
                            else { //Single match
                                autoSettleLeague.removeClass('on');
								calId.removeClass('cal-button');
                                unCalId.removeClass('cal-button');
								calId.addClass('atc-cal-button');
                                unCalId.addClass('atc-uncal-button');
                            }
                        }
                        else {//ON
                            if (leagueId == 0) { //All
                                autoSettleButton.addClass('on');
                            }
                            else { //Single match
                                autoSettleLeague.addClass('on');
								calId.removeClass('atc-cal-button');
                                unCalId.removeClass('atc-uncal-button');
								calId.addClass('cal-button');
                                unCalId.addClass('cal-button');
                            }
                        }

                        if (leagueId == 0) {
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
                    OUTRIGHT.processSwitchAutoSettle(0, 0);
                }, {width:width, title: title});
            }
            else {
                confirm(Languages.AutoSettlementAll_OFF_to_ON,   function () {
                    OUTRIGHT.processSwitchAutoSettle(1, 0);
                }, {width:width, title: title});
            }
        },

		switchAutoSettleSingle: function (leagueId) {
			var autoSettleButton = $('.btn-auto-settle-all');
            var isAutoSettleOn = autoSettleButton.hasClass('on');

			// Can switch ON/OFF for a match when and only when Global Switch is ON
            if(!isAutoSettleOn){
                return;
            }

            var canSetAutoSettlement = $('#CanSetAutoSettlement').val() == 'True';
            if (!canSetAutoSettlement) {
                alert('You are not authorized to use this function. Please contact the admin for help.');
                return;
            }

            var autoSettleSingleLeague = $('#auto-id-' + leagueId);
            var isAutoSettleOn = autoSettleSingleLeague.hasClass('on');
            var title = 'Auto Settlement';
            var width = 430;
            if (isAutoSettleOn) {
                confirm(Languages.AutoSettlementOutright_ON_to_OFF, function () {
                    OUTRIGHT.processSwitchAutoSettle(0, leagueId);
                }, {width:width, title: title});
            }
            else {
                confirm(Languages.AutoSettlementOutright_OFF_to_ON, function () {
                    OUTRIGHT.processSwitchAutoSettle(1, leagueId);
                }, {width:width, title: title});
            }
        },

		// Open popup Calculate
		openPopupCalculate: function()
		{
			var title = 'Outright Calculation';
            var tr = $(this).parents('tr');
			var leagueId = tr.attr("data-league-item");
			var url = Site.getAbsoluteUrl("Settlement/Outright/Matches?leagueId=" + leagueId);
         	 DialogBox.openDialogFrame(url, title,{ width: 780, height: 450, modal: true }, true);
		},

        // Open popup UnCalculate
		openPopupUnCalculate: function()
		{
			var title = 'Outright Uncalculation';
            var tr = $(this).parents('tr');
			var leagueId = tr.attr("data-league-item");
			var url = Site.getAbsoluteUrl("Settlement/Outright/SettledMatches?leagueId=" + leagueId);
         	DialogBox.openDialogFrame(url, title,{ width: 780, height: 450, modal: true }, true);
		},

		// Open popup View Log
		directlyPopupLog: function()
		{
            var tr = $(this).parents('tr');
			var leagueId = tr.attr("data-league-item")
			var logId = $(this).attr("data-orid");
			var teamId = $(this).attr("data-team-id");
			var url = Site.getAbsoluteUrl("Settlement/Outright/LogAction?ordId=" + logId+"&teamId="+teamId+"&leagueId="+leagueId);

			var title = "Reference Action Log";
			//DialogBox.openPopup(url, title, false, 450, false);
			 DialogBox.openDialogFrame(url, title,{ width: 780, height: 450, modal: false }, true);
		},

		// Open popup View Log
		openPopupLog: function()
		{
			var leagueId = $("#leagueId").val();
			var logId = $(this).attr("data-orid");
			var teamId = $(this).attr("data-team-id");
			var url = Site.getAbsoluteUrl("Settlement/Outright/LogAction?ordId=" + logId+"&teamId="+teamId+"&leagueId="+leagueId);

			parent.$(".ui-dialog-title").text("Reference Action Log");
		    location.href = url;
		},

        // Event Submit
        submitForm: function () {
			// Check all value filter select list
            if (!Calculate.checkAllFilterSelect()) {
                return false;
            }

		    Site.blockUI();
		    var formId = $('#outright-form');
            Calculate.saveFilterParams();
            var params = JSON.parse($('#HiddenCriteria').val());
			$.ajax({
                type: "POST",
                url: formId.attr("action"),
                data: params,
                success: function (data) {
                    $("#content-cotainer").html(data);
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
    };
} (jQuery));