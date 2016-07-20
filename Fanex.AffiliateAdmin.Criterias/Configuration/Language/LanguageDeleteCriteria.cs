namespace Fanex.AffiliateAdmin.Criterias.Configuration.Language
{
    using Fanex.Data.Repository;

    /// <summary>
    /// LanguageDelete Criteria class.
    /// </summary>
    public class LanguageDeleteCriteria : CriteriaBase
    {
        public int Updatedby { get; set; }

        public string IDList { get; set; }

        public string IP { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_LanguageDel";
        }

        public override bool IsValid()
        {
            return !string.IsNullOrEmpty(IDList);
        }
    }
}