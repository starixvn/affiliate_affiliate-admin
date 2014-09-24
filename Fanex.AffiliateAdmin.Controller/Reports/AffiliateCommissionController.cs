// --------------------------------------------------------------------------
// <copyright file="AffiliateCommissionController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2014.9.19">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Controller
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.AffiliateAdmin.Core.Configurations;
    using Fanex.AffiliateAdmin.Core.Utilities;
    using Fanex.AffiliateAdmin.Criterias;
    using Fanex.AffiliateAdmin.DTO;
    using Fanex.AffiliateAdmin.Services;

    /// <summary>
    /// Affiliate Commission Controller.
    /// </summary>
    public class AffiliateCommissionController : ReportController
    {
        public AffiliateCommissionController(IReportService reportService, ICommGroupService commGroupService)
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
        public ActionResult GetData(AffiliateCommissionCriteria criteria)
        {
            criteria.UserId = CurrentSession.CustID;

            criteria.IsITAdmin = CurrentSession.IsIT;

            criteria.FromDate = Common.GetFirstDayOfMonth(criteria.FromDate, AppSettings.SystemStartDate);

            criteria.ToDate = Common.GetLastDayOfMonth(criteria.ToDate, DateTime.Now);

            var data = ReportService.GetAffiliateCommission(criteria);

            return Json(new
            {
                Groups = GroupByRowIndex(data.AffiliateCommissionList),
                SubTotalData = GroupDataByCurrency(data.AffiliateCommissionInAffiliateCurrency),
                TotalRowData = new AffiliateCommissionByProduct
                {
                    Currency = AppSettings.BaseExchange.Code,
                    AffiliateCommissionByProductList = data.AffiliateCommissionInSystemCurrency
                },
                CurrentPage = criteria.PageCurrent - 1,
                ItemsPerPage = criteria.PageSize,
                TotalItem = criteria.NRows
            });
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