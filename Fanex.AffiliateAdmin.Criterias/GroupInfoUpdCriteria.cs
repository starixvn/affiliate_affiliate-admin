namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Group Info Update Criteria.
    /// </summary>
    public class GroupInfoUpdCriteria : CriteriaBase
    {
        public int GroupId { get; set; }

        public string GroupName { get; set; }

        [SpParam(SqlParamName = "GroupDesc")]
        public string Description { get; set; }

        public string Language { get; set; }

        public int ErrCode { get; set; }

        public int UserId { get; set; }

        public string Ip { get; set; }

        public int LicenseeId { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_Con_GroupInfoUpd";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}