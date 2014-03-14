// --------------------------------------------------------------------------
// <copyright file="ExchangeCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.06.14">Created</change>
// </history>
namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Exchange Criteria.
    /// </summary>
    public class AffiliateCurrencySelCriteria : CriteriaBase
    {
        /// <summary>
        /// Get Setting Key.
        /// </summary>
        /// <returns>Setting Key.</returns>
        public override string GetSettingKey()
        {
            return "AffAdmin_AffiliateCurrenciesSel";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}