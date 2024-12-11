namespace Fanex.AffiliateAdmin.Services
{
    using Criterias;
    using DTO;
    using Models;
    using System.Collections.Generic;

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

        ResultViewModel CheckPaymentInProgress(PaymentCheckInProgressCriteria criteria);
    }
}