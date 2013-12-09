// -----------------------------------------------------------------------
// <copyright file="UserManagementViewModel.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
// --------------------------------------------------------------------------
// <history>
//   <change who="Muller.Nguyen" date="2013.09.01">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Models
{
    using System.Collections.Generic;
    using DTO;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class UserManagementViewModel
    {
        public int GroupId { get; set; }

        public IEnumerable<Account> AccountList { get; set; }

        public string KeyWord { get; set; }

        public string FirstLetter { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public int TotalPages { get; set; }
    }
}
