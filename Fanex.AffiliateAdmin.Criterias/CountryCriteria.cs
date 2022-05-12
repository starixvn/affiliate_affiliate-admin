// -----------------------------------------------------------------------
// <copyright file="CountryCriteria.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.Data.Repository;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class CountryCriteria : BaseCriteria
    {
        public CountryCriteria()
        {
            this.PageSize = 500;
            this.PageIndex = 1;
        }

        public string Name { get; set; }

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
