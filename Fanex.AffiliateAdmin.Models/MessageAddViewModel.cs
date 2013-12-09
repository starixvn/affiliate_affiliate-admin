// --------------------------------------------------------------------------
// <copyright file="MessageAddViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Johnny.Trinh" date="2013.08.20">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Models
{
    using System.ComponentModel.DataAnnotations;
    using Resources;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MessageAddViewModel
    {
        [Display(Name = "Important", ResourceType = typeof(Lang))]
        public bool Important { get; set; }
    }
}