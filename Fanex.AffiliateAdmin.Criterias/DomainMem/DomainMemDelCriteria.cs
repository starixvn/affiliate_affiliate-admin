namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Domain Member Delete Criteria.
    /// </summary>
    public class DomainMemDelCriteria : CriteriaBase
    {
        public int DomainID { get; set; }

        public int UserID { get; set; }

        public string IP { get; set; }

        public int ErrCode { get; set; }

        public int LicenseeId { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_DomainMemDel";
        }

        public override bool IsValid()
        {
            return DomainID > 0;
        }
    }
}