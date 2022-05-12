namespace Fanex.AffiliateAdmin.Criterias.Configuration.Language
{
    using Fanex.Data.Repository;

    /// <summary>
    /// LanguageSelect Criteria class.
    /// </summary>
    public class LanguageSelectCriteria : BaseCriteria
    {
        public string Name { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_LanguageSelAll";
        }

        public override bool IsValid()
        {
            return !string.IsNullOrEmpty(Name);
        }
    }
}