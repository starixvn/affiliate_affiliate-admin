namespace Fanex.AffiliateAdmin.Controller
{
    using System;
    using System.Web.Mvc;
    using Fanex.AffiliateAdmin.Services;

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