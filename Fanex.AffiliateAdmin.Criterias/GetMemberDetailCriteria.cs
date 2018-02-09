namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.Data.Repository;

    public class GetMemberDetailCriteria : CriteriaBase
    {
        public string UserName { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_GetMemberDetail";
        }

        public override bool IsValid()
        {
            return !string.IsNullOrEmpty(UserName);
        }
    }
}
