// --------------------------------------------------------------------------
// <copyright file="CurrentSession.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.05.28">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Core.Session
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using Fanex.AffiliateAdmin.DTO;

    /// <summary>
    /// Properties of an authenticated user.
    /// The implementation is applied when user config is state server mode.
    /// </summary>
    [Serializable]
    public class CurrentSession : ICurrentSession
    {
        private readonly IEnumerable<FunctionPolicy> _policies;

        /// <summary>
        /// Initializes a new instance of the <see cref="CurrentSession" /> class.
        /// </summary>
        /// <param name="user">User login in system.</param>
        public CurrentSession(Account user, IEnumerable<FunctionPolicy> policies)
        {
            this.CustID = user.CustID;
            this.Username = user.Username;
            this.OnUserID = user.OnUserID;
            this.IsIT = user.IsIT;
            this._policies = policies ?? new List<FunctionPolicy>();
        }

        public int CustID
        {
            get
            {
                return Convert.ToInt32(HttpContext.Current.Session["CustID"]);
            }

            set
            {
                HttpContext.Current.Session["CustID"] = value;
            }
        }

        public string Username
        {
            get
            {
                return Convert.ToString(HttpContext.Current.Session["Username"]);
            }

            set
            {
                HttpContext.Current.Session["Username"] = value;
            }
        }

        public string Email
        {
            get
            {
                return Convert.ToString(HttpContext.Current.Session["Email"]);
            }

            set
            {
                HttpContext.Current.Session["Email"] = value;
            }
        }

        public int OnUserID
        {
            get
            {
                return Convert.ToInt32(HttpContext.Current.Session["OnUserID"]);
            }

            set
            {
                HttpContext.Current.Session["OnUserID"] = value;
            }
        }

        public bool IsIT
        {
            get
            {
                return Convert.ToBoolean(HttpContext.Current.Session["IsIT"]);
            }

            set
            {
                HttpContext.Current.Session["IsIT"] = value;
            }
        }

        public IEnumerable<Currency> CurrencyList
        {
            get
            {
                return (IEnumerable<Currency>)HttpContext.Current.Session["CurrencyList"];
            }

            set
            {
                HttpContext.Current.Session["CurrencyList"] = value;
            }
        }

        public AffiliateSession Affiliate
        {
            get
            {
                return (AffiliateSession)HttpContext.Current.Session["Affiliate"];
            }

            set
            {
                HttpContext.Current.Session["Affiliate"] = value;
            }
        }

        public bool HasReadPolicy(string functionName)
        {
            return this._policies.Any(i => i.FuncName == functionName && i.CanRead);
        }

        public bool HasHostPolicy(string functionName)
        {
            return this._policies.Any(i => i.FuncName == functionName && i.CanHost);
        }
    }
}