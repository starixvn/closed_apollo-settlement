// -----------------------------------------------------------------------
// <copyright file="ICalculateDataAccess.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
// <summary>
//  Calculate Data Access Interface
// </summary>
//
// <history>
//  <change who="Kaden" date="2012.08.06">Created</change>
//  <change who="Kaden" date="2012.09.10">Implement Interface for refund</change>
// </history>

namespace Fanex.NAP.Settlement.DataAccess.Interface
{
    using System.Collections.Generic;
    using Fanex.NAP.Settlement.DTO;
    using Fanex.NAP.Settlement.DTO.DBResult;
    using Fanex.NAP.Settlement.MetaModels;

    /// <summary>
    ///     TODO: Update summary.
    /// </summary>
    public interface ICalculateDataAccess
    {
        #region All interface for calculate page

        /// <summary>
        /// Get list match for SportBook.
        /// </summary>
        /// <param name="request">Request Info.</param>
        /// <param name="matchLog">Output match log action.</param>
        /// <returns>Return match detail.</returns>
        IList<Match> SearchMatch(CalculateMetaModel request, out IList<MatchLog> matchLog);

        /// <summary>
        /// Get list match OverOnly for SportBook.
        /// </summary>
        /// <param name="request">Request Info.</param>
        /// <param name="matchLog">Output match log action.</param>
        /// <returns>Return match list.</returns>
        IList<Match> SearchMatchOverOnly(CalculateMetaModel request, out IList<MatchLog> matchLog);

        /// <summary>
        /// Get list match Half time OverOnly for SportBook.
        /// </summary>
        /// <param name="request">Request info for search.</param>
        /// <param name="matchLog">Output match log action.</param>
        /// <returns>Return list match.</returns>
        IList<Match> SearchMatchHtOverOnly(CalculateMetaModel request, out IList<MatchLog> matchLog);

        /// <summary>
        /// Get list action.
        /// </summary>
        /// <param name="matchId">Match Id parameter.</param>
        /// <returns>Return match list action.</returns>
        IList<MatchLog> GetListConfirmedAction(int matchId);

        /// <summary>
        /// Get Detail log action.
        /// </summary>
        /// <param name="logId">Log Id parameter.</param>
        /// <returns>Return MatchLog detail.</returns>
        MatchLog GetMatchLogDetail(int logId);

        #endregion All interface for calculate page

        #region All interface for refund and calculate popup page

        /// <summary>
        /// Get detail match info.
        /// </summary>
        /// <param name="matchId">Match Id parameter.</param>
        /// <param name="excludeRefundFGLG">Exclude Refund FG/LG.</param>
        /// <returns>Return match detail.</returns>
        MatchDetail GetMatchDetail(int matchId, bool excludeRefundFGLG);

        #endregion All interface for refund and calculate popup page

        #region All interface for NumberGame

        /// <summary>
        /// Get list match for NumberGame.
        /// </summary>
        /// <param name="request">Request parameter.</param>
        /// <param name="matchLog">Out match log.</param>
        /// <returns>Return match list. </returns>
        IList<Match> SearchMatchNumberGame(CalculateMetaModel request, out IList<MatchLog> matchLog);

        /// <summary>
        /// List number.
        /// </summary>
        /// <param name="matchId">Match Id parameter.</param>
        /// <returns>Return match list.</returns>
        IList<Match> SearchMatchNumberGame(int matchId);

        /// <summary>
        /// Update Ball number game.
        /// </summary>
        /// <param name="request">Ball number.</param>
        void UpdateBallNumber(NumberGameMetaModel request);

        #endregion All interface for NumberGame

        #region Excute Calculate or Refund match

        IResult Calculate(DoSingleTicketMetaModel criteria, string action, bool isHalfTimeCalculate);

        IResult CalculateMultiple(DoMultipleTicketMetaModel criteria);

        IResult Refund(DoSingleTicketMetaModel criteria);

        #endregion Excute Calculate or Refund match

        #region Close Match

        IResult CloseMatch(CloseMatchMetaModel value);

        #endregion Close Match
    }
}