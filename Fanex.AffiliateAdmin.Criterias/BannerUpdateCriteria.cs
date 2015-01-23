// --------------------------------------------------------------------------
// <copyright file="BannerUpdateCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Leo.Vo" date="2013.8.21">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.Data.Repository;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class BannerUpdateCriteria : CriteriaBase
    {
        public int BannerId { get; set; }

        public string Content { get; set; }

        public short Category { get; set; }

        [SpParam(SqlParamName = "BannerType")]
        public short AffType { get; set; }

        public short ImageType { get; set; }

        public string Language { get; set; }

        public short Currency { get; set; }

        public string Size { get; set; }

        public int UserId { get; set; }

        public string Ip { get; set; }

        public int ErrCode { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_BannerUpd";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}