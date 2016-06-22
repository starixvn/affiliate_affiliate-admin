namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Exchange Criteria.
    /// </summary>
    public class AffiliateCurrenciesSelCriteria : CriteriaBase
    {
        /// <summary>
        /// Get Setting Key.
        /// </summary>
        /// <returns>Setting Key.</returns>
        public override string GetSettingKey()
        {
            return "AffAdmin_AffiliateCurrenciesSel";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}