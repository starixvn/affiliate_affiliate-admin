// --------------------------------------------------------------------------
// <copyright file="ExchangeRateViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2014.3.7">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Models
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ExchangeRateViewModel
    {
        public int ExchangeId { get; set; }

        public double Rate { get; set; }

        public string CodeFrom { get; set; }

        public string CodeTo { get; set; }
    }
}