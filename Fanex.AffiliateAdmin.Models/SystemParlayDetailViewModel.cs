// -----------------------------------------------------------------------
// <copyright file="AccountPasswordViewModel.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Fanex.NAP.Framework.Data;

namespace Fanex.AffiliateAdmin.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using Resources;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SystemParlayDetailViewModel
    {
        [SpParam(Ignored = true)]
        public string AffiliateGroup { get; set; }

        public int UserId { get; set; }

        public long TransId { get; set; }

        public long RefNo { get; set; }

        public long BetId { get; set; }

        public int CustId { get; set; }

        public DateTime MoveBetsDay { get; set; }

        public DateTime BalanceUpDay { get; set; }

        // Output parameters
        public decimal Outstanding { get; set; }

        public DateTime? WinlostDate { get; set; }

        public int AffCurrencyId { get; set; }

        public int PlayerCurrencyId { get; set; }
    }
}
