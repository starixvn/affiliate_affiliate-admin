// --------------------------------------------------------------------------
// <copyright file="AffiliateWinLossByOverviewController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2014.9.18">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Controller
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using Criterias;
    using Fanex.AffiliateAdmin.Core.Configurations;
    using Fanex.AffiliateAdmin.Core.Utilities;
    using Fanex.AffiliateAdmin.Services;

    /// <summary>
    /// Report Controller.
    /// </summary>
    public class AffiliateWinLossByOverviewController : ReportController
    {
        #region Affiliate Win Loss Report

        public AffiliateWinLossByOverviewController(IReportService reportService, ICommGroupService commGroupService)
            : base(reportService, commGroupService)
        {
            ReportService = reportService;
            CommGroupService = commGroupService;
        }

        /// <summary>
        /// Affiliate win loss action.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>Affiliate win loss result.</returns>
        [HttpPost]
        public ActionResult GetData(AffiliateWinLossOverviewCriteria criteria)
        {
            criteria.UserId = CurrentSession.CustID;
            criteria.IsITAdmin = CurrentSession.IsIT;
            criteria.FromDate = Common.GetFirstDayOfMonth(criteria.FromDate, AppSettings.SystemStartDate);
            criteria.ToDate = Common.GetLastDayOfMonth(criteria.ToDate, DateTime.Now);

            var data = ReportService.GetAffiliateWinLossOverview(criteria);

            var totalCommission = data.CommissionInSystemCurrency;

            if (totalCommission != null)
            {
                totalCommission.Currency = AppSettings.BaseExchange.Code;
            }

            var hasMultipleCurrency = data.CommissionList.GroupBy(t => t.Currency)
                                           .Select(t => new { Currency = t.Key, RowCount = t.Count() })
                                           .Any(t => t.RowCount > 1);

            return Json(new
            {
                List = data.CommissionList,
                TotalRowData = totalCommission,
                SubTotalData = data.CommissionInAffiliateCurrency,
                hasMultipleCurrency = hasMultipleCurrency,
                CurrentPage = criteria.PageIndex - 1,
                ItemsPerPage = criteria.PageSize,
                TotalItem = criteria.NRows
            });
        }

        /// <summary>
        /// Affiliate win loss action.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>Affiliate Win Loss result.</returns>
        [HttpPost]
        public ActionResult GetDataByAffiliate(AffiliateWinLossOverviewCriteria criteria)
        {
            const int GroupByMonth = 1;
            criteria.AffName = CurrentSession.Affiliate.Username;
            criteria.AffTypes = string.Join(",", AppSettings.AffTypes.Select(t => t.TypeID));
            criteria.CommGroups = string.Join(",", AppSettings.GetCommGroups(null).Select(t => t.GroupID));
            criteria.PreferLanguages = string.Join(",", AppSettings.AffSupportLanguages.Select(t => t.LanguageID));
            criteria.Currencies = CurrentSession.Affiliate.CurrencyID.ToString();
            criteria.GroupBy = GroupByMonth;

            return GetData(criteria);
        }

        #endregion Affiliate Win Loss Report
    }
}