// -----------------------------------------------------------------------
// <copyright file="SelByCustIDCriteria.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.Data.Repository;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SelByCustIDCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_Con_SelByCustID";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}