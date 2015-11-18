namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Domain Member Select Criteria.
    /// </summary>
    public class DomainMemSelCriteria : CriteriaBase
    {
        public int LicenseeId { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_DomainMemSel";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}