namespace Fanex.AffiliateAdmin.Criterias.Configuration.SystemSetting
{
    using Fanex.Data.Repository;

    /// <summary>
    /// System Settings Select By Type Criteria.
    /// </summary>
    public class SystemSettingsSelByTypeCriteria : CriteriaBase
    {
        public string Type { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_SystemSettingsSelByType";
        }

        public override bool IsValid()
        {
            return !string.IsNullOrEmpty(Type);
        }
    }
}