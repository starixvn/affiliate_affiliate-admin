namespace Fanex.AffiliateAdmin.DTO
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AffiliateCommissionByProduct
    {
        public long RowIdx { get; set; }

        public int? AffId { get; set; }

        public DateTime? WinlostDate { get; set; }

        public string Username { get; set; }

        public string Currency { get; set; }

        public int? GroupID { get; set; }

        public string GroupName { get; set; }

        public decimal? TotalPayment { get; set; }

        public double? TotalPaymentBase { get; set; }

        public int MergeRowNumber
        {
            get
            {
                var count = AffiliateCommissionByProductList.Count();
                return count > 1 ? count + 1 : count;
            }
        }

        public IEnumerable<AffiliateCommission> AffiliateCommissionByProductList { get; set; }

        public AffiliateCommission TotalAffiliateCommissionByProduct
        {
            get
            {
                return GetTotalAffiliateCommission();
            }
        }

        /// <summary>
        /// Get Total Affiliate Win Loss.
        /// </summary>
        /// <returns>Total Affiliate Win Loss.</returns>
        private AffiliateCommission GetTotalAffiliateCommission()
        {
            var properties = typeof(AffiliateCommission).GetProperties().Where(IsPropertyAbleSumUp).ToList();

            var totalAffiliateCommission = new AffiliateCommission();

            foreach (var property in properties)
            {
                if (property.Name.EndsWith("TotalQM", StringComparison.OrdinalIgnoreCase))
                {
                    var value1 = AffiliateCommissionByProductList.Sum(g =>
                    {
                        var changeType = Convert.ChangeType(property.GetValue(g, null).ToString(), TypeCode.Int32);
                        return changeType != null ? (int)changeType : 0;
                    });

                    if (property.CanWrite)
                    {
                        property.SetValue(totalAffiliateCommission, Convert.ChangeType(value1, property.PropertyType), null);
                    }

                    continue;
                }

                var value = AffiliateCommissionByProductList.Sum(g =>
                {
                    var changeType = Convert.ChangeType(property.GetValue(g, null), TypeCode.Decimal);
                    return changeType != null ? (decimal)changeType : 0;
                });

                if (property.CanWrite)
                {
                    property.SetValue(totalAffiliateCommission, Convert.ChangeType(value, property.PropertyType), null);
                }
            }

            return totalAffiliateCommission;
        }

        /// <summary>
        /// Check property can sum up or not.
        /// </summary>
        /// <param name="p">Property Input.</param>
        /// <returns>Can/Can not sum up.</returns>
        private bool IsPropertyAbleSumUp(PropertyInfo p)
        {
            var uncalculatedPropertyList = new List<string> { "ExRate", "AffExRate", "CommissionPercent" };

            if (uncalculatedPropertyList.Any(t => t.Equals(p.Name, StringComparison.OrdinalIgnoreCase)))
            {
                return false;
            }

            var typeList = new List<Type> { typeof(int), typeof(decimal), typeof(double), typeof(float), typeof(byte) };

            return typeList.Any(t => t == p.PropertyType);
        }
    }
}