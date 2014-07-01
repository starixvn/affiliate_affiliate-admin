// --------------------------------------------------------------------------
// <copyright file="SystemSetting.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.06.10">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.DTO
{
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// System Setting class.
    /// </summary>
    public class SystemSetting
    {
        public int SId { get; set; }

        [SuppressMessage("Microsoft.Naming", "CA1721:PropertyNamesShouldNotMatchGetMethods", Justification = "Allow Type Property")]
        public string Type { get; set; }

        public string SName { get; set; }

        public string SValue { get; set; }

        public string Description { get; set; }

        public string Environment { get; set; }
    }
}