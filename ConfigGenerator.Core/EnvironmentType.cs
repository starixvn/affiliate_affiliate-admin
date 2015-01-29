// --------------------------------------------------------------------------
// <copyright file="EnvironmentType.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2015.1.29">Created</change>
// </history>

namespace ConfigGenerator.Core
{
    using System;

    [Flags]
    public enum EnvironmentType
    {
        /// <summary>
        /// None Type.
        /// </summary>
        None = 0,

        /// <summary>
        /// Local Type.
        /// </summary>
        Local = 1 << 0,

        /// <summary>
        /// Master Type.
        /// </summary>
        Master = 1 << 1,

        /// <summary>
        /// User Acceptance Test Type.
        /// </summary>
        Uat = 1 << 2,

        /// <summary>
        /// Production Type.
        /// </summary>
        Production = 1 << 3
    }
}