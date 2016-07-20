namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Member Site Language Criteria.
    /// </summary>
    public class MemberSiteLanguageCriteria : CriteriaBase
    {
        public string Environment { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_MemberSupportedLanguagesSel";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}