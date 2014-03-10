// --------------------------------------------------------------------------
// <copyright file="PlayerCurrencyCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Muller.Nguyen" date="2013.07.04">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class PlayerCurrencyCriteria : CriteriaBase
    {
        public override string GetSettingKey()
        {
            return "AffAdmin_PlayersCurrenciesSel";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}