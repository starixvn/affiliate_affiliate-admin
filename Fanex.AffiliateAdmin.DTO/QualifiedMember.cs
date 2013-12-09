// -----------------------------------------------------------------------
// <copyright file="QualifiedMember.cs" company="Nexcel Solution">
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
    public class QualifiedMember
    {
        public string Username { get; set; }

        public string Currency { get; set; }

        public decimal QualifiedAmount { get; set; }

        public double SportsbookTurnover { get; set; }

        public double NumberGameTurnover { get; set; }

        public double LiveCasinoTurnover { get; set; }

        public double RNGCasinoTurnover { get; set; }
    }
}
