// -----------------------------------------------------------------------
// <copyright file="DoMultipleTicketMetaModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAP.Settlement.MetaModels
{
    /// <summary>
    ///     TODO: Update summary.
    /// </summary>
    public class DoMultipleTicketMetaModel
    {
        /// <summary>
        ///     Gets or sets MatchId.
        /// </summary>
        /// <value>The match id of the multiple ticket.</value>
        public int MatchId { get; set; }

        /// <summary>
        ///     Gets or sets MatchCode.
        /// </summary>
        /// <value>The match code of the multiple ticket.</value>
        public string MatchCode { get; set; }

        /// <summary>
        ///     Gets or sets MatchIds for calculate mix parlay.
        /// </summary>
        /// <value>The match ids of the multiple ticket.</value>
        public string MatchIds { get; set; }

        /// <summary>
        ///     Gets or sets ActionType.
        /// </summary>
        /// <value>The action type of the multiple ticket.</value>
        public string ActionType { get; set; }
    }
}