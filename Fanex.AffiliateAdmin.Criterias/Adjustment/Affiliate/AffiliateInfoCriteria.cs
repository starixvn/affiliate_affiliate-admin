// -----------------------------------------------------------------------
// <copyright file="AffiliateInfoCriteria.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.Data.Repository;

    public class AffiliateInfoCriteria : CriteriaBase
    {
        public string Username { get; set; }

        public bool IsITAdmin { get; set; }

        public string UserCurrencies { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_AffiliateSelGeneralInfo";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
