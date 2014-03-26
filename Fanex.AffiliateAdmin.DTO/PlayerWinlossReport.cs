// -----------------------------------------------------------------------
// <copyright file="PlayerWinlossReport.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.AffiliateAdmin.DTO
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class PlayerWinlossReport
    {
        public IEnumerable<PlayerWinloss> PlayerWinlossList { get; set; }

        public IEnumerable<PlayerWinloss> PlayerWinlossInMemberCurrencies { get; set; }

        public PlayerWinloss PlayerWinlossInSystemCurrency { get; set; }

        public PlayerWinlossReport()
        {
            PlayerWinlossList = new List<PlayerWinloss>();
            PlayerWinlossInMemberCurrencies = new List<PlayerWinloss>();
        }
    }
}
