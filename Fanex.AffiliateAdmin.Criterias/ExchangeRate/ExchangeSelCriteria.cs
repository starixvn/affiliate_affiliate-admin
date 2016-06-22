namespace Fanex.AffiliateAdmin.Criterias
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// Exchange Select Criteria.
    /// </summary>
    public class ExchangeSelCriteria : CriteriaBase
    {
        public DateTime? Time { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_ExchangeSel";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}