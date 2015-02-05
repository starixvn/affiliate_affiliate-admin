namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Domain Affiliate Select Criteria.
    /// </summary>
    public class DomainAffSelCriteria : CriteriaBase
    {
        public override string GetSettingKey()
        {
            return "AffAdmin_DomainAffSel";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}