var DoMultipleTicket = (function ($) {
	return {
		// Action refund parlay button
		refundParlay: function (type) {
			var message = "";
			switch (type) {
				case Type.RefundParlayAll:
					message = Languages.RefundParlayAll;
					break;

				case Type.RefundParlayFullTime:
					message = Languages.RefundParlayFullTime;

					break;

				case Type.RefundParlayHalfTime:
					message = Languages.RefundParlayHalfTime;
					break;
			}

			confirm(message, function () { return DoMultipleTicket.doRefundParlaySubmit(type); });
		},

		// Do Refund Parlay
		doRefundParlaySubmit: function (type) {
			$('#ActionType').val(type);
			Site.blockUI();
			$.ajax({
				type: 'POST',
				async: true,
				url: Site.getAbsoluteUrl("DoMultipleTicket/RefundParlay"),
				data: $("#frm-calculate").serialize(),
				success: function (result) {
					DoMultipleTicket.refundResult(result);
				},
				error: function () {
					alert(Languages.SystemError);
				},
				complete: function () {
					Site.unBlockUI();
				}
			});
		},

		// Refund Result
		refundResult: function (result) {
			if (!result.IsSuccessful) {
				Site.showErrorMessage(result.ErrorMessage);
			}
			else {
				parent.DialogBox.closePopup();
				parent.Calculate.submitForm();
			}
		},

		// Do Calculate Mix Parlay
		doCalculateMixParlaySubmit: function (matchIds) {
			var params =
            {
            	"matchIds": matchIds
            };
			Site.blockUI();
			$.ajax({
				type: 'POST',
				async: true,
				url: Site.getAbsoluteUrl("DoMultipleTicket/CalculateMixParlay"),
				data: params,
				success: function (result) {
					if (!result.IsSuccessful) {
						alert(result.ErrorMessage);
					}
					else {
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
		}
	};
} (jQuery));