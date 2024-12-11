namespace Fanex.AffiliateAdmin.Core.Options
{
    using Fanex.AffiliateAdmin.Resources;
    using System.Collections.Generic;
    using System.Web.Mvc;

    /// <summary>
    /// Manage the hard-code options.
    /// </summary>
    public static class OptionManager
    {
        #region Define variable

        private static List<SelectListItem> _affiliateStatuses;

        #endregion Define variable

        public static List<SelectListItem> AffiliateStatuses
        {
            get
            {
                _affiliateStatuses = new List<SelectListItem>
                {
                    new SelectListItem { Value = "approved", Text = Lang.Approved },
                    new SelectListItem { Value = "pending", Text = Lang.Pending },
                    new SelectListItem { Value = "disapproved", Text = Lang.Disapproved },
                    new SelectListItem { Value = "closed", Text = Lang.Closed },
                    new SelectListItem { Value = "lockedOut", Text = Lang.Locked }
                };

                return _affiliateStatuses;
            }
        }

        public static List<SelectListItem> DrawFilters
        {
            get
            {
                return new List<SelectListItem>
                {
                    new SelectListItem { Value = "0", Text = Lang.Normal },
                    new SelectListItem { Value = "1", Text = Lang.NoDraw },
                    new SelectListItem { Value = "2", Text = Lang.NoDrawForTurnover }
                };
            }
        }
    }
}