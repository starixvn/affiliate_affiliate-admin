namespace Fanex.AffiliateAdmin.Criterias.Configuration.SystemSetting
{
    using Fanex.Data.Repository;

    /// <summary>
    /// SystemSettingUpdate Criteria class.
    /// </summary>
    public class SystemSettingExchangeCriteria : CriteriaBase
    {
        public override string GetSettingKey()
        {
            return "AffAdmin_SystemSettingsExchange";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}