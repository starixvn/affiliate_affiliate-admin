// -----------------------------------------------------------------------
// <copyright file="ChangePasswordCriteria.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.Data.Repository;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ChangePasswordCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public string UserName { get; set; }

        public int UserId { get; set; }
        
        [SpParam(SqlParamName = "UserPwd")]
        public string Password { get; set; }

        [SpParam(Ignored = true)]
        public string ConfirmPassword { get; set; }

        public string Ip { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_AccountUpdPwd";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
