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
            if (UnderMaintenanceService.GetDownTime(ref startTime, ref endTime))
            {
                if (!filterContext.HttpContext.Response.IsRequestBeingRedirected)
                {
                    filterContext.HttpContext.Response.Redirect("~/Notify/Index");
                }
            }

            base.OnActionExecuting(filterContext);
        }
    }
}