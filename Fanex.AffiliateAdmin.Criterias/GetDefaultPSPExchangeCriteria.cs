namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.Data.Repository;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class GetDefaultPspExchangeCriteria : CriteriaBase
    {
        public string CurrencyCode { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_GetDefaultPSPExchange";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}