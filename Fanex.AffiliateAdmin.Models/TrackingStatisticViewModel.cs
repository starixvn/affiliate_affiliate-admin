// -----------------------------------------------------------------------
// <copyright file="TrackingStatisticViewModel.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.AffiliateAdmin.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web.Mvc;
    using DTO;
    using Fanex.AffiliateAdmin.Resources;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class TrackingStatisticViewModel
    {
        [Display(Name = "From", ResourceType = typeof(Lang))]
        public DateTime FromDate { get; set; }

        [Display(Name = "To", ResourceType = typeof(Lang))]
        public DateTime ToDate { get; set; }

        [Display(Name = "Language", ResourceType = typeof(Lang))]
        public string BaseLanguage { get; set; }

        [Display(Name = "MemberPage", ResourceType = typeof(Lang))]
        public string CustomerPage { get; set; }

        public IList<SystemSetting> CustomerPages { get; set; }

        public IEnumerable<SelectListItem> CustomerPageListItem
        {
            get
            {
                return CustomerPages.Select(custPage => new SelectListItem
                {
                    Value = custPage.SValue,
                    Text = custPage.SValue
                }).ToList();
            }
        }
    }
}
