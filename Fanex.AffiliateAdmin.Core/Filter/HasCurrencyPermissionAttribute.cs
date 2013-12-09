// --------------------------------------------------------------------------
// <copyright file="HasCurrencyPermissionAttribute.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Johnny.Trinh" date="2013.11.01">Created</change>
// </history>
namespace Fanex.AffiliateAdmin.Core.Filter
{
    using System.Web.Mvc;
    using Fanex.AffiliateAdmin.Core.Services;
    using Session;

    /// <summary>
    /// Attribute for under maintenance service.
    /// </summary>
    public class HasCurrencyPermissionAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Action filter for under maintenance service.
        /// </summary>
        /// <param name="filterContext">Filter Context.</param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (SessionManager.CurrentSession.CurrencyList.Count == 0)
            {
                filterContext.HttpContext.Response.Redirect("~/notify/AccessDenied");
            }

            base.OnActionExecuting(filterContext);
        }
    }
}