namespace Fanex.AffiliateAdmin.Models.AffiliateInfo
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Fanex.AffiliateAdmin.Resources;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ReferrerViewModel
    {
        [Display(ResourceType = typeof(Lang), Name = "FromDate")]
        public DateTime? FromDate { get; set; }

        [Display(ResourceType = typeof(Lang), Name = "ToDate")]
        public DateTime? ToDate { get; set; }
    }
}
