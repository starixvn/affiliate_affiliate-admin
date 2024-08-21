namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.Data.Repository;

    public class GroupInfoInsCriteria : CriteriaBase
    {
        public int GroupId { get; set; }

        public string GroupName { get; set; }

        public string GroupDesc { get; set; }

        public int ErrCode { get; set; }

        public int UserId { get; set; }

        public int LicenseeId { get; set; }

        public override string GetSettingKey()
        {
            return "Aff_Admin_Con_GroupInfoIns";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}