namespace Fanex.AffiliateAdmin.Core.Utilities
{
    using System.Collections.Generic;

    /// <summary>
    /// List Menu item.
    /// </summary>
    public class Menu
    {
        public string Text { get; set; }

        public string Link { get; set; }

        public List<Menu> SubMenu { get; set; }
    }
}
