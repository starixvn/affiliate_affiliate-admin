namespace Fanex.AffiliateAdmin.DTO
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class CompanyWinlossReportByOverview
    {
        public string GroupInfo { get; set; }

        public string Level { get; set; }

        public IEnumerable<CompanyWinLossItem> CompanyWinlossInMemberCurrencies { get; set; }

        public CompanyWinLossItem CompanyWinlossInSystemCurrency
        {
            get { return GetTotalCompanyWinloss(); }
        }

        public int MergeRowNumber
        {
            get { return CompanyWinlossInMemberCurrencies.Count(); }
        }

        /// <summary>
        /// Get Total Company Win Loss.
        /// </summary>
        /// <returns>Total Company Win Loss.</returns>
        private CompanyWinLossItem GetTotalCompanyWinloss()
        {
            var properties = typeof(CompanyWinLossItem).GetProperties().Where(IsPropertyAbleSumUp).ToList();

            var totalCompanlyWinloss = new CompanyWinLossItem();

            // Sub total by date, in system currency.
            foreach (var property in properties)
            {
                if (property.Name.EndsWith("ActivePlayer", StringComparison.OrdinalIgnoreCase) || property.Name.EndsWith("BetCount", StringComparison.OrdinalIgnoreCase))
                {
                    var value1 = CompanyWinlossInMemberCurrencies.Sum(g =>
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

                if (property.Name.EndsWith("Base", StringComparison.OrdinalIgnoreCase))
                {
                    var value = CompanyWinlossInMemberCurrencies.Sum(g =>
                        {
                            var changeType = Convert.ChangeType(property.GetValue(g, null), TypeCode.Decimal);
                            return changeType != null ? (decimal)changeType : 0;
                        });

                    if (property.CanWrite)
                    {
                        property.SetValue(totalCompanlyWinloss, Convert.ChangeType(value, property.PropertyType), null);
                    }
                }
            }

            return totalCompanlyWinloss;
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

            var typeList = new List<Type> { typeof(int), typeof(decimal), typeof(double), typeof(float), typeof(byte) };

            return typeList.Any(t => t == p.PropertyType);
        }
    }
}