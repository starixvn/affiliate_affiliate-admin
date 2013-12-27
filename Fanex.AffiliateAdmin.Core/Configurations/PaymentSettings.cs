// --------------------------------------------------------------------------
// <copyright file="PaymentSettings.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Leo.Vo" date="2013.8.19">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Core.Configurations
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Resources;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class PaymentSettings
    {
        public static List<SelectListItem> ListStatus
        {
            get
            {
                return new List<SelectListItem>
                {
                    new SelectListItem { Value = "Pending", Text = Lang.Processing, Selected = true },
                    new SelectListItem { Value = "Approved", Text = Lang.Approved, Selected = true },
                    new SelectListItem { Value = "Rejected", Text = Lang.Rejected },
                    new SelectListItem { Value = "Cancelled", Text = Lang.Cancelled }
                };
            }
        }

        public static List<SelectListItem> ListDateOption
        {
            get
            {
                return new List<SelectListItem>
                {
                    new SelectListItem { Value = "0", Text = Lang.All },
                    new SelectListItem { Value = "1", Text = Lang.TransactionDate },
                    new SelectListItem { Value = "2", Text = Lang.CompletedDate }
                };
            }
        }

        public static List<SelectListItem> ListSortBy
        {
            get
            {
                return new List<SelectListItem>
                {
                    new SelectListItem { Value = "0", Text = Lang.Affiliate },
                    new SelectListItem { Value = "1", Text = Lang.Amount }
                };
            }
        }
    }
}