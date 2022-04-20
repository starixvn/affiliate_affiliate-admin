namespace Fanex.AffiliateAdmin.Controller
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using Core.Enumerations;
    using Core.Services.Interface;
    using Core.Session;
    using Core.Utilities;
    using Criterias;
    using Criterias.Configuration.Country;
    using DTO;
    using Fanex.AffiliateAdmin.Core.Configurations;
    using Fanex.AffiliateAdmin.Core.Filter;
    using Fanex.AffiliateAdmin.Core.Services;
    using Fanex.AffiliateAdmin.Services;
    using Models;
    using Resources;

    [FunctionAuth(Function = FunctionPolicyNames.Domain)]
    [AuthorizeRestriction(AppMode.Self)]
    public partial class DomainController : BaseController
    {
        #region Services

        private readonly IDomainService _domainService;

        #endregion Services

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainController" /> class.
        /// </summary>
        /// <param name="service">Domain Service.</param>
        public DomainController(
            IDomainService service,
            ISystemSettingService systemSettingService) : base(systemSettingService)
        {
            _domainService = service;
        }

        #endregion Constructor

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult AffDomain()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AffDomainList()
        {
            var result = _domainService.AffDomainList(new DomainAffSelCriteria());
            return Json(new { List = result });
        }

        /// <summary>
        /// Show affiliate domain Add form.
        /// </summary>
        /// <returns>Affiliate domain add form.</returns>
        [HttpGet]
        public ActionResult AffDomainAdd()
        {
            return View(new DomainViewModel());
        }

        /// <summary>
        /// Post action to add new affiliate domain.
        /// </summary>
        /// <param name="domain">Domain name.</param>
        /// <returns>Result View Model.</returns>
        [HttpPost]
        public ActionResult AffDomainAdd(DomainViewModel domain)
        {
            if (ModelState.IsValid)
            {
                var session = SessionManager.CurrentSession;

                var criteria = new DomainAffInsCriteria
                {
                    UserID = session.CustID,
                    DomainName = domain.DomainName,
                    IP = Remote.GetClientIPAddress(),
                    AdminUserName = session.Username
                };

                _domainService.AffDomainAdd(criteria);

                ViewBag.Result = GetResultViewModel(criteria.ErrCode, Lang.AddSucc);
            }

            return AffDomainAdd();
        }

        /// <summary>
        /// Show update form.
        /// </summary>
        /// <param name="id">Domain Id need to update.</param>
        /// <returns>Show update domain form.</returns>
        [HttpGet]
        public ActionResult AffDomainUpd(int id)
        {
            Domain result = _domainService.AffDomainSelectById(new DomainAffSelByIdCriteria
            {
                DomainId = id
            });

            return View(new DomainViewModel { DomainID = result.DomainID, DomainName = result.DomainName });
        }

        /// <summary>
        /// Post to update affiliate domain.
        /// </summary>
        /// <param name="domain">Domain name.</param>
        /// <returns>Result view model.</returns>
        [HttpPost]
        public ActionResult AffDomainUpd(DomainViewModel domain)
        {
            if (ModelState.IsValid)
            {
                var session = SessionManager.CurrentSession;

                var criteria = new DomainAffUpdCriteria
                {
                    UserID = session.CustID,
                    DomainID = domain.DomainID,
                    DomainName = domain.DomainName,
                    IP = Remote.GetClientIPAddress(),
                    AdminUserName = session.Username
                };

                _domainService.AffDomainUpdate(criteria);

                ViewBag.Result = GetResultViewModel(criteria.ErrCode);
            }

            return AffDomainUpd(domain.DomainID);
        }

        /// <summary>
        /// Post to remove affiliate domain.
        /// </summary>
        /// <param name="id">Domain Id need to update.</param>
        /// <returns>Result view model.</returns>
        [HttpPost]
        public ActionResult AffDomainRemove(int id)
        {
            var criteria = new DomainAffDelCriteria
            {
                UserID = SessionManager.CurrentSession.CustID,
                DomainID = Convert.ToInt32(id),
                IP = Remote.GetClientIPAddress(),
                LicenseeId = AppSettings.LicenseeID
            };
            _domainService.AffDomainDelete(criteria);

            var result = GetResultViewModel(criteria.ErrCode);

            return Json(result);
        }

        /// <summary>
        /// Member domain page.
        /// </summary>
        /// <returns>Return Member domain page.</returns>
        [HttpGet]
        public ActionResult MemDomain()
        {
            return View();
        }

        /// <summary>
        /// Get list Member domain.
        /// </summary>
        /// <returns>List member domain.</returns>
        [HttpPost]
        public ActionResult MemDomainList()
        {
            var result = _domainService.MemDomainList(new DomainMemSelCriteria());
            return Json(new { List = result });
        }

        /// <summary>
        /// Show Member domain Add form.
        /// </summary>
        /// <returns>Show member domain add form.</returns>
        [HttpGet]
        public ActionResult MemDomainAdd()
        {
            return View(new DomainViewModel());
        }

        /// <summary>
        /// Post action to add new Member domain.
        /// </summary>
        /// <param name="domain">Domain view model.</param>
        /// <returns>Result view model.</returns>
        [HttpPost]
        public ActionResult MemDomainAdd(DomainViewModel domain)
        {
            if (ModelState.IsValid)
            {
                var session = SessionManager.CurrentSession;

                var criteria = new DomainMemInsCriteria
                {
                    UserID = session.CustID,
                    DomainName = domain.DomainName,
                    IP = Remote.GetClientIPAddress(),
                    AdminUserName = session.Username
                };

                _domainService.MemDomainAdd(criteria);

                ViewBag.Result = GetResultViewModel(criteria.ErrCode, Lang.AddSucc);
            }

            return MemDomainAdd();
        }

        /// <summary>
        /// Show update form.
        /// </summary>
        /// <param name="id">Domain Id need to update.</param>
        /// <returns>Show member domain update form.</returns>
        [HttpGet]
        public ActionResult MemDomainUpd(int id)
        {
            Domain result = _domainService.MemDomainSelectById(new DomainMemSelByIdCriteria
            {
                DomainId = id
            });

            return View(new DomainViewModel { DomainID = result.DomainID, DomainName = result.DomainName });
        }

        /// <summary>
        /// Post to update Member domain.
        /// </summary>
        /// <param name="domain">Domain View Model.</param>
        /// <returns>Result view model.</returns>
        [HttpPost]
        public ActionResult MemDomainUpd(DomainViewModel domain)
        {
            if (ModelState.IsValid)
            {
                var session = SessionManager.CurrentSession;

                var criteria = new DomainMemUpdCriteria
                {
                    UserID = session.CustID,
                    DomainID = domain.DomainID,
                    DomainName = domain.DomainName,
                    IP = Remote.GetClientIPAddress(),
                    AdminUserName = session.Username
                };

                _domainService.MemDomainUpdate(criteria);

                ViewBag.Result = GetResultViewModel(criteria.ErrCode);
            }

            return MemDomainUpd(domain.DomainID);
        }

        /// <summary>
        /// Post to remove Member domain.
        /// </summary>
        /// <param name="id">Domain Id need to update.</param>
        /// <returns>Result view model.</returns>
        [HttpPost]
        public ActionResult MemDomainRemove(int id)
        {
            var criteria = new DomainMemDelCriteria
            {
                UserID = SessionManager.CurrentSession.CustID,
                DomainID = Convert.ToInt32(id),
                IP = Remote.GetClientIPAddress(),
                LicenseeId = AppSettings.LicenseeID
            };
            _domainService.MemDomainDelete(criteria);

            var result = GetResultViewModel(criteria.ErrCode);

            return Json(result);
        }

        [HttpGet]
        public ActionResult DomainControl()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DomainControlList()
        {
            var result = _domainService.DomainControlList(new DomainControlSelCriteria());
            return Json(new { List = result });
        }

        /// <summary>
        /// Add Domain Control.
        /// </summary>
        /// <returns>Action Result.</returns>
        [HttpGet]
        public ActionResult DomainControlAdd()
        {
            var model = new DomainControlViewModel
            {
                Status = true
            };

            model.AddCountryList(SystemSettingService.GetCountryList(new CountrySelectCriteria
            {
                PageSize = 300,
                PageIndex = 1
            }).ToList());

            model.AddAffDomainList(_domainService.AffDomainList(new DomainAffSelCriteria()).ToList());
            model.AddMemberDomainList(_domainService.MemDomainList(new DomainMemSelCriteria()).ToList());

            return View(model);
        }

        /// <summary>
        /// Add Domain Control.
        /// </summary>
        /// <param name="domain">Domain Control View Model.</param>
        /// <returns>Action Result.</returns>
        [HttpPost]
        public ActionResult DomainControlAdd(DomainControlViewModel domain)
        {
            if (ModelState.IsValid)
            {
                var session = SessionManager.CurrentSession;

                var criteria = new DomainControlInsCriteria
                {
                    UserID = session.CustID,
                    CountryID = domain.Country,
                    AffDomainID = domain.AffDomain,
                    MemberDomainID = domain.MemberDomain,
                    Status = domain.Status,
                    IP = Remote.GetClientIPAddress(),
                    AdminUserName = session.Username
                };

                _domainService.DomainControlAdd(criteria);

                ViewBag.Result = GetResultViewModel(criteria.ErrCode, Lang.AddSucc);
            }

            return DomainControlAdd();
        }

        /// <summary>
        /// Update Domain Control.
        /// </summary>
        /// <param name="id">Id of Domain Control.</param>
        /// <returns>Action Result.</returns>
        [HttpGet]
        public ActionResult DomainControlUpd(int id)
        {
            DomainControl result = _domainService.DomainControlSelectById(new DomainControlSelByIdCriteria
            {
                ID = id
            });

            var model = new DomainControlViewModel
            {
                ID = result.ID,
                Status = result.Status,
                Country = result.CountryId,
                MemberDomain = result.MemberDomainId,
                AffDomain = result.AffDomainId
            };

            model.AddCountryList(SystemSettingService.GetCountryList(new CountrySelectCriteria
            {
                PageSize = 300,
                PageIndex = 1
            }).ToList());

            model.AddAffDomainList(_domainService.AffDomainList(new DomainAffSelCriteria()).ToList());

            model.AddMemberDomainList(_domainService.MemDomainList(new DomainMemSelCriteria()).ToList());

            return View(model);
        }

        /// <summary>
        /// Update Domain Control.
        /// </summary>
        /// <param name="domain">Domain Control View Model.</param>
        /// <returns>Action Result.</returns>
        [HttpPost]
        public ActionResult DomainControlUpd(DomainControlViewModel domain)
        {
            if (ModelState.IsValid)
            {
                var session = SessionManager.CurrentSession;

                var criteria = new DomainControlUpdCriteria
                {
                    UserID = session.CustID,
                    ID = domain.ID,
                    CountryID = domain.Country,
                    MemberDomainID = domain.MemberDomain,
                    AffDomainID = domain.AffDomain,
                    Status = domain.Status,
                    IP = Remote.GetClientIPAddress(),
                    AdminUserName = session.Username
                };

                _domainService.DomainControlUpdate(criteria);

                ViewBag.Result = GetResultViewModel(criteria.ErrCode);
            }

            return DomainControlUpd(domain.ID);
        }

        /// <summary>
        /// Remove Domain Control.
        /// </summary>
        /// <param name="id">Id of Domain Control.</param>
        /// <returns>Action Result.</returns>
        [HttpPost]
        public ActionResult DomainControlRemove(int id)
        {
            var criteria = new DomainControlDelCriteria
            {
                UserID = SessionManager.CurrentSession.CustID,
                Id = id,
                IP = Remote.GetClientIPAddress(),
                LicenseeId = AppSettings.LicenseeID
            };
            _domainService.DomainControlDelete(criteria);

            var result = GetResultViewModel(criteria.ErrCode);

            return Json(result);
        }
    }
}