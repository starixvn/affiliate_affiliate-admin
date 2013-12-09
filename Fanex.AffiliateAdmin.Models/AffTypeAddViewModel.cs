// --------------------------------------------------------------------------
// <copyright file="AffTypeAddViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2013.9.11">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Models
{
    using System.ComponentModel.DataAnnotations;
    using Resources;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AffTypeAddViewModel
    {
        [Required(ErrorMessageResourceType = typeof(Lang), ErrorMessageResourceName = "NameFieldIsRequired")]
        public string Name { get; set; }

        public string Description { get; set; }
    }
}
