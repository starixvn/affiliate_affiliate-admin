namespace Fanex.AffiliateAdmin.Models.UserManagement
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
    }
}
