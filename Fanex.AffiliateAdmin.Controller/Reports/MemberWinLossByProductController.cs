// --------------------------------------------------------------------------
// <copyright file="MemberWinLossByProductController.cs" company="Nexcel Solutions Vietnam">
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
    using Criterias;
    using Fanex.AffiliateAdmin.Core.Configurations;
    using Fanex.AffiliateAdmin.DTO;
    using Fanex.AffiliateAdmin.Services;
    using Fanex.MemberAdmin.DTO;

    /// <summary>
    /// Report Controller.
    /// </summary>
    public class MemberWinlossByProductController : ReportController
    {
        #region Member Win Loss Report

        public MemberWinlossByProductController(IReportService reportService, ICommGroupService commGroupService)
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
        public ActionResult GetData(MemberWinlossByProductCriteria criteria)
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

            var data = ReportService.GetMemberWinlossByProduct(criteria);

            var groups = GroupDataByMember(data.MemberWinlossList);
            var groupsByCurrency = GroupDataByCurrency(data.TotalMemberWinlossInMemberCurrencies);

            return Json(new
            {
                Groups = GroupDataByMember(data.MemberWinlossList),
                TotalRowData = new MemberWinLossByProduct
                    {
                        Currency = AppSettings.BaseExchange.Code,
                        MemberWinLossByProductList = data.TotalMemberWinlossInSystemCurrency
                    },
                SubTotalData = GroupDataByCurrency(data.TotalMemberWinlossInMemberCurrencies),
                HasMultipleRows = groups.Count() > 1,
                HasMultipleCurrencyRows = groupsByCurrency.Count() > 1,
                CurrentPage = criteria.PageIndex - 1,
                ItemsPerPage = criteria.PageSize,
                TotalItem = criteria.NRows
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