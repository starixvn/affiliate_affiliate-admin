// --------------------------------------------------------------------------
// <copyright file="FunctionAuthAttribute.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2014.1.8">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Core.Filter
{
    using System;
    using System.Net;
    using System.Web.Mvc;
    using System.Web.Security;
    using Fanex.AffiliateAdmin.Core.Session;
    using Fanex.AffiliateAdmin.Resources;
    using Fanex.Logging;

    /// <summary>
    /// Authorization Attribute class.
    /// </summary>
    public class FunctionAuthAttribute : ActionFilterAttribute
    {
        public string Function { get; set; }

        /// <summary>
        /// Action filter for under maintenance service.
        /// </summary>
        /// <param name="filterContext">Filter Context.</param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var response = filterContext.HttpContext.Response;
           
            if (SessionManager.CurrentSession.CurrencyList.Count == 0 || !SessionManager.CurrentSession.RoleAccess.HasPolicy(Function))
            {
                filterContext.HttpContext.Response.Redirect("~/notify/AccessDenied");
            }    

            base.OnActionExecuting(filterContext);
        }
    }
}