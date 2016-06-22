namespace Fanex.AffiliateAdmin.DTO
{
    /// <summary>
    /// Currency Statistics.
    /// </summary>
    public class CurrencyStatistics
    {
        public string CurrencyCode { get; set; }

        public short? Currency { get; set; }

        public decimal? Total { get; set; }

        public decimal? TotalMembers { get; set; }
    }
}