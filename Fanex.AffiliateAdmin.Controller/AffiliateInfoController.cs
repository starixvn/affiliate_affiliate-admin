// -----------------------------------------------------------------------
// <copyright file="AffiliateInfoController.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.AffiliateAdmin.Controller
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.SessionState;
    using Fanex.AffiliateAdmin.Core.Configurations;
    using Fanex.AffiliateAdmin.Core.Entities;
    using Fanex.AffiliateAdmin.Core.Enumerations;
    using Fanex.AffiliateAdmin.Core.Filter;
    using Fanex.AffiliateAdmin.Core.Mailinator;
    using Fanex.AffiliateAdmin.Core.Services;
    using Fanex.AffiliateAdmin.Core.Services.Interface;
    using Fanex.AffiliateAdmin.Core.Session;
    using Fanex.AffiliateAdmin.Core.Utilities;
    using Fanex.AffiliateAdmin.Criterias;
    using Fanex.AffiliateAdmin.DTO;
    using Fanex.AffiliateAdmin.Models;
    using Fanex.AffiliateAdmin.Resources;
    using Fanex.AffiliateAdmin.Services;
    using Fanex.Common;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [SessionState(SessionStateBehavior.Default)]
    [FunctionAuth(Function = FunctionPolicyNames.Affiliate, CurrenciesRequired = true)]
    public class AffiliateInfoController : BaseController
    {
        #region Services

        private readonly IAffiliateService _affiliateService;

        private readonly ICountryService _countryService;

        private readonly ISystemSettingService _systemSettingService;

        #endregion Services

        #region Constructor

        public AffiliateInfoController()
            : this(null, null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AffiliateInfoController" /> class.
        /// </summary>
        /// <param name="affiliateService">Affiliate Service.</param>
        /// <param name="countryService">Country Service.</param>
        /// <param name="systemSettingService">System Setting Service.</param>
        public AffiliateInfoController(
            IAffiliateService affiliateService,
            ICountryService countryService,
            ISystemSettingService systemSettingService)
        {
            _affiliateService = affiliateService ?? new AffiliateService();
            _countryService = countryService ?? new CountryService();
            _systemSettingService = systemSettingService ?? new SystemSettingService();
        }

        #endregion Constructor

        /// <summary>
        /// Load main page.
        /// </summary>
        /// <param name="userName">User name.</param>
        /// <returns>View main page.</returns>
        [HttpGet]
        public ActionResult Index(string userName)
        {
            var affiliateSession = SessionManager.CurrentSession.Affiliate;
            var isKiosk = affiliateSession != null && affiliateSession.AffTypeName.Equals("kiosk", StringComparison.CurrentCultureIgnoreCase);

            var siteDomainInfo = SiteDomainInfo.Parse(this, isKiosk);

            if (string.IsNullOrEmpty(userName) && affiliateSession != null)
            {
                userName = affiliateSession.Username;
            }

            var baseUrl = siteDomainInfo.AdminSiteUrl.ToString();

            if (baseUrl.LastIndexOf('/') != baseUrl.Length - 1)
            {
                baseUrl = baseUrl + "/";
            }

            return View(new AffiliateMasterViewModel
            {
                SiteDomainUrl = new Uri(baseUrl),
                UserName = userName
            });
        }

        /// <summary>
        /// Get affiliate information.
        /// </summary>
        /// <param name="userName">Affiliate username.</param>
        /// <returns>Action Result.</returns>
        [HttpGet]
        public ActionResult Info(string userName)
        {
            if (!string.IsNullOrEmpty(userName))
            {
                var session = SessionManager.CurrentSession;
                session.Affiliate = null;
                var model = _affiliateService.GetAffiliateGeneralInfo(new AffiliateInfoCriteria
                {
                    Username = userName,
                    UserId = session.CustID,
                    Currencies = string.Join(",", session.CurrencyList.Select(c => c.CurrencyID).ToArray()),
                    IsITAdmin = session.IsIT,
                    LicenseeId = AppSettings.LicenseeID
                });

                if (model != null && model.AffiliateInfo != null)
                {
                    UpdateAffiliateSession(model.AffiliateInfo);

                    FormaterCurrency.SetDefaultCurrency(model.AffiliateInfo.CurrencyCode);

                    model.ExternalLoginUrl = ExternalLogin.ExternalLoginUrl(new ExternalLoginInfo
                        {
                            Controller = this,
                            Username = session.Affiliate.Username,
                            CustId = session.CustID,
                            OnUserID = session.OnUserID,
                            SessionID = Session.SessionID,
                            AffTypeName = session.Affiliate.AffTypeName
                        });

                    ViewBag.IsLoadOtherPage = Request["IsLoadOtherPage"];
                    return View(model);
                }
                else
                {
                    ModelState.AddModelError("NoUser", Lang.NoUser);
                }
            }

            return View("Search");
        }

        /// <summary>
        /// Get view change password.
        /// </summary>
        /// <param name="password">New password.</param>
        /// <param name="confirmPassword">Confirm password.</param>
        /// <returns>View model.</returns>
        [HttpGet]
        public ActionResult ChangePassword(string password, string confirmPassword)
        {
            var affiliateSession = SessionManager.CurrentSession.Affiliate;
            return View(new PasswordViewModel
                {
                    CustId = affiliateSession.AffiliateId,
                    UserName = affiliateSession.Username,
                    Password = password,
                    ConfirmPass = confirmPassword
                });
        }

        /// <summary>
        /// Change user password.
        /// </summary>
        /// <param name="model">Password view model.</param>
        /// <returns>Action Result.</returns>
        [HttpPost]
        public ActionResult ChangePassword(PasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var session = SessionManager.CurrentSession;
                var criteria = new ChangeAffPasswordCriteria
                    {
                        UserId = session.CustID,
                        Id = session.Affiliate.AffiliateId,
                        ConfirmPassword = Cryptography.CfsEnCode(model.Password),
                        Password = null,
                        Ip = Remote.GetClientIPAddress()
                    };

                _affiliateService.ChangePassword(criteria);

                ViewBag.Result = GetResultViewModel(criteria.ErrCode, Lang.ResetPassSuccess);
            }

            return ChangePassword(model.Password, model.ConfirmPass);
        }

        /// <summary>
        /// Change status.
        /// </summary>
        /// <param name="criteria">Affiliate update status criteria.</param>
        /// <returns>View model.</returns>
        [HttpPost]
        public ActionResult ChangeStatus(AffiliateUpdStatusCriteria criteria)
        {
            const int PASSWORD_LENGTH = 6;
            var session = SessionManager.CurrentSession;
            var password = Generator.GetRandomString(PASSWORD_LENGTH);
            criteria.UserId = session.CustID;
            criteria.AffId = session.Affiliate.AffiliateId;
            criteria.Ip = Remote.GetClientIPAddress();
            criteria.LicenseeId = AppSettings.LicenseeID;

            criteria.Password = Cryptography.CfsEnCode(password);

            this._affiliateService.ChangeAffiliateStatus(criteria);

            if (criteria.ErrCode == 0)
            {
                session.Affiliate.Status = criteria.Status.ToLower();

                if (!criteria.Status.Equals("pending", StringComparison.CurrentCultureIgnoreCase))
                {
                    var siteInfo = SiteDomainInfo.Parse(this, session.Affiliate.AffTypeName.Equals("kiosk", System.StringComparison.CurrentCultureIgnoreCase));
                    var mailinator = AffiliateMailinator.GetMailinator(session.Affiliate.AffTypeName);
                    mailinator.Server = Server;
                    mailinator.EmailTemplateType = criteria.Status.Equals("approved", System.StringComparison.CurrentCultureIgnoreCase) ?
                                        EmailTemplateType.AffiliateApprove :
                                        EmailTemplateType.AffiliateDisapprove;
                    mailinator.Language = session.Affiliate.PrefferredLanguage;
                    mailinator.FormatType = session.Affiliate.MailOption ? EmailFormatType.Text : EmailFormatType.Html;
                    mailinator.EmailOfAffiliate = session.Affiliate.Email;
                    mailinator.EmailTemplateInfo = new EmailTemplateInfo
                    {
                        Status = criteria.Status,
                        Username = session.Affiliate.Username,
                        FirstName = session.Affiliate.FirstName,
                        Password = password,
                        AdminSiteUrl = siteInfo.AdminSiteUrl,
                        AffiliateSiteUrl = siteInfo.AffiliateSiteUrl,
                        MemberSiteUrl = siteInfo.MemberSiteUrl,
                        MemberSiteDomain = siteInfo.MemberSiteDomain
                    };

                    mailinator.Send();
                }
            }

            return Json(GetResultViewModel(criteria.ErrCode, string.Format(Lang.UpdateStaSucc, session.Affiliate.Username)));
        }

        /// <summary>
        /// Change open close.
        /// </summary>
        /// <param name="criteria">Affiliate update status criteria.</param>
        /// <returns>View model.</returns>
        [HttpPost]
        public ActionResult ChangeOpenClose(AffiliateUpdClosedStatusCriteria criteria)
        {
            var session = SessionManager.CurrentSession;
            criteria.UserId = session.CustID;
            criteria.AffId = session.Affiliate.AffiliateId;
            criteria.Ip = Remote.GetClientIPAddress();
            criteria.LicenseeId = AppSettings.LicenseeID;

            this._affiliateService.ChangeAffiliateOpenClose(criteria);

            return Json(GetResultViewModel(criteria.ErrCode, string.Format(Lang.UpdateStaSucc, session.Affiliate.Username)));
        }

        /// <summary>
        /// Load statistic.
        /// </summary>
        /// <param name="criteria">Affiliate statistic criteria.</param>
        /// <returns>View model.</returns>
        [HttpGet]
        public ActionResult Statistic(AffiliateStatisticCriteria criteria)
        {
            var affiliateSession = SessionManager.CurrentSession.Affiliate;
            criteria.AffId = affiliateSession.AffiliateId;
            criteria.MonthDate = criteria.GetFullDate(SystemTimeService.GetSystemDateTime());

            var model = _affiliateService.GetAffiliateStatistic(criteria);
            model.FullDateTime = criteria.GetFullDate(SystemTimeService.GetSystemDateTime());
            model.AffId = affiliateSession.AffiliateId;
            model.AffName = affiliateSession.Username;
            model.BaseCurrency = AppSettings.BaseExchange.Code;
            model.AffCurrency = affiliateSession.Currency;

            return View(model);
        }

        /// <summary>
        /// Load affiliate information.
        /// </summary>
        /// <param name="model">Model input.</param>
        /// <returns>View model.</returns>
        [HttpGet]
        public ActionResult Edit(AffiliateEditViewModel model)
        {
            if (model.Affiliate == null)
            {
                var criteria = new AffiliateSelCriteria
                    {
                        AffId = SessionManager.CurrentSession.Affiliate.AffiliateId
                    };

                var affiliate = _affiliateService.GetAffiliateInfo(criteria);

                model = new AffiliateEditViewModel
                    {
                        Affiliate = affiliate,
                        AffMinCheque = criteria.AffMinCheque,
                        AffMinMoneyBookers = criteria.AffMinMoneyBookers,
                        AffMinNeteller = criteria.AffMinNeteller,
                        AffMinPlayer = criteria.AffMinPlayer,
                        AffMinWire = criteria.AffMinWire
                    };

                model.AddCountries(_countryService.GetAll(new CountryCriteria()));

                model.AddPaymentMethods(_systemSettingService.GetPaymentMethod(new PaymentMethodCriteria()));

                model.Affiliate.HtmlDecode();
            }

            return View(model);
        }

        /// <summary>
        /// Update information affiliate.
        /// </summary>
        /// <param name="criteria">Affiliate update criteria.</param>
        /// <returns>View model.</returns>
        [HttpPost]
        public ActionResult Edit(AffiliateUpdCriteria criteria)
        {
            var model = CacheUserInput(criteria);
            criteria.HtmlEncode();

            if (ModelState.IsValid)
            {
                var session = SessionManager.CurrentSession;
                criteria.Ip = Remote.GetClientIPAddress();
                criteria.AffId = session.Affiliate.AffiliateId;
                criteria.UserId = session.CustID;
                criteria.PreferredPayment = criteria.PreferredPayment ?? session.Affiliate.PrefferredPayment;
                criteria.LicenseeId = AppSettings.LicenseeID;

                _affiliateService.UpdateAffiliateInfo(criteria);

                if (criteria.ErrCode == 0)
                {
                    var affiliateInfo = _affiliateService.GetAffiliateInfo(new AffiliateSelCriteria { AffId = criteria.AffId });

                    UpdateAffiliateSession(affiliateInfo);
                }

                ViewBag.Result = GetResultViewModel(criteria.ErrCode, string.Format(Lang.UpdAffSuccess, session.Affiliate.Username));
            }

            return this.Edit(model);
        }

        /// <summary>
        /// Affiliate change language.
        /// </summary>
        /// <param name="criteria">Affiliate update language criteria.</param>
        /// <returns>JSON status update language.</returns>
        [HttpPost]
        public ActionResult ChangeLanguage(AffiliateUpdLanguageCriteria criteria)
        {
            var session = SessionManager.CurrentSession;
            if (ModelState.IsValid)
            {
                criteria.Ip = Remote.GetClientIPAddress();
                criteria.AffId = session.Affiliate.AffiliateId;
                criteria.UserId = session.CustID;
                criteria.LicenseeId = AppSettings.LicenseeID;

                this._affiliateService.ChangeAffiliateLanguage(criteria);

                if (criteria.ErrCode == 0)
                {
                    var affiliateInfo = _affiliateService.GetAffiliateInfo(new AffiliateSelCriteria { AffId = criteria.AffId });
                    UpdateAffiliateSession(affiliateInfo);
                }
            }

            return Json(GetResultViewModel(criteria.ErrCode, string.Format(Lang.ErrLanguageSucc, session.Affiliate.Username)));
        }

        /// <summary>
        /// Affiliate change language.
        /// </summary>
        /// <param name="criteria">Affiliate update language criteria.</param>
        /// <returns>JSON status update language.</returns>
        [HttpPost]
        public ActionResult ChangeAffType(AffiliateUpdAffTypeCriteria criteria)
        {
            var session = SessionManager.CurrentSession;
            if (ModelState.IsValid)
            {
                criteria.Ip = Remote.GetClientIPAddress();
                criteria.AffId = session.Affiliate.AffiliateId;
                criteria.UserId = session.CustID;
                criteria.LicenseeId = AppSettings.LicenseeID;

                this._affiliateService.ChangeAffType(criteria);

                if (criteria.ErrCode == 0)
                {
                    var affiliateInfo = _affiliateService.GetAffiliateInfo(new AffiliateSelCriteria { AffId = criteria.AffId });
                    UpdateAffiliateSession(affiliateInfo);
                }
            }

            return Json(GetResultViewModel(criteria.ErrCode, string.Format(Lang.UpdateAffType, session.Affiliate.Username)));
        }

        /// <summary>
        /// Change status.
        /// </summary>
        /// <param name="criteria">Affiliate update status criteria.</param>
        /// <returns>View model.</returns>
        [HttpPost]
        public ActionResult UnLocked(AffiliateUnlockCriteria criteria)
        {
            var session = SessionManager.CurrentSession;
            criteria.UserId = session.CustID;
            criteria.AffId = session.Affiliate.AffiliateId;
            criteria.Ip = Remote.GetClientIPAddress();
            criteria.LicenseeId = AppSettings.LicenseeID;

            this._affiliateService.AffiliateUnLocked(criteria);

            return Json(GetResultViewModel(criteria.ErrCode));
        }

        #region Helpers

        /// <summary>
        /// Cache user input when editing affiliate.
        /// </summary>
        /// <param name="criteria">Criteria input.</param>
        /// <returns>Model Object.</returns>
        private AffiliateEditViewModel CacheUserInput(AffiliateUpdCriteria criteria)
        {
            var affiliateSession = SessionManager.CurrentSession.Affiliate;
            var selectCriteria = new AffiliateSelCriteria
            {
                AffId = affiliateSession.AffiliateId
            };

            var affiliate = _affiliateService.GetAffiliateInfo(selectCriteria);
            affiliate.AffId = affiliateSession.AffiliateId;
            affiliate.Username = affiliateSession.Username;
            affiliate.FirstName = criteria.FirstName;
            affiliate.LastName = criteria.LastName;
            affiliate.Address = criteria.Address;
            affiliate.City = criteria.City;
            affiliate.State = criteria.State;
            affiliate.Gender = criteria.Gender;
            affiliate.Email = criteria.Email;
            affiliate.Phone = criteria.Phone;
            affiliate.MobilePhone = criteria.MobilePhone;
            affiliate.Postal = criteria.Postal;
            affiliate.CountryId = criteria.Country;
            affiliate.Currency = short.Parse(criteria.Currency);
            affiliate.MailOption = criteria.MailOption;
            affiliate.SiteLink = criteria.SiteLink;
            affiliate.MoreSiteURLs = criteria.MoreSiteUrls;
            affiliate.PreferredPayment = criteria.PreferredPayment;
            affiliate.Player_Username = criteria.PlayerUserName;

            var model = new AffiliateEditViewModel
            {
                Affiliate = affiliate,
                AffMinCheque = selectCriteria.AffMinCheque,
                AffMinMoneyBookers = selectCriteria.AffMinMoneyBookers,
                AffMinNeteller = selectCriteria.AffMinNeteller,
                AffMinPlayer = selectCriteria.AffMinPlayer,
                AffMinWire = selectCriteria.AffMinWire
            };

            model.AddCountries(_countryService.GetAll(new CountryCriteria()));

            model.AddPaymentMethods(_systemSettingService.GetPaymentMethod(new PaymentMethodCriteria()));

            return model;
        }

        /// <summary>
        /// Update Affiliate Session.
        /// </summary>
        /// <param name="affiliateInfo">Affiliate info.</param>
        private void UpdateAffiliateSession(Affiliate affiliateInfo)
        {
            SessionManager.CurrentSession.Affiliate = new AffiliateSession
            {
                AffiliateId = affiliateInfo.AffId,
                Username = affiliateInfo.Username,
                FirstName = affiliateInfo.FirstName,
                Prefix = affiliateInfo.Prefix,
                Status = affiliateInfo.Status,
                PrefferredLanguage = affiliateInfo.PreferredLanguage,
                AffType = affiliateInfo.AffType,
                AffTypeName = AppSettings.AffTypes.FirstOrDefault(t => t.TypeID == affiliateInfo.AffType).Name,
                MailOption = affiliateInfo.MailOption,
                Email = affiliateInfo.Email,
                PrefferredPayment = affiliateInfo.PreferredPayment,
                GroupID = affiliateInfo.GroupId,
                CurrencyID = affiliateInfo.Currency,
                Currency = affiliateInfo.CurrencyCode
            };
        }

        #endregion Helpers
    }
}