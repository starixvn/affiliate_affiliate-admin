namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.Data.Repository;

    /// <summary>
    /// COMMGroupAddCriteria class.
    /// </summary>
    public class CommGroupDeleteCriteria : CriteriaBase
    {
        public int UserId { get; set; }

        public int GroupId { get; set; }

        public string IP { get; set; }

        public int ErrCode { get; set; }

        public int LicenseeId { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_CommGroupDel";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}