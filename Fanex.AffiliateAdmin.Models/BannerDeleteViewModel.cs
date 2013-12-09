// --------------------------------------------------------------------------
// <copyright file="BannerDeleteViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Leo.Vo" date="2013.8.21">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Models
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class BannerDeleteViewModel
    {
        [Required]
        public string BannerIds { get; set; }

        [Required]
        public string Contents { get; set; }
    }
}