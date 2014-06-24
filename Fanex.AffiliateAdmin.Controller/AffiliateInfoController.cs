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
    using Common;
    using Core.Configurations;
    using Core.Entities;
    using Core.Enumerations;
    using Core.Filter;
    using Core.Services.Interface;
    using Core.Utilities;
    using Criterias;
    using DTO;
    using Fanex.AffiliateAdmin.Core.Mailinator;
    using Models;
    using Resources;
    using Services.Interface;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [FunctionAuth(Function = "[Affiliate]")]
    public class AffiliateInfoController : BaseController
    {
        #region Services

        private readonly IAffiliateService _affiliateService;

        private readonly ICountryService _countryService;

        private readonly ISystemSettingService _systemSettingService;

        #endregion Services

        #region Constructor

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
            _affiliateService = affiliateService;
            _countryService = countryService;
            _systemSettingService = systemSettingService;
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
            var isKiosk = CurrentSession.Affiliate != null && CurrentSession.Affiliate.AffTypeName.Equals("kiosk", StringComparison.CurrentCultureIgnoreCase);

            var siteDomainInfo = SiteDomainInfo.Parse(this, isKiosk);

            if (string.IsNullOrEmpty(userName) && this.CurrentSession.Affiliate != null)
            {
                userName = this.CurrentSession.Affiliate.Username;
            }

            return View(new AffiliateMasterViewModel
            {
                SiteDomainUrl = new Uri(siteDomainInfo.AdminSiteUrl),
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
                this.CurrentSession.Affiliate = null;
                var model = _affiliateService.GetAffiliateGeneralInfo(new AffiliateInfoCriteria
                {
                    Username = userName,
                    UserId = CurrentSession.CustID,
                    Currencies = string.Join(",", CurrentSession.CurrencyList.Select(c => c.CurrencyID).ToArray()),
                    IsITAdmin = CurrentSession.IsIT
                });

                if (model != null && model.AffiliateInfo != null)
                {
                    UpdateAffiliateSession(model.AffiliateInfo);
                    
                    FormaterCurrency.SetDefaultCurrency(model.AffiliateInfo.CurrencyCode);
                  
                    model.ExternalLoginUrl = new Uri(ExternalLogin.ExternalLoginUrl(new ExternalLoginInfo
                        {
                            Controller = this,
                            Username = CurrentSession.Affiliate.Username,
                            CustId = CurrentSession.CustID,
                            OnUserID = CurrentSession.OnUserID,
                            SessionID = Session.SessionID,
                            AffTypeName = CurrentSession.Affiliate.AffTypeName
                        }));

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
        /// <returns>View model.</returns>
        [HttpGet]
        public ActionResult ChangePassword()
        {
            return View(new PasswordViewModel
                {
                    CustId = CurrentSession.Affiliate.AffiliateId,
                    UserName = CurrentSession.Affiliate.Username,
                });
        }

        /// <summary>
        /// Change user password.
        /// </summary>
        /// <param name="criteria">Change Password Criteria.</param>
        /// <returns>Action Result.</returns>
        [HttpPost]
        public ActionResult ChangePassword(PasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var criteria = new ChangeAffPasswordCriteria
                    {
                        UserId = CurrentSession.CustID,
                        Id = CurrentSession.Affiliate.AffiliateId,
                        ConfirmPassword = Cryptography.CfsEnCode(model.Password),
                        Password = null,
                        Ip = Remote.GetClientIPAddress()
                    };

                _affiliateService.ChangePassword(criteria);

                ViewBag.Result = GetResultViewModel(criteria.ErrCode, Lang.ResetPassSuccess);
            }

            return ChangePassword(); 
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
            var password = Generator.GetRandomString(PASSWORD_LENGTH);
            criteria.UserId = CurrentSession.CustID;
            criteria.AffId = CurrentSession.Affiliate.AffiliateId;
            criteria.Ip = Remote.GetClientIPAddress();
            criteria.Type = "Status";
            criteria.Password = Cryptography.CfsEnCode(password); 

            this._affiliateService.ChangeAffiliateStatus(criteria);

            if (criteria.ErrCode == 0)
            {
                this.CurrentSession.Affiliate.Status = criteria.Status.ToLower();

                if (!criteria.Status.Equals("pending", StringComparison.CurrentCultureIgnoreCase))
                {
                    var siteInfo = SiteDomainInfo.Parse(this, CurrentSession.Affiliate.AffTypeName.Equals("kiosk", System.StringComparison.CurrentCultureIgnoreCase));
                    var mailinator = AffiliateMailinator.GetMailinator(CurrentSession.Affiliate.AffTypeName);
                    mailinator.Server = Server;
                    mailinator.EmailTemplateType = criteria.Status.Equals("approved", System.StringComparison.CurrentCultureIgnoreCase) ?
                                        EmailTemplateType.AffiliateApprove :
                                        EmailTemplateType.AffiliateDisapprove;
                    mailinator.Language = CurrentSession.Affiliate.PrefferredLanguage;
                    mailinator.FormatType = CurrentSession.Affiliate.MailOption ? EmailFormatType.Text : EmailFormatType.Html;
                    mailinator.EmailOfAffiliate = CurrentSession.Affiliate.Email;
                    mailinator.EmailTemplateInfo = new EmailTemplateInfo
                    {
                        Status = criteria.Status,
                        Username = CurrentSession.Affiliate.Username,
                        FirstName = CurrentSession.Affiliate.FirstName,
                        Password = password,
                        AdminSiteUrl = siteInfo.AdminSiteUrl,
                        AffiliateSiteUrl = siteInfo.AffiliateSiteUrl,
                        MemberSiteUrl = siteInfo.MemberSiteUrl,
                        MemberSiteDomain = siteInfo.MemberSiteDomain
                    };

                    mailinator.Send();
                }
            }

            return Json(GetResultViewModel(criteria.ErrCode, string.Format(Lang.UpdateStaSucc, CurrentSession.Affiliate.Username)));
        }

        /// <summary>
        /// Change open close.
        /// </summary>
        /// <param name="criteria">Affiliate update status criteria.</param>
        /// <returns>View model.</returns>
        [HttpPost]
        public ActionResult ChangeOpenClose(AffiliateUpdStatusCriteria criteria)
        {
            criteria.UserId = CurrentSession.CustID;
            criteria.AffId = CurrentSession.Affiliate.AffiliateId;
            criteria.Ip = Remote.GetClientIPAddress();

            this._affiliateService.ChangeAffiliateOpenClose(criteria);

            return Json(GetResultViewModel(criteria.ErrCode, string.Format(Lang.UpdateStaSucc, CurrentSession.Affiliate.Username)));
        }

        /// <summary>
        /// Load statistic.
        /// </summary>
        /// <param name="criteria">Affiliate statistic criteria.</param>
        /// <returns>View model.</returns>
        [HttpGet]
        public ActionResult Statistic(AffiliateStatisticCriteria criteria)
        {
            criteria.AffId = CurrentSession.Affiliate.AffiliateId;

            var model = _affiliateService.GetAffiliateStatistic(criteria);
            model.FullDateTime = criteria.MonthDate;
            model.AffId = CurrentSession.Affiliate.AffiliateId;
            model.AffName = CurrentSession.Affiliate.Username;
            model.BaseCurrency = AppSettings.BaseExchange.Code;
            model.AffCurrency = CurrentSession.Affiliate.Currency;

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
                        AffId = CurrentSession.Affiliate.AffiliateId
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
                criteria.Ip = Remote.GetClientIPAddress();
                criteria.AffId = this.CurrentSession.Affiliate.AffiliateId;
                criteria.UserId = this.CurrentSession.CustID;
                criteria.PreferredPayment = criteria.PreferredPayment ?? CurrentSession.Affiliate.PrefferredPayment;

                _affiliateService.UpdateAffiliateInfo(criteria);

                if (criteria.ErrCode == 0)
                {
                    var affiliateInfo = _affiliateService.GetAffiliateInfo(new AffiliateSelCriteria { AffId = criteria.AffId });

                    UpdateAffiliateSession(affiliateInfo);
                }

                ViewBag.Result = GetResultViewModel(criteria.ErrCode, string.Format(Lang.UpdAffSuccess, CurrentSession.Affiliate.Username));
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
            if (ModelState.IsValid)
            {
                criteria.Ip = Remote.GetClientIPAddress();
                criteria.AffId = this.CurrentSession.Affiliate.AffiliateId;
                criteria.UserId = this.CurrentSession.CustID;
                this._affiliateService.ChangeAffiliateLanguage(criteria);

                if (criteria.ErrCode == 0)
                {
                    var affiliateInfo = _affiliateService.GetAffiliateInfo(new AffiliateSelCriteria { AffId = criteria.AffId });
                    UpdateAffiliateSession(affiliateInfo);
                }
            }

            return Json(GetResultViewModel(criteria.ErrCode, string.Format(Lang.ErrLanguageSucc, CurrentSession.Affiliate.Username)));
        }

        /// <summary>
        /// Affiliate change language.
        /// </summary>
        /// <param name="criteria">Affiliate update language criteria.</param>
        /// <returns>JSON status update language.</returns>
        [HttpPost]
        public ActionResult ChangeAffType(AffiliateUpdAffTypeCriteria criteria)
        {
            if (ModelState.IsValid)
            {
                criteria.Ip = Remote.GetClientIPAddress();
                criteria.AffId = this.CurrentSession.Affiliate.AffiliateId;
                criteria.UserId = this.CurrentSession.CustID;

                this._affiliateService.ChangeAffType(criteria);

                if (criteria.ErrCode == 0)
                {
                    var affiliateInfo = _affiliateService.GetAffiliateInfo(new AffiliateSelCriteria { AffId = criteria.AffId });
                    UpdateAffiliateSession(affiliateInfo);
                }
            }

            return Json(GetResultViewModel(criteria.ErrCode, string.Format(Lang.UpdateAffType, CurrentSession.Affiliate.Username)));
        }

        /// <summary>
        /// Change status.
        /// </summary>
        /// <param name="criteria">Affiliate update status criteria.</param>
        /// <returns>View model.</returns>
        [HttpPost]
        public ActionResult UnLocked(AffiliateUnlockCriteria criteria)
        {
            criteria.UserId = CurrentSession.CustID;
            criteria.AffId = CurrentSession.Affiliate.AffiliateId;
            criteria.Ip = Remote.GetClientIPAddress();

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
            var selectCriteria = new AffiliateSelCriteria
            {
                AffId = CurrentSession.Affiliate.AffiliateId
            };

            var affiliate = _affiliateService.GetAffiliateInfo(selectCriteria);
            affiliate.AffId = CurrentSession.Affiliate.AffiliateId;
            affiliate.Username = CurrentSession.Affiliate.Username;
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
            CurrentSession.Affiliate = new AffiliateSession
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
        #endregion
    }
}
