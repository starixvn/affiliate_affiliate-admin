// --------------------------------------------------------------------------
// <copyright file="PaymentInfo.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Leo.Vo" date="2013.7.31">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.DTO
{
    using System;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class PaymentInfo
    {
        public long RowID { get; set; }

        public long Transid { get; set; }

        public string AffType { get; set; }

        public DateTime Transdate { get; set; }

        public string TransdateStr
        {
            get
            {
                return Transdate.ToString(DateFormatManager.DateFormatFull);
            }
        }

        public int AffId { get; set; }

        public string Status { get; set; }

        public long RefId { get; set; }

        public string Username { get; set; }

        public string Currency { get; set; }

        public decimal Amt { get; set; }

        public decimal ConvertedAmt { get; set; }

        public DateTime Completeddate { get; set; }

        public string CompleteddateStr
        {
            get
            {
                return Completeddate.ToString(DateFormatManager.DateFormatFull);
            }
        }

        public string Creator { get; set; }
    }
}
