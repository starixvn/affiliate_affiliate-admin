// --------------------------------------------------------------------------
// <copyright file="Menu.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Johnny.Trinh" date="2013.06.25">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Core.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// List Menu item.
    /// </summary>
    public class Menu
    {
        public string Text { get; set; }

        public string Link { get; set; }

        public List<Menu> SubMenu { get; set; }
    }
}
