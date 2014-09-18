// --------------------------------------------------------------------------
// <copyright file="AffiliateWinlossByProduct.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2014.9.18">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.DTO
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AffiliateWinLossByProduct
    {
        public int AffId { get; set; }

        public string Username { get; set; }

        public string AffType { get; set; }

        public string Status { get; set; }

        public string Language { get; set; }

        public int GroupID { get; set; }

        public string GroupName { get; set; }

        public DateTime? WinlostDate { get; set; }

        public int CurrencyId { get; set; }

        public string Currency { get; set; }

        public int MergeRowNumber
        {
            get
            {
                var count = AffiliateWinLossByProductList.Count();
                return count > 1 ? count + 1 : count;
            }
        }

        public IEnumerable<AffiliateWinLoss> AffiliateWinLossByProductList { get; set; }

        public AffiliateWinLoss TotalAffiliateWinLossByProduct
        {
            get
            {
                return GetTotalAffiliateWinLoss();
            }
        }

        /// <summary>
        /// Get Total Affiliate Win Loss.
        /// </summary>
        /// <returns>Total Affiliate Win Loss.</returns>
        private AffiliateWinLoss GetTotalAffiliateWinLoss()
        {
            var properties = typeof(AffiliateWinLoss).GetProperties().Where(IsPropertyAbleSumUp).ToList();

            var totalAffiliateWinLoss = new AffiliateWinLoss();

            foreach (var property in properties)
            {
                if (property.Name.EndsWith("ActivePlayers", StringComparison.OrdinalIgnoreCase) || property.Name.EndsWith("BetCount", StringComparison.OrdinalIgnoreCase))
                {
                    var value1 = AffiliateWinLossByProductList.Sum(g =>
                    {
                        var changeType = Convert.ChangeType(property.GetValue(g, null).ToString(), TypeCode.Int32);
                        return changeType != null ? (int)changeType : 0;
                    });

                    if (property.CanWrite)
                    {
                        property.SetValue(totalAffiliateWinLoss, Convert.ChangeType(value1, property.PropertyType), null);
                    }

                    continue;
                }

                var value = AffiliateWinLossByProductList.Sum(g =>
                {
                    var changeType = Convert.ChangeType(property.GetValue(g, null), TypeCode.Decimal);
                    return changeType != null ? (decimal)changeType : 0;
                });

                if (property.CanWrite)
                {
                    property.SetValue(totalAffiliateWinLoss, Convert.ChangeType(value, property.PropertyType), null);
                }
            }

            return totalAffiliateWinLoss;
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