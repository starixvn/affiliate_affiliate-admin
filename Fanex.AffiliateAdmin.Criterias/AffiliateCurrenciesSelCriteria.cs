// --------------------------------------------------------------------------
// <copyright file="AffiliateCurrenciesSelCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2015.2.2">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Exchange Criteria.
    /// </summary>
    public class AffiliateCurrenciesSelCriteria : CriteriaBase
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