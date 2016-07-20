namespace Fanex.AffiliateAdmin.Criterias.Configuration.Language
{
    using Fanex.Data.Repository;

    /// <summary>
    /// LanguageAdd Criteria class.
    /// </summary>
    public class LanguageAddCriteria : CriteriaBase
    {
        public int Updatedby { get; set; }

        public string LanguageID { get; set; }

        public string LanguageName { get; set; }

        public string CountryID { get; set; }

        public string IP { get; set; }

        public string Language { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_LanguageIns";
        }

        public override bool IsValid()
        {
            return !string.IsNullOrEmpty(LanguageID);
        }
    }
}