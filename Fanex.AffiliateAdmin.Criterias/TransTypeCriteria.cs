// -----------------------------------------------------------------------
// <copyright file="TransTypeCriteria.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.Data.Repository;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class TransTypeCriteria : CriteriaBase
    {
        public override string GetSettingKey()
        {
            return "AffAdmin_TransTypeSel";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
