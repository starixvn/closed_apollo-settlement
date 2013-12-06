// -----------------------------------------------------------------------
// <copyright file="LeagueDataAccess.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.07.17">Created</change>
//   <change who="Kaden.Trinh" date="2013.07.18">Update GetLeagueList function to get league outright</change>
// </history>

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
    using Fanex.NAP.Settlement.MetaModels;

    /// <summary>
    ///     TODO: Update summary.
    /// </summary>
    public class LeagueDataAccess : ILeagueDataAccess
    {
        /// <summary>
        /// Get list league.
        /// </summary>
        /// <param name="param">Parameter request.</param>
        /// <returns>Return league list.</returns>
        public IList<League> GetLeagueList(LeagueMetaModel param)
        {
            var uspName = param.IsOutright ? "Acc_Set_LeagueSelOutright" : "Acc_Set_LeagueSel";

            var uspParams = new
                {
                    param.FromDate,
                    param.ToDate,
                    SportType = param.SportTypeId
                };

            try
            {
                using (var db = new ObjectDb(uspName))
                {
                    return db.Query<League>(uspParams).ToList();
                }
            }
            catch (SqlException sqlException)
            {
                LogService.Error(sqlException.Message, new DataAccessException(sqlException.Message, uspName, uspParams));
                throw new DataAccessException(sqlException.Message, uspName);
            }
        }
    }
}