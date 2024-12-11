namespace Fanex.AffiliateAdmin.Controller
{
    using Fanex.AffiliateAdmin.Services;
    using System;
    using System.Web.Mvc;

    /// <summary>
    /// Refresh Web Config.
    /// </summary>
    public class RefreshController : Controller
    {
        /// <summary>
        /// View System Time.
        /// </summary>
        /// <returns>System Time.</returns>
        public ActionResult SystemTime()
        {
            var webTime = DateTime.Now;

            var systemTime = SystemTimeService.GetSystemDateTime();

            return View(new Tuple<DateTime, DateTime>(webTime, systemTime));
        }
    }
}