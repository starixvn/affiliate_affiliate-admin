namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.Data.Repository;
    using System;

    /// <summary>
    /// Commission group detail criteria class.
    /// </summary>
    public class TierSettingCriteria : CriteriaBase
    {
        public string GroupIDList { get; set; }

        public DateTime? Date { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_CommGroupList_SettingDetail";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}