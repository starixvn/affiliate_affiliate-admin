namespace Fanex.AffiliateAdmin.Criterias
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// Exchange Rate Criteria.
    /// </summary>
    public class GetExchangeRateCriteria : CriteriaBase
    {
        public int FromCurrency { get; set; }

        public int ToCurrency { get; set; }

        public DateTime? Date { get; set; }

        // Output paramaters
        public float Rate { get; set; }

        public override bool IsValid()
        {
            return FromCurrency > 0 && ToCurrency > 0;
        }

        /// <summary>
        /// Get SP name.
        /// </summary>
        /// <returns>Setting key.</returns>
        public override string GetSettingKey()
        {
            return "Aff_User_GetExchangeRate";
        }
    }
}