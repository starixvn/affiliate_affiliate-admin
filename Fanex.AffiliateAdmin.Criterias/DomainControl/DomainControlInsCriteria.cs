namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Domain Control Insert Criteria.
    /// </summary>
    public class DomainControlInsCriteria : CriteriaBase
    {
        public int AffDomainID { get; set; }

        public int MemberDomainID { get; set; }

        public string CountryID { get; set; }

        public bool Status { get; set; }

        public int UserID { get; set; }

        public string IP { get; set; }

        public int ErrCode { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_DomainControlIns";
        }

        public override bool IsValid()
        {
            return AffDomainID > 0 && MemberDomainID > 0;
        }
    }
}