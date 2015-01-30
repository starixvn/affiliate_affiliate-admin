namespace Fanex.AffiliateAdmin.Criterias.Configuration.SystemSetting
{
    using Fanex.Data.Repository;

    /// <summary>
    /// System Settings Select By Name Criteria.
    /// </summary>
    public class SystemSettingsSelByNameCriteria : CriteriaBase
    {
        public string Name { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_SystemSettingsSelByName";
        }

        public override bool IsValid()
        {
            return !string.IsNullOrEmpty(Name);
        }
    }
}