namespace Fanex.AffiliateAdmin.DTO
{
    using System.Collections.Generic;

    /// <summary>
    /// Member Win Loss By Product Result.
    /// </summary>
    public class MemberWinLossByProductResult
    {
        public IEnumerable<MemberWinLossItem> MemberWinlossList { get; set; }

        public IEnumerable<MemberWinLossItem> TotalMemberWinlossInMemberCurrencies { get; set; }

        public IEnumerable<MemberWinLossItem> TotalMemberWinlossInSystemCurrency { get; set; }

        public double Duration { get; set; }
    }
}