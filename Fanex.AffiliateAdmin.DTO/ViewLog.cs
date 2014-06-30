// --------------------------------------------------------------------------
// <copyright file="ViewLog.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2014.6.30">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.DTO
{
    using System;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ViewLog
    {
        public int RefNo { get; set; }

        public int TypeId { get; set; }

        public int AffId { get; set; }

        public string UserName { get; set; }

        public int Creator { get; set; }

        public DateTime LoggedDate { get; set; }

        public string SpName { get; set; }

        public string Details { get; set; }

        public string Ip { get; set; }

        public string TypeName { get; set; }

        public string Description { get; set; }
    }
}
