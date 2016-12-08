// -----------------------------------------------------------------------
// <copyright file="AffiliateSelCriteria.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.Data.Repository;

    public class AffiliateSelCriteria : CriteriaBase
    {
        public int AffId { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_AffiliateSelEditInfo";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}