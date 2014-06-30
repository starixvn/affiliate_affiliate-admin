// -----------------------------------------------------------------------
// <copyright file="AccountPasswordCriteria.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;

namespace Fanex.AffiliateAdmin.Criterias
{
    using NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AccountPasswordCriteria : CriteriaBase
    {
        public int CustId { get; set; }
        
        public string OldPassword { get; set; }

        public string NewPassword { get; set; }

        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly")]
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
