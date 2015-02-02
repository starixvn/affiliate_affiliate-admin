namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Group Info Delete Criteria.
    /// </summary>
    public class GroupInfoDelCriteria : CriteriaBase
    {
        public int GroupId { get; set; }

        public string Language { get; set; }

        public int ErrCode { get; set; }

        public int UserId { get; set; }

        public string Ip { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_Con_GroupInfoDel";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}