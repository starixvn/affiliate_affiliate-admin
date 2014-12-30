// ----------------------------------------------------------------------
// <copyright file="RNGCasinoResultWinlossDetail.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
// <history>
//   <change who="Long.Luu" date="2013/12/04">Created</change>
//   <change who="" date=""></change>
// </history>

namespace Fanex.Affiliate.DTO
{
    /// <summary>
    /// RNGCasinoResult class.
    /// </summary>
    public class RNGCasinoResultWinlossDetail
    {
        public int BetTypeId { get; set; }

        public string BetTypeName { get; set; }

        public decimal Bet { get; set; }

        public decimal Odds { get; set; }

        public decimal Win { get; set; }

        public decimal WinLoss { get; set; }

        public string BetContent { get; set; }

        public int GameTypeId { get; set; }

        public string Currency { get; set; }
    }
}