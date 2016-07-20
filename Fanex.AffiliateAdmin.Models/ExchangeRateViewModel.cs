namespace Fanex.AffiliateAdmin.Models
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ExchangeRateViewModel
    {
        public int ExchangeId { get; set; }
        
        public double Rate { get; set; }

        public string CodeFrom { get; set; }

        public string CodeTo { get; set; }
    }
}