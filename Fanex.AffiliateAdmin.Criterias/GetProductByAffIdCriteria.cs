namespace Fanex.AffiliateAdmin.Criterias
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// Exchange Rate Criteria.
    /// </summary>
    public class GetProductByAffIdCriteria : CriteriaBase
    {
        public GetProductByAffIdCriteria()
        {
            MonthDate = null;
            Language = "en-US";
        }

        public int AffId { get; set; }

        public DateTime? MonthDate { get; set; }

        public string Language { get; set; }

        public override bool IsValid()
        {
            return AffId > 0;
        }

        /// <summary>
        /// Get SP name.
        /// </summary>
        /// <returns>Setting key.</returns>
        public override string GetSettingKey()
        {
            return "Aff_User_ProductSelByAffID";
        }
    }
}