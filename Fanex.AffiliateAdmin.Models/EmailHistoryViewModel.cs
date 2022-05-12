namespace Fanex.AffiliateAdmin.Models
{
    using System;

    /// <summary>
    /// Email History ViewModel.
    /// </summary>
    public class EmailHistoryViewModel
    {
        public EmailHistoryViewModel()
        {
            SearchOption = -1;
            IsAll = true;
        }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public bool IsAll { get; set; }

        public string SearchText { get; set; }

        public int SearchOption { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }
    }
}