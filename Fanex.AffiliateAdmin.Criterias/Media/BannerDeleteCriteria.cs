// --------------------------------------------------------------------------
// <copyright file="BannerDeleteCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Leo.Vo" date="2013.8.30">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.Data.Repository;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class BannerDeleteCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "ListBannerID")]
        public string BannerIds { get; set; }

        public int ErrCode { get; set; }

        public int UserId { get; set; }

        public string Ip { get; set; }

        public int LicenseeId { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_BannerDel";
        }

        public override bool IsValid()
        {
            return !string.IsNullOrEmpty(BannerIds);
        }
    }
}