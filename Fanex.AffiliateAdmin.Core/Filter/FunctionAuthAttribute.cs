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
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.AffiliateAdmin.Core.Session;
    using Fanex.AffiliateAdmin.Resources;

    /// <summary>
    /// Authorization Attribute class.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class FunctionAuthAttribute : ActionFilterAttribute
    {
        private const int AccessDeniedErrorCode = 500;

        public string Function { get; set; }

        public bool HasHostPolicy { get; set; }

        /// <summary>
        /// Action filter for under maintenance service.
        /// </summary>
        /// <param name="filterContext">Filter Context.</param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var hasPermission = false;

            if (SessionManager.CurrentSession != null)
            {
                hasPermission = HasHostPolicy ?
                    SessionManager.CurrentSession.HasHostPolicy(Function) :
                    SessionManager.CurrentSession.HasReadPolicy(Function);

                if (hasPermission && !SessionManager.CurrentSession.IsIT)
                {
                    hasPermission = SessionManager.CurrentSession.CurrencyList.Any();
                }
            }

            if (!hasPermission)
            {
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    var data = new { ErrorCode = AccessDeniedErrorCode, Message = ErrorMsg.Msg500 };
                    filterContext.Result = new JsonResult() { Data = data, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }
                else
                {
                    filterContext.HttpContext.Response.Redirect("~/notify/AccessDenied");
                }
            }

            base.OnActionExecuting(filterContext);
        }
    }
}