// --------------------------------------------------------------------------
// <copyright file="OutrightCalculateMetaModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.07.17">Created</change>
// </history>
namespace Fanex.NAP.Settlement.MetaModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Outright Calculate MetaModel.
    /// </summary>
    public class OutrightCalculateMetaModel
    {
        /// <summary>
        ///     Gets or sets AdminId.
        /// </summary>
        /// <value>The AdminId of outright.</value>
        public int AdminId { get; set; }

        /// <summary>
        ///     Gets or sets MatchId.
        /// </summary>
        /// <value>The match id of outright calculate.</value>
        public int MatchId { get; set; }

        /// <summary>
        ///     Gets or sets Status.
        /// </summary>
        /// <value>The status of outright calculate.</value>
        public string Status { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether the item is DeadHeat.
        /// </summary>
        /// <value>The dead heat of outright calculate.</value>
        public bool IsDeadheat { get; set; }

        /// <summary>
        ///     Gets or sets DeadHeat.
        /// </summary>
        /// <value>The dead heat of outright calculate.</value>
        public float DeadHeat { get; set; }

        /// <summary>
        ///     Gets or sets IP.
        /// </summary>
        /// <value>The IP of outright calculate.</value>
        public string IP { get; set; }
    }
}