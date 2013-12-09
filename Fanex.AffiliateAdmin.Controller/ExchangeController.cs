// --------------------------------------------------------------------------
// <copyright file="ExchangeController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Joseph.Pham" date="2013.8.29">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Controller
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;
    using Core.Configurations;
    using Core.Session;
    using Core.Utilities;
    using Criterias;
    using Fanex.AffiliateAdmin.Core.Filter;
    using Resources;
    using Services.Interface;

    /// <summary>
    /// Currency Rate Controller.
    /// </summary>
    [Authorization]
    public partial class ExchangeController : BaseController
    {
        private static Dictionary<string, int> _multipliedCurrencies = new Dictionary<string, int>();

        private static StringBuilder _strNotify = new StringBuilder();

        #region Services
        /// <summary>
        /// Service used to work with database.
        /// </summary>
        private readonly IExchangeRateService _service;

        #endregion Services

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ExchangeController" /> class.
        /// </summary>
        /// <param name="service">Email History service.</param>        
        public ExchangeController(IExchangeRateService service)
        {
            _service = service;
        }
        #endregion Constructor

        /// <summary>
        /// Index action.
        /// </summary>        
        /// <returns>Default model.</returns>
        [HttpGet]
        public ActionResult Index()
        {
            if (_multipliedCurrencies.Count == 0)
            {
                foreach (var obj in AppSettings.AffSystemSupportedCurrencies)
                {
                    if (obj.MultipliedBy > 1)
                    {
                        _strNotify.Append(Common.GetCurrencyNotification(obj.CurrencyCode, obj.MultipliedBy));
                        _multipliedCurrencies.Add(obj.CurrencyCode, obj.MultipliedBy);
                    }
                }
            }
            
            return View();
        }        

        /// <summary>
        /// Load data.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>List of email.</returns>
        public ActionResult GetList(ExchangeRateCriteria criteria)
        {
            if (criteria.Time.HasValue && DateTime.Now.Subtract(criteria.Time.Value).Days == 0)
            {
                criteria.Time = null;
            }

            var result = _service.GetList(criteria);

            foreach (var item in result)
            {
                if (item.MultipliedBy > 1 && !_multipliedCurrencies.ContainsKey(item.Code))
                {
                    _strNotify.Append(Common.GetCurrencyNotification(item.Code, item.MultipliedBy));
                    _multipliedCurrencies.Add(item.Code, item.MultipliedBy);
                }
            }

            return Json(new
            {
                Notification = _strNotify.ToString(),
                List = result,
                Currencies = AppSettings.AffSystemAllSupportedCurrencies.Select(x => x.CurrencyCode).ToArray()
            });
        }

        public ActionResult ToggleAutoUpdate(ExchangeRateCriteria criteria)
        {   
            criteria.Type = CriteriaType.Update;
            criteria.UserId = SessionManager.CurrentSession.CustID;
            criteria.Ip = Remote.GetClientIPAddress();
            _service.ExecuteOutput(criteria);
            return Json(GetResultViewModel(criteria.ErrCode));
        }

        [HttpGet]
        public ActionResult UpdateExRate(int exchangeid, double rate, string codeFrom, string codeTo)
        {
            ViewBag.ExchangeId = exchangeid;
            ViewBag.Rate = rate;
            ViewBag.CodeFrom = codeFrom;
            ViewBag.CodeTo = codeTo;

            return View();
        }

        [HttpPost]
        public ActionResult UpdateExRate(ExchangeRateCriteria criteria)
        {
            criteria.Type = CriteriaType.Update;
            criteria.UserId = SessionManager.CurrentSession.CustID;
            criteria.Ip = Remote.GetClientIPAddress();            
            _service.ExecuteOutput(criteria);
            return Json(GetResultViewModel(criteria.ErrCode));
        }

        public ActionResult CopyRate(ExchangeRateCriteria criteria)
        {
            criteria.Type = CriteriaType.Update;
            criteria.UserId = SessionManager.CurrentSession.CustID;
            criteria.Ip = Remote.GetClientIPAddress();
            _service.ExecuteOutput(criteria);
            return Json(GetResultViewModel(criteria.ErrCode));
        }
    }
}