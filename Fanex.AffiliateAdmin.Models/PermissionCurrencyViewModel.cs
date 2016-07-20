namespace Fanex.AffiliateAdmin.Models
{
    using System.Collections.Generic;
    using DTO;
    using Fanex.AffiliateAdmin.Core.Utilities;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class PermissionCurrencyViewModel
    {
        public int CustId { get; set; }

        public IEnumerable<PermissionCurrency> ListPermissionCurrency { get; set; }

        public IEnumerable<Menu> Menus { get; set; }
    }
}