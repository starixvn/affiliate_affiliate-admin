// --------------------------------------------------------------------------
// <copyright file="PSPExchange.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2014.8.29">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.DTO
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class PspExchange
    {
        public short ExchangeID { get; set; }

        public string Code { get; set; }

        public double? DepositPercent { get; set; }

        public double? WithdrawalPercent { get; set; }
    }
}