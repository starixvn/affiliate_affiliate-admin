// --------------------------------------------------------------------------
// <copyright file="PlayerWinlossReport.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2014.3.26">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.DTO
{
    using System.Collections.Generic;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class PlayerWinlossReport
    {
        public PlayerWinlossReport()
        {
            PlayerWinlossList = new List<PlayerWinloss>();
            PlayerWinlossInMemberCurrencies = new List<PlayerWinloss>();
        }

        public IEnumerable<PlayerWinloss> PlayerWinlossList { get; set; }

        public IEnumerable<PlayerWinloss> PlayerWinlossInMemberCurrencies { get; set; }

        public PlayerWinloss PlayerWinlossInSystemCurrency { get; set; }
    }
}
