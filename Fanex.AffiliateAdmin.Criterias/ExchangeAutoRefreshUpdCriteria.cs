namespace Fanex.AffiliateAdmin.Criterias
{
    /// <summary>
    /// Exchange Auto Refresh Update Criteria.
    /// </summary>
    public class ExchangeAutoRefreshUpdCriteria : SimpleCriteriaBase
    {
        public int UserId { get; set; }

        public int ExchangeId { get; set; }

        public string IP { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_ExchangeAutoRefreshUpd";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}