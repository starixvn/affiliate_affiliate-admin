// --------------------------------------------------------------------------
// <copyright file="ChangeAffPasswordViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2013.10.4">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using Fanex.AffiliateAdmin.Resources;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ChangeAffPasswordViewModel
    {
        public int Id { get; set; }

        public string Username { get; set; }
        
        [DataType(DataType.Password)]
        [Required(ErrorMessageResourceType = typeof(Lang), ErrorMessageResourceName = "PasswordIsRequired")]
        [Display(ResourceType = typeof(Lang), Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessageResourceType = typeof(Lang), ErrorMessageResourceName = "PassConfirm")]
        [Compare("Password", ErrorMessageResourceName = "PassConfirm", ErrorMessageResourceType = typeof(Lang))]
        [Display(ResourceType = typeof(Lang), Name = "ConfirmPass")]
        public string ConfirmPassword { get; set; }
    }
}
