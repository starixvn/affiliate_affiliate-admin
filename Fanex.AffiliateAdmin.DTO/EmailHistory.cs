// --------------------------------------------------------------------------
// <copyright file="EmailHistory.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Joseph.Pham" date="2013.8.2">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.DTO
{
    using System;

    /// <summary>
    /// Email History class.
    /// </summary>
    public class EmailHistory
    {
        public int Mid { get; set; }

        public string Content { get; set; }

        public string Sender { get; set; }

        public int AffiliateID { get; set; }

        public string Affname { get; set; }

        public string Email { get; set; }

        public string Subject { get; set; }

        public DateTime CreateDate { get; set; }

        public string Ip { get; set; }

        public string Browser { get; set; }
    }
}