// --------------------------------------------------------------------------
// <copyright file="BannerTypeDetailCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2013.9.19">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Banner type detail.
    /// </summary>
    public class BannerTypeDetailCriteria : CriteriaBase
    {
        public int TypeId { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_BannerTypeSel";
        }

        public override bool IsValid()
        {
            return TypeId > 0;
        }
    }
}