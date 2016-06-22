namespace Fanex.AffiliateAdmin.Criterias.Configuration.SystemSetting
{
    using Fanex.Data.Repository;

    /// <summary>
    /// SystemSettingSelect Criteria class.
    /// </summary>
    public class SystemSettingSelectCriteria : CriteriaBase
    {
        public string Type { get; set; }

        public string Key { get; set; }

        public string Value { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public int TotalPages { get; set; }

        public int NRows { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_SystemSettingsSelAll";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}