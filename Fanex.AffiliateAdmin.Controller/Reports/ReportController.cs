// --------------------------------------------------------------------------
// <copyright file="ReportController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2013.10.15">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Controller
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.Script.Serialization;
    using System.Web.SessionState;
    using Fanex.AffiliateAdmin.Core.Configurations;
    using Fanex.AffiliateAdmin.Core.Filter;
    using Fanex.AffiliateAdmin.Core.Session;
    using Fanex.AffiliateAdmin.Core.Utilities;
    using Fanex.AffiliateAdmin.Criterias;
    using Fanex.AffiliateAdmin.Models;
    using Fanex.AffiliateAdmin.Resources;
    using Fanex.AffiliateAdmin.Services;

    /// <summary>
    /// Report Controller.
    /// </summary>
    [SessionState(SessionStateBehavior.ReadOnly)]
    [FunctionAuth(Function = FunctionPolicyNames.AffReport, CurrenciesRequired = true)]
    public class ReportController : BaseController
    {
        private ICommGroupService _commGroupService;

        private IReportService _reportService;

        public ReportController()
            : this(null, null)
        {
        }

        public ReportController(IReportService reportService, ICommGroupService commGroupService)
        {
            _reportService = reportService ?? new ReportService();
            _commGroupService = commGroupService ?? new CommGroupService();
        }

        public ICommGroupService CommGroupService
        {
            get { return _commGroupService; }
        }

        public IReportService ReportService
        {
            get { return _reportService; }
        }

        #region Company Win Loss Report.

        /// <summary>
        /// Company Win Loss Report.
        /// </summary>
        /// <returns>Company win loss page.</returns>
        [HttpGet]
        public ActionResult CompanyWinloss()
        {
            var serializer = new JavaScriptSerializer();

            ViewBag.Products = serializer.Serialize(AppSettings.Products.Select(x => new
            {
                Id = x.ProductID,
                Name = x.ProductName
            }));

            return View(new CompanyWinlossViewModel());
        }

        /// <summary>
        /// Company Win loss Report for specific affiliate.
        /// </summary>
        /// <param name="model">Company View Model.</param>
        /// <returns>Company win loss page.</returns>
        [HttpGet]
        public ActionResult CompanyWinlossByAffiliate(CompanyWLByAffiliateViewModel model)
        {
            var serializer = new JavaScriptSerializer();

            ViewBag.Products = serializer.Serialize(AppSettings.Products.Select(x => new
            {
                Id = x.ProductID,
                Name = x.ProductName
            }));

            ViewBag.AffiliateCurrencySymbol = FormaterCurrency.GetCurrencySymbol(CurrentSession.Affiliate.Currency);

            var baseCurrencyList = GetBaseCurrencyList();

            model.AddBaseCurrencyList(baseCurrencyList);

            return View("CompanyWinlossByAffiliate", model);
        }

        /// <summary>
        /// Index Page.
        /// </summary>
        /// <returns>Redirect to another action.</returns>
        public ActionResult Index()
        {
            return RedirectToAction("CompanyWinloss");
        }

        #endregion Company Win Loss Report.

        #region Member Win Loss Report

        /// <summary>
        /// Member Win loss Report.
        /// </summary>
        /// <param name="model">Model input.</param>
        /// <returns>Member Win loss page.</returns>
        [HttpGet]
        public ActionResult MemberWinloss(MemberWinlossViewModel model)
        {
            var serializer = new JavaScriptSerializer();

            ViewBag.Products = serializer.Serialize(AppSettings.Products.Select(x => new
            {
                Id = x.ProductID,
                Name = x.ProductName
            }));

            return View("MemberWinloss", model);
        }

        /// <summary>
        /// Member Win loss Report for specific affiliate.
        /// </summary>
        /// <param name="model">Model input.</param>
        /// <returns>Member win loss page.</returns>
        [HttpGet]
        public ActionResult MemberWinlossByAffiliate(MemberWinlossByAffiliateViewModel model)
        {
            var baseCurrencyList = GetBaseCurrencyList();

            model.AddBaseCurrencyList(baseCurrencyList);

            var serializer = new JavaScriptSerializer();

            ViewBag.Products = serializer.Serialize(AppSettings.Products.Select(x => new
            {
                Id = x.ProductID,
                Name = x.ProductName
            }));

            ViewBag.AffiliateCurrencySymbol = FormaterCurrency.GetCurrencySymbol(CurrentSession.Affiliate.Currency);

            return View("MemberWinlossByAffiliate", model);
        }

        #endregion Member Win Loss Report

        #region Member Statistics

        [HttpGet]
        public ActionResult MemberStatistics()
        {
            return View(new MemberStatisticsViewModel());
        }

        /// <summary>
        /// Get data for report.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>Report Data result.</returns>
        [HttpPost]
        public ActionResult MemberStatistics(MemberStatisticReportCriteria criteria)
        {
            criteria.UserId = CurrentSession.CustID;
            criteria.IsITAdmin = CurrentSession.IsIT;
            criteria.LicenseeId = AppSettings.LicenseeID;

            if (criteria.GroupBy == 2)
            {
                // If group by is month, we need to first day and last day of month.
                criteria.FromDate = Common.GetFirstDayOfMonth(criteria.FromDate, AppSettings.SystemStartDate);
                criteria.ToDate = Common.GetLastDayOfMonth(criteria.ToDate, SystemTimeService.GetSystemDateTime());
            }

            var data = _reportService.GetMemberStatistics(criteria).ToList();

            return Json(new
            {
                List = data,
                CurrentPage = 1,
                ItemsPerPage = data.Count,
                TotalItem = data.Count,
                GroupBy = criteria.GroupBy.ToString(),
                SubTotal = new
                {
                    TotalClick = data.Sum(x => x.TotalClick),
                    UniqueClick = data.Sum(x => x.UniqueClick),
                    Signups = data.Sum(x => x.Signups),
                    FDM = data.Sum(x => x.FDM),
                    NonDeposit = data.Sum(x => x.NonDeposit),
                    ActivePlayers = data.Sum(x => x.ActivePlayers)
                }
            });
        }

        #endregion Member Statistics

        #region Affiliate Win Loss Report

        /// <summary>
        /// Commission Report.
        /// </summary>
        /// <param name="model">Input criteria.</param>
        /// <returns>Commission Report Page.</returns>
        [HttpGet]
        public ActionResult AffiliateWinLoss(AffiliateWinLossViewModel model)
        {
            var serializer = new JavaScriptSerializer();

            ViewBag.Products = serializer.Serialize(AppSettings.Products.Select(x => new
            {
                Id = x.ProductID,
                Name = x.ProductName
            }));

            ViewBag.HasCommGroupPolicy = SessionManager.CurrentSession.HasReadPolicy(FunctionPolicyNames.CommissionGroup);
            return View(model);
        }

        /// <summary>
        /// Commission By Affiliate.
        /// </summary>
        /// <param name="model">Model Input.</param>
        /// <returns>Commission By Affiliate Result.</returns>
        [HttpGet]
        public ActionResult AffiliateWinLossByAffiliate(AffiliateWLByAffiliateViewModel model)
        {
            var serializer = new JavaScriptSerializer();

            ViewBag.Products = serializer.Serialize(AppSettings.Products.Select(x => new
            {
                Id = x.ProductID,
                Name = x.ProductName
            }));

            ViewBag.HasCashbackDeduction = AppSettings.DeductionTypes.Any(x => x.ProductID == 0);

            var baseCurrencyList = new List<SelectListItem>
            {
                new SelectListItem { Value = "0",  Text = CurrentSession.Affiliate.Currency }
            };

            model.AddBaseCurrencyList(baseCurrencyList);

            return View(model);
        }

        /// <summary>
        /// Qualified Members.
        /// </summary>
        /// <param name="criteria">Criteria input.</param>
        /// <returns>Action Result.</returns>
        public ActionResult QualifiedMembers(QualifiedMembersReportCriteria criteria)
        {
            var products = GetValidProducts(criteria.GroupId, criteria.MonthDate, criteria.Products);
            criteria.UserId = CurrentSession.CustID;
            criteria.Products = string.Join(",", products);
            criteria.LicenseeId = AppSettings.LicenseeID;
            var members = _reportService.GetQualifiedMembers(criteria);

            ViewBag.SymbolCurrency = FormaterCurrency.GetCurrencySymbol(criteria.Currency);
            ViewBag.BaseCurrency = criteria.BaseCurrency;
            ViewBag.Products = products;

            return View(members);
        }

        #endregion Affiliate Win Loss Report

        #region Affiliate Commission Report

        /// <summary>
        /// Processing Commission.
        /// </summary>
        /// <returns>Processing Commission Page.</returns>
        public ActionResult AffiliateCommission()
        {
            var serializer = new JavaScriptSerializer();

            ViewBag.Products = serializer.Serialize(AppSettings.Products.Select(x => new
            {
                Id = x.ProductID,
                Name = x.ProductName
            }));

            return View(new AffiliateCommissionViewModel());
        }

        /// <summary>
        /// Commission By Affiliate.
        /// </summary>
        /// <param name="model">Model Input.</param>
        /// <returns>Commission By Affiliate Result.</returns>
        [HttpGet]
        public ActionResult AffiliateCommissionByAffiliate(AffiliateCommissionByAffiliateViewModel model)
        {
            var serializer = new JavaScriptSerializer();

            ViewBag.Products = serializer.Serialize(AppSettings.Products.Select(x => new
            {
                Id = x.ProductID,
                Name = x.ProductName
            }));

            ViewBag.HasCashbackDeduction = AppSettings.DeductionTypes.Any(x => x.ProductID == 0);

            var baseCurrencyList = new List<SelectListItem>
            {
                new SelectListItem { Value = "0",  Text = CurrentSession.Affiliate.Currency }
            };

            model.AddBaseCurrencyList(baseCurrencyList);

            return View(model);
        }

        #endregion Affiliate Commission Report

        #region Helpers

        /// <summary>
        /// Get base currency list.
        /// </summary>
        /// <returns>Base currency list.</returns>
        private List<SelectListItem> GetBaseCurrencyList()
        {
            List<SelectListItem> baseCurrencyList;

            if (CurrentSession.Affiliate.Currency.Equals(AppSettings.BaseExchange.Code))
            {
                baseCurrencyList = new List<SelectListItem>
                    {
                        new SelectListItem { Value = "0", Text = Lang.Member },
                        new SelectListItem { Value = "1", Text = AppSettings.BaseExchange.Code },
                    };
            }
            else
            {
                baseCurrencyList = new List<SelectListItem>
                    {
                        new SelectListItem { Value = "0", Text = Lang.Member },
                        new SelectListItem { Value = "1", Text = AppSettings.BaseExchange.Code },
                        new SelectListItem { Value = "2", Text = CurrentSession.Affiliate.Currency }
                    };
            }

            return baseCurrencyList;
        }

        /// <summary>
        /// Get valid products.
        /// </summary>
        /// <param name="groupId">Commission Group.</param>
        /// <param name="month">Selected Month.</param>
        /// <param name="selectedProducts">Selected Products.</param>
        /// <returns>Valid Product List.</returns>
        private IEnumerable<byte> GetValidProducts(int groupId, DateTime month, string selectedProducts)
        {
            var selectedProductList = string.IsNullOrEmpty(selectedProducts)
                                  ? new List<byte>()
                                  : selectedProducts.Split(',').Select(byte.Parse);

            if (groupId == 0)
            {
                return selectedProductList.ToList();
            }

            var detailCriteria = new TierSettingCriteria { GroupIDList = groupId.ToString(CultureInfo.InvariantCulture), Date = month };
            var productListInCommGroup = _commGroupService.GetTierSettings(detailCriteria).Select(t => t.ProductID).Distinct();

            return selectedProductList.Intersect(productListInCommGroup).ToList();
        }

        #endregion Helpers
    }
}