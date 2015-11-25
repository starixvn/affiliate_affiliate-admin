// -----------------------------------------------------------------------
namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.Data.Repository;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AccountInsCriteria : CriteriaBase
    {
        public string UserName { get; set; }

        public int CustId { get; set; }

        public int UserId { get; set; }

        public string Ip { get; set; }

        public string Language { get; set; }

        public int ErrCode { get; set; }

        [SpParam(SqlParamName = "UserPwd")]
        public string Password { get; set; }

        /// <summary>
        /// Get Setting Key.
        /// </summary>
        /// <returns>Setting Key.</returns>
        public override string GetSettingKey()
        {
            return "AffAdmin_Con_AccountIns";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}