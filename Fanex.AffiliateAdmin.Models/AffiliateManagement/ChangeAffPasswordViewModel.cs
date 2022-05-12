namespace Fanex.AffiliateAdmin.Models.AffiliateManagement
{
    using System.ComponentModel.DataAnnotations;
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
        [Required(ErrorMessageResourceType = typeof(Lang), ErrorMessageResourceName = "ConfirmPasswordIsRequired")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessageResourceName = "PassConfirm", ErrorMessageResourceType = typeof(Lang))]
        [Display(ResourceType = typeof(Lang), Name = "ConfirmPass")]
        public string ConfirmPassword { get; set; }
    }
}
