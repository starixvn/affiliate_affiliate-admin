namespace Fanex.MemberAdmin.DTO
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Fanex.AffiliateAdmin.DTO;

    /// <summary>
    /// Player win loss class.
    /// </summary>
    public class MemberWinLossByProduct
    {
        public DateTime WinlostDate { get; set; }

        public string Username { get; set; }

        public string Currency { get; set; }

        public string PlayerUsername { get; set; }

        public string PlayerCurrency { get; set; }

        public double ExRate { get; set; }

        public double AffExRate { get; set; }

        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public int MergeRowNumber
        {
            get
            {
                var count = MemberWinLossByProductList.Count();
                return count > 1 ? count + 1 : count;
            }
        }

        public IEnumerable<MemberWinLossItem> MemberWinLossByProductList { get; set; }

        public MemberWinLossItem TotalMemberWinLossByProduct
        {
            get
            {
                return GetTotalMemberWinLoss();
            }
        }

        /// <summary>
        /// Get Total Member Win Loss.
        /// </summary>
        /// <returns>Total Member Win Loss.</returns>
        private MemberWinLossItem GetTotalMemberWinLoss()
        {
            var properties = typeof(MemberWinLossItem).GetProperties().Where(IsPropertyAbleSumUp).ToList();
            var totalMemberWinLoss = new MemberWinLossItem();
            foreach (var property in properties)
            {
                if (property.Name.EndsWith("ActivePlayers", StringComparison.OrdinalIgnoreCase) || property.Name.EndsWith("BetCount", StringComparison.OrdinalIgnoreCase))
                {
                    var value1 = MemberWinLossByProductList.Sum(g =>
                    {
                        var changeType = Convert.ChangeType(property.GetValue(g, null).ToString(), TypeCode.Int32);
                        return changeType != null ? (int)changeType : 0;
                    });

                    if (property.CanWrite)
                    {
                        property.SetValue(totalMemberWinLoss, Convert.ChangeType(value1, property.PropertyType), null);
                    }

                    continue;
                }

                var value = MemberWinLossByProductList.Sum(g =>
                {
                    var changeType = Convert.ChangeType(property.GetValue(g, null), TypeCode.Decimal);
                    return changeType != null ? (decimal)changeType : 0;
                });

                if (property.CanWrite)
                {
                    property.SetValue(totalMemberWinLoss, Convert.ChangeType(value, property.PropertyType), null);
                }
            }

            var sampleWinLoss = MemberWinLossByProductList.FirstOrDefault();

            if (sampleWinLoss != null)
            {
                totalMemberWinLoss.AffId = sampleWinLoss.AffId;
                totalMemberWinLoss.Username = sampleWinLoss.Username;
                totalMemberWinLoss.PlayerUsername = sampleWinLoss.PlayerUsername;
                totalMemberWinLoss.AffCurrencyId = sampleWinLoss.AffCurrencyId;
                totalMemberWinLoss.PlayerCurrencyId = sampleWinLoss.PlayerCurrencyId;
                totalMemberWinLoss.PlayerCurrency = sampleWinLoss.PlayerCurrency;
                totalMemberWinLoss.ProductName = string.Join(",", MemberWinLossByProductList.Select(t => t.ProductName).Distinct());
                totalMemberWinLoss.OriginalProductName = string.Join(",", MemberWinLossByProductList.Select(t => t.OriginalProductName).Distinct());
                totalMemberWinLoss.WinlostDate = sampleWinLoss.WinlostDate;
            }

            return totalMemberWinLoss;
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