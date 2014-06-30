// --------------------------------------------------------------------------
// <copyright file="CommGroupInfoCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Joseph.Pham" date="2013.9.13">Created</change>
// </history>

using System.Diagnostics.CodeAnalysis;

namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Criteria class.
    /// </summary>
    public class CommGroupInfoCriteria : CriteriaBase
    {
        public int GroupId { get; set; }

        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly")]
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
