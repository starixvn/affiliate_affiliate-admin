// -----------------------------------------------------------------------
// <copyright file="TrackingStatisticDetailViewModel.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.AffiliateAdmin.Models.AffiliateInfo
{
    using System;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class TrackingStatisticDetailViewModel
    {
        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string Domain { get; set; }

        public string SportCode { get; set; }

        public string LanguageCode { get; set; }
    }
}
