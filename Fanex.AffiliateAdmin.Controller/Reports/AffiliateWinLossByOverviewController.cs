namespace Fanex.AffiliateAdmin.Controller
{
    using System.Linq;
    using System.Web.Mvc;
    using Criterias;
    using Fanex.AffiliateAdmin.Core.Configurations;
    using Fanex.AffiliateAdmin.Core.Services.Interface;
    using Fanex.AffiliateAdmin.Core.Session;
    using Fanex.AffiliateAdmin.Core.Utilities;
    using Fanex.AffiliateAdmin.Services;
    using Helpers;

    /// <summary>
    /// Report Controller.
    /// </summary>
    public class AffiliateWinLossByOverviewController : ReportController
    {
        public AffiliateWinLossByOverviewController(
            IAppSetting appSetting,
            IReportService reportService,
            ISystemSettings systemSettings,
            ICommGroupService commGroupService
            )
            : base(appSetting, reportService, systemSettings, commGroupService)
        {
        }

        #region Affiliate Win Loss Report

        /// <summary>
        /// Affiliate win loss action.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>Affiliate win loss result.</returns>
        [HttpPost]
        public ActionResult GetData(AffiliateWinLossOverviewCriteria criteria)
        {
            var session = SessionManager.CurrentSession;
            criteria.IsITAdmin = session.IsIT;
            criteria.UserCurrencies = session.BuildUserCurrencyParam();
            criteria.FromDate = Common.GetFirstDayOfMonth(criteria.FromDate, SystemSettings.SystemStartDate);
            criteria.ToDate = Common.GetLastDayOfMonth(criteria.ToDate, SystemSettings.SystemDateTime);

            var data = ReportService.GetAffiliateWinLossOverview(criteria);

            var totalCommission = data.CommissionInSystemCurrency;

            if (totalCommission != null)
            {
                totalCommission.Currency = SystemSettings.BaseExchange.Code;
            }

            var firstCurrency = data.CommissionInAffiliateCurrency.FirstOrDefault();

            return Json(new
            {
                List = data.CommissionList,
                TotalRowData = totalCommission,
                SubTotalData = data.CommissionInAffiliateCurrency,
                HasMultipleRows = data.CommissionList.Count() > 1,
                HasMultipleCurrencyRows = data.CommissionInAffiliateCurrency.Count() > 1,
                FirstCurrency = firstCurrency != null ? firstCurrency.Currency : string.Empty,
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
            var affiliate = AffiliateInfoCookieHelper.GetAffiliate(HttpContext);
            criteria.AffName = affiliate.UserName;
            criteria.Currencies = affiliate.Currency.ToString();
            criteria.AffTypes = string.Join(",", SystemSettings.AffTypes.Select(t => t.TypeID));
            criteria.CommGroups = string.Join(",", SystemSettings.GetCommGroups().Select(t => t.GroupID));
            criteria.PreferLanguages = string.Join(",", SystemSettings.AffSupportLanguages.Select(t => t.LanguageID));
            criteria.GroupBy = GroupByMonth;

            return GetData(criteria);
        }

        #endregion Affiliate Win Loss Report
    }
}