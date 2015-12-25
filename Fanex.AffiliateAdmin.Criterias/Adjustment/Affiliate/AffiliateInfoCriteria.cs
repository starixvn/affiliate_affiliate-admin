// -----------------------------------------------------------------------
// <copyright file="AffiliateInfoCriteria.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.Data.Repository;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AffiliateInfoCriteria : CriteriaBase
    {
        public string Username { get; set; }

        public int UserId { get; set; }

        public string Currencies { get; set; }

        public bool IsITAdmin { get; set; }

        public int LicenseeId { get; set; }

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
