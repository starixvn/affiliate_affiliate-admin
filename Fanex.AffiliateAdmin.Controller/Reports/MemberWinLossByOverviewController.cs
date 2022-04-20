namespace Fanex.AffiliateAdmin.Controller
{
    using System.Linq;
    using System.Web.Mvc;
    using Criterias;
    using Fanex.AffiliateAdmin.Core.Configurations;
    using Fanex.AffiliateAdmin.Core.Services.Interface;
    using Fanex.AffiliateAdmin.Core.Session;
    using Fanex.AffiliateAdmin.Services;
    using Helpers;

    /// <summary>
    /// Report Controller.
    /// </summary>
    public class MemberWinLossByOverviewController : ReportController
    {
        public MemberWinLossByOverviewController(
            IAppSetting appSetting,
            IReportService reportService,
            ICommGroupService commGroupService,
            ISystemSettingService systemSettingService
            )
            : base(appSetting, reportService, commGroupService, systemSettingService)
        {
        }

        #region Member Win Loss Report

        /// <summary>
        /// Member win loss action.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>Member win loss result.</returns>
        [HttpPost]
        public ActionResult GetData(MemberWinLossOverviewCriteria criteria)
        {
            var session = SessionManager.CurrentSession;
            if (criteria.ByAffiliate)
            {
                criteria.AffName = AffiliateInfoCookieHelper.GetAffiliate(HttpContext).UserName;
                criteria.ExcludeTest = false;
                criteria.AffTypes = string.Join(",", AppSetting.AffTypes.Select(t => t.TypeID));
                criteria.CommGroups = string.Join(",", SystemSettingService.GetCommGroups().Select(t => t.GroupID));
                criteria.PreferLanguages = string.Join(",", AppSetting.AffSupportLanguages.Select(t => t.LanguageID));
            }

            criteria.IsITAdmin = session.IsIT;
            criteria.UserCurrencies = session.BuildUserCurrencyParam();

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