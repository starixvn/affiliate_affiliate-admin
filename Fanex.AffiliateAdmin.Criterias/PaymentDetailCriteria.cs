namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.Data.Repository;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class PaymentDetailCriteria : CriteriaBase
    {
        public long TransId { get; set; }

        public int LicenseeId { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_PaymentRequestSelDetail";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}