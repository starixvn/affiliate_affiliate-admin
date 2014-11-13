// --------------------------------------------------------------------------
// <copyright file="AffiliateDataAccess.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Joseph.Pham" date="2013.8.1">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.DataAccess
{
    using Criterias;
    using Data;

    /// <summary>
    /// Account data access.
    /// </summary>
    public class BetListDataAccess : IBetListDataAccess
    {
        public decimal GetExchangeRate(GetExchangeRateCriteria criteria)
        {
            using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
            {
                var param = criteria.GetSpParams();
                objectDb.ExecuteNonQuery(param);

                return string.IsNullOrEmpty(param["rate"].ToString()) ? 1 : decimal.Parse(param["rate"].ToString());
            }
        }
    }
}
