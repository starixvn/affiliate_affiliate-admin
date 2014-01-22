// --------------------------------------------------------------------------
// <copyright file="ExternalLoginInfo.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2014.1.16">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Core.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web;
    using System.Web.Mvc;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ExternalLoginInfo
    {
        public Controller Controller { get; set; }

        public string Username { get; set; }

        public int CustId { get; set; }

        public int OnUserID { get; set; }

        public string SessionID { get; set; }

        public string AffTypeName { get; set; }

        /// <summary>
        /// Check whether it belong to kiosk type or not.
        /// </summary>
        /// <returns>Return true or false.</returns>
        public bool IsKiosk()
        {
            return AffTypeName.Equals("kiosk", StringComparison.CurrentCultureIgnoreCase);
        }
    }
}
