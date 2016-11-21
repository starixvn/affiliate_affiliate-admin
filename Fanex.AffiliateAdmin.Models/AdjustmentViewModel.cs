// -----------------------------------------------------------------------
// <copyright file="AdjustmentViewModel.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.AffiliateAdmin.Models
{
    using System.ComponentModel.DataAnnotations;
    using Fanex.AffiliateAdmin.DTO;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AdjustmentViewModel
    {
        public int AffId { get; set; }

        public string AffName { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Lang), ErrorMessageResourceName = "RequiredAmount")]
        [Display(ResourceType = typeof(Resources.Lang), Name = "Amount")]
        public decimal Amount { get; set; }

        public string Description { get; set; }

        public Adjustment Adjustment { get; set; }

        public string CurrencyCode { get; set; }
    }
}
