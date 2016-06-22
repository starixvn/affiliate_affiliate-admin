namespace Fanex.AffiliateAdmin.DTO
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class PspExchange
    {
        public short ExchangeID { get; set; }

        public string Code { get; set; }

        public double? DepositPercent { get; set; }

        public double? WithdrawalPercent { get; set; }
    }
}