// -----------------------------------------------------------------------
// <copyright file="ClickStatisticDetail.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.AffiliateAdmin.DTO
{

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ClickStatisticDetail
    {
        public long TheCount { get; set; }

        public string UrlReferrer { get; set; }

        public int TotalClick { get; set; }

        public int UniqueClick { get; set; }

        public int Signups { get; set; }

        public int FDM { get; set; }
    }
}
