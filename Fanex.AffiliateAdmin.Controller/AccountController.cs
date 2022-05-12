namespace Fanex.AffiliateAdmin.Controller
{
    using System;
    using System.Globalization;
    using System.Text.RegularExpressions;
    using System.Web.Mvc;
    using Common;
    using Core.Filter;
    using Core.Session;
    using Core.Utilities;
    using Criterias;
    using DTO;
    using Fanex.AffiliateAdmin.Core;
    using Fanex.AffiliateAdmin.Core.Configurations;
    using Fanex.AffiliateAdmin.Core.Constants;
    using Fanex.AffiliateAdmin.Core.Helpers;
    using Fanex.AffiliateAdmin.Core.Services.Interface;
    using Fanex.AffiliateAdmin.Services;
    using Helpers;
    using Models;
    using ONEbook.UM.AspNetMVC.Filters;
    using Resources;

    public class AccountController : Controller
    {
        private readonly IAppSetting _appSetting;
        private readonly IAuthService _authService;
        private readonly IAccountService _accountService;
        private readonly ISystemSettings _systemSettings;
        private readonly IFormsAuthenticationWrapper _formsAuthenticationWrapper;

        public AccountController(
            IAppSetting appSetting,
            IAuthService authService,
            IAccountService accountService,
            ISystemSettings systemSettings,
            IFormsAuthenticationWrapper formsAuthenticationWrapper)
        {
            var langManager = new LanguageManager();
            langManager.SetCulture(AppSession.Language);

            _appSetting = appSetting;
            _authService = authService;
            _accountService = accountService;
            _systemSettings = systemSettings;
            _formsAuthenticationWrapper = formsAuthenticationWrapper;
        }

        [HttpPost]
        public ActionResult ChangeLanguage(string lang)
        {
            AppSession.Language = lang;
            return Json(string.Empty);
        }

        /// <summary>
        /// Return form login.
        /// </summary>
        /// <returns>Login form.</returns>
        [HttpGet]
        [CheckUMFilter]
        public ActionResult Login()
        {
            var clientIPAddress = Remote.GetClientIPAddress(Request);

            if (_appSetting.IsProduction && !Regex.IsMatch(clientIPAddress, _appSetting.GetValue("CompanyIPRange")))
            {
                LogHelper.WriteInfo($"Access Denied: {_systemSettings.SystemName}; IP: {clientIPAddress}");

                return Redirect("~/Notify/AccessDenied");
            }
            else
            {
                ViewBag.LogInToken = CreateToken();
                return View(new LoginViewModel());
            }
        }

        [HttpPost]
        [CheckUMFilter]
        public ActionResult Login(string username, string password, string logInToken)
        {
            ResultViewModel result = null;
            if (ModelState.IsValid)
            {
                var clientIPAddress = Remote.GetClientIPAddress(Request);
                var criteria = new LoginCriteria
                {
                    Username = username,
                    UserPwd = password,
                    LoginIP = clientIPAddress,
                    LoginDomain = Request.UserHostName,
                };

                result = _authService.Login(criteria, out Account user, logInToken);
                if (result.IsSuccess && user.CustID > 0)
                {
                    user.OnUserID = _authService.CheckIn(new CheckInCriteria
                    {
                        CustId = user.CustID,
                        IP = clientIPAddress,
                        NetSessionId = Session.SessionID,
                        ServerId = user.ServerId
                    });
                    AffiliateInfoCookieHelper.DeleteAffiliate(HttpContext);
                    _formsAuthenticationWrapper.SetAuthCookie(username, false);
                    SessionManager.CurrentSession = _authService.CreateInternalSession(user, _systemSettings.LicenseeID);
                }
                else
                {
                    result = GetResultViewModel(result.ErrorCode, result.ErrorMessage);
                    ViewBag.LogInToken = CreateToken();
                }
            }

            return Json(result);
        }

        [HttpGet]
        [CheckUMFilter]
        public string GetUserSalt(string userName)
        {
            return GetSalt(userName);
        }

        [HttpGet]
        [CheckUMFilter]
        [Authorization]
        [ITAuth]
        public ActionResult ChangePassword()
        {
            return View(new AccountPasswordViewModel());
        }

        [HttpPost]
        [CheckUMFilter]
        [Authorization]
        [ITAuth]
        public ActionResult ChangePassword(AccountPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.NewPassword.Equals(model.OldPassword))
                {
                    ViewBag.Result = new ResultViewModel
                    {
                        ErrorCode = ErrorCodes.InvalidRequest,
                        ErrorMessage = Lang.NewPassworDifferOldPassword
                    };
                }
                else
                {
                    var oldSalt = GetSalt(SessionManager.CurrentSession.Username);
                    var criteria = new AccountPasswordCriteria
                    {
                        CustId = SessionManager.CurrentSession.CustID,
                        OldPassword = Cryptography.CreateHash(model.OldPassword, oldSalt),
                        NewPassword = Cryptography.CreateHash(model.NewPassword, oldSalt),
                        Ip = Remote.GetClientIPAddress()
                    };

                    _accountService.AccountUpdatePassword(criteria);
                    ViewBag.Result = GetResultViewModel(criteria.ErrCode);
                }
            }

            return View(model);
        }

        [CheckUMFilter]
        public ActionResult Logout()
        {
            DoLogOut();
            return Redirect(_formsAuthenticationWrapper.LoginUrl);
        }

        [HttpPost]
        [CheckUMFilter]
        public ActionResult ReCheckIn()
        {
            var result = new ResultViewModel { ErrorCode = ErrorCodes.GeneralErrorCode };

            KickedOutSession(result);

            return Json(result);
        }

        private static ResultViewModel GetResultViewModel(int errCode, string errorMessage = "")
        {
            return new ResultViewModel
            {
                ErrorCode = errCode,
                ErrorMessage = errCode == ErrorCodes.Success ? Lang.UpdateSucc :
                    (string.IsNullOrEmpty(errorMessage) ? GetErrorMessage(errCode) : errorMessage)
            };
        }

        private static string GetErrorMessage(int errCode)
        {
            return ErrorMsg.ResourceManager.GetString("Msg" + errCode);
        }

        private string GetSalt(string userName)
        {
            var user = _authService.GetByUsername(userName);
            if (user != null)
            {
                return user.UserSalt;
            }

            return string.Empty;
        }

        private void KickedOutSession(ResultViewModel result)
        {
            var criteria = new ReCheckInCriteria
            {
                OnUserID = SessionManager.CurrentSession == null ? 0 : SessionManager.CurrentSession.OnUserID
            };

            var isKickedOut = _authService.ReCheckIn(criteria);

            if (!_appSetting.LocalTest && isKickedOut)
            {
                result.ErrorCode = ErrorCodes.Success;
                result.ErrorMessage = Lang.MsgKickout;

                DoLogOut();
            }
        }

        private void DoLogOut()
        {
            if (SessionManager.CurrentSession != null)
            {
                _authService.LogOut(new LogOutCriteria
                {
                    OnUserId = SessionManager.CurrentSession.OnUserID
                });

                _formsAuthenticationWrapper.SignOut();
                SessionManager.ClearAll();
                var cookies = Request.Cookies.AllKeys;
                foreach (string cookie in cookies)
                {
                    Response.Cookies[cookie].Expires = DateTime.Now.AddDays(-1);
                }
            }
        }

        private string CreateToken()
        {
            return Guid.NewGuid().ToString("N", CultureInfo.CurrentCulture);
        }
    }
}