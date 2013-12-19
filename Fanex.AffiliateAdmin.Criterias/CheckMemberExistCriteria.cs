// -----------------------------------------------------------------------
// <copyright file="CheckMemberExistCriteria.cs" company="Nexcel Solution Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class CheckMemberExistCriteria : CriteriaBase
    {
        public string MemberName { get; set; }

        public string AffName { get; set; }

        public bool IsMemberExist { get; set; }

        public string AffStatus { get; set; }

        public int AffID { get; set; }

        [SpParam(Ignored = true)]
        public bool ByAffiliate { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_CheckMemberExist";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
