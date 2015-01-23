// --------------------------------------------------------------------------
// <copyright file="BannerTypeCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Leo.Vo" date="2013.8.19">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.Data.Repository;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class BannerTypeCriteria : CriteriaBase
    {
        public BannerTypeCriteria()
        {
            Language = "en-US";
            ParentType = null;
        }

        public string TypeName { get; set; }

        public string Language { get; set; }

        public short? ParentType { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_BannerSelType";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}