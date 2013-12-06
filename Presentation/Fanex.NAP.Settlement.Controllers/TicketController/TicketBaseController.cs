// -----------------------------------------------------------------------
// <copyright file="TicketBaseController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
// <summary>
//  Calculate Controller
// </summary>
//
// <history>
//  <change who="Kaden" date="2013.07.29">Created and Implement</change>
// </history>
namespace Fanex.NAP.Settlement.Controllers.TicketController
{
    using System.Web.Mvc;
    using Fanex.NAP.Core.Web.Session;
    using Services.Implement;
    using Services.Interface;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class TicketBaseController : Controller
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="TicketBaseController" /> class.
        /// </summary>
        public TicketBaseController()
        {
            this.CalculateService = new CalculateService();
            this.CurrentSession = new CurrentSession();
        }

        /// <summary>
        ///    Gets or sets  Calculation Service.
        /// </summary>
        /// <value>The calculate service of ticket base controller.</value>
        public ICalculateService CalculateService { get; set; }

        /// <summary>
        ///    Gets or sets  Current Session.
        /// </summary>
        /// <value>The current session of ticket base controller.</value>
        public ICurrentSession CurrentSession { get; set; }
    }
}