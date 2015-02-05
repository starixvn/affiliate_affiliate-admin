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

        public int UserId { get; set; }

        public string OldPwd { get; set; }
        
        [SpParam(SqlParamName = "UserPwd")]
        public string Password { get; set; }

        public string Language { get; set; }

        public string Ip { get; set; }

        public int ErrCode { get; set; }

        [SpParam(Ignored = true)]
        public string UserName { get; set; }

        [SpParam(Ignored = true)]
        public string ConfirmPassword { get; set; }

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
