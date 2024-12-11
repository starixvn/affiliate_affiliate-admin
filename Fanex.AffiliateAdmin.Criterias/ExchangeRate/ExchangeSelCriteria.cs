namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.Data.Repository;
    using System;

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