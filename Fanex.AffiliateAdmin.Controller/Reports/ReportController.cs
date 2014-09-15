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
    using System.Reflection;
    using System.Web.Mvc;
    using System.Web.Script.Serialization;
    using Core.Filter;
    using Criterias;
    using Fanex.AffiliateAdmin.Core.Configurations;
    using Fanex.AffiliateAdmin.Core.Utilities;
    using Fanex.AffiliateAdmin.DTO;
    using Fanex.AffiliateAdmin.Resources;
    using Fanex.AffiliateAdmin.Services;
    using Models;

    /// <summary>
    /// Report Controller.
    /// </summary>
    [FunctionAuth(Function = "[AffReport]")]
    public class ReportController : BaseController
    {
        protected ICommGroupService _commGroupService;

        protected IReportService _reportService;

        public ReportController(IReportService reportService, ICommGroupService commGroupService)
        {
            _reportService = reportService;
            _commGroupService = commGroupService;
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
        /// <returns>Company win loss page.</returns>
        [HttpGet]
        public ActionResult CompanyWinlossByAffiliate()
        {
            var serializer = new JavaScriptSerializer();

            ViewBag.Products = serializer.Serialize(AppSettings.Products.Select(x => new
            {
                Id = x.ProductID,
                Name = x.ProductName
            }));

            ViewBag.AffiliateCurrencySymbol = FormaterCurrency.GetCurrencySymbol(CurrentSession.Affiliate.Currency);

            var model = new CompanyWLByAffiliateViewModel();
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
        /// Member win loss action.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>Member win loss result.</returns>
        [HttpPost]
        public ActionResult MemberWinloss(MemberWinlossReportCriteria criteria)
        {
            if (criteria.ByAffiliate)
            {
                criteria.AffName = CurrentSession.Affiliate.Username;
                criteria.ExcludeTest = false;
                criteria.AffTypes = string.Join(",", AppSettings.AffTypes.Select(t => t.TypeID));
                criteria.CommGroups = string.Join(",", AppSettings.GetCommGroups(null).Select(t => t.GroupID));
                criteria.PreferLanguages = string.Join(",", AppSettings.AffSupportLanguages.Select(t => t.LanguageID));
            }

            criteria.UserId = CurrentSession.CustID;
            criteria.IsITAdmin = CurrentSession.IsIT;

            var data = _reportService.GetMemberWinloss(criteria);

            var hasCurrencyOverTwoRows = data.MemberWinlossList.GroupBy(t => t.PlayerCurrency)
                                           .Select(t => new { PlayerCurrency = t.Key, RowCount = t.Count() })
                                           .Any(t => t.RowCount > 1);

            return Json(new
            {
                List = data.MemberWinlossList,
                TotalRowData = data.TotalMemberWinlossInSystemCurrency,
                SubTotalData = data.TotalMemberWinlossInMemberCurrencies,
                HasCurrencyOverTwoRows = hasCurrencyOverTwoRows,
                CurrentPage = criteria.PageIndex - 1,
                ItemsPerPage = criteria.PageSize,
                TotalItem = criteria.NRows,
                DurationDB = criteria.Duration,
                DurationFanexData = data.Duration
            });
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
            return View();
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

            if (criteria.GroupBy == 2)
            {
                // If group by is month, we need to first day and last day of month.
                criteria.FromDate = Core.Utilities.Common.GetFirstDayOfMonth(criteria.FromDate, AppSettings.SystemStartDate, DateTime.Now);
                criteria.ToDate = Core.Utilities.Common.GetLastDayOfMonth(criteria.ToDate, AppSettings.SystemStartDate, DateTime.Now);
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
        /// <returns>Commission Report Page.</returns>
        [HttpGet]
        public ActionResult AffiliateWinLoss()
        {
            var serializer = new JavaScriptSerializer();
            ViewBag.Products = serializer.Serialize(AppSettings.Products.Select(x => new
            {
                Id = x.ProductID,
                Name = x.ProductName
            }));

            ViewBag.HasCashbackDeduction = AppSettings.DeductionTypes.Any(x => x.ProductID == 0);

            return View(new AffiliateWinLossViewModel());
        }

        /// <summary>
        /// Get data for report.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>Report data.</returns>
        [HttpPost]
        public ActionResult AffiliateWinLoss(AffiliateWinLossCriteria criteria)
        {
            criteria.UserId = CurrentSession.CustID;
            criteria.IsITAdmin = CurrentSession.IsIT;
            var data = _reportService.GetAffiliateWinLoss(criteria);

            var totalCommission = data.CommissionInSystemCurrency;
            totalCommission.Currency = AppSettings.BaseExchange.Code;

            var hasShowSubTotalRow = data.CommissionList.GroupBy(t => t.Currency)
                                           .Select(t => new { Currency = t.Key, RowCount = t.Count() })
                                           .Any(t => t.RowCount > 1);

            return Json(new
            {
                List = data.CommissionList,
                TotalRowData = totalCommission,
                SubTotalData = data.CommissionInAffiliateCurrency,
                HasShowSubTotalRow = hasShowSubTotalRow,
                CurrentPage = criteria.PageIndex - 1,
                ItemsPerPage = criteria.PageSize,
                TotalItem = criteria.NRows
            });
        }

        /// <summary>
        /// Commission By Affiliate.
        /// </summary>
        /// <param name="model">Model Input.</param>
        /// <returns>Commission By Affiliate Result.</returns>
        [HttpGet]
        public ActionResult AffiliateWinLossByAffiliate(AffiliateWinLossViewModel model)
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
                new SelectListItem { Value = "0",  Text = CurrentSession.Affiliate.Currency },
                new SelectListItem { Value = "1",  Text = AppSettings.BaseExchange.Code },
            };

            model.AddBaseCurrencyList(baseCurrencyList);

            return View(model);
        }

        /// <summary>
        /// Affiliate Win Loss By Affiliate.
        /// </summary>
        /// <param name="criteria">Affiliate Win Loss Criteria.</param>
        /// <returns>Action Result.</returns>
        [HttpPost]
        public ActionResult AffiliateWinLossByAffiliate(AffiliateWinLossCriteria criteria)
        {
            const int GroupByMonth = 1;
            criteria.AffName = CurrentSession.Affiliate.Username;
            criteria.AffTypes = string.Join(",", AppSettings.AffTypes.Select(t => t.TypeID));
            criteria.CommGroups = string.Join(",", AppSettings.GetCommGroups(null).Select(t => t.GroupID));
            criteria.PreferLanguages = string.Join(",", AppSettings.AffSupportLanguages.Select(t => t.LanguageID));
            criteria.Currencies = CurrentSession.Affiliate.CurrencyID.ToString();
            criteria.GroupBy = GroupByMonth;

            return AffiliateWinLoss(criteria);
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
        /// Processing Commission.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>Processing Commission Result.</returns>
        [HttpPost]
        public ActionResult AffiliateCommission(AffiliateCommissionCriteria criteria)
        {
            criteria.UserId = CurrentSession.CustID;
            criteria.IsITAdmin = CurrentSession.IsIT;

            var data = _reportService.GetAffiliateCommission(criteria);

            return Json(new
            {
                List = data,
                ItemsPerPage = data.Count(),
                TotalItem = data.Count(),
                HasCashbackDeduction = AppSettings.DeductionTypes.Any(x => x.ProductID == 0)
            });
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