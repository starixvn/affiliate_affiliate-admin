// --------------------------------------------------------------------------
// <copyright file="Message.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Leo.Vo" date="2013.8.19">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.DTO
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class Message
    {
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly")]
        [SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores")]
        public string Message_en { get; set; }

        public int Creator { get; set; }
        
        public DateTime PostedDate { get; set; }

        public int Msgid { get; set; }

        public bool Important { get; set; }

        public string CreatorName { get; set; }

        public string UpdateName { get; set; }

        public DateTime? UpdatedDate { get; set; }
    }
}
