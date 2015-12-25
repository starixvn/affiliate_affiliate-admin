namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Group Info Insert Criteria.
    /// </summary>
    public class GroupInfoInsCriteria : CriteriaBase
    {
        public string GroupName { get; set; }

        public string GroupDesc { get; set; }

        public string Language { get; set; }

        public int ErrCode { get; set; }

        public int UserId { get; set; }

        public string Ip { get; set; }

        public int LicenseeId { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_Con_GroupInfoIns";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}