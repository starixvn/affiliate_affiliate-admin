// --------------------------------------------------------------------------
// <copyright file="ReCheckInCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.06.07">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Criterias
{
    using System.Diagnostics.CodeAnalysis;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Re-CheckIn Criteria.
    /// </summary>
    [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly")]
    public class ReCheckInCriteria : CriteriaBase
    {
        public int OnUserID { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_Recheckin";
        }
    }
}