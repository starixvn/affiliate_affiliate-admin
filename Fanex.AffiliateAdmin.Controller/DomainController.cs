// --------------------------------------------------------------------------
// <copyright file="DomainController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Joseph.Pham" date="2013.8.1">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Controller
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using Core.Services.Interface;
    using Core.Session;
    using Core.Utilities;
    using Criterias;
    using Criterias.Configuration.Country;
    using DTO;
    using Fanex.AffiliateAdmin.Core.Filter;
    using Models;
    using Resources;
    using Services.Interface;

    /// <summary>
    /// Domain Controller.
    /// </summary>
    [FunctionAuth(Function = "[Domain]")]
    public partial class DomainController : BaseController
    {
        #region Services

        private readonly IDomainService _domainService;
        private readonly ISystemSettingService _systemSettingService;

        #endregion Services

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainController" /> class.
        /// </summary>
        /// <param name="service">Domain Service.</param>        
        /// <param name="systemSetting">System Setting Service.</param>        
        public DomainController(IDomainService service, ISystemSettingService systemSetting)
        {
            _domainService = service;
            _systemSettingService = systemSetting;
        }
        #endregion Constructor

        /// <summary>
        /// Default Index view.
        /// </summary>
        /// <returns>Index view.</returns>
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Affiliate domain page.
        /// </summary>
        /// <returns>Return affiliate domain name.</returns>
        [HttpGet]
        public ActionResult AffDomain()
        {
            return View();
        }

        /// <summary>
        /// Get list affiliate domain.
        /// </summary>
        /// <returns>Return affiliate domain list.</returns>
        [HttpPost]
        public ActionResult AffDomainList()
        {
            var result = _domainService.AffDomainList(new DomainAffCriteria { DBAction = DatabaseAction.Select });
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
                var criteria = new DomainAffCriteria
                {
                    UserID = SessionManager.CurrentSession.CustID,
                    DomainName = domain.DomainName,
                    IP = Remote.GetClientIPAddress(),
                    DBAction = DatabaseAction.Insert
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
            Domain result = _domainService.AffDomainSelectById(new DomainAffCriteria
            {
                DomainID = id,
                DBAction = DatabaseAction.Detail
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
                var criteria = new DomainAffCriteria
                {
                    UserID = SessionManager.CurrentSession.CustID,
                    DomainID = domain.DomainID,
                    DomainName = domain.DomainName,
                    IP = Remote.GetClientIPAddress(),
                    DBAction = DatabaseAction.Update
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
            var criteria = new DomainAffCriteria
            {
                UserID = SessionManager.CurrentSession.CustID,
                DomainID = Convert.ToInt32(id),
                IP = Remote.GetClientIPAddress(),
                DBAction = DatabaseAction.Delete
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
            var result = _domainService.MemDomainList(new DomainMemCriteria { DBAction = DatabaseAction.Select });
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
                var criteria = new DomainMemCriteria
                {
                    UserID = SessionManager.CurrentSession.CustID,
                    DomainName = domain.DomainName,
                    IP = Remote.GetClientIPAddress(),
                    DBAction = DatabaseAction.Insert
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
            Domain result = _domainService.MemDomainSelectById(new DomainMemCriteria
            {
                DomainID = id,
                DBAction = DatabaseAction.Detail
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
                var criteria = new DomainMemCriteria
                {
                    UserID = SessionManager.CurrentSession.CustID,
                    DomainID = domain.DomainID,
                    DomainName = domain.DomainName,
                    IP = Remote.GetClientIPAddress(),
                    DBAction = DatabaseAction.Update
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
            var criteria = new DomainMemCriteria
            {
                UserID = SessionManager.CurrentSession.CustID,
                DomainID = Convert.ToInt32(id),
                IP = Remote.GetClientIPAddress(),
                DBAction = DatabaseAction.Delete
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
            var result = _domainService.DomainControlList(new DomainControlCriteria { DBAction = DatabaseAction.Select });
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

            model.AddCountryList(_systemSettingService.GetCountryList(new CountrySelectCriteria
                {
                    PageSize = 300,
                    PageIndex = 1
                }).ToList());

            model.AddAffDomainList(_domainService.AffDomainList(new DomainAffCriteria()).ToList());
            model.AddMemberDomainList(_domainService.MemDomainList(new DomainMemCriteria()).ToList());

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
                var criteria = new DomainControlCriteria
                {
                    UserID = SessionManager.CurrentSession.CustID,
                    ID = domain.ID,
                    CountryID = domain.Country,
                    AffDomainID = domain.AffDomain,
                    MemberDomainID = domain.MemberDomain,
                    Status = domain.Status,
                    IP = Remote.GetClientIPAddress(),
                    DBAction = DatabaseAction.Insert
                };

                _domainService.DomainControlAdd(criteria);

                ViewBag.Result = GetResultViewModel(criteria.ErrCode);
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
            DomainControl result = _domainService.DomainControlSelectById(new DomainControlCriteria
            {
                ID = id,
                DBAction = DatabaseAction.Detail
            });

            var model = new DomainControlViewModel 
            {
                ID = result.ID,
                Status = result.Status,
                Country = result.CountryId,
                MemberDomain = result.MemberDomainId,
                AffDomain = result.AffDomainId
            };

            model.AddCountryList(_systemSettingService.GetCountryList(new CountrySelectCriteria
            {
                PageSize = 300,
                PageIndex = 1
            }).ToList());

            model.AddAffDomainList(_domainService.AffDomainList(new DomainAffCriteria()).ToList());

            model.AddMemberDomainList(_domainService.MemDomainList(new DomainMemCriteria()).ToList());

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
                var criteria = new DomainControlCriteria
                {
                    UserID = SessionManager.CurrentSession.CustID,
                    ID = domain.ID,
                    CountryID = domain.Country,
                    MemberDomainID = domain.MemberDomain,
                    AffDomainID = domain.AffDomain,
                    Status = domain.Status,
                    IP = Remote.GetClientIPAddress(),
                    DBAction = DatabaseAction.Update
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
            var criteria = new DomainControlCriteria
            {
                UserID = SessionManager.CurrentSession.CustID,
                ID = id,
                IP = Remote.GetClientIPAddress(),
                DBAction = DatabaseAction.Delete
            };
            _domainService.DomainControlDelete(criteria);

            var result = GetResultViewModel(criteria.ErrCode);

            return Json(result);
        }
    }
}
