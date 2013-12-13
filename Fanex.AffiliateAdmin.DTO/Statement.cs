// -----------------------------------------------------------------------
// <copyright file="Statement.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.AffiliateAdmin.DTO
{
    using System;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class Statement
    {
        public long TransId { get; set; }

        public DateTime TransDate { get; set; }

        public DateTime? CompletedDate { get; set; }

        public int TransType { get; set; }

        public double Amount { get; set; }

        public string Status { get; set; }

        public string Creator { get; set; }

        public string Description { get; set; }
    }
}
