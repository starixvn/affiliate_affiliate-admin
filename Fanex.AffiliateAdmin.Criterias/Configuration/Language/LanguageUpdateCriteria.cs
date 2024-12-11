namespace Fanex.AffiliateAdmin.Criterias.Configuration.Language
{
    using Fanex.Data.Repository;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// LanguageUpdate Criteria class.
    /// </summary>
    public class LanguageUpdateCriteria : CriteriaBase
    {
        public int Updatedby { get; set; }

        [Required]
        public string LanguageID { get; set; }

        [Required]
        public string LanguageName { get; set; }

        public string CountryID { get; set; }

        public string IP { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_LanguageUpd";
        }

        public override bool IsValid()
        {
            return !string.IsNullOrEmpty(LanguageID)
                   && !string.IsNullOrEmpty(LanguageName)
                   && !string.IsNullOrEmpty(CountryID);
        }
    }
}