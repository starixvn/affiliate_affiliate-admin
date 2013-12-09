// --------------------------------------------------------------------------
// <copyright file="PaymentService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Leo.Vo" date="2013.7.31">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Services.Impl
{
    using System.Collections.Generic;
    using System.Linq;
    using Core.Services;
    using Criterias;
    using DTO;
    using Interface;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class PaymentService : QueryService, IPaymentService
    {
        public IEnumerable<PaymentInfo> GetPayments(PaymentCriteria criteria)
        {
            return GetDataOutput<PaymentInfo>(criteria);
        }

        public PaymentDetail GetPaymentDetail(PaymentDetailCriteria criteria)
        {
            var data = GetData<PaymentDetail>(criteria);
            if (data.Count() > 0)
            {
                return data.First();
            }

            return null;
        }

        public PlayerPayoutInfo GetPlayerPayoutInfo(PlayerPayoutInfoSelCriteria criteria)
        {
            var data = GetData<PlayerPayoutInfo>(criteria);
            if (data.Count() > 0)
            {
                return data.First();
            }

            return null;
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