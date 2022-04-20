namespace Fanex.AffiliateAdmin.Controller
{
    using System.Web.Mvc;
    using System.Web.SessionState;
    using Core.Constants;
    using Fanex.AffiliateAdmin.Core.Services.Interface;
    using Fanex.AffiliateAdmin.Core.Session;
    using Resources;

    /// <summary>
    /// Notify Controller class.
    /// </summary>
    [SessionState(SessionStateBehavior.Default)]
    [AllowAnonymous]
    public class NotifyController : Controller
    {
        private readonly IUnderMaintenanceService _underMaintenanceService;
        private readonly IFormsAuthenticationWrapper _formsAuthenticationWrapper;

        public NotifyController(
            IUnderMaintenanceService underMaintenanceService,
            IFormsAuthenticationWrapper formsAuthenticationWrapper)
        {
            _underMaintenanceService = underMaintenanceService;
            _formsAuthenticationWrapper = formsAuthenticationWrapper;
        }

        /// <summary>
        /// Under maintenance.
        /// </summary>
        /// <returns>Under maintenance page.</returns>
        public ActionResult Index()
        {
            if (_underMaintenanceService.IsUnderMaintenance())
            {
                LogOutWhenUMTime();

                var underMaintenanceTime = _underMaintenanceService.GetUnderMaintenanceTime();
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
                case ErrorCodes.TokenWasExpired:
                    return Lang.TokenWasExpired;

                case ErrorCodes.LicenseeIsNotExisted:
                    return Lang.LicenseeIsNotExisted;

                case ErrorCodes.TokenIsInValid:
                    return Lang.TokenIsInValid;

                case ErrorCodes.LocationRestrictionMsg:
                    return string.Format(Lang.LocationRestrictionMsg, Lang.Administrator);

                case ErrorCodes.LicenseeNotOfferedCurrency:
                    return Lang.LicenseeNotOfferedCurrency;

                case ErrorCodes.YouAreNotAllowToAccessThisPage:
                    return Lang.YouAreNotAllowToAccessThisPage;

                default:
                    return Lang.OneTokenRestrictionDefaultMsg;
            }
        }

        private void LogOutWhenUMTime()
        {
            if (SessionManager.CurrentSession != null)
            {
                _formsAuthenticationWrapper.SignOut();
                SessionManager.ClearAll();
            }
        }
    }
}