namespace Fanex.AffiliateAdmin.Controller
{
    using System.Web.Mvc;
    using System.Web.Security;
    using System.Web.SessionState;
    using Core.Constants;
    using Fanex.AffiliateAdmin.Controller.Helpers;
    using Fanex.AffiliateAdmin.Core.Session;
    using ONEbook.UM.Services;
    using Resources;

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
            _maintenanceService = maintenanceService ?? MaintenanceServiceHelper.GetMaintenanceService();
        }

        /// <summary>
        /// Under maintenance.
        /// </summary>
        /// <returns>Under maintenance page.</returns>
        public ActionResult Index()
        {
            if (_maintenanceService.IsUnderMaintenance())
            {
                LogOutWhenUMTime();

                var underMaintenanceTime = _maintenanceService.GetUnderMaintenanceTime();
                ViewBag.StartTime = underMaintenanceTime.From;
                ViewBag.EndTime = underMaintenanceTime.To;

                return View();
            }

            return RedirectPermanent(Url.Content("~/"));
        }

        public ActionResult OneTokenRestriction(int code)
        {
            ViewBag.Message = GetMessageRestriction(code);

            return View();
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

        private static string GetMessageRestriction(int code)
        {
            switch (code)
            {
                case OneAuthorizationErrorCode.TokenWasExpired:
                    return Lang.TokenWasExpired;

                case OneAuthorizationErrorCode.LicenseeIsNotExisted:
                    return Lang.LicenseeIsNotExisted;

                case OneAuthorizationErrorCode.TokenIsInValid:
                    return Lang.TokenIsInValid;

                case OneAuthorizationErrorCode.LocationRestrictionMsg:
                    return string.Format(Lang.LocationRestrictionMsg, Lang.Administrator);

                case OneAuthorizationErrorCode.LicenseeNotOfferedCurrency:
                    return Lang.LicenseeNotOfferedCurrency;

                case OneAuthorizationErrorCode.YouAreNotAllowToAccessThisPage:
                    return Lang.YouAreNotAllowToAccessThisPage;

                default:
                    return Lang.OneTokenRestrictionDefaultMsg;
            }
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