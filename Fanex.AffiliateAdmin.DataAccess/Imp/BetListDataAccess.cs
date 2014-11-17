// --------------------------------------------------------------------------
// <copyright file="BetListDataAccess.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2014.11.14">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.DataAccess
{
    using System.Collections;
    using Criterias;
    using Data;
    using Fanex.AffiliateAdmin.DTO;

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

        /// <summary>
        /// Get player's username.
        /// </summary>
        /// <param name="playerUsername">Affiliate username.</param>
        /// <returns>Return player's Info.</returns>
        public CustomerInfoBetList GetPlayerIbcInfo(string playerUsername)
        {
            var hashParam = new Hashtable
            { 
                { "username", playerUsername },
                { "custid", 0 },
                { "ibcusername", string.Empty },
                { "currencyid", 0 }
            };
            var objectDb = new ObjectDb("Aff_User_DummyAccID");
            objectDb.ExecuteNonQuery(hashParam);

            return new CustomerInfoBetList
            {
                CustId = int.Parse(hashParam["custid"].ToString()),
                Username = playerUsername,
                IBCUsername = hashParam["ibcusername"].ToString(),
                CurrencyId = int.Parse(hashParam["currencyid"].ToString())
            };
        }
    }
}
