// --------------------------------------------------------------------------
// <copyright file="MessageViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Leo.Vo" date="2013.8.21">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Resources;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MessageViewModel
    {
        [Display(Name = "SearchBy", ResourceType = typeof(Lang))]
        public short? SearchBy { get; set; }

        public bool DateChecked { get; set; }

        [Display(Name = "From", ResourceType = typeof(Lang))]
        public DateTime FromDate { get; set; }

        [Display(Name = "To", ResourceType = typeof(Lang))]
        public DateTime ToDate { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }
    }
}