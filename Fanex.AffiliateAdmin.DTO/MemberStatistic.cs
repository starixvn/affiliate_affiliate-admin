// -----------------------------------------------------------------------
// <copyright file="MemberStatistic.cs" company="Nexcel Solution">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.AffiliateAdmin.DTO
{
    using System;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MemberStatistic
    {
        public int AffId { get; set; }

        public string Username { get; set; }

        public string AffType { get; set; }

        public string Language { get; set; }

        public string Currency { get; set; }

        public string Status { get; set; }

        public DateTime? DateClick { get; set; }

        public int TotalClick { get; set; }

        public int UniqueClick { get; set; }

        public int Signups { get; set; }

        public int FDM { get; set; }

        public int NonDeposit { get; set; }

        public int ActivePlayers { get; set; }
    }
}
