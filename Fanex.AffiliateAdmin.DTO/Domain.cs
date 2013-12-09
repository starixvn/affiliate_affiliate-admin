// --------------------------------------------------------------------------
// <copyright file="Domain.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Johnny.Trinh" date="2013.8.16">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.DTO
{
    using System;

    /// <summary>
    /// Domain class.
    /// </summary>
    public class Domain
    {
        public int DomainID { get; set; }

        public string DomainName { get; set; }

        public string Creator { get; set; }

        public DateTime? CreateDate { get; set; }
    }
}
