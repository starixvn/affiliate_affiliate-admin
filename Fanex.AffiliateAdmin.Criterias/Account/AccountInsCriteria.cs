namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class AccountInsCriteria : CriteriaBase
    {
        public string UserName { get; set; }

        public int CustId { get; set; }

        public int UserId { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        [SpParam(SqlParamName = "UserPwd")]
        public string Password { get; set; }

        public string UserSalt { get; set; }

        public int LicenseeId { get; set; }

        public override string GetSettingKey()
        {
            return "Aff_Admin_Con_AccountIns";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}