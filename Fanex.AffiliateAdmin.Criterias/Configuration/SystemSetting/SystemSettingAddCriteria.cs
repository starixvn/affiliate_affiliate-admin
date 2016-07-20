namespace Fanex.AffiliateAdmin.Criterias.Configuration.SystemSetting
{
    using System.ComponentModel.DataAnnotations;
    using Fanex.AffiliateAdmin.Resources;
    using Fanex.Data.Repository;

    /// <summary>
    /// SystemSettingAdd Criteria class.
    /// </summary>
    public class SystemSettingAddCriteria : CriteriaBase
    {
        public int Updatedby { get; set; }

        [Required(ErrorMessageResourceType = typeof(Lang), ErrorMessageResourceName = "KeyFieldIsRequired")]
        public string Key { get; set; }

        [Required(ErrorMessageResourceType = typeof(Lang), ErrorMessageResourceName = "ValueFieldIsRequired")]
        public string Value { get; set; }

        public string Type { get; set; }

        public string Description { get; set; }

        public string Environment { get; set; }

        public string IP { get; set; }

        public int ID { get; set; }

        public string Language { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public int LicenseeId { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_SystemSettingsIns";
        }

        public override bool IsValid()
        {
            return !string.IsNullOrEmpty(Key) && !string.IsNullOrEmpty(Value);
        }
    }
}