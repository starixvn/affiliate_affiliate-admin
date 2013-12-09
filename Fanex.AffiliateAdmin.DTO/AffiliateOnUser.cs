// -----------------------------------------------------------------------
// <copyright file="AffiliateOnUser.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.AffiliateAdmin.DTO
{
    using System;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AffiliateOnUser
    {
        public int On_UserId { get; set; }

        public int On_SessionId { get; set; }

        public int CustId { get; set; }

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
