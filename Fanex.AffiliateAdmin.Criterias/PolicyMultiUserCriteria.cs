// -----------------------------------------------------------------------
// <copyright file="PolicyMultiUserCriteria.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.Data.Repository;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class PolicyMultiUserCriteria : CriteriaBase
    {
        public string CustIds { get; set; }

        public int GroupId { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_Con_Batch_PolicySel";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
