// -----------------------------------------------------------------------
// <copyright file="BalanceUpDataAccess.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAP.Settlement.DataAccess.Implement
{
    #region Using

    using System.Data.SqlClient;
    using Fanex.Common.Extensions;
    using Fanex.Data;
    using Fanex.Logging;
    using Fanex.NAP.Settlement.DataAccess.Exception;
    using Fanex.NAP.Settlement.DataAccess.Interface;
    using Fanex.NAP.Settlement.DTO.Configuration;

    #endregion Using

    /// <summary>
    /// Balance up DataAccess.
    /// </summary>
    public class BalanceUpDataAccess : IBalanceUpDataAccess
    {
        /// <summary>
        /// Gets BalanceUp day.
        /// </summary>
        /// <returns>Return balance up info.</returns>
        public BalanceUp GetBalanceUpDay()
        {
            var uspName = "Age_SelBalanceUpDay";
            try
            {
                using (var db = new ObjectDb(uspName))
                {
                    var balanceUp = new BalanceUp();
                    using (var reader = db.ExecuteReader())
                    {
                        if (reader != null && reader.Read())
                        {
                            balanceUp.BalanceUpDay = reader.ToDateTime("balanceupday");
                            balanceUp.MoveBetsDay = reader.ToDateTime("movebetsday");
                        }

                        return balanceUp;
                    }
                }
            }
            catch (SqlException sqlException)
            {
                LogService.Error(sqlException.Message, sqlException);
                throw new DataAccessException(sqlException.Message, uspName);
            }
        }
    }
}