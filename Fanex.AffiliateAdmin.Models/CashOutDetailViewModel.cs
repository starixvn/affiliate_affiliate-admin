namespace Fanex.AffiliateAdmin.Models
{
    public class CashOutDetailViewModel
    {
        public long RefNo { get; set; }

        public decimal ExchangeRate { get; set; }

        public long TransId { get; set; }

        public long OriginalTransId { get; set; }

        public int BetType { get; set; }
    }
}