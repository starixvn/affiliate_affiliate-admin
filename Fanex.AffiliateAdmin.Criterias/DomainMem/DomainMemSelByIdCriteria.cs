namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Domain Member Select By Id Criteria.
    /// </summary>
    public class DomainMemSelByIdCriteria : CriteriaBase
    {
        public int DomainId { get; set; }

        public int LicenseeId { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_DomainMemSelById";
        }

        public override bool IsValid()
        {
            return DomainId > 0;
        }
    }
}