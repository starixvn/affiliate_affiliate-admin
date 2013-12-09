// --------------------------------------------------------------------------
// <copyright file="BannerDetail.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Leo.Vo" date="2013.8.30">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.DTO
{
    using System;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class BannerDetail
    {
        public int BannerID { get; set; }

        public string Content { get; set; }

        public short? Category { get; set; }

        public short? ParentID { get; set; }

        public string Language { get; set; }

        public short? ImageType { get; set; }

        public short Size { get; set; }

        public DateTime? Lastupdate { get; set; }

        public short? Currency { get; set; }

        public byte? Type { get; set; }

        public int? Version { get; set; }

        public long? TotalRequest { get; set; }

        public DateTime? LastRequest { get; set; }

        public short GetCategory()
        {
            return Category != null ? Category.Value : ParentID.Value;
        }
    }
}