// --------------------------------------------------------------------------
// <copyright file="MediaType.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Leo.Vo" date="2013.8.19">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.DTO
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MediaType
    {
        public short TypeId { get; set; }

        public string Value { get; set; }

        public short? ParentId { get; set; }

        public int HasSubCategory { get; set; }
    }
}