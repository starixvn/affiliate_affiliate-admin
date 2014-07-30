// --------------------------------------------------------------------------
// <copyright file="EmailFormatType.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Johnny.Trinh" date="2013.08.22">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Core.Enumerations
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public enum EmailFormatType : int
    {
        /// <summary>
        /// None format.
        /// </summary>
        None = 0,
        /// <summary>
        /// Email text only.
        /// </summary>
        Text = 1,

        /// <summary>
        /// Format email as html.
        /// </summary>
        Html = 2
    }
}