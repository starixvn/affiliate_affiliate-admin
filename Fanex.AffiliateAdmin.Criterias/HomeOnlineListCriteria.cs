namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Home Online List Criteria.
    /// </summary>
    public class HomeOnlineListCriteria : PagingCriteriaBase
    {
        public bool IsIT { get; set; }

        public int LicenseeId { get; set; }

        public string UserCurrencies { get; set; }

        /// <summary>
        /// Store procedure Name.
        /// </summary>
        /// <returns>Return Store procedure name.</returns>
        public override string GetSettingKey()
        {
            return "AffAdmin_UserOnlineList";
        }

        /// <summary>
        /// Validation of criteria.
        /// </summary>
        /// <returns>Validate criteria.</returns>
        public override bool IsValid()
        {
            return true;
        }
    }
}
