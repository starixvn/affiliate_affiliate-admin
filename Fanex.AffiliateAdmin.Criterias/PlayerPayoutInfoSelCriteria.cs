namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.Data.Repository;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class PlayerPayoutInfoSelCriteria : CriteriaBase
    {
        public long AffId { get; set; }

        public long RefId { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_PlayerPayoutInfoSel";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}