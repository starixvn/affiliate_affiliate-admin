// --------------------------------------------------------------------------
// <copyright file="CompanyWinLossByOverviewController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2014.9.16">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Controller
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.Script.Serialization;
    using Criterias;
    using Fanex.AffiliateAdmin.Core.Configurations;
    using Fanex.AffiliateAdmin.Core.Utilities;
    using Fanex.AffiliateAdmin.DTO;
    using Fanex.AffiliateAdmin.Resources;
    using Fanex.AffiliateAdmin.Services;

    /// <summary>
    /// Report Controller.
    /// </summary>
    public class CompanyWinLossByOverviewController : ReportController
    {
        #region Company Win Loss Report

        public CompanyWinLossByOverviewController(IReportService reportService, ICommGroupService commGroupService)
            : base(reportService, commGroupService)
        {
            ReportService = reportService;
            CommGroupService = commGroupService;
        }

        /// <summary>
        /// Company win loss action.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>Company win loss result.</returns>
        [HttpPost]
        public ActionResult GetData(CompanyWinlossOverviewCriteria criteria)
        {
            AddMoreParameterForCompanyWinlossReportCriteria(criteria);

            var data = ReportService.GetCompanyWinlossOverview(criteria);

            var serializer = new JavaScriptSerializer();

            ViewBag.Products = serializer.Serialize(AppSettings.Products.Select(x => new
            {
                Id = x.ProductID,
                Name = x.ProductName
            }));

            var companyWinlossByProduct = GroupDataCompanyWinlossByOverview(criteria, data.CompanyWinlossList);

            var total = new CompanyWinlossReportByOverview
                {
                    GroupInfo = Lang.Total,
                    Level = "All",
                    CompanyWinlossInMemberCurrencies = data.TotalCompanyWinlossInMemberCurrencies
                };

            return Json(new
            {
                Groups = companyWinlossByProduct,
                TotalData = total,
                HasMultipleRows = companyWinlossByProduct.Count > 1,
                HasMultipleCurrencyRows = total.CompanyWinlossInMemberCurrencies.Count() > 1,
                CurrentPage = criteria.PageIndex - 1,
                ItemsPerPage = criteria.PageSize,
                TotalItem = criteria.NRows,
            });
        }

        #endregion Company Win Loss Report

        #region Helpers

        /// <summary>
        /// Add more parameter for company win-loss report criteria.
        /// </summary>
        /// <param name="criteria">Criteria input.</param>
        private void AddMoreParameterForCompanyWinlossReportCriteria(CompanyWinlossOverviewCriteria criteria)
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
                criteria.FromDate = Common.GetFirstDayOfMonth(criteria.FromDate, AppSettings.SystemStartDate);
                criteria.ToDate = Common.GetLastDayOfMonth(criteria.ToDate, DateTime.Now);
            }

            criteria.UserId = CurrentSession.CustID;
            criteria.IsITAdmin = CurrentSession.IsIT;
        }

        /// <summary>
        /// Filter Group Data Company Win Loss By Product.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <param name="companyWinlosses">Company win loss.</param>
        /// <returns>Company Win Loss By Product.</returns>
        private List<CompanyWinlossReportByOverview> GroupDataCompanyWinlossByOverview(
            CompanyWinlossOverviewCriteria criteria, IEnumerable<CompanyWinLossItem> companyWinlosses)
        {
            var companyWinlossByOverviews = new List<CompanyWinlossReportByOverview>();

            var dateFormat = criteria.GroupBy == 0 ? "MM/dd/yyyy" : "MM/yyyy";

            var groupByDates = companyWinlosses.GroupBy(t => t.WinlostDate.ToString(dateFormat)).ToList();

            foreach (var groupByDate in groupByDates)
            {
                var group = new CompanyWinlossReportByOverview
                {
                    GroupInfo = groupByDate.Key,
                };

                @group.CompanyWinlossInMemberCurrencies = groupByDate;

                companyWinlossByOverviews.Add(@group);
            }

            return companyWinlossByOverviews;
        }

        #endregion Helpers
    }
}