// --------------------------------------------------------------------------
// <copyright file="BannerType.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2013.9.19">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.DTO
{
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Banner Type class.
    /// </summary>
    public class BannerType
    {
        public short TypeID { get; set; }

        [SuppressMessage("Microsoft.Naming", "CA1721:PropertyNamesShouldNotMatchGetMethods")]
        public string Type { get; set; }

        public string Value { get; set; }

        public byte? DisplayOrder { get; set; }

        public short? ParentID { get; set; }
    }
}