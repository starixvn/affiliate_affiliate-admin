// --------------------------------------------------------------------------
// <copyright file="CustomerInfoBetList.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2014.11.14">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.DTO
{
    /// <summary>
    /// User data object.
    /// </summary>
    public class CustomerInfoBetList
    {
        public int CustId { get; set; }

        public string Username { get; set; }

        public string IBCUsername { get; set; }

        public int CurrencyId { get; set; }
    }
}
