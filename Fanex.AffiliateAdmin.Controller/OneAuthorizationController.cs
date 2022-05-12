namespace Fanex.AffiliateAdmin.Controller
{
    using System.Web.Mvc;
    using Core.Configurations;
    using Core.Constants;
    using DTO.OneAuth;
    using Fanex.AffiliateAdmin.Core.Helpers;
    using Newtonsoft.Json;
    using ONEbook.UM.AspNetMVC.Filters;
    using Services.Interfaces;

    public class OneAuthorizationController : Controller
    {
        private readonly IAppSetting _appSetting;
        private readonly ISystemSettings _systemSettings;
        private readonly IOneAuthenticationService _oneAuthenticationService;

        internal OneAuthorizationController(
            IAppSetting appSetting,
            ISystemSettings systemSettings,
            IOneAuthenticationService oneAuthenticationService)
        {
            _appSetting = appSetting;
            _systemSettings = systemSettings;
            _oneAuthenticationService = oneAuthenticationService;
        }

        [HttpGet]
        public ActionResult SessionIsAlive()
        {
            return Json(HttpContext.User.Identity.IsAuthenticated, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        [HttpGet]
        [CheckUMFilter]
        [Route("Auth/CreateSession")]
        public RedirectResult CreateSession(string token)
        {
            var returnUrl = "~/Notify/OneTokenRestriction?code=";
            var errMsg = string.Empty;
            if (!string.IsNullOrWhiteSpace(token))
            {
                var tokenInfo = _oneAuthenticationService.GetTokenInfo(token);
                if (tokenInfo != null && VerifyToken(tokenInfo))
                {
                    if (!tokenInfo.SiteName.Equals(_systemSettings.SystemName, System.StringComparison.OrdinalIgnoreCase))
                    {
                        returnUrl += ErrorCodes.LicenseeIsNotExisted;
                    }
                    else
                    {
                        var result = _oneAuthenticationService.SignIn(HttpContext, tokenInfo);
                        returnUrl = result.IsSuccessful ? "~/" + result.ExtendedData : returnUrl + result.Code;
                    }
                }
                else
                {
                    errMsg = tokenInfo != null ? JsonConvert.SerializeObject(tokenInfo) : "token is null";
                    returnUrl += ErrorCodes.TokenIsInValid;
                }
            }

            if (_appSetting.GetValue("EnableLogForOneAuthorization") == "1" && !string.IsNullOrEmpty(errMsg))
            {
                LogHelper.WriteError("OneAuthorization Token is invalid - Token = " + errMsg);
            }

            return Redirect(Url.Content(returnUrl));
        }

        private static bool VerifyToken(OneAuthToken tokenInfo)
        {
            return !string.IsNullOrEmpty(tokenInfo.SiteName) && tokenInfo.UserSEQ > 0
                && !string.IsNullOrEmpty(tokenInfo.VendorID) && !string.IsNullOrEmpty(tokenInfo.UserID)
                && !string.IsNullOrEmpty(tokenInfo.Language) && !string.IsNullOrEmpty(tokenInfo.ActionLink);
        }
    }
}