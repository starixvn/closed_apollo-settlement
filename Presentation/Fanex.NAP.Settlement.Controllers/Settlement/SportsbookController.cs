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
    using System.Collections;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;
    using Fanex.NAP.Core.Web.Session;
    using Fanex.NAP.Settlement.Controllers.Helper;
    using Fanex.NAP.Settlement.Core.Configuration;
    using Fanex.NAP.Settlement.Core.Constants;
    using Fanex.NAP.Settlement.Core.Resources;
    using Fanex.NAP.Settlement.DTO;
    using Fanex.NAP.Settlement.DTO.Configuration;
    using Fanex.NAP.Settlement.DTO.DBResult;
    using Fanex.NAP.Settlement.DTO.Permission;
    using Fanex.NAP.Settlement.MetaModels;
    using Fanex.NAP.Settlement.Services.Implement;
    using Fanex.NAP.Settlement.Services.Interface;
    using Fanex.NAP.Settlement.ViewModels;

    #endregion

    /// <summary>
    ///     TODO: Update summary.
    /// </summary>
    [NapSuperAuth]
    [NapAuth(FunctionKey = SettlementFunctionKeys.PrepareSportsbookSettlement)]
    public class SportsbookController : BaseController
    {
        #region Fields

        private const string UnReadyContentUrl = "~/Areas/Settlement/Views/UserControl/UnreadyMatch.cshtml";

        /// <summary>
        ///     Sport list show full button.
        /// </summary>
        private static readonly IList<int> FullButtonSportList = new List<int>
        {
            SportType.Soccer,
            SportType.BaseBall,
            SportType.FootBall,
            SportType.Rugby,
            SportType.NetBall,
            SportType.BasketBall
        };

        private static readonly IList<int> TennisGroupSportId = new List<int>
        {
            5, 7, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 27, 28, 29, 30, 31, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 99
        };

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

        private readonly ICurrentSession _currentSession;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SportsbookController" /> class.
        /// </summary>
        public SportsbookController()
            : this(null, null, null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SportsbookController" /> class.
        /// </summary>
        /// <param name="calculateService">Calculate service.</param>
        /// <param name="sportService">Sport service.</param>
        /// <param name="leagueService">League service.</param>
        /// <param name="currentSession">Current session service.</param>
        public SportsbookController(ICalculateService calculateService, ISportService sportService, ILeagueService leagueService, ICurrentSession currentSession)
        {
            this._calculateService = calculateService ?? new CalculateService();
            this._sportService = sportService ?? new SportService();
            this._leagueService = leagueService ?? new LeagueService();
            this._currentSession = currentSession ?? new CurrentSession();
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
                    CanViewSettlementLog = HasPermission(_currentSession.UserId, SettlementFunctionKeys.ViewLogSettlement, Constant.Policy.Read),
                    CanSetAutoSettlement = HasPermission(_currentSession.UserId, SettlementFunctionKeys.AutoSettlement, Constant.Policy.Host),
                    SportsBook = new SportsBookPermission()
                    {
                        CanCalculate = HasPermission(_currentSession.UserId, SettlementFunctionKeys.SportsbookCalculate, Constant.Policy.Host),
                        CanUncalculate = HasPermission(_currentSession.UserId, SettlementFunctionKeys.SportsbookUncalculate, Constant.Policy.Host),
                        CanRefund = HasPermission(_currentSession.UserId, SettlementFunctionKeys.SportsbookRefund, Constant.Policy.Host),
                        CanCalculateMixParlay = HasPermission(_currentSession.UserId, SettlementFunctionKeys.SportsbookCalculateMixParlay, Constant.Policy.Host),
                        CanCloseMatch = HasPermission(_currentSession.UserId, SettlementFunctionKeys.SportsbookCloseMatch, Constant.Policy.Host)
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
                        CanViewSettlementLog = HasPermission(_currentSession.UserId, SettlementFunctionKeys.ViewLogSettlement, Constant.Policy.Read),
                        CanSetAutoSettlement = HasPermission(_currentSession.UserId, SettlementFunctionKeys.AutoSettlement, Constant.Policy.Host),
                        SportsBook = new SportsBookPermission()
                        {
                            CanCalculate = HasPermission(_currentSession.UserId, SettlementFunctionKeys.SportsbookCalculate, Constant.Policy.Host),
                            CanUncalculate = HasPermission(_currentSession.UserId, SettlementFunctionKeys.SportsbookUncalculate, Constant.Policy.Host),
                            CanRefund = HasPermission(_currentSession.UserId, SettlementFunctionKeys.SportsbookRefund, Constant.Policy.Host),
                            CanCalculateMixParlay = HasPermission(_currentSession.UserId, SettlementFunctionKeys.SportsbookCalculateMixParlay, Constant.Policy.Host),
                            CanCloseMatch = HasPermission(_currentSession.UserId, SettlementFunctionKeys.SportsbookCloseMatch, Constant.Policy.Host)
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

            bool canSetAutoSettlement = HasPermission(_currentSession.UserId, SettlementFunctionKeys.AutoSettlement, Constant.Policy.Host);

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
                    CanViewSettlementLog = HasPermission(_currentSession.UserId, SettlementFunctionKeys.ViewLogSettlement, Constant.Policy.Read),
                    CanSetAutoSettlement = canSetAutoSettlement,
                    SportsBook = new SportsBookPermission
                    {
                        CanCalculate = HasPermission(_currentSession.UserId, SettlementFunctionKeys.SportsbookCalculate, Constant.Policy.Host),
                        CanUncalculate = HasPermission(_currentSession.UserId, SettlementFunctionKeys.SportsbookUncalculate, Constant.Policy.Host),
                        CanRefund = HasPermission(_currentSession.UserId, SettlementFunctionKeys.SportsbookRefund, Constant.Policy.Host),
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
                    CanViewSettlementLog = HasPermission(_currentSession.UserId, SettlementFunctionKeys.ViewLogSettlement, Constant.Policy.Read),
                    CanSetAutoSettlement = HasPermission(_currentSession.UserId, SettlementFunctionKeys.AutoSettlement, Constant.Policy.Host),
                    SportsBook = new SportsBookPermission
                    {
                        CanCalculate = HasPermission(_currentSession.UserId, SettlementFunctionKeys.SportsbookCalculate, Constant.Policy.Host),
                        CanUncalculate = HasPermission(_currentSession.UserId, SettlementFunctionKeys.SportsbookUncalculate, Constant.Policy.Host),
                        CanRefund = HasPermission(_currentSession.UserId, SettlementFunctionKeys.SportsbookRefund, Constant.Policy.Host),
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
            model.AdminId = _currentSession.UserId;
            var result = _calculateService.CloseMatch(model);

            return this.Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///     Controller calculation popup page when click on reference action.
        /// </summary>
        /// <param name="sportId">Sport type.</param>
        /// <param name="matchId">Match id parameter.</param>
        /// <param name="actionId">Action Id from IBC.</param>
        /// <param name="logId">Log Id parameter.</param>
        /// <returns>Return Action result.</returns>
        [HttpGet]
        [OutputCache(Duration = 0, VaryByParam = "none")]
        [NapAuth(FunctionKey = SettlementFunctionKeys.SportsbookSettlement)]
        public ActionResult PopupCalculate(byte sportId, int matchId, int actionId, int logId)
        {
            // Default is Soccer
            var sportViewSetting = SportViewSettingFactory.Create(sportId);
            var match = this._calculateService.GetMatchDetail(matchId, false);

            if (!IsMatchReadyForCalc(match) && !match.EventStatus.Equals(EventStatus.Completed, StringComparison.OrdinalIgnoreCase))
            {
                return View(UnReadyContentUrl, (object)ViewResources.NotReadyMatchError);
            }

            if (logId > 0)
            {
                // Set new value action from IBC
                var matchLog = this._calculateService.GetMatchLogDetail(logId);
                matchLog.IsExistHTFGLG = match.IsExistHTFGLG;
                matchLog.IsExistFGLG = match.IsExistFGLG;
                matchLog.GroupSport = sportViewSetting.GroupSport;
                match.HtHomeScore = matchLog.HtHomeScore;
                match.HtAwayScore = matchLog.HtAwayScore;
                match.FinalHomeScore = matchLog.FinalHomeScore;
                match.FinalAwayScore = matchLog.FinalAwayScore;
                match.GoalRecord = matchLog.GoalRecord;
                match.BetStatusAccess = matchLog.BetStatusAccess;
            }

            // Full buttons: Soccer, Basketball, Football, Rugby, Netball, Baseball
            var showFullButton = FullButtonSportList.Contains(match.SportType);

            var permission = SetCalcPopupPermission();

            var data = new CalcViewModel
            {
                PageType = PageType.Calc,
                HeaderEvent = ViewResources.Team,
                LeagueId = match.LeagueId,
                CurrentMatch = match,
                ShowFullButton = showFullButton,
                GroupSport = sportViewSetting.GroupSport,
                Template = sportViewSetting.Template,
                DirectlyTemplate = sportViewSetting.DirectlyTemplate,
                Permission = permission
            };

            return this.View(data);
        }

        /// <summary>
        /// Controller refund popup page.
        /// </summary>
        /// <param name="sportId">Sport id value.</param>
        /// <param name="matchId">Match id value.</param>
        /// <param name="actionId">Action id value.</param>
        /// <param name="logId">Log id value.</param>
        /// <returns>View result.</returns>
        [HttpGet]
        [NapAuth(FunctionKey = SettlementFunctionKeys.SportsbookRefund, Policy = Constant.Policy.Read)]
        public ActionResult PopupRefund(byte sportId, int matchId, int actionId, int logId)
        {
            var model = new RefundMetaModel
           {
               MatchId = matchId,
               SportTypeId = sportId
           };

            // Default is Soccer
            var sportViewSetting = SportViewSettingFactory.Create(model.SportTypeId);

            var match = this._calculateService.GetMatchDetail(matchId, true);

            if (actionId > 0 && logId > 0)
            {
                // Set new value action from IBC
                var matchLog = this._calculateService.GetMatchLogDetail(logId);
                matchLog.GroupSport = sportViewSetting.GroupSport;
                matchLog.IsExistHTFGLG = match.IsExistHTFGLG;
                matchLog.IsExistFGLG = match.IsExistFGLG;
                match.HtHomeScore = matchLog.HtHomeScore;
                match.HtAwayScore = matchLog.HtAwayScore;
                match.FinalHomeScore = matchLog.FinalHomeScore;
                match.FinalAwayScore = matchLog.FinalAwayScore;
                match.GoalRecord = matchLog.GoalRecord;
                match.BetStatusAccess = matchLog.BetStatusAccess;
            }

            var data = new RefundViewModel
            {
                PageType = PageType.Refund,
                HeaderEvent = ViewResources.Team,
                LeagueId = match.LeagueId,
                CurrentMatch = match,
                Template = sportViewSetting.Template,
                DirectlyTemplate = sportViewSetting.DirectlyTemplate,
                GroupSport = sportViewSetting.GroupSport,
            };

            return this.View(data);
        }

        /// <summary>
        /// We need split two functions Calculate and Un-Calculate so that we can check permission on action.
        /// </summary>
        /// <param name="model">Model single ticket.</param>
        /// <returns>View result.</returns>
        [NapAuth(FunctionKey = SettlementFunctionKeys.SportsbookCalculate, Policy = Constant.Policy.Host)]
        [HttpPost]
        public ActionResult Calculate(DoSingleTicketMetaModel model)
        {
            var result = CalculateSingle(model);
            return this.Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Un Calculate.
        /// </summary>
        /// <param name="model">Model single ticket.</param>
        /// <returns>View result.</returns>
        [NapAuth(FunctionKey = SettlementFunctionKeys.SportsbookUncalculate, Policy = Constant.Policy.Host)]
        [HttpPost]
        public ActionResult UnCalculate(DoSingleTicketMetaModel model)
        {
            var result = CalculateSingle(model);
            return this.Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Refund
        /// This function also include RefundHalfTime, RefundFullTime and RefundAll.
        /// </summary>
        /// <param name="criteria">The value.</param>
        /// <returns>Return result.</returns>
        [NapAuth(FunctionKey = SettlementFunctionKeys.SportsbookRefund, Policy = Constant.Policy.Host)]
        [HttpPost]
        public ActionResult Refund(DoSingleTicketMetaModel criteria)
        {
            var result = new Result();
            criteria.AdminId = _currentSession.UserId;
            result = _calculateService.Refund(criteria) as Result;
            if (result.IsSuccessful)
            {
                var adminService = new AutoSettlementService();
                result = adminService.ClearFailActionAutoSettlement(_currentSession.UserId, criteria.MatchId, string.Empty) as Result;
            }

            return this.Json(result, JsonRequestBehavior.AllowGet);
        }

        #region Calculate Mix Parlay

        /// <summary>
        /// Calculate Mix Parlay.
        /// </summary>
        /// <param name="criteria">Model Multiple.</param>
        /// <returns>Return result.</returns>
        [NapAuth(FunctionKey = SettlementFunctionKeys.SportsbookCalculateMixParlay, Policy = Constant.Policy.Host)]
        [HttpPost]
        public ActionResult CalculateMixParlay(DoMultipleTicketMetaModel criteria)
        {
            criteria.ActionType = CalculationType.CalculateParlay;
            var result = CalculateParlay(criteria);
            return this.Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion Calculate Mix Parlay

        #region Refund Parlay

        /// <summary>
        ///     Refund Parlay.
        /// </summary>
        /// <param name="criteria">Meta Model.</param>
        /// <returns>View result.</returns>
        [NapAuth(FunctionKey = SettlementFunctionKeys.SportsbookRefund, Policy = Constant.Policy.Host)]
        [HttpPost]
        public ActionResult RefundParlay(DoMultipleTicketMetaModel criteria)
        {
            var result = CalculateParlay(criteria);
            return this.Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Refund Parlay.
        /// </summary>
        /// <param name="criteria">The value.</param>
        /// <returns>Return result.</returns>
        [NonAction]
        private Result CalculateParlay(DoMultipleTicketMetaModel criteria)
        {
            var result = new Result();
            criteria.AdminId = _currentSession.UserId;
            result = _calculateService.CalculateMultiple(criteria) as Result;

            if (result.IsSuccessful)
            {
                var adminService = new AutoSettlementService();
                result = adminService.ClearFailActionAutoSettlement(_currentSession.UserId, 0, criteria.MatchIds) as Result;
            }

            return result;
        }

        #endregion

        #region Helper

        /// <summary>
        /// Calculate command with detail value.
        /// </summary>
        /// <param name="criteria">Single ticket Model.</param>
        /// <returns>Return result.</returns>
        private IResult CalculateUpdate(DoSingleTicketMetaModel criteria)
        {
            var isHalfTimeCalculate = false;
            string action = null;
            switch (criteria.ActionType.ToLower())
            {
                case CalculationType.CalculateHalfTime:
                    isHalfTimeCalculate = true;
                    action = CalculateAction.CalculateHalfTime;
                    break;

                case CalculationType.CalculateFullTime:
                    action = CalculateAction.CalculateFullTime;
                    criteria.FinalHomeScore = criteria.FinalHomeScore ?? 0;
                    criteria.FinalAwayScore = criteria.FinalAwayScore ?? 0;
                    break;

                case CalculationType.CalculateAll:
                    action = CalculateAction.CalculateAll;
                    criteria.FinalHomeScore = criteria.FinalHomeScore ?? 0;
                    criteria.FinalAwayScore = criteria.FinalAwayScore ?? 0;
                    break;

                case CalculationType.Uncalculate:
                    criteria.IsScoreDirect = false;
                    action = CalculateAction.Uncalculate;
                    break;

                case CalculationType.CalculateFgng:
                    action = (!string.IsNullOrWhiteSpace(criteria.GoalRecord) && criteria.GoalRecord.IndexOf(";", StringComparison.Ordinal) > -1)
                              ? CalculateAction.CalculateFullTime
                              : CalculateAction.CalculateHalfTime;
                    break;
            }

            criteria.AdminId = _currentSession.UserId;
            CalculationHelper.CalculateScoreAndBetStatus(criteria, isHalfTimeCalculate);
            return _calculateService.Calculate(criteria, action, isHalfTimeCalculate);
        }

        private SettlementPermission SetCalcPopupPermission()
        {
            return new SettlementPermission
            {
                SportsBook = new SportsBookPermission
                {
                    CanUncalculate = HasPermission(_currentSession.UserId, SettlementFunctionKeys.SportsbookUncalculate, Constant.Policy.Host),
                    CanCalculate = HasPermission(_currentSession.UserId, SettlementFunctionKeys.SportsbookCalculate, Constant.Policy.Read)
                }
            };
        }

        /// <summary>
        ///     Check if match is ready for calculation.
        ///     This function is used to replace [NAP_Set_MatchCheckForBill].
        /// </summary>
        /// <param name="match">Match detail.</param>
        /// <returns>true or false.</returns>
        private bool IsMatchReadyForCalc(MatchDetail match)
        {
            var soccerHtMinute = 45;
            var basketBallMinute = 30;
            var hockeyMinute = 60;
            var tennisMinute = 30;
            var sportType = match.SportType;

            if (match.ConfirmFt > 0 || match.ConfirmHt > 0)
            {
                return true;
            }

            // Bill soccer HT
            if (sportType == SportType.Soccer || sportType == SportType.VirtualSport)
            {
                return match.PlayTime > soccerHtMinute;
            }

            // Bill Basket, Football HT
            if (sportType == SportType.BasketBall || sportType == SportType.FootBall || sportType == SportType.Rugby || sportType == SportType.NetBall)
            {
                return match.PlayTime > basketBallMinute;
            }

            if (sportType == SportType.IceHockey || sportType == SportType.VolleyBall || sportType == SportType.BaseBall)
            {
                return match.PlayTime >= hockeyMinute;
            }

            if (TennisGroupSportId.Contains(sportType))
            {
                return match.PlayTime > tennisMinute;
            }

            return true;
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

        /// <summary>
        /// Calculate single.
        /// </summary>
        /// <param name="model">Model value.</param>
        /// <returns>Return result.</returns>
        [NonAction]
        private Result CalculateSingle(DoSingleTicketMetaModel model)
        {
            model.AdminId = _currentSession.UserId;
            var result = this.CalculateUpdate(model) as Result;
            if (result.IsSuccessful)
            {
                var adminService = new AutoSettlementService();
                result = adminService.ClearFailActionAutoSettlement(_currentSession.UserId, model.MatchId, string.Empty) as Result;
            }

            return result;
        }

        #endregion
    }
}