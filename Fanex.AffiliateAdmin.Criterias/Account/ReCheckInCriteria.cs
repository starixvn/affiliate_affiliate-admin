namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.Data;
    using Fanex.Data.Repository;

    /// <summary>
    /// Re-CheckIn Criteria.
    /// </summary>
    public class ReCheckInCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "On_UserID")]
        public int OnUserID { get; set; }

        public bool IsKickedOut { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_Recheckin";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}