/*global DoSingleTicket, DoMultipleTicket, Languages, Site*/
var Refund = (function ($) {
    "use strict";
    return {
        init: function () {
            Site.unBlockUI();

              // Event click to Refund half time button
            $('.act-refund-ht').click(function () {
                return Refund.refundNormal(Type.RefundHalfTime);
            });

            // Event click to Refund full time button
            $('.act-refund-ft').click(function () {
               return Refund.refundNormal(Type.RefundFullTime);
            });

            // Event click to Refund all button
            $('.act-refund-all').click(function () {
               return Refund.refundNormal(Type.RefundAll);
            });

             // Event click to Refund all FGLG button
            $('.act-refund-all-fglg').click(function () {
               return Refund.refundNormal(Type.RefundAllFgLg);
            });

             // Event click to Refund all LG button
            $('.act-refund-all-lg').click(function () {
              return Refund.refundNormal(Type.RefundAllLg);
            });

            // Offer 1H Total Goal and 1H First Goal/Last Goal Bet Types
            // Event click to Refund half time first goal last goal
            $('.act-refund-ht-fglg').click(function () {
                return Refund.refundNormal(Type.RefundHTFGLG);
            });

             // Event click to Refund half time last goal
            $('.act-refund-ht-lg').click(function () {
                return Refund.refundNormal(Type.RefundHTLastGoal);
            });

            // Event click to Refund full time first goal last goal
            $('.act-refund-ft-fglg').click(function () {
                return Refund.refundNormal(Type.RefundFTFGLG);
            });

            // Event click to Refund full time last goal
            $('.act-refund-ft-lg').click(function () {
                return Refund.refundNormal(Type.RefundFTLastGoal);
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
                case Type.RefundAllFgLg:
                    message = Languages.RefundAllFGLG;
                    break;
                case Type.RefundAllLg:
                    message = Languages.RefundAllLastGoal;
                    break;

                case Type.RefundHTFGLG:
                    message = Languages.RefundHTFGLG;
                    break;
                case Type.RefundHTLastGoal:
                    message = Languages.RefundHTLG;
                    break;
                case Type.RefundFTFGLG:
                    message = Languages.RefundFTFGLG;
                    break;
                case Type.RefundFTLastGoal:
                    message = Languages.RefundFTLG;
                    break;
            }
            confirm(message, function () { return DoSingleTicket.doRefundSubmit(type); });
        },
    };
} (jQuery));