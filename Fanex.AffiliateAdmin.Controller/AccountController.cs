// --------------------------------------------------------------------------
// <copyright file="AccountController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Johnny.Trinh" date="2013.8.2">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Controller
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.Security;
    using Common;
    using Core.Configurations;
    using Core.Filter;
    using Core.Services;
    using Core.Services.Interface;
    using Core.Session;
    using Core.Utilities;
    using Criterias;
    using DTO;
    using Fanex.AffiliateAdmin.Core;
    using Fanex.AffiliateAdmin.Services;
    using Models;
    using Resources;

    /// <summary>
    /// Account controller.
    /// </summary>
    public class AccountController : Controller
    {
        #region Services

        private readonly IAccountService _accountService;
        private readonly ISystemSettingService _systemSettingService;

        #endregion Services

        #region Constructor

        public AccountController()
            : this(null, null)
        {
            ////Set default language
            var langManager = new LanguageManager();
            langManager.SetCulture(AppSession.Language);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountController" /> class.
        /// </summary>
        /// <param name="accountService">Account Service.</param>
        /// <param name="systemSettingService">System Setting Service.</param>
        public AccountController(IAccountService accountService, ISystemSettingService systemSettingService)
        {
            _accountService = accountService ?? new AccountService();
            _systemSettingService = systemSettingService ?? new SystemSettingService();
        }

        #endregion Constructor

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
                    FormsAuthentication.SetAuthCookie(username, true);

                    SessionManager.CurrentSession = new CurrentSession(user)
                    {
                        RoleAccess = _accountService.UserPolicySel(new UserPolicyCriteria
                        {
                            CustID = user.CustID,
                            LicenseeId = AppSettings.LicenseeID
                        }).ToList(),
                        CurrencyList = _systemSettingService.GetAffSystemSupportedCurrencyList(new SystemSupportedCurrencyCriteria
                        {
                            UserId = user.CustID,
                            LicenseeId = AppSettings.LicenseeID
                        }).ToList()
                    };
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
        public string GetUserSalt(string userName)
        {
            var user = _accountService.GetByUsername(userName);
            if (user != null)
            {
                return user.UserSalt;
            }

            return string.Empty;
        }

        /// <summary>
        /// Change password.
        /// </summary>
        /// <returns>Return change password form.</returns>
        [HttpGet]
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
                    var criteria = new AccountPasswordCriteria
                        {
                            CustId = SessionManager.CurrentSession.CustID,
                            OldPassword = Cryptography.CfsEnCode(model.OldPassword),
                            NewPassword = Cryptography.CfsEnCode(model.NewPassword),
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
        public ActionResult ReCheckIn()
        {
            var result = new ResultViewModel();

            KickedOutSession(result);

            if (result.Success)
            {
                return Json(result);
            }

            KickedOutDownTime(result);

            return Json(result);
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

            result.Success = isKickedOut;
            result.Message = isKickedOut ? Lang.MsgKickout : string.Empty;

            if (!AppSettings.LocalTest)
            {
                if (isKickedOut)
                {
                    result.ErrorMessage = "Expired";

                    // Signout and clean all session.
                    DoLogOut();
                }
            }
        }

        /// <summary>
        /// Kicked out down time.
        /// </summary>
        /// <param name="result">Kick-out result.</param>
        private void KickedOutDownTime(ResultViewModel result)
        {
            string startTime = string.Empty, endTime = string.Empty;
            result.Success = UnderMaintenanceService.GetDownTime(ref startTime, ref endTime);
            if (result.Success)
            {
                result.Message = "Under-maintenance";
                result.ErrorMessage = "UM";
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