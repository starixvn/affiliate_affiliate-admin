// --------------------------------------------------------------------------
// <copyright file="IPaymentService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Leo.Vo" date="2013.7.31">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Services.Interface
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

        IEnumerable<ProcessingComm> GetProcessingCommissions(ProcessingCommCriteria criteria);

        void UpdateCashBack(CashBackUpdateCriteria criteria);

        string MonthlyTransfer(MonthlyTransferCriteria criteria);

        void CheckMonthlyTransfer(IsMonthlyTransferCriteria criteria);
    }
}