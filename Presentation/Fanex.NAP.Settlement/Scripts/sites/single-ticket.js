var DoSingleTicket = (function ($) {
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
					return Site.isValidScore(['TextBoxHome1Q', 'TextBoxHome2Q', 'TextBoxHome3Q', 'TextBoxHome4Q',
                        'TextBoxAway1Q', 'TextBoxAway2Q', 'TextBoxAway3Q', 'TextBoxAway4Q']);

				case 4:     // ice hockey
				case 28:    // field hockey
					return Site.isValidScore(['TextBoxHome1Q', 'TextBoxHome2Q', 'TextBoxHome3Q', 'TextBoxAway1Q',
                        'TextBoxAway2Q', 'TextBoxAway3Q']);

				case 5:     // tennis
					return Site.isValidScore(['TextBoxHome1Q', 'TextBoxHome2Q', 'TextBoxHome3Q', 'TextBoxHome4Q',
                        'TextBoxHome5Q', 'TextBoxAway1Q', 'TextBoxAway2Q', 'TextBoxAway3Q', 'TextBoxAway4Q',
                        'TextBoxAway5Q']);

				case 8:     // baseball
					return Site.isValidScore(['TextBoxHome1Q', 'TextBoxHome2Q', 'TextBoxHome3Q', 'TextBoxHome4Q',
                        'TextBoxHome5Q', 'TextBoxHome6Q', 'TextBoxHome7Q', 'TextBoxHome8Q', 'TextBoxHome9Q',
                        'TextBoxAway1Q', 'TextBoxAway2Q', 'TextBoxAway3Q', 'TextBoxAway4Q', 'TextBoxAway5Q',
                        'TextBoxAway6Q', 'TextBoxAway7Q', 'TextBoxAway8Q', 'TextBoxAway9Q']);

				case 6:     // volleyball
				case 9:     // badminton
					return Site.isValidScore(['TextBoxHome1Q', 'TextBoxHome2Q', 'TextBoxHome3Q', 'TextBoxHome4Q',
                        'TextBoxHome5Q', 'TextBoxAway1Q', 'TextBoxAway2Q', 'TextBoxAway3Q', 'TextBoxAway4Q',
                        'TextBoxAway5Q']);

				default:
					return true;
			}
		},

		// is exist first goal and last goal
		checkValueGoalRecord: function (type, sportId) {
			// only check for soccer and FT or ALL
			if (sportId == 1 && (type == Type.CalculateAll || type == Type.CalculateFullTime
				|| type == Type.CalculateFgNg)) {
				var goalRecord = $('#GoalRecord').val().toLowerCase();
				var homeF = ($('#TextBoxFinalHomeScore').val() == 0) ? 0 : parseInt($('#TextBoxFinalHomeScore').val());
				var awayF = ($('#TextBoxFinalAwayScore').val() == 0) ? 0 : parseInt($('#TextBoxFinalAwayScore').val());
				var homeH = ($('#TextBoxHtHomeScore').val() == 0) ? 0 : parseInt($('#TextBoxHtHomeScore').val());
				var awayH = ($('#TextBoxHtAwayScore').val() == 0) ? 0 : parseInt($('#TextBoxHtAwayScore').val());

				if (type == Type.CalculateFgNg) {
					if (homeH > homeF || awayH > awayF) {
						homeF = homeH;
						awayF = awayH;
					}
				}
				var isLive = $('#IsLive').val() == 'True';
				var isHedge = $('#MatchCode').val().toLowerCase().indexOf('hedge') >= 0;
				var isExistFgLg = $('#IsExistFgLg').val() == 'True';

				if (!isHedge && isExistFgLg) {
					if (isLive) {
						if ((homeF + awayF) != goalRecord.length || Site.replaceAll(goalRecord, 'a', '').length != homeF) {
							Site.showErrorMessage(Languages.GoalRecordNotConsistent);
							return false;
						}
					} else {
						if (homeF < 0 || awayF < 0) {
							Site.showErrorMessage(Languages.GoalRecordNotConsistent);
							return false;
						}

						if (homeF + awayF != goalRecord.length) {
							Site.showErrorMessage(Languages.GoalRecordNotConsistent);
							return false;
						}

						if (goalRecord.length > 0) {
							var goalRecordTrimH = Site.replaceAll(goalRecord, 'h', '');
							var goalRecordTrimA = Site.replaceAll(goalRecord, 'a', '');

							if (goalRecordTrimA.length != homeF || goalRecordTrimH.length != awayF) {
								Site.showErrorMessage(Languages.GoalRecordNotConsistent);
								return false;
							}
						}
					}
				}
			}
			return true;
		},

		// Do calculation
		doCalculate: function (type, ext) {
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
					var message = "";
					if ($('#IsAlreadyRefundHT').val().toLowerCase() == 'true') {
						message = Languages.AlreadyRefundHT;
					}
					message += Languages.DoubleCheckCalculateHalfTime;
					messageFirst = Languages.Warning + message;
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

			if (type == Type.UnCalculate) {
				url = "DoSingleTicket/UnCalculate";
			} else {
				url = "DoSingleTicket/Calculate";
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
			url = "DoSingleTicket/Refund";

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