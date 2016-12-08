namespace Fanex.AffiliateAdmin.Controller
{
    using System.Web.Mvc;
    using System.Web.Script.Serialization;
    using Core.Configurations;
    using Core.Constants;
    using DTO.Common;
    using Newtonsoft.Json;
    using ONEbook.UM.Services;
    using ONEbook.UM.Services.Impl;
    using Services.Impl.Core;
    using Services.Interfaces;

    public class OneAuthorizationController : Controller
    {
        private readonly IMaintenanceService _maintenanceService;

        private readonly IOneAuthenticationService _oneAuthenticationService;

        public OneAuthorizationController() : this(null, null)
        {
        }

        internal OneAuthorizationController(
            IMaintenanceService maintenanceService,
            IOneAuthenticationService oneAuthenticationService)
        {
            _maintenanceService = maintenanceService ?? new MaintenanceService();
            _oneAuthenticationService = oneAuthenticationService ?? new OneAuthenticationService();
        }

        [HttpGet]
        public ActionResult SessionIsAlive()
        {
            return Json(HttpContext.User.Identity.IsAuthenticated, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("Auth/CreateSession")]
        public RedirectResult CreateSession(string token)
        {
            if (_maintenanceService.IsUnderMaintenance())
            {
                return new RedirectResult("~/Notify");
            }

            var returnUrl = "~/Notify/OneTokenRestriction?code=";
            var errMsg = string.Empty;
            if (!string.IsNullOrWhiteSpace(token))
            {
                var tokenInfo = _oneAuthenticationService.GetTokenInfo(token);
                if (tokenInfo != null && VerifyToken(tokenInfo))
                {
                    if (!tokenInfo.SiteName.Equals(AppSettings.SystemName, System.StringComparison.OrdinalIgnoreCase))
                    {
                        returnUrl += OneAuthorizationErrorCode.LicenseeIsNotExisted;
                    }
                    else
                    {
                        var result = _oneAuthenticationService.SignIn(HttpContext, tokenInfo);
                        returnUrl = result.IsSuccessful ? "~/" + result.ExtendedData : returnUrl += result.Code;
                    }
                }
                else
                {
                    errMsg = tokenInfo != null ? JsonConvert.SerializeObject(tokenInfo) : "token is null";
                    returnUrl += OneAuthorizationErrorCode.TokenIsInValid;
                }
            }

            if (AppSettings.GetValue("EnableLogForOneAuthorization") == "1" && !string.IsNullOrEmpty(errMsg))
            {
                Logging.LogService.Error("OneAuthorization Token is invalid - Token = " + errMsg);
            }

            return Redirect(Url.Content(returnUrl));
        }

        private static bool VerifyToken(OneAuthenticationToken tokenInfo)
        {
            return !string.IsNullOrEmpty(tokenInfo.SiteName) && tokenInfo.UserSEQ > 0
                && !string.IsNullOrEmpty(tokenInfo.VendorID) && !string.IsNullOrEmpty(tokenInfo.UserID)
                && !string.IsNullOrEmpty(tokenInfo.Language) && !string.IsNullOrEmpty(tokenInfo.ActionLink);
        }
    }
}