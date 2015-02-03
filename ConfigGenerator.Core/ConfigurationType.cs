// --------------------------------------------------------------------------
// <copyright file="ConfigurationType.cs" company="Nexcel Solutions Vietnam">
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
    public enum ConfigurationType
    {
        /// <summary>
        /// None Type.
        /// </summary>
        None = 0,

        /// <summary>
        /// Web Configuration Type.
        /// </summary>
        WebConfig = 1 << 0,

        /// <summary>
        /// Application Setting Type.
        /// </summary>
        AppSetting = 1 << 1,

        /// <summary>
        /// Connection String Type.
        /// </summary>
        ConnectionStrings = 1 << 2,

        /// <summary>
        /// Sp Name Type.
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly",
            Justification = "Reviewed. Suppression is OK here.")]
        SpName = 1 << 3
    }
}