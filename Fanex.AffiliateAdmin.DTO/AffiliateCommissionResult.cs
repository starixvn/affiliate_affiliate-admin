// --------------------------------------------------------------------------
// <copyright file="AffiliateCommissionResult.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2014.9.22">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.DTO
{
    using System.Collections.Generic;
    
    /// <summary>
    /// Affiliate Commission Result.
    /// </summary>
    public class AffiliateCommissionResult
    {
        public IEnumerable<AffiliateCommission> AffiliateCommissionList { get; set; }

        public IEnumerable<AffiliateCommission> AffiliateCommissionInAffiliateCurrency { get; set; }

        public IEnumerable<AffiliateCommission> AffiliateCommissionInSystemCurrency { get; set; } 
    }
}