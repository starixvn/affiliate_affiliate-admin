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
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.Script.Serialization;
    using Core.Filter;
    using Criterias;
    using Fanex.AffiliateAdmin.Core.Configurations;
    using Fanex.AffiliateAdmin.Core.Services.Interface;
    using Fanex.AffiliateAdmin.Criterias.Configuration.Product;
    using Fanex.AffiliateAdmin.DTO;
    using Fanex.AffiliateAdmin.Resources;
    using Models;
    using Services.Interface;

    /// <summary>
    /// Report Controller.
    /// </summary>
    [HasCurrencyPermission]
    public class ReportController : BaseController
    {
        private readonly IReportService _reportService;

        private readonly ISystemSettingService _systemSettingService;

        public ReportController(IReportService reportService, ISystemSettingService systemSettingService)
        {
            _reportService = reportService;
            _systemSettingService = systemSettingService;
        }

        #region Company Winloss Report

        /// <summary>
        /// Index Page.
        /// </summary>
        /// <returns>Redirect to another action.</returns>
        public ActionResult Index()
        {
            return RedirectToAction("CompanyWinloss");
        }

        /// <summary>
        /// Company Win loss Report.
        /// </summary>
        /// <returns>Company win loss page.</returns>
        [HttpGet]
        public ActionResult CompanyWinloss()
        {
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
            if (criteria.ByAffiliate)
            {
                criteria.AffName = CurrentSession.Affiliate.UserName;
                criteria.AffTypes = string.Join(",", AppSettings.AffTypes.Select(t => t.TypeID));
                criteria.CommGroups = string.Join(",", AppSettings.GetCommGroups(null).Select(t => t.GroupID));
                criteria.PreferLanguages = string.Join(",", AppSettings.AffSupportLanguages.Select(t => t.LanguageID));
                criteria.ExcludeTest = false;
            }

            if (criteria.GroupBy == 1)
            {
                // If group by is month, we need to first day and last day of month.
                criteria.FromDate = Core.Utilities.Common.GetFirstDayOfMonth(criteria.FromDate, AppSettings.SystemStartDate, DateTime.Now);
                criteria.ToDate = Core.Utilities.Common.GetLastDayOfMonth(criteria.ToDate, AppSettings.SystemStartDate, DateTime.Now);
            }

            criteria.UserId = CurrentSession.CustID;
            criteria.IsITAdmin = CurrentSession.IsIT;

            var data = _reportService.GetCompanyWinloss(criteria).ToList();
            var groups = data.GroupBy(t => t.WinlostDate)
                              .Select(g => new
                                  {
                                      Name = g.Key,
                                      Items = g
                                  });

            var products = _systemSettingService.GetProductList(new ProductSelectCriteria()).OrderBy(t => t.ProductName);

            var serializer = new JavaScriptSerializer();
            ViewBag.Products = serializer.Serialize(AppSettings.Products.Select(x => new
            {
                Id = x.ProductID,
                Name = x.ProductName
            }));

            return Json(new
            {
                Groups = groups,
                Products = products.Select(t => t.ProductName.Replace(" ", string.Empty).ToLower()),
                ItemsPerPage = data.Count,
                TotalItem = data.Count,
                GroupBy = criteria.GroupBy.ToString(),
                BaseCurrency = criteria.BaseCurrency.ToString()
            });
        }

        /// <summary>
        /// Company Win loss Report for specific affiliate.
        /// </summary>
        /// <returns>Company win loss page.</returns>
        [HttpGet]
        public ActionResult CompanyWinlossByAffiliate()
        {
            var baseCurrencyList = new List<SelectListItem>();

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

            var model = new CompanyWLByAffiliateViewModel
                {
                    BaseCurrencyList = baseCurrencyList
                };

            return View("CompanyWinlossByAffiliate", model);
        }

        #endregion

        #region Player Winloss Report

        /// <summary>
        /// Player Win loss Report.
        /// </summary>
        /// <param name="model">Model input.</param>
        /// <returns>Player Win loss page.</returns>
        public ActionResult PlayerWinloss(PlayerWinlossViewModel model)
        {
            var serializer = new JavaScriptSerializer();

            ViewBag.Products = serializer.Serialize(AppSettings.Products.Select(x => new
            {
                Id = x.ProductID,
                Name = x.ProductName
            }));

            return View("PlayerWinloss", model);
        }

        /// <summary>
        /// Player win loss action.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>Player win loss result.</returns>
        [HttpPost]
        public ActionResult PlayerWinloss(PlayerWinlossReportCriteria criteria)
        {
            if (criteria.ByAffiliate)
            {
                criteria.AffName = CurrentSession.Affiliate.UserName;
                criteria.ExcludeTest = false;
                criteria.AffTypes = string.Join(",", AppSettings.AffTypes.Select(t => t.TypeID));
                criteria.CommGroups = string.Join(",", AppSettings.GetCommGroups(null).Select(t => t.GroupID));
                criteria.PreferLanguages = string.Join(",", AppSettings.AffSupportLanguages.Select(t => t.LanguageID));
            }

            criteria.UserId = CurrentSession.CustID;
            criteria.IsITAdmin = CurrentSession.IsIT;

            var data = _reportService.GetPlayerWinloss(criteria).ToList();
            var subTotalPlayerWinlosses = new List<PlayerWinloss>();
            var totalPlayerWinloss = new PlayerWinloss();

            SetDataForTotalMemberWinloss(data, subTotalPlayerWinlosses, totalPlayerWinloss);

            return Json(new
            {
                List = data,
                TotalRowData = totalPlayerWinloss,
                SubTotalData = subTotalPlayerWinlosses,
                CurrentPage = 1,
                ItemsPerPage = data.Count,
                TotalItem = data.Count
            });
        }

        /// <summary>
        /// Player Win loss Report for specific affiliate.
        /// </summary>
        /// <param name="model">Model input.</param>
        /// <returns>Player win loss page.</returns>
        [HttpGet]
        public ActionResult PlayerWinlossByAffiliate(PlayerWLByAffiliateViewModel model)
        {
            var baseCurrencyList = new List<SelectListItem>();

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

            model.BaseCurrencyList = baseCurrencyList;

            var serializer = new JavaScriptSerializer();

            ViewBag.Products = serializer.Serialize(AppSettings.Products.Select(x => new
            {
                Id = x.ProductID,
                Name = x.ProductName
            }));

            return View("PlayerWinlossByAffiliate", model);
        }

        #endregion

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

        /// <summary>
        /// Commission By Affiliate. 
        /// </summary>
        /// <param name="model">Model Input.</param>
        /// <returns>Commission By Affiliate Result.</returns>
        [HttpGet]
        public ActionResult CommissionByAffiliate(CommissionReportViewModel model)
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

            model.BaseCurrencyList = baseCurrencyList;

            return View(model);
        }
        
        public ActionResult QualifiedMembers(QualifiedMembersReportCriteria criteria)
        {
            criteria.UserId = CurrentSession.CustID;            
            var members = _reportService.GetQualifiedMembers(criteria);
            
            return View(members);
        }

        /// <summary>
        /// Commission Report.
        /// </summary>
        /// <returns>Commission Report Page.</returns>
        [HttpGet]
        public ActionResult Commission()
        {
            var serializer = new JavaScriptSerializer();
            ViewBag.Products = serializer.Serialize(AppSettings.Products.Select(x => new
            {
                Id = x.ProductID,
                Name = x.ProductName
            }));

            ViewBag.HasCashbackDeduction = AppSettings.DeductionTypes.Any(x => x.ProductID == 0);

            return View(new CommissionReportViewModel());
        }

        [HttpPost]
        public ActionResult CommissionByAffiliate(CommissionReportCriteria criteria)
        {
            criteria.AffName = CurrentSession.Affiliate.UserName;
            criteria.AffTypes = CurrentSession.Affiliate.AffType.ToString();
            criteria.CommGroups = CurrentSession.Affiliate.GroupID.ToString();
            criteria.Currencies = CurrentSession.Affiliate.CurrencyID.ToString();
            criteria.PreferLanguages = CurrentSession.Affiliate.PrefferredLanguage;
            criteria.GroupBy = 0;

            return Commission(criteria);
        }

        /// <summary>
        /// Get data for report.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>        
        /// <returns>Report data.</returns>
        [HttpPost]
        public ActionResult Commission(CommissionReportCriteria criteria)
        {
            criteria.UserId = CurrentSession.CustID;
            criteria.IsITAdmin = CurrentSession.IsIT;           
            var data = _reportService.GetCommission(criteria).ToList();
            var subTotalCommissions = new List<CommissionReport>();
            var totalCommission = new CommissionReport()
            {
                Currency = AppSettings.BaseExchange.Code
            };

            if (data.Count > 0)
            {
                SetDataForTotalCommissions(data, subTotalCommissions, totalCommission);
            }

            return Json(new
            {
                List = data,
                TotalRowData = totalCommission,
                SubTotalData = subTotalCommissions,
                CurrentPage = 1,
                ItemsPerPage = data.Count,
                TotalItem = data.Count
            });
        }

        #region Helpers

        /// <summary>
        /// Set data to sub total and total objects.
        /// </summary>
        /// <param name="commissions">Commission data.</param>
        /// <param name="subTotalData">List of Sub total object.</param>
        /// <param name="totalRowData">Total object.</param>
        private void SetDataForTotalCommissions(IEnumerable<CommissionReport> commissions, List<CommissionReport> subTotalData, CommissionReport totalRowData)
        {
            // Sub total
            var groups = commissions.GroupBy(t => t.Currency);
            var properties = typeof(CommissionReport).GetProperties()
                .Where(p => p.Name != "AffId" && (p.PropertyType == typeof(int) || p.PropertyType == typeof(decimal)));

            foreach (var group in groups)
            {
                var commission = new CommissionReport
                    {
                        Currency = group.Key
                    };

                foreach (var property in properties)
                {
                    var value = group.Sum(g => (decimal)Convert.ChangeType(property.GetValue(g, null), TypeCode.Decimal));
                    property.SetValue(commission, Convert.ChangeType(value, property.PropertyType), null);
                }

                subTotalData.Add(commission);
            }

            foreach (var property in properties)
            {
                if (property.Name.EndsWith("Base"))
                {
                    var value = commissions.Sum(g => (decimal)Convert.ChangeType(property.GetValue(g, null), TypeCode.Decimal));
                    property.SetValue(totalRowData, Convert.ChangeType(value, property.PropertyType), null);
                }

                if (property.Name.EndsWith("ActivePlayers") || property.Name.EndsWith("BetCount") || property.Name.EndsWith("QM"))
                {
                    var value = commissions.Sum(g => (int)Convert.ChangeType(property.GetValue(g, null), TypeCode.Int32));
                    property.SetValue(totalRowData, Convert.ChangeType(value, property.PropertyType), null);
                }
            }
        }

        /// <summary>
        /// Player Win loss Report.
        /// </summary>
        /// <param name="playerWinlosses">Player Win losses.</param>
        /// <param name="subTotalData">Sub Total Data.</param>
        /// <param name="totalRowData">Total Data.</param>
        private void SetDataForTotalMemberWinloss(
            List<PlayerWinloss> playerWinlosses, 
            List<PlayerWinloss> subTotalData, 
            PlayerWinloss totalRowData)
        {
            var groups = playerWinlosses.GroupBy(t => t.PlayerCurrency);

            var properties = typeof(PlayerWinloss).GetProperties()
                .Where(p => p.Name != "ExRate" && (p.PropertyType == typeof(int) || p.PropertyType == typeof(decimal) || p.PropertyType == typeof(double)));

            foreach (var group in groups)
            {
                var playerWinloss = new PlayerWinloss { PlayerCurrency = group.Key };

                foreach (var property in properties)
                {
                    var value = group.Sum(g => (decimal)Convert.ChangeType(property.GetValue(g, null), TypeCode.Decimal));

                    if (property.CanWrite)
                    {
                        property.SetValue(playerWinloss, Convert.ChangeType(value, property.PropertyType), null);
                    }
                }

                subTotalData.Add(playerWinloss);
            }

            // Calculate Total Data
            foreach (var property in properties)
            {
                if (property.Name.EndsWith("Base"))
                {
                    var value = playerWinlosses.Sum(g => (decimal)Convert.ChangeType(property.GetValue(g, null), TypeCode.Decimal));

                    if (property.CanWrite)
                    {
                        property.SetValue(totalRowData, Convert.ChangeType(value, property.PropertyType), null);
                    }
                }

                if (property.Name.EndsWith("BetCount"))
                {
                    var value = playerWinlosses.Sum(g => (int)Convert.ChangeType(property.GetValue(g, null), TypeCode.Int32));

                    if (property.CanWrite)
                    {
                        property.SetValue(totalRowData, Convert.ChangeType(value, property.PropertyType), null);
                    }
                }
            }

            totalRowData.PlayerCurrency = AppSettings.BaseExchange.Code;
        }

        #endregion
    }
}
