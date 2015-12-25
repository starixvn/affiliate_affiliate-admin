// -----------------------------------------------------------------------
// <copyright file="GroupListCriteria.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.Data.Repository;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class GroupListCriteria : CriteriaBase
    {
        public int? GroupId { get; set; }

        public bool IsIt { get; set; }

        public int LicenseeId { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_Con_GroupInfoSel";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
