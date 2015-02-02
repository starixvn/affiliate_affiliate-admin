// --------------------------------------------------------------------------
// <copyright file="ExchangeSelCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2015.2.2">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Criterias
{
    using System;

    /// <summary>
    /// Exchange Select Criteria.
    /// </summary>
    public class ExchangeSelCriteria : SimpleCriteriaBase
    {
        public DateTime? Time { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_ExchangeSel";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}