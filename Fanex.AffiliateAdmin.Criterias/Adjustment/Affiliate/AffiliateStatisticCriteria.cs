// -----------------------------------------------------------------------
// <copyright file="AffiliateStatisticCriteria.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.AffiliateAdmin.Criterias
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AffiliateStatisticCriteria : CriteriaBase
    {
        public int AffId { get; set; }

        public DateTime MonthDate { get; set; }

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

        [SpParam(Ignored = true)]
        public int Month { get; set; }

        [SpParam(Ignored = true)]
        public int Year { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_StatisticsSel";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}