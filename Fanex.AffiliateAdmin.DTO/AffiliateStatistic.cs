// -----------------------------------------------------------------------
// <copyright file="AffiliateStatistic.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.AffiliateAdmin.DTO
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AffiliateStatistic
    {
        public int TotalActivePlayer { get; set; }

        public int TotalPlayerDepUp { get; set; }

        public int NewSignUpDep { get; set; }

        public int NewSignUp { get; set; }

        public int TotalPlayer { get; set; }

        public decimal BalBroughtforward { get; set; }

        public decimal TotalDeposit { get; set; }

        public decimal TotalDepositUp { get; set; }

        public decimal TotalTurnover { get; set; }

        public decimal TotalTurnoverUp { get; set; }

        public decimal TotalWinlostUp { get; set; }

        public decimal TotalWinlost { get; set; }

        public int TotalUniqueClicks { get; set; }

        public int TotalClicks { get; set; }
    }
}
