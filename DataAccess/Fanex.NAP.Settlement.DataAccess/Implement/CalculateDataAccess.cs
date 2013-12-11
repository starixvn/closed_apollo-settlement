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
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using Fanex.Data;
    using Fanex.Data.Dapper;
    using Fanex.Logging;
    using Fanex.NAP.Settlement.DataAccess.Exception;
    using Fanex.NAP.Settlement.DataAccess.Interface;
    using Fanex.NAP.Settlement.DTO;
    using Fanex.NAP.Settlement.DTO.DBResult;
    using Fanex.NAP.Settlement.DTO.Domain;
    using Fanex.NAP.Settlement.MetaModels;
    using Fanex.NAP.Settlement.Web.Common;

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
            p.Add("adminid", value.Creator);
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
        /// Submit action calculate.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <param name="type">Action type.</param>
        /// <returns>Return result.</returns>
        public IResult Calculate(Hashtable parameters, string type)
        {
            var uspName = this.GetSPNameCalculate(type);

            switch (type)
            {
                case CalculationType.CalculateHalfTime:
                case CalculationType.CalculateFullTime:
                case CalculationType.CalculateAll:
                    using (var db = new ObjectDb("Settlement_Checking"))
                    {
                        // Assign Params and ExecuteQuery
                        Hashtable p = new Hashtable();
                        p.Add("matchid", parameters["matchid"]);
                        p.Add("action", parameters["action"]);
                        p.Add("errcode", 0);
                        p.Add("errmsg", string.Empty);

                        db.AssignParamValues(p);
                        db.ExecuteNonQuery();
                        var rs = this.GetResult(db);
                        if (rs.ErrorCode > 0)
                        {
                            return rs;
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

                case CalculationType.RefundAllFgng:
                case CalculationType.RefundAllLg:
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

                case CalculationType.RefundParlayHalfTime:
                case CalculationType.RefundParlayFullTime:
                case CalculationType.RefundParlayAll:
                case CalculationType.CalculateParlay:
                    uspName = "CalculateParlay";
                    break;

                case CalculationType.CalculateNumberGame:
                    uspName = "ReCalculateNumberGame";
                    break;
            }

            return uspName;
        }

        #endregion Execute Calculate match
    }
}