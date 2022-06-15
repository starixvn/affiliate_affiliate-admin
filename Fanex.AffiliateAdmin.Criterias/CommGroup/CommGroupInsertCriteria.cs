namespace Fanex.AffiliateAdmin.Criterias
{
    /// <summary>
    /// COMMGroupAddCriteria class.
    /// </summary>
    public class CommGroupInsertCriteria : CommGroupInsertUpdateCriteria
    {
        public string AdminUserName { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_CommGroupIns";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}