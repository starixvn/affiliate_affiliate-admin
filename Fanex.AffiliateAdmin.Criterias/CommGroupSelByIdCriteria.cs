// -----------------------------------------------------------------------
// <copyright file="CommGroupSelByIdCriteria.cs" company="Nexcel Solution">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class CommGroupSelByIdCriteria : CriteriaBase
    {
        public int GroupID { get; set; }

        public int UserId { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_CommGroupSelByID";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
