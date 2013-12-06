// -----------------------------------------------------------------------
// <copyright file="CalcViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
// <summary>
//  Calc Popup Controller
// </summary>
//
// <history>
//  <change who="Kaden" date="2012.09.12">Created and Implement</change>
// </history>

namespace Fanex.NAP.Settlement.ViewModels
{
    using System.Diagnostics.CodeAnalysis;
    using Fanex.NAP.Settlement.DTO;

    /// <summary>
    ///     TODO: Update summary.
    /// </summary>
    public class CalcViewModel : BaseViewModel
    {
        /// <summary>
        ///     Gets or sets Match object.
        /// </summary>
        /// <value>The match detail of the calculate view.</value>
        public MatchDetail CurrentMatch { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether the item is ShowFullButton
        ///     if this value is true, show Calculate HT, Calculate FT,
        ///     Calculate FGNG, Calculate All, Un-calculate
        ///     else show Calculate All, Un-calculate.
        /// </summary>
        /// <value>The shoe button of the calculate view.</value>
        public bool ShowFullButton { get; set; }

        /// <summary>
        ///     Gets or sets template on popup.
        ///     Get template directly for other sport.
        /// </summary>
        /// <value>The directly template of the calculate view.</value>
        public string DirectlyTemplate { get; set; }
    }
}