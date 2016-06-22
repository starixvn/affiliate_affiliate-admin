namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.Data.Repository;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class CheckMemberExistCriteria : CriteriaBase
    {
        public string MemberName { get; set; }

        public string AffName { get; set; }

        public bool IsMemberExist { get; set; }

        public string AffStatus { get; set; }

        public int AffID { get; set; }

        public int MemberAffID { get; set; }

        [SpParam(Ignored = true)]
        public bool ByAffiliate { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_CheckMemberExist";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
