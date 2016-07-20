namespace Fanex.AffiliateAdmin.Services
{
    using System.Collections.Generic;
    using Criterias;
    using DTO;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IPaymentService
    {
        IEnumerable<PaymentInfo> GetPayments(PaymentCriteria criteria);

        PaymentDetail GetPaymentDetail(PaymentDetailCriteria criteria);

        PlayerPayoutInfo GetPlayerPayoutInfo(PlayerPayoutInfoSelCriteria criteria);

        int GetCustIDByUsername(DummyAccIdCriteria criteria);

        void UpdatePayment(PaymentUpdateCriteria criteria);
    }
}