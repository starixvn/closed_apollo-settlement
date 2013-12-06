// -----------------------------------------------------------------------
// <copyright file="NumberGameMetaModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAP.Settlement.MetaModels
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class NumberGameMetaModel
    {
        /// <summary>
        /// Gets or sets Match Id.
        /// </summary>
        /// <value>The match id of number game.</value>
        public int MatchId { get; set; }

        /// <summary>
        /// Gets or sets BallNo.
        /// </summary>
        /// <value>The ball of number game.</value>
        public string BallNo { get; set; }

        /// <summary>
        /// Gets or sets AdminId.
        /// </summary>
        /// <value>The admin id of number game.</value>
        public int AdminId { get; set; }

        /// <summary>
        /// Gets or sets AdminName.
        /// </summary>
        /// <value>The admin name of number game.</value>
        public string AdminName { get; set; }

        /// <summary>
        /// Gets or sets SiteNo.
        /// </summary>
        /// <value>The site of number game.</value>
        public string SiteNo { get; set; }

        /// <summary>
        /// Gets or sets FromIP.
        /// </summary>
        /// <value>The IP of number game.</value>
        public string FromIp { get; set; }
    }
}