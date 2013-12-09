// -----------------------------------------------------------------------
// <copyright file="GroupViewModel.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.AffiliateAdmin.Models
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class GroupViewModel
    {
        public int GroupId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Lang), ErrorMessageResourceName = "ErrDepartment")]
        [Display(ResourceType = typeof(Resources.Lang), Name = "GroupName")]
        public string GroupName { get; set; }

        [Display(ResourceType = typeof(Resources.Lang), Name = "Description")]
        public string Description { get; set; }
    }
}
