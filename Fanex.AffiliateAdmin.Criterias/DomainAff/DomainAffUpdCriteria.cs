namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Domain Affiliate Update Criteria.
    /// </summary>
    public class DomainAffUpdCriteria : CriteriaBase
    {
        public int DomainID { get; set; }

        public string DomainName { get; set; }

        public int UserID { get; set; }

        public string IP { get; set; }

        public int ErrCode { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_DomainAffUpd";
        }

        public override bool IsValid()
        {
            return DomainID > 0;
        }
    }
}