namespace Fanex.AffiliateAdmin.Models
{
    using Fanex.AffiliateAdmin.Resources;
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MemberListViewModel
    {
        private readonly IList<SelectListItem> _dateOptions = new List<SelectListItem>
            {
                new SelectListItem { Value = "0", Text = Lang.All },
                new SelectListItem { Value = "1", Text = Lang.RegisteredDate },
                new SelectListItem { Value = "2", Text = Lang.FirstDepositDate },
            };

        public string Username { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public int DateOption { get; set; }

        public SelectList DateOptionList
        {
            get
            {
                return new SelectList(_dateOptions, "Value", "Text", DateOption.ToString());
            }
        }
    }
}