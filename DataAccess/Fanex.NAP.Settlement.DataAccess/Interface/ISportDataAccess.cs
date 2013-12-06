// -----------------------------------------------------------------------
// <copyright file="ISportDataAccess.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAP.Settlement.DataAccess.Interface
{
    #region

    using System.Collections.Generic;
    using Fanex.NAP.Settlement.DTO;

    #endregion

    /// <summary>
    ///     TODO: Update summary.
    /// </summary>
    public interface ISportDataAccess
    {
        /// <summary>
        /// Get list sports.
        /// </summary>
        /// <param name="allSport">Get all sport.</param>
        /// <returns>Return list sport.</returns>
        IList<Sport> GetSportList(bool allSport);
    }
}