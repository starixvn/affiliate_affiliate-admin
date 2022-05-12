// -----------------------------------------------------------------------
// <copyright file="EditUserViewModel.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.AffiliateAdmin.Models.UserManagement
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using DTO;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class EditUserViewModel
    {
        public int CustID { get; set; }

        [Display(ResourceType = typeof(Resources.Lang), Name = "Username")]
        public string Username { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Lang), ErrorMessageResourceName = "ErrDepartmentChoose")]
        public int GroupID { get; set; }

        public string GroupName { get; set; }

        public IEnumerable<GroupInfo> GroupList { get; set; }
    }
}
