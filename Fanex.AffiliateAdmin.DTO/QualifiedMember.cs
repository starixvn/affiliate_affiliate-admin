// -----------------------------------------------------------------------
// <copyright file="QualifiedMember.cs" company="Nexcel Solution">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.AffiliateAdmin.DTO
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class QualifiedMember
    {
        public string Username { get; set; }

        public string Currency { get; set; }

        public double QualifiedAmountBase { get; set; }

        public decimal QualifiedAmount { get; set; }

        public double SportsbookTurnoverBase { get; set; }

        public double SportsbookTurnover { get; set; }

        public double NumberGameTurnoverBase { get; set; }

        public double NumberGameTurnover { get; set; }

        public double LiveCasinoTurnoverBase { get; set; }

        public double LiveCasinoTurnover { get; set; }

        public double RNGCasinoTurnoverBase { get; set; }

        public double RNGCasinoTurnover { get; set; }

        public double PlaytechCasinoTurnoverBase { get; set; }

        public double PlaytechCasinoTurnover { get; set; }

        public double VirtualSportTurnoverBase { get; set; }

        public double VirtualSportTurnover { get; set; }
    }
}
