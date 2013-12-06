var Refund = (function ($) {
	return {
		init: function () {
			Site.unBlockUI();

			// Event click to Refund half time button
			$('.act-refund-ht').click(function () {
				Refund.scoreConfirm(Type.RefundHalfTime);
			});

			// Event click to Refund full time button
			$('.act-refund-ft').click(function () {
				Refund.scoreConfirm(Type.RefundFullTime);
			});

			// Event click to Refund all button
			$('.act-refund-all').click(function () {
				Refund.scoreConfirm(Type.RefundAll);
			});

			// Event click to Refund all FGLG button
			$('.act-refund-all-fglg').click(function () {
				confirm(Languages.RefundAllFGLG, function () {
					return DoSingleTicket.doRefundSubmit(Type.RefundAllFgLg);
				});
			});

			// Event click to Refund all LG button
			$('.act-refund-all-lg').click(function () {
				confirm(Languages.RefundAllLastGoal, function () {
					return DoSingleTicket.doRefundSubmit(Type.RefundAllLg);
				});
			});

			// Event click to Refund Parlay half time button
			$('.act-refund-parlay-ht').click(function () {
				DoMultipleTicket.refundParlay(Type.RefundParlayHalfTime);
			});

			// Event click to Refund Parlay full time button
			$('.act-refund-parlay-ft').click(function () {
				DoMultipleTicket.refundParlay(Type.RefundParlayFullTime);
			});

			// Event click to Refund Parlay all
			$('.act-refund-parlay-all').click(function () {
				DoMultipleTicket.refundParlay(Type.RefundParlayAll);
			});
		},

		// Action refund normal
		refundNormal: function (type) {
			var message = "";
			switch (type) {
				case Type.RefundAll:
					message = Languages.RefundNormalAll;
					break;

				case Type.RefundFullTime:
					message = Languages.RefundNormalFullTime;
					break;

				case Type.RefundHalfTime:
					message = Languages.RefundNormalHalfTime;
					break;
			}
			confirm(message, function () { return DoSingleTicket.doRefundSubmit(type); }); 
		},
		// Refund action
		scoreConfirm: function (type) {
			Refund.refundNormal(type);
		}
	};
} (jQuery));