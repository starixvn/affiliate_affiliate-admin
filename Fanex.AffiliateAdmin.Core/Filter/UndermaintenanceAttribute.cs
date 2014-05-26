// --------------------------------------------------------------------------
// <copyright file="UndermaintenanceAttribute.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.07.05">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Core.Filter
{
    using System;
    using System.Web.Mvc;
    using Fanex.AffiliateAdmin.Core.Services;

    /// <summary>
    /// Attribute for under maintenance service.
    /// </summary>
    public class UndermaintenanceAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Action filter for under maintenance service.
        /// </summary>
        /// <param name="filterContext">Filter Context.</param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string startTime = string.Empty, endTime = string.Empty;

            if (UnderMaintenanceService.GetDownTime(ref startTime, ref endTime) && !IsUnderMaintenancePage(filterContext))
            {
                if (!filterContext.HttpContext.Response.IsRequestBeingRedirected)
                {
                    filterContext.HttpContext.Response.Redirect("~/Notify/Index");
                }
            }

            base.OnActionExecuting(filterContext);
        }

        /// <summary>
        /// Whether or not on the Under-maintenance page.
        /// </summary>
        /// <param name="filterContext">Filter context.</param>
        /// <returns>Be true if on under-maintenance page.</returns>
        private bool IsUnderMaintenancePage(ActionExecutingContext filterContext)
        {
            var currentAcction = filterContext.Controller.ControllerContext.RouteData.GetRequiredString("action");
            var currentController = filterContext.Controller.ControllerContext.RouteData.GetRequiredString("controller");

            return currentController.Equals("notify", StringComparison.OrdinalIgnoreCase)
                   && currentAcction.Equals("index", StringComparison.OrdinalIgnoreCase);
        }
    }
}