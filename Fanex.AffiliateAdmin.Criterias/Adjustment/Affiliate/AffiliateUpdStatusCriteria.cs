namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Affiliate Updated Status Criteria.
    /// </summary>
    public class AffiliateUpdStatusCriteria : CriteriaBase
    {
        public int AffId { get; set; }

        public int UserId { get; set; }

        public string Status { get; set; }

        public string Password { get; set; }

        public int GroupID { get; set; }

        public string Ip { get; set; }

        public string Language { get; set; }

        public int ErrCode { get; set; }

        public int LicenseeId { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_AffiliateUpdStatus";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}