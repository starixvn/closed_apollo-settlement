// --------------------------------------------------------------------------
// <copyright file="OutrightDataAccess.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.07.17">Created</change>
//   <change who="Kaden.Trinh" date="2013.07.18">Add function get all matches under this league</change>
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
    using Fanex.NAP.Settlement.MetaModels;

    /// <summary>
    /// Data Access Outright.
    /// </summary>
    public class OutrightDataAccess : IOutrightDataAccess
    {
        /// <summary>
        /// Get list outright settlement.
        /// </summary>
        /// <param name="request">Request parameter outright.</param>
        /// <param name="actionLog">Out list action log.</param>
        /// <returns>Return outright list.</returns>
        public IList<Outright> GetOutrightList(OutrightMetaModel request, out IList<OutrightLogAction> actionLog)
        {
            var uspName = "Acc_Set_OutrightSel";

            var uspParams = new
            {
                request.FromDate,
                request.ToDate,
                SportType = request.SportTypeId,
                request.LeagueId,
                request.IsSettled
            };

            try
            {
                var outrightList = new List<Outright>();
                using (var db = new ObjectDb(uspName))
                {
                    using (var multiTable = db.QueryMultiple(uspParams))
                    {
                        outrightList = multiTable.Read<Outright>().ToList();
                        actionLog = multiTable.Read<OutrightLogAction>().ToList();
                    }

                    return outrightList;
                }
            }
            catch (SqlException sqlException)
            {
                LogService.Error(sqlException.Message, new DataAccessException(sqlException.Message, uspName, uspParams));
                throw new DataAccessException(sqlException.Message, uspName);
            }
        }

        /// <summary>
        /// Get all matches under this league.
        /// </summary>
        /// <param name="leagueId">League id.</param>
        /// <param name="isSettled">Match is settled.</param>
        /// <param name="actionLog">Action log.</param>
        /// <returns>Return outright list.</returns>
        public IList<Outright> GetMatchesByLeagueId(int leagueId, bool isSettled, out IList<OutrightLogAction> actionLog)
        {
            var uspName = "Acc_Set_OutrightSelDetail";

            var uspParams = new
            {
                leagueId,
                isSettled
            };

            try
            {
                var outrightList = new List<Outright>();
                using (var db = new ObjectDb(uspName))
                {
                    using (var multiTable = db.QueryMultiple(uspParams))
                    {
                        outrightList = multiTable.Read<Outright>().ToList();
                        actionLog = multiTable.Read<OutrightLogAction>().ToList();
                    }

                    return outrightList;
                }
            }
            catch (SqlException sqlException)
            {
                LogService.Error(sqlException.Message, new DataAccessException(sqlException.Message, uspName, uspParams));
                throw new DataAccessException(sqlException.Message, uspName);
            }
        }

        /// <summary>
        /// Get log action outright.
        /// </summary>
        /// <param name="outrightId">Outright Id.</param>
        /// <returns>Return outright log action list.</returns>
        public IList<OutrightLogAction> GetLogAction(int outrightId)
        {
            var uspName = "Acc_Set_GetConfirmedActionOutright";

            var uspParams = new
            {
                orId = outrightId
            };

            try
            {
                using (var db = new ObjectDb(uspName))
                {
                    return db.Query<OutrightLogAction>(uspParams).ToList();
                }
            }
            catch (SqlException sqlException)
            {
                LogService.Error(sqlException.Message, new DataAccessException(sqlException.Message, uspName, uspParams));
                throw new DataAccessException(sqlException.Message, uspName);
            }
        }

        /// <summary>
        /// Update Outright Event.
        /// </summary>
        /// <param name="request">Outright parameter.</param>
        /// <returns>Return result.</returns>
        public IResult OutrightEvent(OutrightCalculateMetaModel request)
        {
            var uspName = "Dot_Admin_SettleOutright_Event";

            var p = new Hashtable();
            p.Add("creator", request.Creator);
            p.Add("matchid", request.MatchId);
            p.Add("status", request.Status);
            p.Add("isdeadheat", request.IsDeadheat);
            p.Add("ip", request.IP);
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

        /// <summary>
        /// Update Outright Trans.
        /// </summary>
        /// <param name="request">Outright parameter.</param>
        /// <returns>Return result.</returns>
        public IResult OutrightTrans(OutrightCalculateMetaModel request)
        {
            var uspName = "Dot_Admin_SettleOutright_Trans";

            var p = new Hashtable();
            p.Add("creator", request.Creator);
            p.Add("matchid", request.MatchId);
            p.Add("status", request.Status);
            p.Add("deadheat", request.DeadHeat);
            p.Add("ip", request.IP);
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

        /// <summary>
        /// Update Flag Outright.
        /// </summary>
        /// <param name="request">Outright auto parameter.</param>
        /// <returns>Return result.</returns>
        public IResult UpdateFlagOutright(OutrightAutoSettleMetaModel request)
        {
            var uspName = "Admin_AutoSettlement_UpdateFlagOutright";

            var p = new Hashtable();
            p.Add("creator", request.Creator);
            p.Add("leagueid", request.LeagueId);
            p.Add("isturnon", request.IsTurnOn);
            p.Add("ip", request.IP);
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

        /// <summary>
        /// Clear Fail Action Outright.
        /// </summary>
        /// <param name="outrightId">Outright Id.</param>
        /// <returns>Return result.</returns>
        public IResult ClearFailActionOutright(int outrightId)
        {
            var uspName = "Admin_AutoSettlement_ClearFailActionOutright";

            var p = new Hashtable();
            p.Add("orid", outrightId);
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

        #region Utilities

        /// <summary>
        ///   The get result.
        /// </summary>
        /// <param name="db"> The database. </param>
        /// <returns> Return Result. </returns>
        private IResult GetResult(ObjectDb db)
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
    }
}