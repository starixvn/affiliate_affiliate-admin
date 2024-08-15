namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class GroupInfoUpdCriteria : CriteriaBase
    {
        public int GroupId { get; set; }

        public string GroupName { get; set; }

        [SpParam(SqlParamName = "GroupDesc")]
        public string Description { get; set; }

        public int ErrCode { get; set; }

        public int UserId { get; set; }

        public int LicenseeId { get; set; }

        public override string GetSettingKey()
        {
            return "Aff_Admin_Con_GroupInfoUpd";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}