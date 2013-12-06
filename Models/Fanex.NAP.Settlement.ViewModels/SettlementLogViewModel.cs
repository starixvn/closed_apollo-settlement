// -----------------------------------------------------------------------
// <copyright file="SettlementLogViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAP.Settlement.ViewModels
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.NAP.Settlement.DTO;
    using Fanex.NAP.Settlement.MetaModels;

    /// <summary>
    ///     Match log's view model.
    /// </summary>
    public class SettlementLogViewModel
    {
        /// <summary>
        /// Gets or sets parameter settlement log.
        /// </summary>
        /// <value>The parameter settlement log of the view.</value>
        public SettlementLogMetaModel Param { get; set; }

        /// <summary>
        /// Gets or sets list log settlement.
        /// </summary>
        /// <value>The list settlement log of the view.</value>
        public IList<SettlementLog> MainData { get; set; }
    }
}