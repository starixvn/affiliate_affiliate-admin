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
    using System.Linq;
    using Fanex.AffiliateAdmin.DTO;

    /// <summary>
    /// Properties of an authenticated user.
    /// </summary>
    public interface ICurrentSession
    {
        int CustID { get; set; }

        string Username { get; set; }

        int OnUserID { get; set; }

        bool IsIT { get; set; }

        IEnumerable<Currency> CurrencyList { get; set; }

        AffiliateSession Affiliate { get; set; }

        bool HasReadPolicy(string functionName);
        
        bool HasHostPolicy(string functionName);
    }
}