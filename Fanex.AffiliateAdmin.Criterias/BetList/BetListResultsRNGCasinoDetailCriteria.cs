// ----------------------------------------------------------------------
// <copyright file="BetListResultsRNGCasinoDetailCriteria.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Long.Luu" date="2013/12/04">Created</change>
//   <change who="" date=""></change>
// </history>

namespace Fanex.AffiliateAdmin.Criteria
{
    using System;
    using NAP.Framework.Data;

    /// <summary>
    /// Bet List Criteria.
    /// </summary>
    public class BetListResultsRNGCasinoDetailCriteria : CriteriaBase
    {
        public int UserBetId { get; set; }

        public int GameTypeId { get; set; }

        public string GameTypeName { get; set; }

        public int CurrencyId { get; set; }

        public string CurrencyName { get; set; }

        public string PlayerHistoryNo { get; set; }

        public decimal ExchangeRate { get; set; }

        public override bool IsValid()
        {
            return UserBetId > 0 && !string.IsNullOrEmpty(CurrencyName);
        }
    }
}