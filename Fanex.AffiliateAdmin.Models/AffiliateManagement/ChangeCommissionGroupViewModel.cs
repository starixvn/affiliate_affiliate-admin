namespace Fanex.AffiliateAdmin.Models.AffiliateManagement
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ChangeCommissionGroupViewModel
    {
        public string AffIds { get; set; }

        public string Usernames { get; set; }

        public int GroupId { get; set; }

        public int UserId { get; set; }

        public string Currency { get; set; }

        public string Status { get; set; }

        public IEnumerable<SelectListItem> CommissionGroupOptionList { get; set; }
    }
}