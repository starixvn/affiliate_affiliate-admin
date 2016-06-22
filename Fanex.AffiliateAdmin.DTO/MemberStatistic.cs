namespace Fanex.AffiliateAdmin.DTO
{
    using System;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MemberStatistic
    {
        public int AffId { get; set; }

        public string Username { get; set; }

        public string AffType { get; set; }

        public string Language { get; set; }

        public string Currency { get; set; }

        public string Status { get; set; }

        public DateTime? DateClick { get; set; }

        public string DateClickStr
        {
            get
            {
                return DateClick.HasValue ? DateClick.Value.ToString(DateFormatManager.DateFormat) : string.Empty;
            }
        }

        public int TotalClick { get; set; }

        public int UniqueClick { get; set; }

        public int Signups { get; set; }

        public int FDM { get; set; }

        public int NonDeposit { get; set; }

        public int ActivePlayers { get; set; }
    }
}
