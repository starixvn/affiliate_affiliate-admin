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
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class FunctionAuthAttribute : ActionFilterAttribute
    {
        public string Function { get; set; }

        /// <summary>
        /// Action filter for under maintenance service.
        /// </summary>
        /// <param name="filterContext">Filter Context.</param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (SessionManager.CurrentSession != null && 
                (SessionManager.CurrentSession.CurrencyList.Count == 0 || !SessionManager.CurrentSession.RoleAccess.HasPolicy(Function)))
            {
                filterContext.HttpContext.Response.Redirect("~/notify/AccessDenied");
            }    

            base.OnActionExecuting(filterContext);
        }
    }
}