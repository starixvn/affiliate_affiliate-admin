namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Domain Control Select Criteria.
    /// </summary>
    public class DomainControlSelCriteria : CriteriaBase
    {
        public string CountryID { get; set; }

        public int LicenseeId { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_DomainControlSel";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}