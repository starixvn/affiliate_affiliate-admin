namespace Fanex.AffiliateAdmin.DTO
{
    using System;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class PaymentInfo
    {
        public long RowID { get; set; }

        public long Transid { get; set; }

        public string AffType { get; set; }

        public DateTime Transdate { get; set; }

        public string TransdateStr
        {
            get
            {
                return Transdate.ToString(DateFormatManager.DateFormatFull);
            }
        }

        public int AffId { get; set; }

        public string Status { get; set; }

        public string OriginalStatus { get; set; }

        public long RefId { get; set; }

        public string Username { get; set; }

        public string Currency { get; set; }

        public decimal Amt { get; set; }

        public decimal ConvertedAmt { get; set; }

        public DateTime Completeddate { get; set; }

        public string CompleteddateStr
        {
            get
            {
                return Completeddate.ToString(DateFormatManager.DateFormatFull);
            }
        }

        public string Creator { get; set; }
    }
}
