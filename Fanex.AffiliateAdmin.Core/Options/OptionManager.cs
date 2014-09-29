// --------------------------------------------------------------------------
// <copyright file="OptionManager.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2014.3.14">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Core.Options
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Fanex.AffiliateAdmin.Resources;

    /// <summary>
    /// Manage the hard-code options.
    /// </summary>
    public static class OptionManager
    {
        #region Define variable

        private static List<SelectListItem> _affiliateStatuses;
        private static List<SelectListItem> _drawFilters;

        #endregion Define variable

        public static List<SelectListItem> AffiliateStatuses
        {
            get
            {
                if (_affiliateStatuses == null)
                {
                    _affiliateStatuses = new List<SelectListItem>
                    {
                        new SelectListItem { Value = "approved", Text = "Approved" },
                        new SelectListItem { Value = "pending", Text = "Pending" },
                        new SelectListItem { Value = "disapproved", Text = "Disapproved" },
                        new SelectListItem { Value = "closed", Text = "Closed" },
                        new SelectListItem { Value = "lockedOut", Text = "Locked" }
                    };
                }

                return _affiliateStatuses;
            }
        }

        public static List<SelectListItem> DrawFilters
        {
            get
            {
                if (_drawFilters == null)
                {
                    _drawFilters = new List<SelectListItem>
                    {
                        new SelectListItem { Value = "0", Text = Lang.Normal },
                        new SelectListItem { Value = "1", Text = Lang.NoDraw },
                        new SelectListItem { Value = "2", Text = Lang.NoDrawForTurnover }
                    };
                }

                return _drawFilters;
            }
        }
    }
}