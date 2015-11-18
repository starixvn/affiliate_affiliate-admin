namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Domain Affiliate Select By Id Criteria.
    /// </summary>
    public class DomainAffSelByIdCriteria : CriteriaBase
    {
        public int DomainId { get; set; }

        public int LicenseeId { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_DomainAffSelById";
        }

        public override bool IsValid()
        {
            return DomainId > 0;
        }
    }
}