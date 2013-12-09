// --------------------------------------------------------------------------
// <copyright file="DummyAccIdCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Johnny.Trinh" date="2013.07.24">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class DummyAccIdCriteria : CriteriaBase
    {
        public string Username { get; set; }

        public int CustId { get; set; }

        public override string GetSettingKey()
        {
            return "Aff_DummyAccID";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}