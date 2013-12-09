// --------------------------------------------------------------------------
// <copyright file="ErrorCode.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.05.28">Created</change>
// </history>
namespace Fanex.AffiliateAdmin.Core.Enumerations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public enum ErrorCode : int
    {
        /// <summary>
        /// Wrong username or password.
        /// </summary>
        WrongUserOrPassword = 1001,

        /// <summary>
        /// The A/C is closed due to 5 wrong password attempts.
        /// </summary>
        LoginFailedFiveTimes = 1002,
        
        /// <summary>
        /// Your account is closed.
        /// </summary>
        AccountClosed = 1003
    }
}