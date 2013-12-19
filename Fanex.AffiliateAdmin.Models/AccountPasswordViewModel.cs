// -----------------------------------------------------------------------
// <copyright file="AccountPasswordViewModel.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.AffiliateAdmin.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using Resources;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AccountPasswordViewModel
    {
        [DataType(DataType.Password)]
        [Required(ErrorMessageResourceType = typeof(Lang), ErrorMessageResourceName = "OldRequire")]
        [Display(ResourceType = typeof(Lang), Name = "OldPass")]
        public string OldPassword { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessageResourceType = typeof(Lang), ErrorMessageResourceName = "PasswordIsRequired")]
        [Display(ResourceType = typeof(Lang), Name = "PassNew")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessageResourceType = typeof(Lang), ErrorMessageResourceName = "PassConfirm")]
        [Display(ResourceType = typeof(Lang), Name = "ConfirmPass")]
        public string ConfirmPassword { get; set; }
    }
}
