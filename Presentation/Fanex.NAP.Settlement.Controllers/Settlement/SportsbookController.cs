// -----------------------------------------------------------------------
// <copyright file="SportsbookController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
// <summary>
//  Calculate Controller
// </summary>
//
// <history>
//  <change who="Kaden" date="2012.08.14">Created and Implement</change>
// </history>

namespace Fanex.NAP.Settlement.Controllers.Settlement
{
    #region

    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Mvc;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;
    using Fanex.NAP.Settlement.DTO;
    using Fanex.NAP.Settlement.DTO.Configuration;
    using Fanex.NAP.Settlement.DTO.Domain;
    using Fanex.NAP.Settlement.DTO.Permission;
    using Fanex.NAP.Settlement.MetaModels;
    using Fanex.NAP.Settlement.Services.Implement;
    using Fanex.NAP.Settlement.Services.Interface;
    using Fanex.NAP.Settlement.ViewModels;
    using Fanex.NAP.Settlement.Web.Configuration;
    using Fanex.NAP.Settlement.Web.Services;

    #endregion

    /// <summary>
    ///     TODO: Update summary.
    /// </summary>
    [NapSuperAuth]
    [NapAuth(FunctionKey = SettlementFunctionKeys.PrepareSportsbookSettlement)]
    public partial class SportsbookController : BaseController
    {
        #region Fields

        /// <summary>
        /// Calculate Service.
        /// </summary>
        private readonly ICalculateService _calculateService;

        /// <summary>
        /// Sport Service.
        /// </summary>
        private readonly ISportService _sportService;

        /// <summary>
        /// League Service.
        /// </summary>
        private readonly ILeagueService _leagueService;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SportsbookController" /> class.
        /// </summary>
        public SportsbookController()
            : this(null, null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SportsbookController" /> class.
        /// </summary>
        /// <param name="calculateService">Calculate service.</param>
        /// <param name="sportService">Sport service.</param>
        /// <param name="leagueService">League service.</param>
        public SportsbookController(ICalculateService calculateService, ISportService sportService, ILeagueService leagueService)
        {
            this._calculateService = calculateService ?? new CalculateService();
            this._sportService = sportService ?? new SportService();
            this._leagueService = leagueService ?? new LeagueService();
        }

        #endregion Constructor

        /// <summary>
        ///     Controller get list match.
        /// DisabledAutoSettle = NULL or 0  : ON.
        /// DisabledAutoSettle = 1          : OFF.
        /// </summary>
        /// <returns>View result.</returns>
        [HttpGet]
        public ActionResult Index()
        {
            var model = new CalculateMetaModel { SportsList = this._sportService.GetSportList(false) };
            var leagueList = this._leagueService.GetLeagueList(new LeagueMetaModel
                        {
                            SportTypeId = model.SportTypeId,
                            FromDate = model.FromDate,
                            ToDate = model.FromDate
                        });
            model.LeaguesList = leagueList;
            var sportViewSetting = SportViewSettingFactory.Create(model.SportTypeId);
            model = this.SetFilterBy(model);
            bool canSetAutoSettlement = PermissionService.HasHostPermission(SettlementFunctionKeys.AutoSettlement);
            var data = new CalculateViewModel
            {
                LeaguesList = model.LeaguesList,
                SportsList = model.SportsList,
                HeaderEvent = ViewResources.Event,
                Param = model,
                Template = null,
                MainMatchList = new List<Match>(),
                Permission = new SettlementPermission
                {
                    CanViewSettlementLog = PermissionService.HasReadPermission(SettlementFunctionKeys.ViewLogSettlement),
                    CanSetAutoSettlement = canSetAutoSettlement,
                    SportsBook = new SportsBookPermission()
                    {
                        CanCalculate = PermissionService.HasHostPermission(SettlementFunctionKeys.SportsbookCalculate),
                        CanUncalculate = PermissionService.HasHostPermission(SettlementFunctionKeys.SportsbookUncalculate),
                        CanRefund = PermissionService.HasHostPermission(SettlementFunctionKeys.SportsbookRefund),
                        CanCalculateMixParlay = PermissionService.HasHostPermission(SettlementFunctionKeys.SportsbookCalculateMixParlay),
                        CanCloseMatch = PermissionService.HasHostPermission(SettlementFunctionKeys.SportsbookCloseMatch)
                    }
                }
            };

            var adminService = new AutoSettlementService();
            var disabledAutoSettle = !adminService.GetStatusAutoSettlement();
            ViewBag.DisabledAutoSettle = disabledAutoSettle;
            return this.View(data);
        }

        /// <summary>
        ///     Controller get list match.
        /// DisabledAutoSettle = NULL or 0  : ON.
        /// DisabledAutoSettle = 1          : OFF.
        /// </summary>
        /// <param name="model">Calculate MetaModel.</param>
        /// <returns>View result.</returns>
        [HttpPost]
        public ActionResult Index(CalculateMetaModel model)
        {
            var sportViewSetting = SportViewSettingFactory.Create(model.SportTypeId);
            model = this.SetFilterBy(model);
            IList<MatchLog> matchLog;
            var listCalculate = this._calculateService.SearchMatch(model, out matchLog);
            bool canSetAutoSettlement = PermissionService.HasHostPermission(SettlementFunctionKeys.AutoSettlement);
            foreach (Match match in listCalculate)
            {
                match.GroupSport = sportViewSetting.GroupSport;
            }

            var data = new CalculateViewModel
                {
                    ActionList = matchLog,
                    LeaguesList = model.LeaguesList,
                    SportsList = model.SportsList,
                    HeaderEvent = ViewResources.Event,
                    Param = model,
                    MainMatchList = listCalculate,
                    Template = sportViewSetting.Template,
                    GroupSport = sportViewSetting.GroupSport,
                    IsCheckBoxCalcMp = sportViewSetting.IsCheckBoxCalcMp,
                    Colspan = sportViewSetting.Colspan,
                    Permission = new SettlementPermission
                    {
                        CanViewSettlementLog = PermissionService.HasReadPermission(SettlementFunctionKeys.ViewLogSettlement),
                        CanSetAutoSettlement = canSetAutoSettlement,
                        SportsBook = new SportsBookPermission()
                        {
                            CanCalculate = PermissionService.HasHostPermission(SettlementFunctionKeys.SportsbookCalculate),
                            CanUncalculate = PermissionService.HasHostPermission(SettlementFunctionKeys.SportsbookUncalculate),
                            CanRefund = PermissionService.HasHostPermission(SettlementFunctionKeys.SportsbookRefund),
                            CanCalculateMixParlay = PermissionService.HasHostPermission(SettlementFunctionKeys.SportsbookCalculateMixParlay),
                            CanCloseMatch = PermissionService.HasHostPermission(SettlementFunctionKeys.SportsbookCloseMatch)
                        }
                    }
                };

            var adminService = new AutoSettlementService();
            var disabledAutoSettle = !adminService.GetStatusAutoSettlement();
            ViewBag.DisabledAutoSettle = disabledAutoSettle;
            if (model.IsReLoad)
            {
                return this.View("~/Areas/Settlement/Views/Sportsbook/RefreshContent.cshtml", data);
            }
            else
            {
                return this.View(data);
            }
        }

        /// <summary>
        /// Get list match OverOnly by get.
        /// </summary>
        /// <returns>Return View result.</returns>
        [HttpGet]
        public ActionResult OverOnly()
        {
            var model = new CalculateMetaModel();
            return this.OverOnly(model);
        }

        /// <summary>
        /// Get list match OverOnly by post.
        /// </summary>
        /// <param name="model">Calculate MetaModel.</param>
        /// <returns>View result.</returns>
        [HttpPost]
        public ActionResult OverOnly(CalculateMetaModel model)
        {
            // Get default filter list by SportId
            var sportViewSetting = SportViewSettingFactory.Create(model.SportTypeId);
            IList<MatchLog> matchLog;
            var listCalculate = this._calculateService.SearchMatchOverOnly(model, out matchLog);

            bool canSetAutoSettlement = PermissionService.HasHostPermission(SettlementFunctionKeys.AutoSettlement);

            var data = new CalculateViewModel
            {
                ActionList = matchLog,
                HeaderEvent = ViewResources.Event,
                Param = model,
                MainMatchList = listCalculate,
                Template = sportViewSetting.Template,
                GroupSport = sportViewSetting.GroupSport,
                Colspan = sportViewSetting.Colspan,
                PageType = PageType.OverOnly,
                Permission = new SettlementPermission
                {
                    CanViewSettlementLog = PermissionService.HasReadPermission(SettlementFunctionKeys.ViewLogSettlement),
                    CanSetAutoSettlement = canSetAutoSettlement,
                    SportsBook = new SportsBookPermission
                    {
                        CanCalculate = PermissionService.HasHostPermission(SettlementFunctionKeys.SportsbookCalculate),
                        CanUncalculate = PermissionService.HasHostPermission(SettlementFunctionKeys.SportsbookUncalculate),
                        CanRefund = PermissionService.HasHostPermission(SettlementFunctionKeys.SportsbookRefund),
                    }
                }
            };

            var adminService = new AutoSettlementService();
            var disabledAutoSettle = !adminService.GetStatusAutoSettlement();

            // DisabledAutoSettle = NULL or 0  : ON
            // DisabledAutoSettle = 1          : OFF
            ViewBag.DisabledAutoSettle = disabledAutoSettle;

            // We need to check when click on icon reload match
            if (model.IsReLoad)
            {
                return this.View("~/Areas/Settlement/Views/Sportsbook/RefreshContent.cshtml", data);
            }
            else
            {
                return this.View(data);
            }
        }

        /// <summary>
        /// Get list match HTOverOnly by get.
        /// </summary>
        /// <returns>Return View result.</returns>
        [HttpGet]
        public ActionResult HtOverOnly()
        {
            var model = new CalculateMetaModel();
            return this.HtOverOnly(model);
        }

        /// <summary>
        /// Get list match OverOnly by post.
        /// </summary>
        /// <param name="model">Calculate MetaModel.</param>
        /// <returns>View result.</returns>
        [HttpPost]
        public ActionResult HtOverOnly(CalculateMetaModel model)
        {
            // Get default filter list by SportId
            var sportViewSetting = SportViewSettingFactory.Create(model.SportTypeId);
            IList<MatchLog> matchLog;
            var listCalculate = this._calculateService.SearchMatchHtOverOnly(model, out matchLog);

            bool canSetAutoSettlement = PermissionService.HasHostPermission(SettlementFunctionKeys.AutoSettlement);

            var data = new CalculateViewModel
            {
                ActionList = matchLog,
                HeaderEvent = ViewResources.Event,
                Param = model,
                MainMatchList = listCalculate,
                Template = sportViewSetting.Template,
                GroupSport = sportViewSetting.GroupSport,
                Colspan = sportViewSetting.Colspan,
                PageType = PageType.OverOnly,
                Permission = new SettlementPermission
                {
                    CanViewSettlementLog = PermissionService.HasReadPermission(SettlementFunctionKeys.ViewLogSettlement),
                    CanSetAutoSettlement = canSetAutoSettlement,
                    SportsBook = new SportsBookPermission
                    {
                        CanCalculate = PermissionService.HasHostPermission(SettlementFunctionKeys.SportsbookCalculate),
                        CanUncalculate = PermissionService.HasHostPermission(SettlementFunctionKeys.SportsbookUncalculate),
                        CanRefund = PermissionService.HasHostPermission(SettlementFunctionKeys.SportsbookRefund),
                    }
                }
            };

            var adminService = new AutoSettlementService();
            var disabledAutoSettle = !adminService.GetStatusAutoSettlement();

            // DisabledAutoSettle = NULL or 0  : ON
            // DisabledAutoSettle = 1          : OFF
            ViewBag.DisabledAutoSettle = disabledAutoSettle;

            // We need to check when click on icon reload match
            if (model.IsReLoad)
            {
                return this.View("~/Areas/Settlement/Views/Sportsbook/RefreshContent.cshtml", data);
            }
            else
            {
                return this.View(data);
            }
        }

        /// <summary>
        /// Get list action confirmed.
        /// </summary>
        /// <param name="groupSport">Group sport.</param>
        /// <param name="listAction">List action.</param>
        /// <returns>Return View result.</returns>
        [ChildActionOnly]
        public PartialViewResult ListAction(int groupSport, IList<MatchLog> listAction)
        {
            return this.PartialView("~/Areas/Settlement/Views/UserControl/ListAction.cshtml", Tuple.Create(listAction, groupSport));
        }

        /// <summary>
        ///     Re-load Leagues list when change sport type.
        /// </summary>
        /// <param name="sportTypeId">SportType Id.</param>
        /// <param name="dateFrom">From date parameter.</param>
        /// <param name="dateTo">To date parameter.</param>
        /// <param name="showAllLeagues">Show all leagues.</param>
        /// <returns>View Result.</returns>
        [HttpPost]
        public ActionResult GetLeaguesList(int sportTypeId, DateTime dateFrom, DateTime dateTo, bool showAllLeagues)
        {
            // Get default all league by sport
            var leagues = this._leagueService.GetLeagueList(new LeagueMetaModel
            {
                SportTypeId = sportTypeId,
                FromDate = dateFrom,
                ToDate = dateTo
            });

            return this.View(showAllLeagues ? "~/Views/UserControl/LeaguesList.cshtml" : "~/Views/UserControl/LeaguesListNotShowAll.cshtml", leagues);
        }

        /// <summary>
        /// Close match is running.
        /// </summary>
        /// <param name="model">Parameter for update.</param>
        /// <returns>Return result value.</returns>
        [NapAuth(FunctionKey = SettlementFunctionKeys.SportsbookCloseMatch, Policy = Constant.Policy.Host)]
        [HttpPost]
        public ActionResult CloseMatch(CloseMatchMetaModel model)
        {
            model.Creator = CurrentSession.UserId;
            model.IP = Request.ServerVariables["REMOTE_ADDR"];

            var result = _calculateService.CloseMatch(model);

            return this.Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Set value filter by.
        /// </summary>
        /// <param name="model">Model value.</param>
        /// <returns>Return calculate value.</returns>
        private CalculateMetaModel SetFilterBy(CalculateMetaModel model)
        {
            if (model.UnCalculate && model.UncalculateMultiple)
            {
                model.FilterBy = 3;
            }
            else if (model.UncalculateMultiple)
            {
                model.FilterBy = 2;
            }
            else if (model.UnCalculate)
            {
                model.FilterBy = 1;
            }
            else
            {
                model.FilterBy = 0;
            }

            return model;
        }
    }
}