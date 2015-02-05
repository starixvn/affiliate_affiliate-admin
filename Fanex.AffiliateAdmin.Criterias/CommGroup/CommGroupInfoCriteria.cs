// --------------------------------------------------------------------------
// <copyright file="CommGroupInfoCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Joseph.Pham" date="2013.9.13">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Criteria class.
    /// </summary>
    public class CommGroupInfoCriteria : CriteriaBase
    {
        public int GroupId { get; set; }

        public int AffID { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_CommGroupSelDetail";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
