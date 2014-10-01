// --------------------------------------------------------------------------
// <copyright file="AffiliateCommission.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2014.9.4">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.DTO
{
    using System;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AffiliateCommission
    {
        public long RowIdx { get; set; }

        public int? AffId { get; set; }

        public string Username { get; set; }

        public DateTime? WinlostDate { get; set; }

        public string WinlostDateStr
        {
            get
            {
                return WinlostDate.HasValue ? WinlostDate.Value.ToString(DateFormatManager.DateFormatMonth) : string.Empty;
            }
        }

        public int GroupID { get; set; }

        public string GroupName { get; set; }

        public string Currency { get; set; }

        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public decimal NetWinlost { get; set; }

        public double NetWinlostBase { get; set; }

        public int TotalQM { get; set; }

        public decimal CommissionPercent { get; set; }

        public decimal Commission { get; set; }

        public double CommissionBase { get; set; }

        public decimal TotalPayment { get; set; }

        public double TotalPaymentBase { get; set; }
    }
}