namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Domain Affiliate Delete Criteria.
    /// </summary>
    public class DomainAffDelCriteria : CriteriaBase
    {
        public int DomainID { get; set; }

        public int UserID { get; set; }

        public string IP { get; set; }

        public int ErrCode { get; set; }

        public int LicenseeId { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_DomainAffDel";
        }

        public override bool IsValid()
        {
            return DomainID > 0;
        }
    }
}