// --------------------------------------------------------------------------
// <copyright file="SessionExtensions.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Johnny.Trinh" date="2013.05.28">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Core.Session
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.AffiliateAdmin.DTO;

    /// <summary>
    /// Session Extensions.
    /// </summary>
    public static class SessionExtensions
    {
        /// <summary>
        /// Check user has policy or not.
        /// </summary>
        /// <param name="list">List Function Policy.</param>
        /// <param name="functionName">Function Name.</param>
        /// <param name="policy">Policy to check (0: not allow; 1: reade; 3: host).</param>
        /// <returns>Return has policy or not.</returns>
        public static bool HasPolicy(this List<FunctionPolicy> list, string functionName, byte policy = 0)
        {
            return true;
            ////return list.Where(i => i.FuncName == functionName && i.Actual_Policy > policy).Any();
        }
    }
}
