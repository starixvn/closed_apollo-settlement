// -----------------------------------------------------------------------
// <copyright file="AutoSettlementDataAccess.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//  <change who="Kaden" date="2013.07.24">Created</change>
// </history>
namespace Fanex.NAP.Settlement.DataAccess.Implement
{
    using System;
    using System.Collections;
    using System.Data.SqlClient;
    using Fanex.Data;
    using Fanex.Logging;
    using Fanex.NAP.Settlement.DataAccess.Exception;
    using Fanex.NAP.Settlement.DataAccess.Interface;
    using Fanex.NAP.Settlement.DTO.DBResult;

    /// <summary>
    /// Auto Settlement DataAccess.
    /// </summary>
    public class AutoSettlementDataAccess : IAutoSettlementDataAccess
    {
        /// <summary>
        /// Get Result.
        /// </summary>
        /// <param name="db">Object database.</param>
        /// <returns>Return result.</returns>
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

        /// <summary>
        /// Update status AutoSettlement.
        /// </summary>
        /// <param name="adminid">AdminId by.</param>
        /// <param name="matchId">Match Id parameter.</param>
        /// <param name="turnOn">Turn on parameter.</param>
        /// <param name="ip">Internet protocol.</param>
        /// <returns>Return result.</returns>
        public IResult UpdateStatusAutoSettlement(int adminid, int matchId, bool turnOn, string ip)
        {
            var uspName = "Admin_AutoSettlement_UpdateFlag";
            IResult result = new Result();
            try
            {
                using (var db = new ObjectDb(uspName))
                {
                    // Assign Params and ExecuteQuery
                    Hashtable p = new Hashtable();
                    p.Add("adminid", adminid);
                    p.Add("matchid", matchId);
                    p.Add("isturnon", turnOn);
                    p.Add("ip", ip);
                    p.Add("errcode", 0);
                    p.Add("errmsg", string.Empty);

                    db.AssignParamValues(p);
                    db.ExecuteNonQuery();
                    result = this.GetResult(db);
                }
            }
            catch (SqlException sqlException)
            {
                LogService.Error(sqlException.Message, new DataAccessException(sqlException.Message, uspName));
                throw new DataAccessException(sqlException.Message, uspName);
            }

            return result;
        }

        /// <summary>
        /// Get status AutoSettlement.
        /// </summary>
        /// <returns>Return boolean.</returns>
        public bool GetStatusAutoSettlement()
        {
            bool autoSettlement = false;
            var uspName = "Admin_AutoSettlement_GetFlag";
            try
            {
                using (var db = new ObjectDb(uspName))
                {
                    var dataSet = db.ExecuteDataSet();
                    if (dataSet != null && dataSet.Tables.Count > 0)
                    {
                        var dataTable = dataSet.Tables[0];
                        if (dataTable.Rows.Count > 0)
                        {
                            autoSettlement = Convert.ToBoolean(dataTable.Rows[0]["isUsingAutoSettlement"]);
                        }
                    }
                }
            }
            catch (SqlException sqlException)
            {
                LogService.Error(sqlException.Message, new DataAccessException(sqlException.Message, uspName));
                throw new DataAccessException(sqlException.Message, uspName);
            }

            return autoSettlement;
        }

        /// <summary>
        /// Clear fail action AutoSettlement.
        /// </summary>
        /// <param name="adminId">AdminId by.</param>
        /// <param name="matchId">Match id parameter.</param>
        /// <param name="matchIds">List match ids.</param>
        /// <returns>Return result.</returns>
        public IResult ClearFailActionAutoSettlement(int adminId, int matchId, string matchIds)
        {
            var uspName = "Admin_AutoSettlement_ClearFailAction";
            IResult result = new Result();
            try
            {
                using (var db = new ObjectDb(uspName))
                {
                    // Assign Params and ExecuteQuery
                    Hashtable p = new Hashtable();
                    p.Add("adminid", adminId);
                    p.Add("matchid", matchId);
                    p.Add("matchids", matchIds);
                    p.Add("errcode", 0);
                    p.Add("errmsg", string.Empty);

                    db.AssignParamValues(p);
                    db.ExecuteNonQuery();
                    result = this.GetResult(db);
                }
            }
            catch (SqlException sqlException)
            {
                LogService.Error(sqlException.Message, new DataAccessException(sqlException.Message, uspName));
                throw new DataAccessException(sqlException.Message, uspName);
            }

            return result;
        }
    }
}