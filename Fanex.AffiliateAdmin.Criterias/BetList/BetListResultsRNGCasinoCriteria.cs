// --------------------------------------------------------------------------
// <copyright file="BetListResultsRNGCasinoCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2014.11.14">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Criterias
{
    using System;
    using NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class BetListResultsRNGCasinoCriteria : CriteriaBase
    {
        public string BetType
        {
            get
            {
                return "Casino";
            }
        }

        public long RefNo { get; set; }

        public bool IsShowPic { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string ExternalId { get; set; }

        public string SystemId { get; set; }

        public string UserName
        {
            get
            {
                return SystemId;
            }
        }

        public int CustId { get; set; }

        public int CurrencyId { get; set; }

        public string CurrencyName { get; set; }

        public string BaseCurrency { get; set; }

        public decimal ExchangeRate { get; set; }

        public int AffCurrencyId { get; set; }

        public string AffCurrencyCode { get; set; }

        public override bool IsValid()
        {
            return RefNo > 0
                && FromDate <= ToDate && CustId > 0
                && !string.IsNullOrEmpty(UserName)
                && !string.IsNullOrEmpty(BetType);
        }
    }
}
