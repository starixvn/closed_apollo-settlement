// -----------------------------------------------------------------------
// <copyright file="SportDataAccess.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAP.Settlement.DataAccess.Implement
{
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Linq;
    using Fanex.Data;
    using Fanex.Logging;
    using Fanex.NAP.Settlement.DataAccess.Exception;
    using Fanex.NAP.Settlement.DataAccess.Interface;
    using Fanex.NAP.Settlement.DTO;

    /// <summary>
    ///     TODO: Update summary.
    /// </summary>
    public class SportDataAccess : ISportDataAccess
    {
        /// <summary>
        /// Get list sports.
        /// </summary>
        /// <param name="allSport">Get all sport.</param>
        /// <returns>Return list sport.</returns>
        public IList<Sport> GetSportList(bool allSport)
        {
            var uspName = "Acc_SelSports";
            var uspParams = new
                {
                    IsGetAll = allSport,
                };

            try
            {
                using (var db = new ObjectDb(uspName))
                {
                    return db.Query<Sport>(uspParams).ToList();
                }
            }
            catch (SqlException sqlException)
            {
                LogService.Error(sqlException.Message, new DataAccessException(sqlException.Message, uspName));
                throw new DataAccessException(sqlException.Message, uspName);
            }
        }
    }
}