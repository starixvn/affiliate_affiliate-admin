namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.Data.Repository;

    public class GroupInfoDelCriteria : CriteriaBase
    {
        public int GroupId { get; set; }

        public int LicenseeId { get; set; }

        public int ErrCode { get; set; }

        public override string GetSettingKey()
        {
            return "Aff_Admin_Con_GroupInfoDel";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}