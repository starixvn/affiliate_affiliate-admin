namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.Data.Repository;
    using System;

    /// <summary>
    /// DeductionSetting Criteria class.
    /// </summary>
    public class DeductionSettingCriteria : CriteriaBase
    {
        public string GroupIDList { get; set; }

        public DateTime? Date { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_DeductionSettingSel";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}