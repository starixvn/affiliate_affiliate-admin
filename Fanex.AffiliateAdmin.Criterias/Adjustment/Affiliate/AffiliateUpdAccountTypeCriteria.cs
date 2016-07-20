namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Affiliate Update Account Type Criteria.
    /// </summary>
    public class AffiliateUpdAccountTypeCriteria : CriteriaBase
    {
        public string AffIds { get; set; }

        [SpParam(Ignored = true)]
        public string Usernames { get; set; }

        public byte TestLevel { get; set; }

        public int UserId { get; set; }

        public string Ip { get; set; }

        public int ErrCode { get; set; }

        public int LicenseeId { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_AffiliateUpdAccountType";
        }

        public override bool IsValid()
        {
            return !string.IsNullOrEmpty(AffIds);
        }
    }
}