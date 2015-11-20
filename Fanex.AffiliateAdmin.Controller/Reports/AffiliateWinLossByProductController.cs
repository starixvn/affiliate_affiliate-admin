// --------------------------------------------------------------------------
// <copyright file="AffiliateWinLossByProductController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2014.9.18">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Controller
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Criterias;
    using Fanex.AffiliateAdmin.Core.Configurations;
    using Fanex.AffiliateAdmin.Core.Utilities;
    using Fanex.AffiliateAdmin.DTO;
    using Fanex.AffiliateAdmin.Services;

    /// <summary>
    /// Report Controller.
    /// </summary>
    public class AffiliateWinLossByProductController : ReportController
    {
        #region Affiliate Win Loss Report

        /// <summary>
        /// Affiliate win loss action.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>Affiliate win loss result.</returns>
        [HttpPost]
        public ActionResult GetData(AffiliateWinLossByProductCriteria criteria)
        {
            criteria.UserId = CurrentSession.CustID;
            criteria.IsITAdmin = CurrentSession.IsIT;
            criteria.LicenseeId = AppSettings.LicenseeID;
            criteria.FromDate = Common.GetFirstDayOfMonth(criteria.FromDate, AppSettings.SystemStartDate);
            criteria.ToDate = Common.GetLastDayOfMonth(criteria.ToDate, SystemTimeService.GetSystemDateTime());

            var data = ReportService.GetAffiliateWinLossByProduct(criteria);

            var groups = GroupDataByAffiliate(data.AffiliateWinLossList);

            var groupsByCurrency = GroupDataByCurrency(data.AffiliateWinLossInAffiliateCurrency);

            var firstCurrency = groupsByCurrency.FirstOrDefault();

            return Json(new
            {
                Groups = groups,
                SubTotalData = groupsByCurrency,
                TotalRowData = new AffiliateWinLossByProduct
                    {
                        Currency = AppSettings.BaseExchange.Code,
                        AffiliateWinLossByProductList = data.AffiliateWinLossInSystemCurrency
                    },
                HasMultipleRows = groups.Count() > 1,
                HasMultipleCurrencyRows = groupsByCurrency.Count() > 1,
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
        public ActionResult GetDataByAffiliate(AffiliateWinLossByProductCriteria criteria)
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

        /// <summary>
        /// Group Data By Affiliate.
        /// </summary>
        /// <param name="affiliateWinLosses">Affiliate Win Loss.</param>
        /// <returns>Affiliate WinLoss By Product List.</returns>
        private IEnumerable<AffiliateWinLossByProduct> GroupDataByAffiliate(IEnumerable<AffiliateWinLossItem> affiliateWinLosses)
        {
            var list = new List<AffiliateWinLossByProduct>();

            var groupByAffiliates = affiliateWinLosses.GroupBy(t => t.RowIdx);

            foreach (var groupByAffiliate in groupByAffiliates)
            {
                var first = groupByAffiliate.First();

                list.Add(new AffiliateWinLossByProduct
                    {
                        AffId = first.AffId,
                        Username = first.Username,
                        AffType = first.AffType,
                        CurrencyId = first.CurrencyId,
                        Currency = first.Currency,
                        GroupID = first.GroupID,
                        GroupName = first.GroupName,
                        Language = first.Language,
                        Status = first.Status,
                        WinlostDate = first.WinlostDate,
                        AffiliateWinLossByProductList = groupByAffiliate
                    });
            }

            return list;
        }

        /// <summary>
        /// Group Data By Currency.
        /// </summary>
        /// <param name="affiliateWinLosses">Affiliate Win Loss.</param>
        /// <returns>Affiliate WinLoss By Product List.</returns>
        private IEnumerable<AffiliateWinLossByProduct> GroupDataByCurrency(IEnumerable<AffiliateWinLossItem> affiliateWinLosses)
        {
            var list = new List<AffiliateWinLossByProduct>();

            var groupByCurrencies = affiliateWinLosses.GroupBy(t => t.CurrencyId);

            foreach (var groupByCurrency in groupByCurrencies)
            {
                var first = groupByCurrency.First();

                list.Add(new AffiliateWinLossByProduct
                {
                    AffId = first.AffId,
                    Username = first.Username,
                    AffType = first.AffType,
                    CurrencyId = first.CurrencyId,
                    Currency = first.Currency,
                    GroupID = first.GroupID,
                    GroupName = first.GroupName,
                    Language = first.Language,
                    Status = first.Status,
                    WinlostDate = first.WinlostDate,
                    AffiliateWinLossByProductList = groupByCurrency
                });
            }

            return list;
        }

        #endregion Affiliate Win Loss Report
    }
}