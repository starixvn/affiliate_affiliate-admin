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

        public decimal QualifiedAmountBase { get; set; }

        public double QualifiedAmount { get; set; }

        public decimal SportsbookTurnoverBase { get; set; }

        public double SportsbookTurnover { get; set; }

        public decimal NumberGameTurnoverBase { get; set; }

        public double NumberGameTurnover { get; set; }

        public decimal LiveCasinoTurnoverBase { get; set; }

        public double LiveCasinoTurnover { get; set; }

        public decimal RNGCasinoTurnoverBase { get; set; }

        public double RNGCasinoTurnover { get; set; }
    }
}
