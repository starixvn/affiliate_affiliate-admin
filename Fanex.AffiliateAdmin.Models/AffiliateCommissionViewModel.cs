namespace Fanex.AffiliateAdmin.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.AffiliateAdmin.Core.Options;
    using Resources;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AffiliateCommissionViewModel
    {
        public AffiliateCommissionViewModel()
        {
            ExcludeTest = true;
            GroupBy = 1;
            SortBy = 1;
        }

        public MultiSelectList AffiliateCurrencyOptionList { get; set; }

        [Display(Name = "AffType", ResourceType = typeof(Lang))]
        public short? AffType { get; set; }

        public MultiSelectList AffTypeOptionList { get; set; }

        [Display(Name = "BaseCurrency", ResourceType = typeof(Lang))]
        public byte BaseCurrency { get; set; }

        public SelectList BaseCurrencyOptionList { get; set; }

        [Display(Name = "CommGroup", ResourceType = typeof(Lang))]
        public string CommGroup { get; set; }

        public MultiSelectList CommissionGroupOptionList { get; set; }

        [Display(Name = "Currency", ResourceType = typeof(Lang))]
        public int? Currency { get; set; }

        [Display(Name = "DateOption", ResourceType = typeof(Lang))]
        public short? DateOption { get; set; }

        [Display(Name = "ExcludeTestAccounts", ResourceType = typeof(Lang))]
        public bool ExcludeTest { get; set; }

        [Display(Name = "Language", ResourceType = typeof(Lang))]
        public string Language { get; set; }

        public MultiSelectList PreferLanguageOptionList { get; set; }

        public MultiSelectList ProductOptionList { get; set; }

        public string Products { get; set; }

        [Display(Name = "Affiliate", ResourceType = typeof(Lang))]
        public string Username { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        [Display(Name = "TransferState", ResourceType = typeof(Lang))]
        public byte TransferStatus { get; set; }

        public SelectList TransferStatusOptionList => new SelectList(OptionManager.TransferStatusList, "Value", "Text", TransferStatus);

        [Display(Name = "GroupBy", ResourceType = typeof(Lang))]
        public byte GroupBy { get; set; }

        public SelectList GroupByOptionList => new SelectList(OptionManager.GroupByList, "Value", "Text", GroupBy);

        public byte SortBy { get; set; }

        public SelectList SortByOptionList => new SelectList(OptionManager.SortByForAffiliateCommission.OrderBy(t => t.Text), "Value", "Text", SortBy);
    }
}