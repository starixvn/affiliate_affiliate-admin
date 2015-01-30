namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.Data.Repository;

    /// <summary>
    /// All System Currency Criteria.
    /// </summary>
    public class AllSystemCurrencyCriteria : CriteriaBase
    {
        /// <summary>
        /// Get Setting Key.
        /// </summary>
        /// <returns>Setting Key.</returns>
        public override string GetSettingKey()
        {
            return "AffAdmin_SystemAllSupportedCurrencies";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}