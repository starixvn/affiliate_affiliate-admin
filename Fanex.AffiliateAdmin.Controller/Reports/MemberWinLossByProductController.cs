namespace Fanex.AffiliateAdmin.Controller
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Criterias;
    using Fanex.AffiliateAdmin.Core.Configurations;
    using Fanex.AffiliateAdmin.Core.Services.Interface;
    using Fanex.AffiliateAdmin.Core.Session;
    using Fanex.AffiliateAdmin.DTO;
    using Fanex.AffiliateAdmin.Services;
    using Fanex.MemberAdmin.DTO;
    using Helpers;

    /// <summary>
    /// Report Controller.
    /// </summary>
    public class MemberWinlossByProductController : ReportController
    {
        public MemberWinlossByProductController(
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
        public ActionResult GetData(MemberWinlossByProductCriteria criteria)
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

            var data = ReportService.GetMemberWinlossByProduct(criteria);

            var groups = GroupDataByMember(data.MemberWinlossList);

            var groupsByCurrency = GroupDataByCurrency(data.TotalMemberWinlossInMemberCurrencies);

            var firstCurrency = groupsByCurrency.FirstOrDefault();

            return Json(new
            {
                Groups = GroupDataByMember(data.MemberWinlossList),
                TotalRowData = new MemberWinLossByProduct
                {
                    Currency = AppSetting.BaseExchange.Code,
                    MemberWinLossByProductList = data.TotalMemberWinlossInSystemCurrency
                },
                SubTotalData = GroupDataByCurrency(data.TotalMemberWinlossInMemberCurrencies),
                HasMultipleRows = groups.Count() > 1,
                HasMultipleCurrencyRows = groupsByCurrency.Count() > 1,
                FirstCurrency = firstCurrency != null ? firstCurrency.PlayerCurrency : string.Empty,
                CurrentPage = criteria.PageIndex - 1,
                ItemsPerPage = criteria.PageSize,
                TotalItem = criteria.NRows,
                DurationDB = criteria.Duration,
                DurationFanexData = data.Duration
            });
        }

        /// <summary>
        /// Group Data By Member.
        /// </summary>
        /// <param name="memberWinLosses">Member Win Loss.</param>
        /// <returns>Member Win Loss By Product.</returns>
        private IEnumerable<MemberWinLossByProduct> GroupDataByMember(IEnumerable<MemberWinLossItem> memberWinLosses)
        {
            var list = new List<MemberWinLossByProduct>();

            var groupByRowIndexes = memberWinLosses.GroupBy(t => t.RowIdx);

            foreach (var groupByRowIndex in groupByRowIndexes)
            {
                var groupByMembers = groupByRowIndex.GroupBy(t => t.PlayerUsername);

                foreach (var groupByMember in groupByMembers)
                {
                    var first = groupByMember.First();

                    list.Add(new MemberWinLossByProduct
                    {
                        AffExRate = first.AffExRate,
                        Currency = first.Currency,
                        ExRate = first.ExRate,
                        PlayerCurrency = first.PlayerCurrency,
                        PlayerUsername = first.PlayerUsername,
                        Username = first.Username,
                        WinlostDate = first.WinlostDate,
                        ProductId = first.ProductId,
                        ProductName = first.ProductName,
                        MemberWinLossByProductList = groupByMember
                    });
                }
            }

            return list;
        }

        /// <summary>
        /// Group Data By Currency.
        /// </summary>
        /// <param name="memberWinLosses">Member Win Loss.</param>
        /// <returns>Member Win Loss By Product.</returns>
        private IEnumerable<MemberWinLossByProduct> GroupDataByCurrency(IEnumerable<MemberWinLossItem> memberWinLosses)
        {
            var list = new List<MemberWinLossByProduct>();

            var groupByMembers = memberWinLosses.GroupBy(t => t.PlayerCurrency);

            foreach (var groupByMember in groupByMembers)
            {
                var first = groupByMember.First();

                list.Add(new MemberWinLossByProduct
                {
                    AffExRate = first.AffExRate,
                    Currency = first.Currency,
                    ExRate = first.ExRate,
                    PlayerCurrency = first.PlayerCurrency,
                    PlayerUsername = first.PlayerUsername,
                    Username = first.Username,
                    WinlostDate = first.WinlostDate,
                    ProductId = first.ProductId,
                    ProductName = first.ProductName,
                    MemberWinLossByProductList = groupByMember
                });
            }

            return list;
        }

        #endregion Member Win Loss Report
    }
}