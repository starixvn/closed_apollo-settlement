// -----------------------------------------------------------------------
// <copyright file="RefundMetaModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
// <summary>
//  Refund Popup Request Class
// </summary>
//
// <history>
//  <change who="Kaden" date="2012.09.10">Created and Implement</change>
// </history>

namespace Fanex.NAP.Settlement.MetaModels
{
    /// <summary>
    ///     TODO: Update summary.
    /// </summary>
    public class RefundMetaModel
    {
        /// <summary>
        ///     Gets or sets MatchId.
        /// </summary>
        /// <value>The match id of refund.</value>
        public int MatchId { get; set; }

        /// <summary>
        ///     Gets or sets sportId.
        /// </summary>
        /// <value>The sport id of refund.</value>
        public int SportTypeId { get; set; }
    }
}