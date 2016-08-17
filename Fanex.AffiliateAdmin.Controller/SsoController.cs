namespace Fanex.AffiliateAdmin.Controller
{
    using Core.Configurations;
    using Core.Mvc;
    using Core.Services;
    using Core.Services.Interface;
    using Core.Session;
    using Fanex.AffiliateAdmin.Core;
    using Fanex.AffiliateAdmin.Core.Utilities;
    using Fanex.AffiliateAdmin.Criterias;
    using Fanex.AffiliateAdmin.DTO;
    using Fanex.AffiliateAdmin.Services;
    using Fanex.Logging;
    using Models;
    using ONEbook.UM.Filters;
    using Resources;
    using RSACryptoPad;
    using System;
    using System.Collections.Specialized;
    using System.Configuration;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Security;

    /// <summary>
    /// Account controller.
    /// </summary>
    [AllowAnonymous]
    [CheckUMFilter]
    public class SsoController : Controller
    {
        private const string InvalidSettingsMessage = "Invalid settings!";
        private const string Pass = "2conthanhlanconduanhaucanchoi";

        private const string InvalidParametersMessage = "Invalid parameters!";

        private const string InternalLoginErrorMessage = "Internal login failed.";

        private const int DecryptKeySize = 1024;

        private const int InvalidParamErrorCode = 1;
        private const int DisableSsoErrorCode = 2;

        private const string DefaultLanguage = "en-US";

        private readonly string _keyPath = string.Empty;

        #region Services

        private readonly IAccountService _accountService;
        private readonly ISystemSettingService _systemSettingService;

        #endregion Services

        #region Constructor

        public SsoController()
            : this(null, null)
        {
            _keyPath = ConfigurationManager.AppSettings["SSOKeyPath"] ?? string.Empty;
            var langManager = new LanguageManager();
            langManager.SetCulture(AppSession.Language);
        }

        public SsoController(IAccountService accountService, ISystemSettingService systemSettingService)
        {
            _accountService = accountService ?? new AccountService();
            _systemSettingService = systemSettingService ?? new SystemSettingService();
        }

        #endregion Constructor

        private static string CurrentUserName
        {
            get
            {
                var user = System.Web.HttpContext.Current.User;
                if (user != null)
                {
                    return user.Identity == null ? string.Empty : user.Identity.Name;
                }

                return string.Empty;
            }
        }

        private bool IsUserAuthenticated
        {
            get
            {
                var context = System.Web.HttpContext.Current;
                var isAuthenticated = context.User != null
                    && context.User.Identity != null
                    && context.User.Identity.IsAuthenticated
                    && context.Request.IsAuthenticated;
                var session = SessionManager.CurrentSession;
                var isSessionValid = session != null && session.CustID > 0;

                return isAuthenticated && isSessionValid;
            }
        }

        /// <summary>
        /// Return form login.
        /// </summary>
        /// <returns>Login form.</returns>
        [OutputCache(NoStore = true, Duration = 0)]
        [HttpGet]
        public ActionResult Login(string key)
        {
            return SSO(key);
        }

        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0)]
        public ActionResult Index(string key)
        {
            return SSO(key);
        }

        /// <summary>
        /// The denied page.
        /// </summary>
        /// <returns>The denied message.</returns>
        public ActionResult DenyAccess()
        {
            int code;

            if (!int.TryParse(Request.QueryString["Code"] != null ? Request.QueryString["Code"] : "0", out code))
            {
                code = -1;
            }

            ViewBag.Message = Resources.Lang.AccessDenied;
            return View();
        }

        private ResultViewModel InternalLogin(LoginInternalCriteria criteria)
        {
            Account user;
            var result = _accountService.LoginInternal(criteria, out user);

            if (result.ErrorCode == 0 && user.CustID > 0)
            {
                user.OnUserID = _accountService.CheckIn(new CheckInCriteria()
                {
                    CustId = user.CustID,
                    IP = Remote.GetClientIPAddress(),
                    NetSessionId = Session.SessionID,
                    ServerId = user.ServerId
                });

                InitializeSession(user);

                FormsAuthentication.SetAuthCookie(user.Username, false);
            }

            return result;
        }

        private void InitializeSession(Account user)
        {
            var policies = _accountService.UserPolicySel(new UserPolicyCriteria
            {
                CustID = user.CustID,
                LicenseeId = AppSettings.LicenseeID
            });
            var currencyList = _systemSettingService.GetAffSystemSupportedCurrencyList(new SystemSupportedCurrencyCriteria
            {
                UserId = user.CustID,
                LicenseeId = AppSettings.LicenseeID
            }).ToList();
            SessionManager.CurrentSession = new CurrentSession(user, policies, currencyList);
        }

        private string ErrorMessage(int errCode)
        {
            return ErrorMsg.ResourceManager.GetString("Msg" + errCode);
        }

        private ResultViewModel GetResultViewModel(int errCode, string messageSuccess = "", string errorMessage = "")
        {
            return new ResultViewModel
            {
                ErrorCode = errCode,
                Message = errCode == 0 ?
                    (string.IsNullOrEmpty(messageSuccess) ? Lang.UpdateSucc : messageSuccess) :
                    (string.IsNullOrEmpty(errorMessage) ? ErrorMessage(errCode) : errorMessage),
                Success = errCode == 0
            };
        }

        private RedirectResult RedirectToErrorPage(int code, string message)
        {
            TempData["Message"] = message;
            return Redirect("~/Notify/AccessDenied?code=" + code);
        }

        private NameValueCollection GetQueryParams(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return null;
            }

            var parameter = key.Replace(" ", "+");
            var descryptParam = new Crypt().Decrypt(parameter, Pass);

            return HttpUtility.ParseQueryString(descryptParam);
        }

        private bool IsValidLanguage(string languageCode)
        {
            return AppSettings.BaseLanguages.Any(x => x.LanguageID.Equals(languageCode, StringComparison.OrdinalIgnoreCase));
        }

        [SuppressMessage("Microsoft.Usage", "CA1806:DoNotIgnoreMethodResults",
            Justification = "Reviewed. Suppression is OK here.")]
        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed.")]
        private RedirectResult SSO(string key)
        {
            if (string.IsNullOrEmpty(_keyPath))
            {
                return RedirectToErrorPage(DisableSsoErrorCode, InvalidSettingsMessage);
            }

            var queryParams = GetQueryParams(key);
            if (queryParams == null)
            {
                return RedirectToErrorPage(InvalidParamErrorCode, InvalidParametersMessage);
            }

            var criteria = new LoginInternalCriteria()
            {
                LicenseeName = queryParams["lic"] ?? string.Empty,
                Username = queryParams["user"] ?? string.Empty,
                Password = queryParams["pwd"] ?? string.Empty,
                Language = queryParams["lang"],
                LoginIP = HttpContext.Request.UserHostAddress,
                BrowserName = HttpContext.Request.Browser.Browser,
                Platform = HttpContext.Request.Browser.Platform
            };

            if (string.IsNullOrEmpty(criteria.Username) || string.IsNullOrEmpty(criteria.Password))
            {
                return RedirectToErrorPage(InvalidParamErrorCode, InvalidParametersMessage);
            }

            if (!IsValidLanguage(criteria.Language))
            {
                criteria.Language = DefaultLanguage;
            }

            if (IsUserAuthenticated && criteria.Username == CurrentUserName)
            {
                AppSession.Language = criteria.Language;
                var session = SessionManager.CurrentSession;
                var account = new Account()
                {
                    CustID = session.CustID,
                    Username = session.Username,
                    OnUserID = session.OnUserID,
                    IsIT = session.IsIT
                };
                InitializeSession(account);

                FormsAuthentication.SetAuthCookie(session.Username, false);
                return Redirect("~/Home/Index");
            }

            long timeSpan;
            long.TryParse(queryParams["time"], out timeSpan);
            criteria.LoginTime = new DateTime(timeSpan);

            var result = InternalLogin(criteria);
            if (result.ErrorCode == 0)
            {
                return Redirect("~/Home/Index");
            }
            else
            {
                string message = InternalLoginErrorMessage +
                    "\r\nCode: " + result.ErrorCode +
                    "\r\nMessage: " + result.ErrorMessage +
                    "\r\nLicensee: " + criteria.LicenseeName +
                     "\r\nUserName: " + criteria.Username +
                     "\r\nTimeSpan: " + timeSpan +
                     "\r\nLanguage: " + criteria.Language;

                LogService.Info(message);
                TempData["Message"] = message;
                return RedirectToErrorPage(result.ErrorCode, message);
            }
        }
    }
}