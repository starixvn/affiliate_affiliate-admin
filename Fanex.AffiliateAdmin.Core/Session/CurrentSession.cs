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
    using System.Web;
    using Fanex.AffiliateAdmin.DTO;

    /// <summary>
    /// Properties of an authenticated user.
    /// The implementation is applied when user config is state server mode.
    /// </summary>
    [Serializable]
    public class CurrentSession : ICurrentSession
    {
        private List<FunctionPolicy> _roleAccess;

        /// <summary>
        /// Initializes a new instance of the <see cref="CurrentSession" /> class.
        /// </summary>
        public CurrentSession()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CurrentSession" /> class.
        /// </summary>
        /// <param name="user">User login in system.</param>
        public CurrentSession(Account user)
        {
            this.CustID = user.CustID;
            this.Username = user.Username;
            this.OnUserID = user.OnUserID;
            this.IsIT = user.IsIT;
        }

        /// <summary>
        /// Gets or sets Customer ID.
        /// </summary>
        /// <value>Return Customer ID.</value>
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

        /// <summary>
        /// Gets or sets UserName.
        /// </summary>
        /// <value>Return UserName.</value>
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

        /// <summary>
        /// Gets or sets Email.
        /// </summary>
        /// <value>Return Email.</value>
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

        /// <summary>
        /// Gets or sets OnUser.
        /// </summary>
        /// <value>Return OnUser.</value>
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

        /// <summary>
        /// Gets or sets List supported currency by Admin user Id.
        /// </summary>
        /// <value>Supported currency.</value>
        public List<Currency> CurrencyList
        {
            get
            {
                return (List<Currency>)HttpContext.Current.Session["CurrencyList"];
            }

            set
            {
                HttpContext.Current.Session["CurrencyList"] = value;
            }
        }

        public List<FunctionPolicy> RoleAccess
        {
            get
            {
                return this._roleAccess;
            }

            set
            {
                _roleAccess = value;
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
    }
}