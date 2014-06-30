// -----------------------------------------------------------------------
// <copyright file="AffiliateOnUser.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.AffiliateAdmin.DTO
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AffiliateOnUser
    {
        [SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores")]
        public int On_UserId { get; set; }

        [SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores")]
        public int On_SessionId { get; set; }

        public int CustId { get; set; }

        [SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores")]
        public DateTime Reload_Time { get; set; }

        public int ServerID { get; set; }

        public string Ip { get; set; }

        public bool IsKickedOut { get; set; }

        public int SubAccId { get; set; }

        public string NetSessionID { get; set; }

        public short Country { get; set; }

        public int MachineIP { get; set; }

        public int DomainID { get; set; }
    }
}
