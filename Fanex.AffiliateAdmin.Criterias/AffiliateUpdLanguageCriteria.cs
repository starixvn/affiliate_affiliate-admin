// -----------------------------------------------------------------------
// <copyright file="AffiliateUpdLanguageCriteria.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AffiliateUpdLanguageCriteria : CriteriaBase
    {
        public int AffId { get; set; }

        public int UserId { get; set; }

        public string PreferredLanguage { get; set; }

        public string Ip { get; set; }

        public string Language { get; set; }

        public int ErrCode { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_PreferLangUpd";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
