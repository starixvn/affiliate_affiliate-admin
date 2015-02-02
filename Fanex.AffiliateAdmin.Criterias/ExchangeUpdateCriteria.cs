namespace Fanex.AffiliateAdmin.Criterias
{
    /// <summary>
    /// Exchange Update Criteria.
    /// </summary>
    public class ExchangeUpdateCriteria : SimpleCriteriaBase
    {
        public int UserId { get; set; }

        public int ExchangeId { get; set; }

        public double Rate { get; set; }

        public string ToCurrency { get; set; }

        public string IP { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_ExchangeUpdate";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}