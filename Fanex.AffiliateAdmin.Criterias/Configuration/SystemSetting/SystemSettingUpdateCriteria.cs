namespace Fanex.AffiliateAdmin.Criterias.Configuration.SystemSetting
{
    using Fanex.AffiliateAdmin.Resources;
    using Fanex.Data.Repository;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// SystemSettingUpdate Criteria class.
    /// </summary>
    public class SystemSettingUpdateCriteria : CriteriaBase
    {
        public int Updatedby { get; set; }

        public int ID { get; set; }

        [Required(ErrorMessageResourceType = typeof(Lang), ErrorMessageResourceName = "ValueFieldIsRequired")]
        public string Value { get; set; }

        public string Type { get; set; }

        public string Description { get; set; }

        public string Environment { get; set; }

        public string IP { get; set; }

        public string Language { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_SystemSettingsUpd";
        }

        public override bool IsValid()
        {
            return !string.IsNullOrEmpty(Value);
        }
    }
}