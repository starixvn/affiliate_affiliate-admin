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
        private readonly ICommGroupService _commGroupService;

        private readonly IReportService _reportService;

        public ReportController(IReportService reportService, ICommGroupService commGroupService)
        {
            _reportService = reportService;
            _commGroupService = commGroupService;
        }

        #region Company Win Loss Report

        /// <summary>
        /// Company Win loss Report.
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
        /// Company win loss action.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>Company win loss result.</returns>
        [HttpPost]
        public ActionResult CompanyWinloss(CompanyWinlossReportCriteria criteria)
        {
            AddMoreParameterForCompanyWinlossReportCriteria(criteria);
            
            var data = _reportService.GetCompanyWinloss(criteria);

            var serializer = new JavaScriptSerializer();

            ViewBag.Products = serializer.Serialize(AppSettings.Products.Select(x => new
            {
                Id = x.ProductID,
                Name = x.ProductName
            }));

            var companyWinlossByGroups = FilterGroupDataCompanyWinlossByOverview(criteria, data.CompanyWinlossList);

            ////var companyWinlossByProduct = FilterGroupDataCompanyWinlossByProduct(criteria, data.CompanyWinlossList);

            var hasShowSubTotalRow = data.CompanyWinlossList.GroupBy(t => t.Currency)
                                         .Select(t => new { Currency = t.Key, RowCount = t.Count() })
                                         .Any(t => t.RowCount > 1);

            return Json(new
            {
                Groups = companyWinlossByGroups,
                TotalData = new CompanyWinlossReportByOverview
                {
                    GroupInfo = Lang.TotalAll,
                    Level = "All",
                    CompanyWinlossInMemberCurrencies = data.TotalCompanyWinlossInMemberCurrencies,
                    CompanyWinlossInSystemCurrency = data.TotalCompanyWinlossInSystemCurrency
                },
                HasShowSubTotalRow = hasShowSubTotalRow,
                CurrentPage = criteria.PageIndex - 1,
                ItemsPerPage = criteria.PageSize,
                TotalItem = criteria.NRows,
                DurationDB = criteria.Duration,
                DurationFanexData = data.Duration
            });
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

        #endregion Company Winloss Report

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

        #endregion Member Winloss Report

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

        #endregion Commission Report

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

        #endregion

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

        #region Company win-loss report

        /// <summary>
        /// Add more parameter for company win-loss report criteria.
        /// </summary>
        /// <param name="criteria">Criteria input.</param>
        private void AddMoreParameterForCompanyWinlossReportCriteria(CompanyWinlossReportCriteria criteria)
        {
            if (criteria.ByAffiliate)
            {
                criteria.AffName = CurrentSession.Affiliate.Username;
                criteria.AffTypes = string.Join(",", AppSettings.AffTypes.Select(t => t.TypeID));
                criteria.CommGroups = string.Join(",", AppSettings.GetCommGroups(null).Select(t => t.GroupID));
                criteria.PreferLanguages = string.Join(",", AppSettings.AffSupportLanguages.Select(t => t.LanguageID));
                criteria.ExcludeTest = false;
            }

            if (criteria.GroupBy == 1)
            {
                // If group by is month, we need to first day and last day of month.
                criteria.FromDate = Common.GetFirstDayOfMonth(criteria.FromDate, AppSettings.SystemStartDate, DateTime.Now);
                criteria.ToDate = Common.GetLastDayOfMonth(criteria.ToDate, AppSettings.SystemStartDate, DateTime.Now);
            }

            criteria.UserId = CurrentSession.CustID;
            criteria.IsITAdmin = CurrentSession.IsIT;
        }

        /// <summary>
        /// Filter Group Data Company Win loss based Group By criteria.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <param name="companyWinlosses">Company win loss List.</param>
        /// <returns>Company Win loss Group List.</returns>
        private List<CompanyWinlossReportByOverview> FilterGroupDataCompanyWinlossByOverview(
            CompanyWinlossReportCriteria criteria, 
            IEnumerable<CompanyWinloss> companyWinlosses)
        {
            var companyWinlossByGroups = new List<CompanyWinlossReportByOverview>();

            GroupDataForCompanyWinlossByOverview(companyWinlosses.ToList(), companyWinlossByGroups);

            if (criteria.GroupBy == 0)
            {
                companyWinlossByGroups = companyWinlossByGroups.Where(t => !t.Level.Equals("Year")).ToList();

                if (companyWinlossByGroups.Count(t => t.Level.Equals("Month")) == 1)
                {
                    companyWinlossByGroups = companyWinlossByGroups.Where(t => !t.Level.Equals("Month")).ToList();
                }
            }
            else
            {
                companyWinlossByGroups = companyWinlossByGroups.Where(t => !t.Level.Equals("Date")).ToList();

                if (companyWinlossByGroups.Count(t => t.Level.Equals("Year")) == 1)
                {
                    companyWinlossByGroups = companyWinlossByGroups.Where(t => !t.Level.Equals("Year")).ToList();
                }
            }

            return companyWinlossByGroups;
        }

        /// <summary>
        /// Filter Group Data Company Win Loss By Product.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <param name="companyWinlosses">Company win loss.</param>
        /// <returns>Company Win Loss By Product.</returns>
        private List<CompanyWinlossReportByProduct> FilterGroupDataCompanyWinlossByProduct(
            CompanyWinlossReportCriteria criteria, IEnumerable<CompanyWinloss> companyWinlosses)
        {
            var companyWinlossByProducts = new List<CompanyWinlossReportByProduct>();

            GroupDataForCompanyWinlossByProduct(companyWinlosses.ToList(), companyWinlossByProducts);

            if (criteria.GroupBy == 0)
            {
                companyWinlossByProducts = companyWinlossByProducts.Where(t => !t.Level.Equals("Year")).ToList();

                if (companyWinlossByProducts.Count(t => t.Level.Equals("Month")) == 1)
                {
                    companyWinlossByProducts = companyWinlossByProducts.Where(t => !t.Level.Equals("Month")).ToList();
                }
            }
            else
            {
                companyWinlossByProducts = companyWinlossByProducts.Where(t => !t.Level.Equals("Date")).ToList();

                if (companyWinlossByProducts.Count(t => t.Level.Equals("Year")) == 1)
                {
                    companyWinlossByProducts = companyWinlossByProducts.Where(t => !t.Level.Equals("Year")).ToList();
                }
            }

            return companyWinlossByProducts;
        }

        /// <summary>
        /// Get Company Win loss In Member Currencies.
        /// </summary>
        /// <param name="companyWinlosses">Company Win loss List.</param>
        /// <param name="properties">Property List.</param>
        /// <returns>Company Win loss List in Member Currencies.</returns>
        private IList<CompanyWinloss> GetCompanyWinlossInMemberCurrencies(IEnumerable<CompanyWinloss> companyWinlosses, IEnumerable<PropertyInfo> properties)
        {
            var companyWinlossInCurrenciesList = new List<CompanyWinloss>();

            var groupByCurrencies = companyWinlosses.GroupBy(t => t.Currency).ToList();

            foreach (var groupByCurrency in groupByCurrencies)
            {
                var companlyWinloss = GetTotalCompanyWinloss(groupByCurrency.ToList(), properties);

                companlyWinloss.Currency = groupByCurrency.Key;

                companyWinlossInCurrenciesList.Add(companlyWinloss);
            }

            return companyWinlossInCurrenciesList;
        }

        /// <summary>
        /// Get Company Win Loss By Currency In Member Currencies.
        /// </summary>
        /// <param name="companyWinlosses">Company Win Loss.</param>
        /// <param name="properties">Properties input.</param>
        /// <returns>Company Win Loss By Currency List.</returns>
        private IList<CompanyWinlossByCurrency> GetCompanyWinlossByCurrencyInMemberCurrencies(
            IEnumerable<CompanyWinloss> companyWinlosses,
            IEnumerable<PropertyInfo> properties)
        {
            var companyWinlossInProductsList = new List<CompanyWinlossByCurrency>();

            var groupByCurrencies = companyWinlosses.GroupBy(t => t.Currency).ToList();

            foreach (var groupByCurrency in groupByCurrencies)
            {
                var groupByProducts = groupByCurrency.GroupBy(t => t.ProductName);

                var companyWinlossByCurrency = new CompanyWinlossByCurrency { Currency = groupByCurrency.Key };

                var list = groupByProducts.Select(groupByProduct => GetTotalCompanyWinloss(groupByProduct.ToList(), properties)).ToList();

                companyWinlossByCurrency.CompanyWinlossByProductList = list;

                companyWinlossByCurrency.TotalCompanyWinlossByProduct = GetTotalCompanyWinloss(list, properties);

                companyWinlossInProductsList.Add(companyWinlossByCurrency);
            }

            return companyWinlossInProductsList;
        }

        /// <summary>
        /// Get Total Company Win loss In System Currency.
        /// </summary>
        /// <param name="companyWinlosses">Company Win loss List.</param>
        /// <param name="properties">Property List.</param>
        /// <returns>Company Win loss In System Currency.</returns>
        private CompanyWinloss GetTotalCompanyWinloss(IEnumerable<CompanyWinloss> companyWinlosses, IEnumerable<PropertyInfo> properties)
        {
            var totalCompanlyWinloss = new CompanyWinloss();

            // Sub total by date, in system currency.
            foreach (var property in properties)
            {
                if (property.Name.EndsWith("ActivePlayers", StringComparison.OrdinalIgnoreCase) || property.Name.EndsWith("BetCount", StringComparison.OrdinalIgnoreCase))
                {
                    var value1 = companyWinlosses.Sum(g =>
                        {
                            var changeType = Convert.ChangeType(property.GetValue(g, null).ToString(), TypeCode.Int32);
                            return changeType != null ? (int)changeType : 0;
                        });

                    if (property.CanWrite)
                    {
                        property.SetValue(totalCompanlyWinloss, Convert.ChangeType(value1, property.PropertyType), null);
                    }

                    continue;
                }

                var value = companyWinlosses.Sum(g =>
                    {
                        var changeType = Convert.ChangeType(property.GetValue(g, null), TypeCode.Decimal);
                        return changeType != null ? (decimal)changeType : 0;
                    });

                if (property.CanWrite)
                {
                    property.SetValue(totalCompanlyWinloss, Convert.ChangeType(value, property.PropertyType), null);
                }
            }

            return totalCompanlyWinloss;
        }

        /// <summary>
        /// Group data by date.
        /// </summary>
        /// <param name="companyWinlossByGroups">Company win-loss by level.</param>
        /// <param name="groupByMonth">Data group by month.</param>
        /// <param name="properties">Property list.</param>
        /// <param name="year">Year input.</param>
        private void GroupDataByDate(
            List<CompanyWinlossReportByOverview> companyWinlossByGroups,
            IGrouping<int, CompanyWinloss> groupByMonth,
            List<PropertyInfo> properties,
            int year)
        {
            var groupByDates = groupByMonth.GroupBy(t => t.WinlostDate.Day).ToList();

            foreach (var groupByDate in groupByDates)
            {
                companyWinlossByGroups.Add(new CompanyWinlossReportByOverview
                {
                    GroupInfo = string.Format("{0}/{1}/{2}", groupByMonth.Key, groupByDate.Key, year),
                    Level = "Date",
                    CompanyWinlossInMemberCurrencies = groupByDate.ToList(),
                    CompanyWinlossInSystemCurrency = GetTotalCompanyWinloss(groupByDate.ToList(), properties)
                });
            }
        }

        /// <summary>
        /// Group Data For Company Win loss.
        /// </summary>
        /// <param name="companyWinlosses">Company Win loss List.</param>
        /// <param name="companyWinlossByGroups">Company Win loss Groups.</param>
        private void GroupDataForCompanyWinlossByOverview(IEnumerable<CompanyWinloss> companyWinlosses, List<CompanyWinlossReportByOverview> companyWinlossByGroups)
        {
            var properties = typeof(CompanyWinloss).GetProperties().Where(p => IsPropertyAbleSumUp(p)).ToList();
            var groupByYears = companyWinlosses.GroupBy(t => t.WinlostDate.Year).ToList();

            foreach (var groupByYear in groupByYears)
            {
                var groupByMonths = groupByYear.GroupBy(t => t.WinlostDate.Month).ToList();

                foreach (var groupByMonth in groupByMonths)
                {
                    GroupDataByDate(companyWinlossByGroups, groupByMonth, properties, groupByYear.Key);

                    var companyWlInMemberCurrenciesByMonth = GetCompanyWinlossInMemberCurrencies(groupByMonth.ToList(), properties);

                    companyWinlossByGroups.Add(new CompanyWinlossReportByOverview
                        {
                            GroupInfo = string.Format("{0}/{1}/{2}", groupByMonth.Key, 1, groupByYear.Key),
                            Level = "Month",
                            CompanyWinlossInMemberCurrencies = companyWlInMemberCurrenciesByMonth,
                            CompanyWinlossInSystemCurrency = GetTotalCompanyWinloss(companyWlInMemberCurrenciesByMonth, properties)
                        });
                }

                var companyWlInMemberCurrenciesByYear = GetCompanyWinlossInMemberCurrencies(groupByYear.ToList(), properties);

                companyWinlossByGroups.Add(new CompanyWinlossReportByOverview
                {
                    GroupInfo = string.Format("{0}/{1}/{2}", 1, 1, groupByYear.Key),
                    Level = "Year",
                    CompanyWinlossInMemberCurrencies = companyWlInMemberCurrenciesByYear,
                    CompanyWinlossInSystemCurrency = GetTotalCompanyWinloss(companyWlInMemberCurrenciesByYear, properties)
                });
            }
        }

        /// <summary>
        /// Group Data For Company Win Loss By Product.
        /// </summary>
        /// <param name="companyWinlosses">Company Win Loss.</param>
        /// <param name="companyWinlossByProducts">Company Win Loss By Products.</param>
        private void GroupDataForCompanyWinlossByProduct(
            IEnumerable<CompanyWinloss> companyWinlosses,
            List<CompanyWinlossReportByProduct> companyWinlossByProducts)
        {
            var properties = typeof(CompanyWinloss).GetProperties().Where(p => IsPropertyAbleSumUp(p)).ToList();
            var groupByYears = companyWinlosses.GroupBy(t => t.WinlostDate.Year).ToList();

            foreach (var groupByYear in groupByYears)
            {
                var groupByMonths = groupByYear.GroupBy(t => t.WinlostDate.Month).ToList();

                foreach (var groupByMonth in groupByMonths)
                {
                    GroupDataByDate(companyWinlossByProducts, groupByMonth, properties, groupByYear.Key);

                    var companyWinlossByCurrencyInMemberCurrencies = GetCompanyWinlossByCurrencyInMemberCurrencies(groupByMonth.ToList(), properties);

                    companyWinlossByProducts.Add(new CompanyWinlossReportByProduct
                    {
                        GroupInfo = string.Format("{0}/{1}/{2}", groupByMonth.Key, 1, groupByYear.Key),
                        Level = "Month",
                        CompanyWinlossInMemberCurrencies = companyWinlossByCurrencyInMemberCurrencies,
                        CompanyWinlossInSystemCurrency = GetTotalCompanyWinloss(companyWinlossByCurrencyInMemberCurrencies.Select(t => t.TotalCompanyWinlossByProduct), properties)
                    });
                }

                var companyWlInMemberCurrenciesByYear = GetCompanyWinlossByCurrencyInMemberCurrencies(groupByYear.ToList(), properties);

                companyWinlossByProducts.Add(new CompanyWinlossReportByProduct
                {
                    GroupInfo = string.Format("{0}/{1}/{2}", 1, 1, groupByYear.Key),
                    Level = "Year",
                    CompanyWinlossInMemberCurrencies = companyWlInMemberCurrenciesByYear,
                    CompanyWinlossInSystemCurrency = GetTotalCompanyWinloss(companyWlInMemberCurrenciesByYear.Select(t => t.TotalCompanyWinlossByProduct), properties)
                });
            }
        }

        /// <summary>
        /// Group Data By Date.
        /// </summary>
        /// <param name="companyWinlossByProducts">Company Win Loss By Products.</param>
        /// <param name="groupByMonth">Group by Month.</param>
        /// <param name="properties">Properties Input.</param>
        /// <param name="year">Year Input.</param>
        private void GroupDataByDate(
            List<CompanyWinlossReportByProduct> companyWinlossByProducts,
            IGrouping<int, CompanyWinloss> groupByMonth,
            List<PropertyInfo> properties,
            int year)
        {
            var groupByDates = groupByMonth.GroupBy(t => t.WinlostDate.Day).ToList();

            foreach (var groupByDate in groupByDates)
            {
                var group = new CompanyWinlossReportByProduct
                    {
                        Level = "Date",
                        GroupInfo = string.Format("{0}/{1}/{2}", groupByMonth.Key, groupByDate.Key, year),
                    };

                group.CompanyWinlossInMemberCurrencies = GroupDataByCurrency(groupByDate, properties);

                group.CompanyWinlossInSystemCurrency = 
                    GetTotalCompanyWinloss(group.CompanyWinlossInMemberCurrencies.Select(t => t.TotalCompanyWinlossByProduct).ToList(), properties);

                companyWinlossByProducts.Add(group);
            }
        }

        /// <summary>
        /// Group Data By Currency.
        /// </summary>
        /// <param name="groupByDate">Group by date.</param>
        /// <param name="properties">Properties input.</param>
        /// <returns>Company Win Loss By Currency.</returns>
        private IEnumerable<CompanyWinlossByCurrency> GroupDataByCurrency(
            IEnumerable<CompanyWinloss> groupByDate,
            List<PropertyInfo> properties)
        {
            var list = new List<CompanyWinlossByCurrency>();
            var groupByCurrencies = groupByDate.GroupBy(t => t.Currency);

            foreach (var groupByCurrency in groupByCurrencies)
            {
                var currency = new CompanyWinlossByCurrency
                    {
                        Currency = groupByCurrency.Key,
                        CompanyWinlossByProductList = groupByCurrency.ToList(),
                        TotalCompanyWinlossByProduct = GetTotalCompanyWinloss(groupByCurrency.ToList(), properties)
                    };

                list.Add(currency);
            }

            return list;
        }

        /// <summary>
        /// Check property can sum up or not.
        /// </summary>
        /// <param name="p">Property Input.</param>
        /// <returns>Can/Can not sum up.</returns>
        private bool IsPropertyAbleSumUp(PropertyInfo p)
        {
            var uncalculatedPropertyList = new List<string> { "ExRate", "AffExRate" };

            if (uncalculatedPropertyList.Any(t => t.Equals(p.Name, StringComparison.OrdinalIgnoreCase)))
            {
                return false;
            }

            var typeList = new List<Type> { typeof(int), typeof(decimal), typeof(double) };

            return typeList.Any(t => t == p.PropertyType);
        }

        #endregion Company win-loss report

        #endregion Helpers
    }
}