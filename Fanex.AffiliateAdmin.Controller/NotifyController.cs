// --------------------------------------------------------------------------
// <copyright file="NotifyController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Johnny.Trinh" date="2013.8.2">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Controller
{
    using System.Web.Mvc;
    using Core.Filter;
    using Core.Services;

    /// <summary>
    /// Notify Controller class.
    /// </summary>
    public class NotifyController : Controller
    {
        /// <summary>
        /// Under maintenance.
        /// </summary>
        /// <returns>Under maintenance page.</returns>
        public ActionResult Index()
        {
            string startTime = string.Empty, endTime = string.Empty;
            if (UnderMaintenanceService.GetDownTime(ref startTime, ref endTime))
            {
                ViewBag.StartTime = startTime;
                ViewBag.EndTime = endTime;
                return View();
            }
            else
            {
                return RedirectPermanent("/");
            }
        }

        /// <summary>
        /// Error page.
        /// </summary>
        /// <returns>Return error page.</returns>
        public ActionResult Error()
        {
            return View();
        }

        /// <summary>
        /// Page not found.
        /// </summary>
        /// <returns>Return Page not found.</returns>
        public ActionResult PageNotFound()
        {
            return View();
        }

        /// <summary>
        /// Access denied.
        /// </summary>
        /// <returns>Return access denied page.</returns>
        public ActionResult AccessDenied()
        {
            return View();
        }
    }
}
