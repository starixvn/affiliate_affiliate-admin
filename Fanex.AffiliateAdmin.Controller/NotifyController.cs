namespace Fanex.AffiliateAdmin.Controller
{
    using System.Web.Mvc;
    using System.Web.Security;
    using System.Web.SessionState;
    using Fanex.AffiliateAdmin.Core.Session;
    using ONEbook.UM.Services;
    using ONEbook.UM.Services.Impl;

    /// <summary>
    /// Notify Controller class.
    /// </summary>
    [SessionState(SessionStateBehavior.Default)]
    [AllowAnonymous]
    public class NotifyController : Controller
    {
        private readonly IMaintenanceService _maintenanceService;

        public NotifyController() 
            : this(null)
        {
        }

        public NotifyController(IMaintenanceService maintenanceService)
        {
            _maintenanceService = maintenanceService ?? new MaintenanceService();
        }

        /// <summary>
        /// Under maintenance.
        /// </summary>
        /// <returns>Under maintenance page.</returns>
        public ActionResult Index()
        {
            var underMaintenanceTime = _maintenanceService.GetUnderMaintenanceTime();

            if (underMaintenanceTime.IsUnderMaintenanceTime)
            {
                LogOutWhenUMTime();

                ViewBag.StartTime = underMaintenanceTime.From;
                ViewBag.EndTime = underMaintenanceTime.To;

                return View();
            }

            return RedirectPermanent(Url.Content("~/"));
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

        private void LogOutWhenUMTime()
        {
            if (SessionManager.CurrentSession != null)
            {
                FormsAuthentication.SignOut();
                SessionManager.ClearAll();
            }
        }
    }
}