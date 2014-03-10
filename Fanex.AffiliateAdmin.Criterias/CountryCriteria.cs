// -----------------------------------------------------------------------
// <copyright file="CountryCriteria.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class CountryCriteria : CriteriaBase
    {
        public CountryCriteria()
        {
            this.PageSize = 500;
            this.PageIndex = 1;
        }

        public string Name { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public int TotalPages { get; set; }

        public int NRows { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_CountrySelAll";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
