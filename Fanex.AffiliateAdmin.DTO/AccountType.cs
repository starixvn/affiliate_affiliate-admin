// --------------------------------------------------------------------------
// <copyright file="AccountType.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2013.10.4">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.DTO
{
    /// <summary>
    /// Account Type.
    /// </summary>
    public class AccountType
    {
        public byte? TypeID { get; set; }

        public string TypeName { get; set; }

        public string Description { get; set; }
    }
}