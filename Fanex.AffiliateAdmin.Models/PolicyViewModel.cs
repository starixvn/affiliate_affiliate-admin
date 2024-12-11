namespace Fanex.AffiliateAdmin.Models
{
    using DTO;
    using Fanex.AffiliateAdmin.Core.Utilities;
    using System.Collections.Generic;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class PolicyViewModel
    {
        public int CustId { get; set; }

        public int GroupId { get; set; }

        public IEnumerable<FunctionPolicy> PolicyUserList { get; set; }

        public IEnumerable<Menu> Menus { get; set; }
    }
}