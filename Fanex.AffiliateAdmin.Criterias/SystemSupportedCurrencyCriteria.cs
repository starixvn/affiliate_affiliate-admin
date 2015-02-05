namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.Data.Repository;

    /// <summary>
    /// System Supported Currency Criteria.
    /// </summary>
    public class SystemSupportedCurrencyCriteria : CriteriaBase
    {
        public int UserId { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_SystemSupportedCurrencies";
        }

        public override bool IsValid()
        {
            return UserId > 0;
        }
    }
}