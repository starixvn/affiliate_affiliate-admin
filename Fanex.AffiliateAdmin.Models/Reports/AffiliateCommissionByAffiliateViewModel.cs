namespace Fanex.AffiliateAdmin.Models.Reports
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using Core.Options;
    using Resources;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AffiliateCommissionByAffiliateViewModel
    {
        public AffiliateCommissionByAffiliateViewModel()
        {
            ExcludeTest = true;
            SortBy = 1;
        }

        [Display(Name = "BaseCurrency", ResourceType = typeof(Lang))]
        public byte BaseCurrency { get; set; }

        public SelectList BaseCurrencyOptionList { get; set; }

        [Display(Name = "Currency", ResourceType = typeof(Lang))]
        public int? Currency { get; set; }

        [Display(Name = "DateOption", ResourceType = typeof(Lang))]
        public short? DateOption { get; set; }

        [Display(Name = "ExcludeTestAccounts", ResourceType = typeof(Lang))]
        public bool ExcludeTest { get; set; }

        public MultiSelectList ProductOptionList { get; set; }

        public string Products { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        [Display(Name = "TransferState", ResourceType = typeof(Lang))]
        public byte TransferStatus { get; set; }

        public SelectList TransferStatusOptionList => new SelectList(OptionManager.TransferStatusList, "Value", "Text", TransferStatus);

        public int SortBy { get; set; }

        public SelectList SortByOptionList => new SelectList(OptionManager.SortByForAffiliateCommission, "Value", "Text", SortBy);

        public int ViewMode { get; set; }
    }
}