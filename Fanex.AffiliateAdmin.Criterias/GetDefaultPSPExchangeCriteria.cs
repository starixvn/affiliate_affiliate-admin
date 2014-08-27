// --------------------------------------------------------------------------
// <copyright file="GetDefaultPSPExchangeCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2014.8.26">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class GetDefaultPspExchangeCriteria : CriteriaBase
    {
        public string CurrencyCode { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_GetDefaultPSPExchange";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}