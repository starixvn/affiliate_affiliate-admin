namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Domain Control Update Criteria.
    /// </summary>
    public class DomainControlUpdCriteria : CriteriaBase
    {
        public int ID { get; set; }

        public int AffDomainID { get; set; }

        public int MemberDomainID { get; set; }

        public string CountryID { get; set; }

        public bool Status { get; set; }

        public int UserID { get; set; }

        public string IP { get; set; }

        public string Language { get; set; }

        public int ErrCode { get; set; }

        public int LicenseeId { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_DomainControlUpd";
        }

        public override bool IsValid()
        {
            return ID > 0;
        }
    }
}