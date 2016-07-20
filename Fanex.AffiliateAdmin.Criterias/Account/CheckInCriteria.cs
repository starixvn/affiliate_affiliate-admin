namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.Data.Repository;

    /// <summary>
    /// CheckIn Criteria.
    /// </summary>
    public class CheckInCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public string NetSessionId { get; set; }

        public int ServerId { get; set; }

        public string IP { get; set; }

        [SpParam(SqlParamName = "On_UserId")]
        public int OnUserId { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_CheckIn";
        }
    }
}