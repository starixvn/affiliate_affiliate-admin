// -----------------------------------------------------------------------
// <copyright file="StatementSettings.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.AffiliateAdmin.Core.Configurations
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Fanex.AffiliateAdmin.Resources;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class StatementSettings
    {
        public static List<SelectListItem> ListStatus
        {
            get
            {
                return new List<SelectListItem>
                {
                    new SelectListItem { Value = "Processing", Text = Lang.Processing, Selected = true },
                    new SelectListItem { Value = "Approved,B/F", Text = Lang.ApprovedBF, Selected = true },
                    new SelectListItem { Value = "Rejected", Text = Lang.Rejected },
                    new SelectListItem { Value = "Cancelled", Text = Lang.Cancelled }
                };
            }
        }
    }
}
