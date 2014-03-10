// -----------------------------------------------------------------------
// <copyright file="PermissionCurrencyCriteria.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class PermissionCurrencyCriteria : CriteriaBase
    {
        public int UserId { get; set; }

        public string CurrencyXML { get; set; }

        public int ModifyBy { get; set; }

        public string Langkey { get; set; }

        public string Ip { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public CriteriaType Type { get; set; }

        /// <summary>
        /// Store procedure Name.
        /// </summary>
        /// <returns>Return Store procedure name.</returns>
        public override string GetSettingKey()
        {
            if (this.Type == CriteriaType.Select)
            {
                return "AffAdmin_UserCurrencySel";
            }
            else
            {
                return "AffAdmin_UserCurrencyIns";
            }
        }

        /// <summary>
        /// Validation for this criteria.
        /// </summary>
        /// <returns>Validate criteria.</returns>
        public override bool IsValid()
        {
            return true;
        }
    }
}
