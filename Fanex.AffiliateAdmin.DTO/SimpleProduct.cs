// --------------------------------------------------------------------------
// <copyright file="SimpleProduct.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2014.11.14">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.DTO
{
    /// <summary>
    /// Product class.
    /// </summary>
    public class SimpleProduct
    {
        public byte ProductId { get; set; }

        public string ProductName { get; set; }

        public string OriginalProductName { get; set; }

        public int TotalQualifiedMembers { get; set; }
    }
}