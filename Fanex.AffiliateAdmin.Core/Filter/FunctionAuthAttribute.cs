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
    using System.Web.Mvc;
    using Fanex.AffiliateAdmin.Core.Session;

    /// <summary>
    /// Authorization Attribute class.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class FunctionAuthAttribute : ActionFilterAttribute
    {
        public string Function { get; set; }

        /// <summary>
        /// Action filter for under maintenance service.
        /// </summary>
        /// <param name="filterContext">Filter Context.</param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var hasPermission = false;

            if (SessionManager.CurrentSession != null)
            {
                hasPermission = SessionManager.CurrentSession.RoleAccess.HasPolicy(Function);

                if (hasPermission && !SessionManager.CurrentSession.IsIT)
                {
                    hasPermission = SessionManager.CurrentSession.CurrencyList.Count > 0;
                }
            }

            if (!hasPermission)
            {
                filterContext.HttpContext.Response.Redirect("~/notify/AccessDenied");
            }    

            base.OnActionExecuting(filterContext);
        }
    }
}