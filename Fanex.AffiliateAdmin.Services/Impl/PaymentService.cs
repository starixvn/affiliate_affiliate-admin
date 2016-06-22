namespace Fanex.AffiliateAdmin.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using Core.Services;
    using Criterias;
    using DTO;
    using Fanex.AffiliateAdmin.Resources;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class PaymentService : QueryService, IPaymentService
    {
        /// <summary>
        /// Get payments.
        /// </summary>
        /// <param name="criteria">Payment criteria.</param>
        /// <returns>Payment List.</returns>
        public IEnumerable<PaymentInfo> GetPayments(PaymentCriteria criteria)
        {
            var result = GetDataOutput<PaymentInfo>(criteria);

            foreach (var payment in result)
            {
                var affType = Lang.ResourceManager.GetString(payment.AffType);
                payment.AffType = string.IsNullOrEmpty(affType) ? payment.AffType : affType;

                payment.OriginalStatus = payment.Status;
                var status = Lang.ResourceManager.GetString(payment.Status);
                payment.Status = string.IsNullOrEmpty(status) ? payment.Status : status;
            }

            return result;
        }

        public PaymentDetail GetPaymentDetail(PaymentDetailCriteria criteria)
        {
            return GetEntity<PaymentDetail>(criteria);
        }

        public PlayerPayoutInfo GetPlayerPayoutInfo(PlayerPayoutInfoSelCriteria criteria)
        {
            return GetEntity<PlayerPayoutInfo>(criteria);
        }

        public int GetCustIDByUsername(DummyAccIdCriteria criteria)
        {
            ExecuteOutput(criteria);
            return criteria.CustId;
        }

        public void UpdatePayment(PaymentUpdateCriteria criteria)
        {
            ExecuteOutput(criteria);
        }
    }
}