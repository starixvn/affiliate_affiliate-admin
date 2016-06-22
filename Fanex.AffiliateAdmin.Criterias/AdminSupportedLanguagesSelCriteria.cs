namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Language criteria class.
    /// </summary>
    public class AdminSupportedLanguagesSelCriteria : CriteriaBase
    {
        /// <summary>
        /// Get store procedure name by key.
        /// </summary>
        /// <returns>Store procedure name.</returns>
        public override string GetSettingKey()
        {
            return "AffAdmin_AdminSupportedLanguagesSel";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}