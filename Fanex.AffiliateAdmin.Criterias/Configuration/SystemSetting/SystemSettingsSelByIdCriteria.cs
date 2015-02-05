namespace Fanex.AffiliateAdmin.Criterias.Configuration.SystemSetting
{
    using Fanex.Data.Repository;

    /// <summary>
    /// System Settings Select By Id Criteria.
    /// </summary>
    public class SystemSettingsSelByIdCriteria : CriteriaBase
    {
        public int ID { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_SystemSettingsSelByID";
        }

        public override bool IsValid()
        {
            return ID > 0;
        }
    }
}