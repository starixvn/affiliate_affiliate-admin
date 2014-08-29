// --------------------------------------------------------------------------
// <copyright file="EmailTemplateType.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Johnny.Trinh" date="2013.08.22">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Core.Enumerations
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public enum EmailTemplateType : int
    {
        /// <summary>
        /// Affiliate none.
        /// </summary>
        None,

        /// <summary>
        /// Affiliate approve.
        /// </summary>
        AffiliateApprove = 1,

        /// <summary>
        /// Affiliate Cancel.
        /// </summary>
        AffiliateCancel = 3,

        /// <summary>
        /// Affiliate Disapprove.
        /// </summary>
        AffiliateDisapprove = 4,

        /// <summary>
        /// Payment Approve.
        /// </summary>
        PaymentApprove = 5,

        /// <summary>
        /// Payment Cancel.
        /// </summary>
        PaymentCancel = 6,

        /// <summary>
        /// Withdrawal money.
        /// </summary>
        Withdrawal = 7
    }
}