namespace Fanex.AffiliateAdmin.DTO
{
    using System.Collections.Generic;

    /// <summary>
    /// Member Win Loss By Overview Result.
    /// </summary>
    public class MemberWinLossByOverviewResult
    {
        public IEnumerable<MemberWinLossItem> MemberWinlossList { get; set; }

        public IEnumerable<MemberWinLossItem> TotalMemberWinlossInMemberCurrencies { get; set; }

        public MemberWinLossItem TotalMemberWinlossInSystemCurrency { get; set; }

        public double Duration { get; set; }
    }
}