namespace Fanex.AffiliateAdmin.Criterias
{
    /// <summary>
    /// Copy Rate From Market Criteria.
    /// </summary>
    public class CopyRateFromMarketCriteria : SimpleCriteriaBase
    {
        public int UserId { get; set; }

        public string ExchangeIdList { get; set; }

        public string IP { get; set; }

        public int LicenseeId { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_CopyRateFromMarket";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}