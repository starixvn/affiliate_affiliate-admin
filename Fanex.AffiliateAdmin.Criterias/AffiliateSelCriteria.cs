// -----------------------------------------------------------------------
// <copyright file="AffiliateSelCriteria.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AffiliateSelCriteria : CriteriaBase
    {
        public int AffId { get; set; }

        public bool GetBalance { get; set; }

        [SpParam(SqlParamName = "aff_minwire")]
        public decimal AffMinWire { get; set; }

        [SpParam(SqlParamName = "aff_minneteller")]
        public decimal AffMinNeteller { get; set; }

        [SpParam(SqlParamName = "aff_mincheque")]
        public decimal AffMinCheque { get; set; }

        [SpParam(SqlParamName = "aff_minmoneybookers")]
        public decimal AffMinMoneyBookers { get; set; }

        [SpParam(SqlParamName = "aff_minplayer")]
        public decimal AffMinPlayer { get; set; }

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
