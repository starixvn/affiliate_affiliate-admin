namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.Data;
    using Fanex.Data.Repository;

    /// <summary>
    /// Affiliate Updated Closed Status Criteria.
    /// </summary>
    public class AffiliateUpdClosedStatusCriteria : CriteriaBase
    {
        public int AffId { get; set; }

        [SpParam(Ignored = true)]
        public string AffName { get; set; }

        public int UserId { get; set; }

        public bool Closed { get; set; }

        public string Ip { get; set; }

        public string Language { get; set; }

        public int ErrCode { get; set; }

        public int LicenseeId { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_AffiliateUpdClosedStatus";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}