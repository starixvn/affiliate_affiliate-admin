// -----------------------------------------------------------------------
// <copyright file="AffiliateEditViewModel.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.AffiliateAdmin.Models.AffiliateInfo
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using DTO;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AffiliateEditViewModel
    {
        #region Properties

        public Affiliate Affiliate { get; set; }

        public IEnumerable<SelectListItem> Countries { get; set; }

        public IEnumerable<SelectListItem> PaymentMethodListItem { get; set; }

        #endregion Properties
    }
}