namespace Fanex.AffiliateAdmin.Criterias
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// Commission group criteria class.
    /// </summary>
    public class CommGroupCriteria : PagingCriteriaBase
    {
        public string GroupName { get; set; }

        public string Currencies { get; set; }

        public DateTime Date { get; set; }

        public int UserId { get; set; }

        public int GroupID { get; set; }

        public string GroupIDList { get; set; }

        public int LicenseeId { get; set; }

        public string UserCurrencies { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_CommGroupSel";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}