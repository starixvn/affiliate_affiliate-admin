namespace Fanex.AffiliateAdmin.Criterias.Configuration.Language
{
    using Fanex.Data.Repository;

    /// <summary>
    /// LanguageSelect Criteria class.
    /// </summary>
    public class LanguageSelectCriteria : CriteriaBase
    {
        public string Name { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public int TotalPages { get; set; }

        public int NRows { get; set; }

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