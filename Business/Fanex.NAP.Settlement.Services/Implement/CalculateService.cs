// -----------------------------------------------------------------------
// <copyright file="CalculateService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
// <summary>
//  Calculate Service Interface
// </summary>
//
// <history>
//  <change who="Kaden" date="2012.08.06">Created</change>
//  <change who="Kaden" date="2012.09.10">Implement function for refund</change>
// </history>

namespace Fanex.NAP.Settlement.Services.Implement
{
    #region

    using System.Collections.Generic;
    using System.Linq;
    using Fanex.NAP.Settlement.DataAccess.Implement;
    using Fanex.NAP.Settlement.DataAccess.Interface;
    using Fanex.NAP.Settlement.DTO;
    using Fanex.NAP.Settlement.DTO.DBResult;
    using Fanex.NAP.Settlement.MetaModels;
    using Fanex.NAP.Settlement.Services.Interface;

    #endregion

    /// <summary>
    ///     TODO: Update summary.
    /// </summary>
    public class CalculateService : ICalculateService
    {
        #region Members

        /// <summary>
        /// Settlement Data Access.
        /// </summary>
        private readonly ICalculateDataAccess _calculateDataAccess;

        #endregion Members

        #region Contructors

        /// <summary>
        ///  Initializes a new instance of the <see cref="CalculateService" /> class.
        /// </summary>
        public CalculateService()
            : this(null)
        {
        }

        /// <summary>
        ///  Initializes a new instance of the <see cref="CalculateService" /> class.
        /// </summary>
        /// <param name="calculateDataAccess">Calculate DataAccess.</param>
        private CalculateService(ICalculateDataAccess calculateDataAccess)
        {
            this._calculateDataAccess = calculateDataAccess ?? new CalculateDataAccess();
        }

        #endregion Contructors

        /// <summary>
        /// Get list match for SportBook.
        /// </summary>
        /// <param name="request">Request Info.</param>
        /// <param name="matchLog">Output match log action.</param>
        /// <returns>Return match detail.</returns>
        public IList<Match> SearchMatch(CalculateMetaModel request, out IList<MatchLog> matchLog)
        {
            return this._calculateDataAccess.SearchMatch(request, out matchLog);
        }

        /// <summary>
        /// Get list match OverOnly for SportBook.
        /// </summary>
        /// <param name="request">Request Info.</param>
        /// <param name="matchLog">Output match log action.</param>
        /// <returns>Return match list.</returns>
        public IList<Match> SearchMatchOverOnly(CalculateMetaModel request, out IList<MatchLog> matchLog)
        {
            return this._calculateDataAccess.SearchMatchOverOnly(request, out matchLog);
        }

        /// <summary>
        /// Get list match Half time OverOnly for SportBook.
        /// </summary>
        /// <param name="request">Request info for search.</param>
        /// <param name="matchLog">Output match log action.</param>
        /// <returns>Return list match.</returns>
        public IList<Match> SearchMatchHtOverOnly(CalculateMetaModel request, out IList<MatchLog> matchLog)
        {
            return this._calculateDataAccess.SearchMatchHtOverOnly(request, out matchLog);
        }

        /// <summary>
        /// Get list match for NumberGame.
        /// </summary>
        /// <param name="request">Request parameter.</param>
        /// <param name="matchLog">Out match log.</param>
        /// <returns>Return match list. </returns>
        public IList<Match> SearchMatchNumberGame(CalculateMetaModel request, out IList<MatchLog> matchLog)
        {
            var matchList = this._calculateDataAccess.SearchMatchNumberGame(request, out matchLog);

            return matchList.GroupBy(m => new
                {
                    m.BetStatus,
                    m.LeagueId,
                    m.MatchId,
                    m.MatchCode,
                    m.EventDate,
                    m.KickOffTime,
                    m.HomeId,
                    m.EventStatus,
                    m.IsRefund
                })
                            .Select(g => new Match
                                {
                                    MatchId = g.Key.MatchId,
                                    MatchCode = g.Key.MatchCode,
                                    LeagueId = g.Key.LeagueId,
                                    EventDate = g.Key.EventDate,
                                    KickOffTime = g.Key.KickOffTime,
                                    BetStatus = g.Key.BetStatus,
                                    HomeId = g.Key.HomeId,
                                    EventStatus = g.Key.EventStatus,
                                    IsRefund = g.Key.IsRefund
                                }).ToList();
        }

        /// <summary>
        /// NumberGame Match Detail.
        /// </summary>
        /// <param name="matchId">Match Id parameter.</param>
        /// <returns>Return match detail.</returns>
        public Match MatchDetailNumberGame(int matchId)
        {
            var matchList = this._calculateDataAccess.SearchMatchNumberGame(matchId);

            return matchList.GroupBy(m => new
            {
                m.MatchId,
                m.MatchCode,
                m.KickOffTime,
            })
                            .Select(g => new Match
                            {
                                MatchId = g.Key.MatchId,
                                MatchCode = g.Key.MatchCode,
                                KickOffTime = g.Key.KickOffTime
                            }).FirstOrDefault();
        }

        /// <summary>
        /// Update Ball number game.
        /// </summary>
        /// <param name="request">Ball number data.</param>
        public void UpdateBallNumber(NumberGameMetaModel request)
        {
            this._calculateDataAccess.UpdateBallNumber(request);
        }

        /// <summary>
        /// Get detail match info.
        /// </summary>
        /// <param name="matchId">Match Id parameter.</param>
        /// <param name="excludeRefundFGLG">Exclude Refund FG/LG.</param>
        /// <returns>Return match detail.</returns>
        public MatchDetail GetMatchDetail(int matchId, bool excludeRefundFGLG)
        {
            return this._calculateDataAccess.GetMatchDetail(matchId, excludeRefundFGLG);
        }

        #region Match Log Action

        /// <summary>
        /// Get list action.
        /// </summary>
        /// <param name="matchId">Match Id parameter.</param>
        /// <returns>Return match log action.</returns>
        public IList<MatchLog> GetListConfirmedAction(int matchId)
        {
            return this._calculateDataAccess.GetListConfirmedAction(matchId);
        }

        /// <summary>
        /// Get Detail log action.
        /// </summary>
        /// <param name="logId">Log Id parameter.</param>
        /// <returns>Return MatchLog detail.</returns>
        public MatchLog GetMatchLogDetail(int logId)
        {
            return this._calculateDataAccess.GetMatchLogDetail(logId);
        }

        #endregion

        #region Excute Calculate or Refund match

        public IResult Calculate(DoSingleTicketMetaModel criteria, string action, bool isHalfTimeCalculate)
        {
            return this._calculateDataAccess.Calculate(criteria, action, isHalfTimeCalculate);
        }

        public IResult CalculateMultiple(DoMultipleTicketMetaModel criteria)
        {
            return this._calculateDataAccess.CalculateMultiple(criteria);
        }

        public IResult Refund(DoSingleTicketMetaModel criteria)
        {
            return this._calculateDataAccess.Refund(criteria);
        }

        #endregion Excute Calculate or Refund match

        #region Close Match

        /// <summary>
        /// Close match is running.
        /// </summary>
        /// <param name="value">Parameter for update.</param>
        /// <returns>Return result value.</returns>
        public IResult CloseMatch(CloseMatchMetaModel value)
        {
            return this._calculateDataAccess.CloseMatch(value);
        }

        #endregion Close Match
    }
}