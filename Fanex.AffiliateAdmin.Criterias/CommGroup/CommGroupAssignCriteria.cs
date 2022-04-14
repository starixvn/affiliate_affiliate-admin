namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.Data;
    using Fanex.Data.Repository;

    /// <summary>
    /// Commission Group Assign Criteria.
    /// </summary>
    public class CommGroupAssignCriteria : CriteriaBase
    {
        public int UserId { get; set; }

        public int GroupId { get; set; }

        [SpParam(SqlParamName = "AffIDList")]
        public string AffIds { get; set; }

        [SpParam(Ignored = true)]
        public string Usernames { get; set; }

        [SpParam(Ignored = true)]
        public string Currency { get; set; }

        public string IP { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public long RefNo { get; set; }

        public int LicenseeId { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_CommGroupAssign";
        }

        public override bool IsValid()
        {
            return GroupId > 0 && !string.IsNullOrEmpty(AffIds);
        }
    }
}