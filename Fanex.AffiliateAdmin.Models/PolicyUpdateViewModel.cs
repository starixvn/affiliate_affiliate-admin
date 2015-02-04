// --------------------------------------------------------------------------
// <copyright file="PolicyUpdateViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2015.2.4">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Models
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class PolicyUpdateViewModel
    {
        public string MemPolicys { get; set; }

        public string GroupPolicys { get; set; }

        public short FuncId { get; set; }

        public int UserId { get; set; }

        public string Ip { get; set; }

        public string Language { get; set; }
    }
}
