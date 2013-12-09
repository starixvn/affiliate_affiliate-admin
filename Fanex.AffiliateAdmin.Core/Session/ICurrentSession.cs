// --------------------------------------------------------------------------
// <copyright file="ICurrentSession.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.05.28">Created</change>
// </history>
namespace Fanex.AffiliateAdmin.Core.Session
{
    using System.Collections.Generic;
    using Fanex.AffiliateAdmin.DTO;

    /// <summary>
    /// Properties of an authenticated user.
    /// </summary>
    public interface ICurrentSession
    {
        /// <summary>
        /// Gets or sets Customer ID.
        /// </summary>
        /// <value>Return Customer ID.</value>
        int CustID { get; set; }

        /// <summary>
        /// Gets or sets UserName.
        /// </summary>
        /// <value>Return UserName.</value>
        string Username { get; set; }

        /// <summary>
        /// Gets or sets On User.
        /// </summary>
        /// <value>Return On User ID.</value>
        int OnUserID { get; set; }

        bool IsIT { get; set; }

        List<FunctionPolicy> RoleAccess { get; set; }

        List<Currency> CurrencyList { get; set; }

        AffiliateSession Affiliate { get; set; }
    }
}