namespace Fanex.AffiliateAdmin.Criterias.Configuration.Language
{
    using Fanex.Data.Repository;

    /// <summary>
    /// LanguageDetail Criteria class.
    /// </summary>
    public class LanguageDetailCriteria : CriteriaBase
    {
        public string ID { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_LanguageSelByID";
        }

        public override bool IsValid()
        {
            return !string.IsNullOrEmpty(ID);
        }
    }
}