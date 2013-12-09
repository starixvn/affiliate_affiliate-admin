// --------------------------------------------------------------------------
// <copyright file="Banner.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Leo.Vo" date="2013.8.28">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.DTO
{
    using System;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class Banner
    {
        public int BannerID { get; set; }

        public string Content { get; set; }

        public string Thumbnail { get; set; }

        public string Category { get; set; }

        public string Language { get; set; }

        public string LanguageCode { get; set; }

        public string ImageType { get; set; }

        public string Size { get; set; }

        public DateTime? Lastupdate { get; set; }

        public string Currency { get; set; }

        public byte? Type { get; set; }

        public int? Version { get; set; }

        public long? TotalRequest { get; set; }

        public DateTime? LastRequest { get; set; }
    }
}