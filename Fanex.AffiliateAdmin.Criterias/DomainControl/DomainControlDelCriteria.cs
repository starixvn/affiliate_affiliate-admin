namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Domain Control Delete Criteria.
    /// </summary>
    public class DomainControlDelCriteria : CriteriaBase
    {
        public int Id { get; set; }

        public int UserID { get; set; }

        public string IP { get; set; }

        public int ErrCode { get; set; }

        public int LicenseeId { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_DomainControlDel";
        }

        public override bool IsValid()
        {
            return Id > 0;
        }
    }
}