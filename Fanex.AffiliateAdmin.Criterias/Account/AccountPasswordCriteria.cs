// -----------------------------------------------------------------------
// <copyright file="AccountPasswordCriteria.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.Data.Repository;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AccountPasswordCriteria : CriteriaBase
    {
        public int CustId { get; set; }
        
        public string OldPassword { get; set; }

        public string NewPassword { get; set; }

        public string Ip { get; set; }

        public int ErrCode { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_AdminUpdPwd";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
