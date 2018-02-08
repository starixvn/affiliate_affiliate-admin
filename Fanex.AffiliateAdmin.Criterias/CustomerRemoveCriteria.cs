namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Country Criteria.
    /// </summary>
    public class CustomerRemoveCriteria : CriteriaBase
    {
        public string Username { get; set; }

        public int UserId { get; set; }

        public string IP { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public long RefNo { get; set; }

        public string AdminUserName { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_MemberDel";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}