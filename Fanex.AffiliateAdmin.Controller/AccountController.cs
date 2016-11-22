namespace Fanex.AffiliateAdmin.Controller
{
    using System;
    using System.Globalization;
    using System.Text.RegularExpressions;
    using System.Web.Mvc;
    using System.Web.Security;
    using Common;
    using Core.Configurations;
    using Core.Filter;
    using Core.Session;
    using Core.Utilities;
    using Criterias;
    using DTO;
    using Fanex.AffiliateAdmin.Core;
    using Fanex.AffiliateAdmin.Services;
    using Helpers;
    using Models;
    using ONEbook.UM.Filters;
    using Resources;

    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;

        public AccountController()
            : this(null)
        {
            var langManager = new LanguageManager();
            langManager.SetCulture(AppSession.Language);
        }

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService ?? new AccountService();
        }

        /// <summary>
        /// Return form login.
        /// </summary>
        /// <returns>Login form.</returns>
        [HttpGet]
        [CheckUMFilter]
        public ActionResult Login()
        {
            if (AppSettings.IsProduction && !Regex.IsMatch(Request.UserHostAddress, AppSettings.GetValue("CompanyIPRange")))
            {
                return Redirect("~/Notify/AccessDenied");
            }
            else
            {
                ViewBag.LogInToken = CreateToken();
                return View(new LoginViewModel());
            }
        }

        /// <summary>
        /// Login user.
        /// </summary>
        /// <param name="username">Username login.</param>
        /// <param name="password">Password login.</param>
        /// <returns>Return Result view model.</returns>
        [HttpPost]
        [CheckUMFilter]
        public ActionResult Login(string username, string password, string logInToken)
        {
            ResultViewModel result = null;
            Account user = null;

            if (ModelState.IsValid)
            {
                var criteria = new LoginCriteria
                {
                    Username = username,
                    UserPwd = password,
                    LoginIP = Remote.GetClientIPAddress(),
                    LoginDomain = Request.UserHostName,
                };

                result = _accountService.Login(criteria, out user, logInToken);
                result.Success = result.ErrorCode == 0;
                if (result.ErrorCode == 0 && user.CustID > 0)
                {
                    user.OnUserID = _accountService.CheckIn(new CheckInCriteria()
                    {
                        CustId = user.CustID,
                        IP = Remote.GetClientIPAddress(),
                        NetSessionId = Session.SessionID,
                        ServerId = user.ServerId
                    });
                    AffiliateInfoCookieHelper.DeleteAffiliateInfoCookie(HttpContext);
                    FormsAuthentication.SetAuthCookie(username, true);
                    SessionManager.CurrentSession = _accountService.CreateSession(user, AppSettings.LicenseeID);
                }
                else
                {
                    result = GetResultViewModel(result.ErrorCode, string.Empty, result.ErrorMessage);
                    ViewBag.LogInToken = CreateToken();
                }
            }

            return Json(result);
        }

        private string CreateToken()
        {
            return Guid.NewGuid().ToString("N", CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Get User Salt.
        /// </summary>
        /// <param name="userName">User name value.</param>
        /// <returns>String value.</returns>
        [HttpGet]
        [CheckUMFilter]
        public string GetUserSalt(string userName)
        {
            return GetSalt(userName);
        }

        /// <summary>
        /// Change password.
        /// </summary>
        /// <returns>Return change password form.</returns>
        [HttpGet]
        [CheckUMFilter]
        [Authorization]
        [ITAuth]
        public ActionResult ChangePassword()
        {
            return View(new AccountPasswordViewModel());
        }

        /// <summary>
        /// Change password.
        /// </summary>
        /// <param name="model">Input parameter.</param>
        /// <returns>Return change password form.</returns>
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
                        ErrorCode = 16,
                        Message = Lang.NewPassworDifferOldPassword
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

        /// <summary>
        /// Logout admin.
        /// </summary>
        /// <returns>Return login form.</returns>
        [CheckUMFilter]
        public ActionResult Logout()
        {
            DoLogOut();
            return View();
        }

        /// <summary>
        /// RecheckIn user.
        /// </summary>
        /// <returns>Kicked out or not.</returns>
        [HttpPost]
        [CheckUMFilter]
        public ActionResult ReCheckIn()
        {
            var result = new ResultViewModel();

            KickedOutSession(result);

            return Json(result);
        }

        private string GetSalt(string userName)
        {
            var user = _accountService.GetByUsername(userName);
            if (user != null)
            {
                return user.UserSalt;
            }

            return string.Empty;
        }

        /// <summary>
        /// Kicked out session.
        /// </summary>
        /// <param name="result">Kick-out result.</param>
        private void KickedOutSession(ResultViewModel result)
        {
            var criteria = new ReCheckInCriteria
            {
                OnUserID = SessionManager.CurrentSession == null ? 0 : SessionManager.CurrentSession.OnUserID
            };

            var isKickedOut = _accountService.ReCheckIn(criteria);

            if (!AppSettings.LocalTest)
            {
                result.Message = isKickedOut ? Lang.MsgKickout : string.Empty;
                if (isKickedOut)
                {
                    result.Success = isKickedOut;
                    result.ErrorMessage = "Expired";

                    // Signout and clean all session.
                    DoLogOut();
                }
            }
        }

        /// <summary>
        /// Sign out and clear all session.
        /// </summary>
        private void DoLogOut()
        {
            if (SessionManager.CurrentSession != null)
            {
                _accountService.UserLogOut(new LogOutCriteria
                {
                    OnUserId = SessionManager.CurrentSession.OnUserID
                });

                FormsAuthentication.SignOut();
                SessionManager.ClearAll();
                var cookies = Request.Cookies.AllKeys;
                foreach (string cookie in cookies)
                {
                    Response.Cookies[cookie].Expires = DateTime.Now.AddDays(-1);
                }
            }
        }

        private ResultViewModel GetResultViewModel(int errCode, string messageSuccess = "", string errorMessage = "")
        {
            return new ResultViewModel
            {
                ErrorCode = errCode,
                Message = errCode == 0 ?
                    (string.IsNullOrEmpty(messageSuccess) ? Lang.UpdateSucc : messageSuccess) :
                    (string.IsNullOrEmpty(errorMessage) ? GetErrorMessage(errCode) : errorMessage),
                Success = errCode == 0
            };
        }

        private string GetErrorMessage(int errCode)
        {
            return ErrorMsg.ResourceManager.GetString("Msg" + errCode);
        }
    }
}