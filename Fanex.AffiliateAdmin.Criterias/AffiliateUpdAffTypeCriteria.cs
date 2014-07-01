// --------------------------------------------------------------------------
// <copyright file="AffiliateUpdAffTypeCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Johnny.Trinh" date="2013.11.19">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Criterias
{
    using System.Diagnostics.CodeAnalysis;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Affiliate Update Affiliate Type Criteria.
    /// </summary>
    public class AffiliateUpdAffTypeCriteria : CriteriaBase
    {
        public int UserId { get; set; }

        public int AffId { get; set; }

        public short AffType { get; set; }

        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly")]
        public string Ip { get; set; }

        public string Language { get; set; }

        public int ErrCode { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_UpdAffType";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
