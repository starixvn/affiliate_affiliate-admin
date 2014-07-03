// -----------------------------------------------------------------------
// <copyright file="EditUserViewModel.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.AffiliateAdmin.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Fanex.AffiliateAdmin.DTO;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class EditUserViewModel
    {
        public int CustID { get; set; }

        [Display(ResourceType = typeof(Resources.Lang), Name = "Username")]
        public string Username { get; set; }
     
        [Required(ErrorMessageResourceType = typeof(Resources.Lang), ErrorMessageResourceName = "ErrFName")]
        [Display(ResourceType = typeof(Resources.Lang), Name = "FirstName")]
        public string FirstName { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Lang), ErrorMessageResourceName = "ErrLName")]
        [Display(ResourceType = typeof(Resources.Lang), Name = "LastName")]
        public string LastName { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Lang), ErrorMessageResourceName = "ErrDepartmentChoose")]
        public int GroupID { get; set; }

        public string GroupName { get; set; }

        public IEnumerable<GroupInfo> GroupList { get; set; }
    }
}
