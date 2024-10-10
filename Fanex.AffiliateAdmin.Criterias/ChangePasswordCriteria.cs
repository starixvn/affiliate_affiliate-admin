// -----------------------------------------------------------------------
// <copyright file="ChangePasswordCriteria.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.Data.Repository;

    public class ChangePasswordCriteria : CriteriaBase
    {
        public int CustId { get; set; }
        
        public string UserPwd { get; set; }

        public string UserSalt { get; set; }

        public int LicenseeId { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey()
        {
            return "Aff_Admin_AccountUpdPwd";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
