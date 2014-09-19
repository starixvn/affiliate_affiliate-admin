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
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
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

            var data = ReportService.GetAffiliateCommission(criteria);

            return Json(new
            {
                Groups = GroupByRowIndex(data),
                CurrentPage = criteria.PageCurrent - 1,
                ItemsPerPage = criteria.PageSize,
                TotalItem = criteria.NRows
            });
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