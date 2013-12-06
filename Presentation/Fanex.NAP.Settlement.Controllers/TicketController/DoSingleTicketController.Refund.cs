// -----------------------------------------------------------------------
// <copyright file="DoSingleTicketController.Refund.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
// <summary>
//  DoSingleTicket Controller
// </summary>
//
// <history>
//  <change who="Kaden" date="2013.07.16">Created and Implement</change>
// </history>
namespace Fanex.NAP.Settlement.Controllers.TicketController
{
    using System.Collections;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.NAP.Settlement.DTO.DBResult;
    using Fanex.NAP.Settlement.DTO.Domain;
    using Fanex.NAP.Settlement.MetaModels;

    /// <summary>
    /// Controller Single ticket Refund for SportBook.
    /// </summary>
    public partial class DoSingleTicketController
    {
        #region Refund Ticket

        /// <summary>
        /// Refund command with model value.
        /// </summary>
        /// <param name="model">Model Single Ticket.</param>
        /// <returns>Return IResult.</returns>
        private IResult RefundCommand(DoSingleTicketMetaModel model)
        {
            string scoreConfirmBetStatus;
            switch (model.SportId)
            {
                case (int)SportType.BasketBall:
                case (int)SportType.IceHockey:
                case (int)SportType.Tennis:
                case (int)SportType.VolleyBall:
                case (int)SportType.BaseBall:
                    scoreConfirmBetStatus = model.HiddenBetStatus;
                    break;
                default:
                    scoreConfirmBetStatus = null;
                    break;
            }

            var action = model.ActionType.ToLowerInvariant();
            var matchId = model.MatchId;
            var matchCode = model.MatchCode;
            var homeScore = model.HiddenHtHomeScore;
            var awayScore = model.HiddenHtAwayScore;
            var finalHomeScore = model.HiddenFinalHomeScore;
            var finaleAwayScore = model.HiddenFinalAwayScore;
            return this.RefundCommand(action, matchId, matchCode, homeScore, awayScore, finalHomeScore, finaleAwayScore, scoreConfirmBetStatus);
        }

        /// <summary>
        /// Refund command with detail value.
        /// </summary>
        /// <param name="refundType">Refund type.</param>
        /// <param name="matchId">Match id parameter.</param>
        /// <param name="matchCode">Match code.</param>
        /// <param name="homeHScore">Home Halftime score.</param>
        /// <param name="awayHScore">Away Halftime score.</param>
        /// <param name="homeFScore">Home Fulltime score.</param>
        /// <param name="awayFScore">Away Fulltime score.</param>
        /// <param name="scoreConfirmBetstatus">Score Confirm Bet status.</param>
        /// <returns>Return IResult.</returns>
        private IResult RefundCommand(
            string refundType = "",
            int matchId = 0,
            string matchCode = "",
            int homeHScore = 0,
            int awayHScore = 0,
            int homeFScore = 0,
            int awayFScore = 0,
            string scoreConfirmBetstatus = "")
        {
            var inputParameters = new Hashtable();

            inputParameters.Add("matchid", matchId);
            inputParameters.Add("adminid", CurrentSession.UserId);
            inputParameters.Add("creator", CurrentSession.UserId);
            inputParameters.Add("ip", HttpContext.Request.UserHostAddress);

            if (!string.IsNullOrEmpty(scoreConfirmBetstatus))
            {
                inputParameters.Add("betstatus", scoreConfirmBetstatus);
            }

            switch (refundType)
            {
                case CalculationType.RefundAll:
                    inputParameters.Add("hth", homeHScore);
                    inputParameters.Add("hta", awayHScore);
                    inputParameters.Add("fth", homeFScore);
                    inputParameters.Add("fta", awayFScore);
                    inputParameters.Add("action", CalculateAction.Refund);
                    break;

                case CalculationType.RefundHalfTime:
                    inputParameters.Add("hth", homeHScore);
                    inputParameters.Add("hta", awayHScore);
                    inputParameters.Add("action", CalculateAction.RefundHalfTime);
                    break;

                case CalculationType.RefundFullTime:
                    inputParameters.Add("fth", homeFScore);
                    inputParameters.Add("fta", awayFScore);
                    inputParameters.Add("action", CalculateAction.RefundFullTime);
                    break;

                case CalculationType.RefundAllFgng:
                    inputParameters.Add("lgonly", false);
                    inputParameters.Add("action", CalculateAction.RefundAllFGNG);
                    break;

                case CalculationType.RefundAllLg:
                    inputParameters.Add("lgonly", true);
                    inputParameters.Add("action", CalculateAction.RefundAllLastGoal);
                    break;
            }

            return CalculateService.Calculate(inputParameters, refundType);
        }

        #endregion Refund Ticket
    }
}