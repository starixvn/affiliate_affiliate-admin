namespace Fanex.AffiliateAdmin.Controller
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Web.Mvc;
    using System.Web.Script.Serialization;
    using Criterias;
    using Fanex.AffiliateAdmin.Core.Configurations;
    using Fanex.AffiliateAdmin.Core.Session;
    using Fanex.AffiliateAdmin.Core.Utilities;
    using Fanex.AffiliateAdmin.DTO;
    using Fanex.AffiliateAdmin.Resources;
    using Fanex.AffiliateAdmin.Services;

    /// <summary>
    /// Report Controller.
    /// </summary>
    public class CompanyWinlossByProductController : ReportController
    {
        #region Company Win Loss Report

        /// <summary>
        /// Company win loss action.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>Company win loss result.</returns>
        [HttpPost]
        public ActionResult GetData(CompanyWinlossByProductCriteria criteria)
        {
            AddMoreParameterForCompanyWinlossReportCriteria(criteria);

            var data = ReportService.GetCompanyWinlossByProduct(criteria);

            var serializer = new JavaScriptSerializer();

            ViewBag.Products = serializer.Serialize(AppSettings.Products.Select(x => new
            {
                Id = x.ProductID,
                Name = x.ProductName
            }));

            var companyWinlossByProduct = GroupDataCompanyWinlossByProduct(criteria, data.CompanyWinlossList);

            var total = new CompanyWinlossReportByProduct
                {
                    GroupInfo = Lang.Total,
                    Level = "All",
                    CompanyWinlossInMemberCurrencies = GroupDataByCurrency(data.TotalCompanyWinlossInMemberCurrencies),
                    CompanyWinlossInSystemCurrency = new CompanyWinlossByCurrency
                        {
                            CompanyWinlossByProductList = GroupDataByProducts(data.TotalCompanyWinlossInSystemCurrency)
                        }
                };

            var firstCurrency = total.CompanyWinlossInMemberCurrencies.FirstOrDefault();

            return Json(new
            {
                Groups = companyWinlossByProduct,
                TotalData = total,
                HasMultipleRows = companyWinlossByProduct.Count > 1,
                HasMultipleCurrencyRows = total.CompanyWinlossInMemberCurrencies.Count() > 1,
                FirstCurrency = firstCurrency != null ? firstCurrency.Currency : string.Empty,
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
        private void AddMoreParameterForCompanyWinlossReportCriteria(CompanyWinlossByProductCriteria criteria)
        {
            var session = SessionManager.CurrentSession;
            if (criteria.ByAffiliate)
            {
                criteria.AffTypes = string.Join(",", AppSettings.AffTypes.Select(t => t.TypeID));
                criteria.CommGroups = string.Join(",", AppSettings.GetCommGroups(null).Select(t => t.GroupID));
                criteria.PreferLanguages = string.Join(",", AppSettings.AffSupportLanguages.Select(t => t.LanguageID));
                criteria.ExcludeTest = false;
            }

            if (criteria.GroupBy == 1)
            {
                // If group by is month, we need to first day and last day of month.
                criteria.FromDate = Common.GetFirstDayOfMonth(criteria.FromDate, AppSettings.SystemStartDate);
                criteria.ToDate = Common.GetLastDayOfMonth(criteria.ToDate, SystemTimeService.GetSystemDateTime());
            }

            criteria.UserId = session.CustID;
            criteria.IsITAdmin = session.IsIT;
            criteria.LicenseeId = AppSettings.LicenseeID;
        }

        /// <summary>
        /// Group Data By Currency.
        /// </summary>
        /// <param name="groupByDate">Group by date.</param>
        /// <returns>Company Win Loss By Currency.</returns>
        private IEnumerable<CompanyWinlossByCurrency> GroupDataByCurrency(
            IEnumerable<CompanyWinLossItem> groupByDate)
        {
            var list = new List<CompanyWinlossByCurrency>();
            var groupByCurrencies = groupByDate.GroupBy(t => t.Currency);

            foreach (var groupByCurrency in groupByCurrencies)
            {
                var firstItem = groupByCurrency.First();
                var currency = new CompanyWinlossByCurrency
                {
                    Currency = groupByCurrency.Key,
                    AffExRate = firstItem.AffExRate,
                    ExRate = firstItem.ExRate,
                    CompanyWinlossByProductList = groupByCurrency.ToList()
                };

                list.Add(currency);
            }

            return list;
        }

        /// <summary>
        /// Filter Group Data Company Win Loss By Product.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <param name="companyWinlosses">Company win loss.</param>
        /// <returns>Company Win Loss By Product.</returns>
        private List<CompanyWinlossReportByProduct> GroupDataCompanyWinlossByProduct(
            CompanyWinlossByProductCriteria criteria, IEnumerable<CompanyWinLossItem> companyWinlosses)
        {
            var companyWinlossByProducts = new List<CompanyWinlossReportByProduct>();

            var dateFormat = criteria.GroupBy == 0 ? "MM/dd/yyyy" : "MM/yyyy";

            var groupByDates = companyWinlosses.GroupBy(t => t.WinlostDate.ToString(dateFormat)).ToList();

            foreach (var groupByDate in groupByDates)
            {
                var group = new CompanyWinlossReportByProduct
                {
                    GroupInfo = groupByDate.Key,
                };

                @group.CompanyWinlossInMemberCurrencies = GroupDataByCurrency(groupByDate);

                @group.CompanyWinlossInSystemCurrency = new CompanyWinlossByCurrency
                {
                    CompanyWinlossByProductList = GroupDataByProducts(groupByDate)
                };

                companyWinlossByProducts.Add(@group);
            }

            return companyWinlossByProducts;
        }

        /// <summary>
        /// Check property can sum up or not.
        /// </summary>
        /// <param name="p">Property Input.</param>
        /// <returns>Can/Can not sum up.</returns>
        private bool IsPropertyAbleSumUp(PropertyInfo p)
        {
            var uncalculatedPropertyList = new List<string> { "ExRate", "AffExRate" };

            if (uncalculatedPropertyList.Any(t => t.Equals(p.Name, StringComparison.OrdinalIgnoreCase)))
            {
                return false;
            }

            var typeList = new List<Type> { typeof(int), typeof(decimal), typeof(double) };

            return typeList.Any(t => t == p.PropertyType);
        }

        /// <summary>
        /// Group Data By Products.
        /// </summary>
        /// <param name="groupByDate">Group By Date.</param>
        /// <returns>Company Win Loss List.</returns>
        private IEnumerable<CompanyWinLossItem> GroupDataByProducts(IEnumerable<CompanyWinLossItem> groupByDate)
        {
            var properties = typeof(CompanyWinLossItem).GetProperties().Where(p => IsPropertyAbleSumUp(p)).ToList();

            var list = new List<CompanyWinLossItem>();

            var groupByProducts = groupByDate.GroupBy(t => t.ProductName);

            foreach (var groupByProduct in groupByProducts)
            {
                var companyWinloss = GetTotalCompanyWinloss(groupByProduct, properties);
                companyWinloss.ProductName = groupByProduct.Key;
                list.Add(companyWinloss);
            }

            return list;
        }

        /// <summary>
        /// Get Total Company Win loss In System Currency.
        /// </summary>
        /// <param name="companyWinlosses">Company Win loss List.</param>
        /// <param name="properties">Property List.</param>
        /// <returns>Company Win loss In System Currency.</returns>
        private CompanyWinLossItem GetTotalCompanyWinloss(IEnumerable<CompanyWinLossItem> companyWinlosses, IEnumerable<PropertyInfo> properties)
        {
            var totalCompanlyWinloss = new CompanyWinLossItem();

            // Sub total by date, in system currency.
            foreach (var property in properties)
            {
                if (property.Name.EndsWith("ActivePlayers", StringComparison.OrdinalIgnoreCase) || property.Name.EndsWith("BetCount", StringComparison.OrdinalIgnoreCase))
                {
                    var value1 = companyWinlosses.Sum(g =>
                    {
                        var changeType = Convert.ChangeType(property.GetValue(g, null).ToString(), TypeCode.Int32);
                        return changeType != null ? (int)changeType : 0;
                    });

                    if (property.CanWrite)
                    {
                        property.SetValue(totalCompanlyWinloss, Convert.ChangeType(value1, property.PropertyType), null);
                    }

                    continue;
                }

                var value = companyWinlosses.Sum(g =>
                {
                    var changeType = Convert.ChangeType(property.GetValue(g, null), TypeCode.Decimal);
                    return changeType != null ? (decimal)changeType : 0;
                });

                if (property.CanWrite)
                {
                    property.SetValue(totalCompanlyWinloss, Convert.ChangeType(value, property.PropertyType), null);
                }
            }

            return totalCompanlyWinloss;
        }

        #endregion Helpers
    }
}