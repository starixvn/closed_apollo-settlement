// -----------------------------------------------------------------------
// <copyright file="OutrightViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
// <summary>
//  Calculation ViewModel Class
// </summary>
//
// <history>
//  <change who="Kaden" date="2012.08.06">Created and Implement</change>
//  <change who="Kaden" date="2012.09.14">Rename file and inherit BaseViewModel</change>
// </history>

namespace Fanex.NAP.Settlement.ViewModels
{
    using System.Collections.Generic;
    using Fanex.NAP.Settlement.DTO;
    using Fanex.NAP.Settlement.MetaModels;

    /// <summary>
    ///     TODO: Update summary.
    /// </summary>
    public class OutrightViewModel : BaseViewModel
    {
        /// <summary>
        ///     Gets or sets Outright list.
        /// </summary>
        /// <value>The list outright of the view.</value>
        public IList<Outright> OutrightList { get; set; }

        /// <summary>
        ///     Gets or sets parameter.
        /// </summary>
        /// <value>The list outright log action of the view.</value>
        public IList<OutrightLogAction> LogActionList { get; set; }

        /// <summary>
        ///     Gets or sets parameter.
        /// </summary>
        /// <value>The parameter outright of the view.</value>
        public OutrightMetaModel Param { get; set; }
    }
}