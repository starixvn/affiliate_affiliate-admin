namespace Fanex.AffiliateAdmin.Controller
{
    using System;
    using System.Web.Mvc;
    using Fanex.AffiliateAdmin.Core.Configurations;
    using Fanex.AffiliateAdmin.Services;

    /// <summary>
    /// Refresh Web Config.
    /// </summary>
    public class RefreshController : Controller
    {
        private readonly ISystemSettings _systemSettings;

        public RefreshController(
            ISystemSettings systemSettings)
        {
            _systemSettings = systemSettings;
        }

        public ActionResult SystemTime()
        {
            var webTime = DateTime.Now;

            var systemTime = _systemSettings.SystemDateTime;

            return View(new Tuple<DateTime, DateTime>(webTime, systemTime));
        }
    }
}