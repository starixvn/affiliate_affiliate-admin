namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Domain Member Update Criteria.
    /// </summary>
    public class DomainMemUpdCriteria : CriteriaBase
    {
        public int DomainID { get; set; }

        public string DomainName { get; set; }

        public int UserID { get; set; }

        public string IP { get; set; }

        public int ErrCode { get; set; }

        public string AdminUserName { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_DomainMemUpd";
        }

        public override bool IsValid()
        {
            return DomainID > 0;
        }
    }
}