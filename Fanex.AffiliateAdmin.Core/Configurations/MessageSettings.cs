namespace Fanex.AffiliateAdmin.Core.Configurations
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Fanex.AffiliateAdmin.Resources;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class MessageSettings
    {
        public static List<SelectListItem> ListSearchBy
        {
            get
            {
                return new List<SelectListItem>
                {
                    new SelectListItem { Value = string.Empty, Text = Lang.All },
                    new SelectListItem { Value = "0", Text = Lang.Normal },
                    new SelectListItem { Value = "1", Text = Lang.Important }
                };
            }
        }
    }
}