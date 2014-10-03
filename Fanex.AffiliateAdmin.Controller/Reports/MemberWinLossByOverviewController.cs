// --------------------------------------------------------------------------
// <copyright file="MemberWinLossByOverviewController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2014.9.19">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Controller
{
    using System.Linq;
    using System.Web.Mvc;
    using Criterias;
    using Fanex.AffiliateAdmin.Core.Configurations;
    using Fanex.AffiliateAdmin.Services;

    /// <summary>
    /// Report Controller.
    /// </summary>
    public class MemberWinLossByOverviewController : ReportController
    {
        #region Member Win Loss Report

        public MemberWinLossByOverviewController(IReportService reportService, ICommGroupService commGroupService)
            : base(reportService, commGroupService)
        {
            ReportService = reportService;
            CommGroupService = commGroupService;
        }

        /// <summary>
        /// Member win loss action.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>Member win loss result.</returns>
        [HttpPost]
        public ActionResult GetData(MemberWinLossOverviewCriteria criteria)
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

            var data = ReportService.GetMemberWinlossOverview(criteria);

            var firstCurrency = data.TotalMemberWinlossInMemberCurrencies.FirstOrDefault();

            return Json(new
            {
                List = data.MemberWinlossList,
                TotalRowData = data.TotalMemberWinlossInSystemCurrency,
                SubTotalData = data.TotalMemberWinlossInMemberCurrencies,
                HasMultipleRows = data.MemberWinlossList.Count() > 1,
                HasMultipleCurrencyRows = data.TotalMemberWinlossInMemberCurrencies.Count() > 1,
                FirstCurrency = firstCurrency != null ? firstCurrency.PlayerCurrency : string.Empty,
                CurrentPage = criteria.PageIndex - 1,
                ItemsPerPage = criteria.PageSize,
                TotalItem = criteria.NRows,
                DurationDB = criteria.Duration,
                DurationFanexData = data.Duration,
                DurationGroupByDataOnWeb = 0
            });
        }

        #endregion Member Win Loss Report
    }
}