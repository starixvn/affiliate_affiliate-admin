// --------------------------------------------------------------------------
// <copyright file="CriteriaType.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Johnny.Trinh" date="2013.07.24">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Criterias
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public enum CriteriaType
    {
        /// <summary>
        /// Select action.
        /// </summary>
        Select = 0,

        /// <summary>
        /// Delete action.
        /// </summary>
        Delete = 1,

        /// <summary>
        /// Insert action.
        /// </summary>
        Insert = 2,

        /// <summary>
        /// Update action.
        /// </summary>
        Update = 3
    }
}
