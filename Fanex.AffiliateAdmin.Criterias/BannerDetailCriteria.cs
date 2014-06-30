// --------------------------------------------------------------------------
// <copyright file="BannerDetailCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Leo.Vo" date="2013.8.30">Created</change>
// </history>

using System.Diagnostics.CodeAnalysis;

namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class BannerDetailCriteria : CriteriaBase
    {
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly")]
        public int BannerId { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_BannerSelDetail";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}