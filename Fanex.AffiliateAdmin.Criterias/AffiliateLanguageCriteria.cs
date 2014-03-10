// -----------------------------------------------------------------------
// <copyright file="AffiliateLanguageCriteria.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AffiliateLanguageCriteria : CriteriaBase
    {
        public override string GetSettingKey()
        {
            return "AffAdmin_AffiliateSupportedLanguagesSel";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
