// --------------------------------------------------------------------------
// <copyright file="BetListViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2014.11.13">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Models
{
    using System;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class BetListViewModel
    {
        public int AffId { get; set; }

        public string Username { get; set; }

        public string Product { get; set; }

        public DateTime? WinlostDate { get; set; }

        public int AffCurrencyId { get; set; }

        public string AffCurrencyCode { get; set; }

        public int PlayerCurrencyId { get; set; }

        public decimal ExchangeRate { get; set; }
    }
}
