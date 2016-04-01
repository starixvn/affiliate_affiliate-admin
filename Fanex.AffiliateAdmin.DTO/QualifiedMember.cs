// -----------------------------------------------------------------------
// <copyright file="QualifiedMember.cs" company="Nexcel Solution">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.AffiliateAdmin.DTO
{
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class QualifiedMember
    {
        public QualifiedMember()
        {
            QualifiedMemberProducts = new Dictionary<int, QualifiedMemberProduct>();
        }

        public string Username { get; set; }

        public string Currency { get; set; }

        public decimal QualifiedAmount { get; set; }

        public double QualifiedAmountBase { get; set; }

        public IDictionary<int, QualifiedMemberProduct> QualifiedMemberProducts { get; private set; }
    }
}
