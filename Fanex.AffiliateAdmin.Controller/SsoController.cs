// --------------------------------------------------------------------------
// <copyright file="SsoController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.AffiliateAdmin.Controller
{
    using System;
    using System.Collections.Specialized;
    using System.Configuration;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Security;
    using Core.Configurations;
    using Core.Services;
    using Core.Services.Interface;
    using Core.Session;
    using Core.Utilities;
    using Criterias;
    using DTO;
    using Fanex.AffiliateAdmin.Core;
    using Fanex.AffiliateAdmin.Services;
    using Fanex.Logging;
    using Models;
    using Resources;
    using RSACryptoPad;

    /// <summary>
    /// Account controller.
    /// </summary>
    public class SsoController : Controller
    {
        private const string InternalLoginErrorMessage = "Internal login failed.";

        private const int DecryptKeySize = 1024;

        private const int InValidParamErrorCode = 1;
        private const int DisableSsoErrorCode = 2;

        private const string PrivateKey = "12bet.snk";
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
        [HttpGet]
        public ActionResult Login()
        {
            string startTime = string.Empty, endTime = string.Empty;

            if (UnderMaintenanceService.GetDownTime(ref startTime, ref endTime))
            {
                ViewBag.StartTime = startTime;
                ViewBag.EndTime = endTime;

                return Redirect("~/Notify");
            }
            else
            {
                return SSO();
            }
        }

        /// <summary>
        /// The denied page.
        /// </summary>
        /// <returns>The denied message.</returns>
        [AllowAnonymous]
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

        private ResultViewModel InternalLogin(string userName, string passWord)
        {
            var result = new ResultViewModel();

            var criteria = new LoginCriteria
            {
                Username = userName,
                UserPwd = passWord,
                LoginIP = Remote.GetClientIPAddress(),
                From = Request.UserHostName,
                NetSessionID = Session.SessionID
            };

            Account user = _accountService.Login(criteria);

            result.Success = criteria.ErrCode == 0;
            if (criteria.ErrCode == 0 && user.CustID > 0)
            {
                FormsAuthentication.SetAuthCookie(userName, true);

                SessionManager.CurrentSession = new CurrentSession(user)
                {
                    RoleAccess = _accountService.UserPolicySel(new UserPolicyCriteria
                    {
                        CustID = user.CustID
                    }).ToList(),
                    CurrencyList = _systemSettingService.GetAffSystemSupportedCurrencyList(new SystemSupportedCurrencyCriteria
                    {
                        UserId = user.CustID
                    }).ToList()
                };
            }
            else
            {
                result = GetResultViewModel(criteria.ErrCode);
            }

            return result;
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
        
        private RedirectResult RedirectToErrorPage(int code)
        {
            return Redirect("~/DenyAccess?code=" + code);
        }

        private string GetPrivateKeyPath()
        {
            var privateKeyPath = Server.MapPath("~/" + _keyPath + PrivateKey);
            if (!System.IO.File.Exists(privateKeyPath))
            {
                RedirectToErrorPage(DisableSsoErrorCode);
            }

            return privateKeyPath;
        }

        private string GetRequestKey()
        {
            string parameter = HttpContext.Request["key"] ?? string.Empty;

            if (string.IsNullOrEmpty(parameter))
            {
                RedirectToErrorPage(InValidParamErrorCode);
            }

            return parameter;
        }

        private NameValueCollection GetQueryParams()
        {
            string parameter = GetRequestKey();
            var privateKeyPath = GetPrivateKeyPath();

            parameter = parameter.Replace(" ", "+");
            var descryptParam = new EncryptionThread().DecryptString(parameter, DecryptKeySize, privateKeyPath);

            return HttpUtility.ParseQueryString(descryptParam);
        }

        private bool IsValidLanguage(string languageCode)
        {
            return AppSettings.BaseLanguages.Any(x => x.LanguageID.Equals(languageCode, StringComparison.OrdinalIgnoreCase));
        }

        [SuppressMessage("Microsoft.Usage", "CA1806:DoNotIgnoreMethodResults",
            Justification = "Reviewed. Suppression is OK here.")]
        private RedirectResult SSO()
        {
            if (string.IsNullOrEmpty(_keyPath))
            {
                RedirectToErrorPage(DisableSsoErrorCode);
            }

            var queryParams = GetQueryParams();
            var licname = queryParams["affname"] ?? string.Empty;
            var userName = queryParams["username"] ?? string.Empty;
            var passWord = queryParams["password"] ?? string.Empty;
            var siteOutUrl = queryParams["loginurl"] ?? string.Empty;
            var languageCode = queryParams["language"] ?? DefaultLanguage;
            if (!IsValidLanguage(languageCode))
            {
                languageCode = DefaultLanguage;
            }

            if (IsUserAuthenticated)
            {
                if (userName == CurrentUserName)
                {
                    AppSession.Language = languageCode;
                    return Redirect("~/Home/Index");
                }
            }

            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(passWord))
            {
                return RedirectToErrorPage(InValidParamErrorCode);
            }

            var resultInfo = InternalLogin(userName, passWord);
            if (resultInfo.Success)
            {
                FormsAuthentication.SetAuthCookie(userName, false);
                return Redirect("~/Home/Index");
            }
            else
            {
                long timeSpan;
                long.TryParse(queryParams["timestamp"], out timeSpan);

                string message = InternalLoginErrorMessage +
                    "\r\nCode: " + resultInfo.ErrorCode +
                    "\r\nMessage: " + resultInfo.Message +
                    "\r\nLicensee: " + licname +
                     "\r\nUserName: " + userName +
                     "\r\nTimeSpan: " + timeSpan +
                     "\r\nSiteOutUrl: " + siteOutUrl +
                     "\r\nLanguageCode: " + languageCode;

                LogService.Info(message);

                return RedirectToErrorPage(resultInfo.ErrorCode);
            }
        }
    }
}