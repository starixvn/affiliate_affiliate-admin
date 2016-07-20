namespace Fanex.AffiliateAdmin.Criteria
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// Bet List Criteria.
    /// </summary>
    public class BetListResultsRNGCasinoDetailCriteria : CriteriaBase
    {
        public int UserBetId { get; set; }

        public int GameTypeId { get; set; }

        public string GameTypeName { get; set; }

        public int CurrencyId { get; set; }

        public string CurrencyName { get; set; }

        public string PlayerHistoryNo { get; set; }

        public decimal ExchangeRate { get; set; }

        public override bool IsValid()
        {
            return UserBetId > 0 && !string.IsNullOrEmpty(CurrencyName);
        }
    }
}