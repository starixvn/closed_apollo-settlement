// -----------------------------------------------------------------------
// <copyright file="CalculateDataAccess.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
// <summary>
//  Calculate Data Access
// </summary>
//
// <history>
//  <change who="Kaden" date="2012.08.06">Created</change>
// </history>

namespace Fanex.NAP.Settlement.DataAccess.Implement
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Linq;
    using Fanex.Data;
    using Fanex.Logging;
    using Fanex.NAP.Settlement.Core.Constants;
    using Fanex.NAP.Settlement.DataAccess.Exception;
    using Fanex.NAP.Settlement.DataAccess.Interface;
    using Fanex.NAP.Settlement.DTO;
    using Fanex.NAP.Settlement.DTO.DBResult;
    using Fanex.NAP.Settlement.MetaModels;

    /// <summary>
    ///     TODO: Update summary.
    /// </summary>
    public class CalculateDataAccess : ICalculateDataAccess
    {
        #region Utilities

        /// <summary>
        ///   The get result.
        /// </summary>
        /// <param name="db"> The database.</param>
        /// <returns> Return Result.</returns>
        public IResult GetResult(ObjectDb db)
        {
            var errorCode = db.GetParameterValue("ErrCode");
            var errorMessage = db.GetParameterValue("ErrMsg");

            // get result message
            return new Result
            {
                ErrorCode = (errorCode is DBNull) ? 0 : Convert.ToInt32(errorCode),
                ErrorMessage = (errorMessage is DBNull) ? string.Empty : Convert.ToString(errorMessage)
            };
        }

        #endregion Utilities

        /// <summary>
        /// Get list match for SportBook.
        /// </summary>
        /// <param name="request">Request Info.</param>
        /// <param name="matchLog">Output match log action.</param>
        /// <returns>Return match detail.</returns>
        public IList<Match> SearchMatch(CalculateMetaModel request, out IList<MatchLog> matchLog)
        {
            var uspName = "Acc_Set_MatchSel";
            var uspParams = new
                {
                    SportType = request.SportTypeId,
                    request.FromDate,
                    request.ToDate,
                    request.LeagueId,
                    MatchCode = string.IsNullOrEmpty(request.MatchCode) ? string.Empty : request.MatchCode.Trim(),
                    Statusid = request.StatusId,
                    request.FilterBy,
                    IsReload = request.IsReLoad
                };

            try
            {
                List<Match> matchList = new List<Match>();

                using (var db = new ObjectDb(uspName))
                {
                    using (var multiTable = db.QueryMultiple(uspParams))
                    {
                        matchList = multiTable.Read<Match>().ToList();
                        matchLog = multiTable.Read<MatchLog>().ToList();
                    }

                    return matchList;
                }
            }
            catch (SqlException sqlException)
            {
                LogService.Error(sqlException.Message, new DataAccessException(sqlException.Message, uspName, uspParams));
                throw new DataAccessException(sqlException.Message, uspName);
            }
        }

        /// <summary>
        /// Get list match OverOnly for SportBook.
        /// </summary>
        /// <param name="request">Request Info.</param>
        /// <param name="matchLog">Output match log action.</param>
        /// <returns>Return match list.</returns>
        public IList<Match> SearchMatchOverOnly(CalculateMetaModel request, out IList<MatchLog> matchLog)
        {
            var uspName = "Acc_Set_MatchSelCompleted";
            var uspParams = new
                {
                    SportType = request.SportTypeId,
                    request.FromDate,
                    ToDate = request.FromDate,
                    request.LeagueId,
                    request.MatchCode,
                    IsReload = request.IsReLoad
                };

            try
            {
                List<Match> matchList = new List<Match>();
                using (var db = new ObjectDb(uspName))
                {
                    using (var multiTable = db.QueryMultiple(uspParams))
                    {
                        matchList = multiTable.Read<Match>().ToList();
                        matchLog = multiTable.Read<MatchLog>().ToList();
                    }

                    return matchList;
                }
            }
            catch (SqlException sqlException)
            {
                LogService.Error(sqlException.Message, new DataAccessException(sqlException.Message, uspName, uspParams));
                throw new DataAccessException(sqlException.Message, uspName);
            }
        }

        /// <summary>
        /// Get list match Half time OverOnly for SportBook.
        /// </summary>
        /// <param name="request">Request info for search.</param>
        /// <param name="matchLog">Output match log action.</param>
        /// <returns>Return list match.</returns>
        public IList<Match> SearchMatchHtOverOnly(CalculateMetaModel request, out IList<MatchLog> matchLog)
        {
            var uspName = "Acc_Set_MatchSelCompletedHT";
            var uspParams = new
                {
                    SportType = request.SportTypeId,
                    request.FromDate,
                    ToDate = request.FromDate,
                    request.LeagueId,
                    request.MatchCode,
                    IsReload = request.IsReLoad
                };

            try
            {
                List<Match> matchList = new List<Match>();
                using (var db = new ObjectDb(uspName))
                {
                    using (var multiTable = db.QueryMultiple(uspParams))
                    {
                        matchList = multiTable.Read<Match>().ToList();
                        matchLog = multiTable.Read<MatchLog>().ToList();
                    }

                    return matchList;
                }
            }
            catch (SqlException sqlException)
            {
                LogService.Error(sqlException.Message, new DataAccessException(sqlException.Message, uspName, uspParams));
                throw new DataAccessException(sqlException.Message, uspName);
            }
        }

        /// <summary>
        /// Get list match for NumberGame.
        /// </summary>
        /// <param name="request">Request parameter.</param>
        /// <param name="matchLog">Out match log.</param>
        /// <returns>Return match list. </returns>
        public IList<Match> SearchMatchNumberGame(CalculateMetaModel request, out IList<MatchLog> matchLog)
        {
            var uspName = "Acc_Set_MatchSelNumberGame";
            var uspParams = new
                {
                    request.FromDate,
                    request.ToDate,
                    request.LeagueId,
                    request.MatchCode,
                    Statusid = request.StatusId,
                    IsReload = request.IsReLoad
                };

            try
            {
                var matchList = new List<Match>();
                using (var db = new ObjectDb(uspName))
                {
                    using (var multiTable = db.QueryMultiple(uspParams))
                    {
                        matchList = multiTable.Read<Match>().ToList();
                        matchLog = multiTable.Read<MatchLog>().ToList();
                    }

                    return matchList;
                }
            }
            catch (SqlException sqlException)
            {
                LogService.Error(sqlException.Message, new DataAccessException(sqlException.Message, uspName, uspParams));
                throw new DataAccessException(sqlException.Message, uspName);
            }
        }

        /// <summary>
        /// List number.
        /// </summary>
        /// <param name="matchId">Match Id parameter.</param>
        /// <returns>Return match list.</returns>
        public IList<Match> SearchMatchNumberGame(int matchId)
        {
            var uspName = "Acc_Set_MatchSelNumberGameDetail";
            var uspParams = new
            {
                MatchId = matchId
            };
            try
            {
                using (var db = new ObjectDb(uspName))
                {
                    return db.Query<Match>(uspParams).ToList();
                }
            }
            catch (SqlException sqlException)
            {
                LogService.Error(sqlException.Message, new DataAccessException(sqlException.Message, uspName, uspParams));
                throw new DataAccessException(sqlException.Message, uspName);
            }
        }

        /// <summary>
        /// Update Ball number game.
        /// </summary>
        /// <param name="request">Ball number.</param>
        public void UpdateBallNumber(NumberGameMetaModel request)
        {
            var uspName = "TP_Game_GameDetail_UpdBallNoByMatchID";
            var uspParams = new
            {
                request.MatchId,
                request.BallNo,
                request.SiteNo,
                request.AdminId,
                request.AdminName,
                request.FromIp
            };

            try
            {
                using (var db = new ObjectDb(uspName))
                {
                    db.ExecuteNonQuery(uspParams);
                }
            }
            catch (SqlException sqlException)
            {
                LogService.Error(sqlException.Message, new DataAccessException(sqlException.Message, uspName, uspParams));
                throw new DataAccessException(sqlException.Message, uspName);
            }
        }

        /// <summary>
        /// Get detail match info.
        /// </summary>
        /// <param name="matchId">Match Id parameter.</param>
        /// <param name="excludeRefundFGLG">Exclude Refund FG/LG.</param>
        /// <returns>Return match detail.</returns>
        public MatchDetail GetMatchDetail(int matchId, bool excludeRefundFGLG)
        {
            var uspName = "Acc_Set_MatchSelDetail";
            var uspParams = new { matchId, excludeRefundFGLG };

            try
            {
                using (var db = new ObjectDb(uspName))
                {
                    return db.Query<MatchDetail>(uspParams).FirstOrDefault();
                }
            }
            catch (SqlException sqlException)
            {
                LogService.Error(sqlException.Message, new DataAccessException(sqlException.Message, uspName, uspParams));
                throw new DataAccessException(sqlException.Message, uspName);
            }
        }

        #region Match Log Action

        /// <summary>
        /// Get list action.
        /// </summary>
        /// <param name="matchId">Match Id parameter.</param>
        /// <returns>Return match list action.</returns>
        public IList<MatchLog> GetListConfirmedAction(int matchId)
        {
            var uspName = "Acc_Set_GetConfirmedAction";
            var uspParams = new
            {
                matchId
            };

            try
            {
                using (var db = new ObjectDb(uspName))
                {
                    return db.Query<MatchLog>(uspParams).ToList();
                }
            }
            catch (SqlException sqlException)
            {
                LogService.Error(sqlException.Message, new DataAccessException(sqlException.Message, uspName, uspParams));
                throw new DataAccessException(sqlException.Message, uspName);
            }
        }

        /// <summary>
        /// Get Detail log action.
        /// </summary>
        /// <param name="logId">Log Id parameter.</param>
        /// <returns>Return MatchLog detail.</returns>
        public MatchLog GetMatchLogDetail(int logId)
        {
            var uspName = "Acc_Set_GetMatchLogDetail";
            var uspParams = new
            {
                logId
            };

            try
            {
                using (var db = new ObjectDb(uspName))
                {
                    return db.QueryEntity<MatchLog>(uspParams);
                }
            }
            catch (SqlException sqlException)
            {
                LogService.Error(sqlException.Message, new DataAccessException(sqlException.Message, uspName, uspParams));
                throw new DataAccessException(sqlException.Message, uspName);
            }
        }

        #endregion Match Log Action

        #region Close Match

        /// <summary>
        /// Close match is running.
        /// </summary>
        /// <param name="value">Parameter for update.</param>
        /// <returns>Return result value.</returns>
        public IResult CloseMatch(CloseMatchMetaModel value)
        {
            var uspName = "Admin_Settlement_CloseMatch";

            var p = new Hashtable();
            p.Add("adminid", value.AdminId);
            p.Add("matchid", value.MatchId);
            p.Add("ip", value.IP);
            p.Add("errcode", 0);
            p.Add("errmsg", string.Empty);

            try
            {
                using (var db = new ObjectDb(uspName))
                {
                    // Assign Params and ExecuteQuery
                    db.AssignParamValues(p);
                    db.ExecuteNonQuery();
                    return this.GetResult(db);
                }
            }
            catch (SqlException sqlException)
            {
                LogService.Error(sqlException.Message, new DataAccessException(sqlException.Message, uspName));
                throw new DataAccessException(sqlException.Message, uspName);
            }
        }

        #endregion Close Match

        #region Execute Calculate match

        /// <summary>
        /// Calculate Parlay.
        /// </summary>
        /// <param name="criteria">Criteria value.</param>
        /// <returns>Result value.</returns>
        public IResult CalculateMultiple(DoMultipleTicketMetaModel criteria)
        {
            var uspName = "CalculateParlay";
            var action = string.Empty;
            var parameters = new Hashtable();

            switch (criteria.ActionType.ToLower())
            {
                case CalculationType.RefundParlayHalfTime:
                    action = CalculateAction.RefundHalfTime;
                    parameters.Add("matchids", criteria.MatchId);
                    break;

                case CalculationType.RefundParlayFullTime:
                    action = CalculateAction.RefundFullTime;
                    parameters.Add("matchids", criteria.MatchId);
                    break;

                case CalculationType.RefundParlayAll:
                    action = CalculateAction.Refund;
                    parameters.Add("matchids", criteria.MatchId);
                    break;

                case CalculationType.CalculateParlay:
                    action = CalculateAction.CalculateAll;
                    parameters.Add("matchids", criteria.MatchIds);
                    break;
            }

            try
            {
                using (var db = new ObjectDb(uspName))
                {
                    parameters.Add("adminid", criteria.AdminId);
                    parameters.Add("action", action);
                    parameters.Add("ip", criteria.IP);
                    parameters.Add("errcode", 0);
                    parameters.Add("errmsg", string.Empty);

                    db.AssignParamValues(parameters);
                    db.ExecuteNonQuery();
                    return this.GetResult(db);
                }
            }
            catch (SqlException sqlException)
            {
                LogService.Error(sqlException.Message, new DataAccessException(sqlException.Message, uspName));
                throw new DataAccessException(sqlException.Message, uspName);
            }
        }

        /// <summary>
        /// Submit action calculate.
        /// </summary>
        /// <param name="criteria">The parameters.</param>
        /// <param name="action">Action type.</param>
        /// <param name="isHalfTimeCalculate">HalfTime Calculate.</param>
        /// <returns>Return result.</returns>
        public IResult Calculate(DoSingleTicketMetaModel criteria, string action, bool isHalfTimeCalculate)
        {
            var uspName = this.GetSPNameCalculate(criteria.ActionType);
            var parameters = CalculateParamters(criteria, action, isHalfTimeCalculate);
            switch (criteria.ActionType.ToLower())
            {
                case CalculationType.CalculateHalfTime:
                case CalculationType.CalculateFullTime:
                case CalculationType.CalculateAll:
                    using (var db = new ObjectDb("Settlement_Checking"))
                    {
                        // Assign Params and ExecuteQuery
                        var p = new Hashtable();
                        p.Add("matchid", criteria.MatchId);
                        p.Add("action", action);
                        p.Add("errcode", 0);
                        p.Add("errmsg", string.Empty);

                        db.AssignParamValues(p);
                        db.ExecuteNonQuery();
                        var result = this.GetResult(db);
                        if (result.ErrorCode > 0)
                        {
                            return result;
                        }
                    }

                    break;
            }

            try
            {
                using (var db = new ObjectDb(uspName))
                {
                    // Assign Params and ExecuteQuery
                    db.AssignParamValues(parameters);
                    db.ExecuteNonQuery();
                    return this.GetResult(db);
                }
            }
            catch (SqlException sqlException)
            {
                LogService.Error(sqlException.Message, new DataAccessException(sqlException.Message, uspName));
                throw new DataAccessException(sqlException.Message, uspName);
            }
        }

        /// <summary>
        /// Refund single.
        /// </summary>
        /// <param name="criteria">Criteria value.</param>
        /// <returns>Result value.</returns>
        public IResult Refund(DoSingleTicketMetaModel criteria)
        {
            var uspName = this.GetSPNameCalculate(criteria.ActionType);

            try
            {
                using (var db = new ObjectDb(uspName))
                {
                    var parameters = RefundParameters(criteria);
                    parameters.Add("matchid", criteria.MatchId);
                    parameters.Add("adminid", criteria.AdminId);
                    parameters.Add("ip", criteria.IP);
                    parameters.Add("errcode", 0);
                    parameters.Add("errmsg", string.Empty);

                    db.AssignParamValues(parameters);
                    db.ExecuteNonQuery();
                    return this.GetResult(db);
                }
            }
            catch (SqlException sqlException)
            {
                LogService.Error(sqlException.Message, new DataAccessException(sqlException.Message, uspName));
                throw new DataAccessException(sqlException.Message, uspName);
            }
        }

        /// <summary>
        /// Calculate value.
        /// </summary>
        /// <param name="criteria">The value.</param>
        /// <param name="action">Action value.</param>
        /// <param name="isHalfTimeCalculate">HalfTime Calculate.</param>
        /// <returns>Return parameters.</returns>
        private Hashtable CalculateParamters(DoSingleTicketMetaModel criteria, string action, bool isHalfTimeCalculate)
        {
            var parameters = new Hashtable();
            parameters.Add("adminid", criteria.AdminId);
            parameters.Add("matchid", criteria.MatchId);
            parameters.Add("betstatus", criteria.BetStatus ?? string.Empty);
            parameters.Add("goalrecord", criteria.GoalRecord);
            parameters.Add("ip", criteria.IP);
            parameters.Add("action", action);
            parameters.Add("errcode", 0);
            parameters.Add("errmsg", string.Empty);
            switch (criteria.ActionType.ToLower())
            {
                case CalculationType.CalculateFgng:
                    parameters.Add("includelg", false);
                    break;

                default:
                    parameters.Add("hth", criteria.HtHomeScore.HasValue ? criteria.HtHomeScore.Value : 0);
                    parameters.Add("hta", criteria.HtAwayScore.HasValue ? criteria.HtAwayScore.Value : 0);
                    parameters.Add("fth", isHalfTimeCalculate ? null : criteria.FinalHomeScore);
                    parameters.Add("fta", isHalfTimeCalculate ? null : criteria.FinalAwayScore);
                    break;
            }

            return parameters;
        }

        /// <summary>
        /// Refund value.
        /// </summary>
        /// <param name="criteria">The value.</param>
        /// <returns>Return parameters.</returns>
        private Hashtable RefundParameters(DoSingleTicketMetaModel criteria)
        {
            var parameters = new Hashtable();
            switch (criteria.ActionType)
            {
                case CalculationType.RefundFTFirstGoalLastGoal:
                    parameters.Add("action", CalculateAction.RefundFullTime);
                    parameters.Add("lgonly", false);
                    break;

                case CalculationType.RefundFTLastGoal:
                    parameters.Add("action", CalculateAction.RefundFullTime);
                    parameters.Add("lgonly", true);
                    break;

                case CalculationType.RefundHTFirstGoalLastGoal:
                    parameters.Add("action", CalculateAction.RefundHalfTime);
                    parameters.Add("lgonly", false);
                    break;

                case CalculationType.RefundHTLastGoal:
                    parameters.Add("action", CalculateAction.RefundHalfTime);
                    parameters.Add("lgonly", true);
                    break;

                case CalculationType.RefundAllFgng:
                    parameters.Add("action", CalculateAction.Refund);
                    parameters.Add("lgonly", false);
                    break;

                case CalculationType.RefundAllLastGoal:
                    parameters.Add("action", CalculateAction.Refund);
                    parameters.Add("lgonly", true);
                    break;
            }

            return parameters;
        }

        /// <summary>
        /// Get SP Name Calculate.
        /// </summary>
        /// <param name="type">Action type.</param>
        /// <returns>Return SP Name.</returns>
        private string GetSPNameCalculate(string type)
        {
            var uspName = string.Empty;
            switch (type)
            {
                case CalculationType.RefundHalfTime:
                    uspName = "RefundHT";
                    break;

                case CalculationType.RefundFullTime:
                    uspName = "RefundFT";
                    break;

                case CalculationType.RefundFTFirstGoalLastGoal:
                case CalculationType.RefundFTLastGoal:
                case CalculationType.RefundHTFirstGoalLastGoal:
                case CalculationType.RefundHTLastGoal:
                case CalculationType.RefundAllFgng:
                case CalculationType.RefundAllLastGoal:

                    uspName = "RefundFGLG";
                    break;

                case CalculationType.RefundAll:
                    uspName = "RefundAll";
                    break;

                case CalculationType.CalculateHalfTime:
                case CalculationType.CalculateFullTime:
                case CalculationType.CalculateAll:
                case CalculationType.Uncalculate:
                    uspName = "Calculate";
                    break;

                case CalculationType.CalculateFgng:
                    uspName = "CalculateFGLG";
                    break;
            }

            return uspName;
        }

        #endregion Execute Calculate match
    }
}