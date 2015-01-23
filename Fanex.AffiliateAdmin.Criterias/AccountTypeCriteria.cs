// --------------------------------------------------------------------------
// <copyright file="AccountTypeCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2013.10.4">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Account Type Criteria.
    /// </summary>
    public class AccountTypeCriteria : CriteriaBase
    {
        public bool IsIT { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_AccountTypeSel";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}