// --------------------------------------------------------------------------
// <copyright file="SystemType.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2015.1.29">Created</change>
// </history>

using System.Diagnostics.CodeAnalysis;

namespace ConfigGenerator.Core
{
    using System;

    [Flags]
    [SuppressMessage("Microsoft.Naming", "CA1714:FlagsEnumsShouldHavePluralNames",
        Justification = "Reviewed. Suppression is OK here.")]
    public enum SystemType
    {
        /// <summary>
        /// None Type.
        /// </summary>
        None = 0,

        /// <summary>
        /// Admin Type.
        /// </summary>
        Admin = 1 << 0,

        /// <summary>
        /// User Type.
        /// </summary>
        User = 1 << 1,

        /// <summary>
        /// Banner Type.
        /// </summary>
        Banner = 1 << 2,

        /// <summary>
        /// Link Type.
        /// </summary>
        Link = 1 << 3,

        /// <summary>
        /// Web Service Type.
        /// </summary>
        WebService = 1 << 4,
    }
}