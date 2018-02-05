namespace Fanex.AffiliateAdmin.Controller
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.AffiliateAdmin.Core.Configurations;
    using Fanex.AffiliateAdmin.Core.Session;
    using Fanex.AffiliateAdmin.Core.Utilities;
    using Fanex.AffiliateAdmin.Criterias;
    using Fanex.AffiliateAdmin.DTO;
    using Fanex.AffiliateAdmin.Services;
    using Helpers;

    /// <summary>
    /// Affiliate Commission Controller.
    /// </summary>
    public class AffiliateCommissionController : ReportController
    {
        public AffiliateCommissionController()
            : this(null, null)
        {
        }

        public AffiliateCommissionController(IReportService reportService, ICommGroupService commGroupService)
            : base(reportService, commGroupService)
        {
        }

        /// <summary>
        /// Affiliate win loss action.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>Affiliate win loss result.</returns>
        [HttpPost]
        public ActionResult GetData(AffiliateCommissionCriteria criteria)
        {
            var session = SessionManager.CurrentSession;
            criteria.UserId = session.CustID;
            criteria.IsITAdmin = session.IsIT;
            criteria.UserCurrencies = session.BuildUserCurrencyParam();
            criteria.LicenseeId = AppSettings.LicenseeID;
            criteria.FromDate = Common.GetFirstDayOfMonth(criteria.FromDate, AppSettings.SystemStartDate);
            criteria.ToDate = Common.GetLastDayOfMonth(criteria.ToDate, SystemTimeService.GetSystemDateTime());

            var data = ReportService.GetAffiliateCommission(criteria);
            var groups = GroupByRowIndex(data.AffiliateCommissionList);
            var groupsByCurrency = GroupDataByCurrency(data.AffiliateCommissionInAffiliateCurrency);
            var firstCurrency = groupsByCurrency.FirstOrDefault();

            return Json(new
            {
                Groups = groups,
                SubTotalData = groupsByCurrency,
                TotalRowData = new AffiliateCommissionByProduct
                {
                    Currency = AppSettings.BaseExchange.Code,
                    AffiliateCommissionByProductList = data.AffiliateCommissionInSystemCurrency
                },
                HasMultipleRows = groups.Count() > 1,
                HasMultipleCurrencyRows = groupsByCurrency.Count() > 1,
                FirstCurrency = firstCurrency != null ? firstCurrency.Currency : string.Empty,
                CurrentPage = criteria.PageCurrent - 1,
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
        public ActionResult GetDataByAffiliate(AffiliateCommissionCriteria criteria)
        {
            const int GroupByMonth = 1;
            var affiliate = AffiliateInfoCookieHelper.GetAffiliate(HttpContext);
            criteria.AffName = affiliate.UserName;
            criteria.AffTypes = string.Join(",", AppSettings.AffTypes.Select(t => t.TypeID));
            criteria.CommGroups = string.Join(",", AppSettings.GetCommGroups(null).Select(t => t.GroupID));
            criteria.PreferLanguages = string.Join(",", AppSettings.AffSupportLanguages.Select(t => t.LanguageID));
            criteria.Currencies = affiliate.Currency.ToString();
            criteria.GroupBy = GroupByMonth;

            return GetData(criteria);
        }

        /// <summary>
        /// Group Data By Currency.
        /// </summary>
        /// <param name="affiliateCommissions">Affiliate Commissions.</param>
        /// <returns>Affiliate Commission List.</returns>
        private IEnumerable<AffiliateCommissionByProduct> GroupDataByCurrency(IEnumerable<AffiliateCommission> affiliateCommissions)
        {
            var list = new List<AffiliateCommissionByProduct>();

            var groupByCurrencies = affiliateCommissions.GroupBy(t => t.Currency);

            foreach (var groupByCurrency in groupByCurrencies)
            {
                list.Add(new AffiliateCommissionByProduct
                {
                    Currency = groupByCurrency.Key,
                    AffiliateCommissionByProductList = groupByCurrency
                });
            }

            return list;
        }

        /// <summary>
        /// Group By Row Index.
        /// </summary>
        /// <param name="affiliateCommissions">Affiliate Commissions.</param>
        /// <returns>Affiliate Commission By Product.</returns>
        private IEnumerable<AffiliateCommissionByProduct> GroupByRowIndex(IEnumerable<AffiliateCommission> affiliateCommissions)
        {
            var list = new List<AffiliateCommissionByProduct>();

            var groupByRowIndexes = affiliateCommissions.GroupBy(t => t.RowIdx);

            foreach (var groupByRowIndex in groupByRowIndexes)
            {
                var first = groupByRowIndex.First();

                list.Add(new AffiliateCommissionByProduct
                {
                    AffId = first.AffId,
                    Currency = first.Currency,
                    GroupID = first.GroupID,
                    GroupName = first.GroupName,
                    Username = first.Username,
                    RowIdx = first.RowIdx,
                    TotalPayment = first.TotalPayment,
                    TotalPaymentBase = first.TotalPaymentBase,
                    WinlostDate = first.WinlostDate,
                    AffiliateCommissionByProductList = groupByRowIndex
                });
            }

            return list;
        }
    }
}