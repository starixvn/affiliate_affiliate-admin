// --------------------------------------------------------------------------
// <copyright file="ExchangeController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Joseph.Pham" date="2013.8.29">Created</change>
// </history>

using Fanex.AffiliateAdmin.Core.Services;

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
    using Fanex.AffiliateAdmin.Models;
    using Fanex.AffiliateAdmin.Services;

    /// <summary>
    /// Currency Rate Controller.
    /// </summary>
    [FunctionAuth(Function = "[AffEx.Rate]")]
    public partial class ExchangeController : BaseController
    {
        private static Dictionary<string, int> _multipliedCurrencies = new Dictionary<string, int>();

        private static StringBuilder _strNotify = new StringBuilder();

        #region Services
        /// <summary>
        /// Service used to work with database.
        /// </summary>
        private readonly IExchangeRateService _exchangeService;

        private readonly QueryService _service;

        #endregion Services

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ExchangeController" /> class.
        /// </summary>
        /// <param name="service">Email History service.</param>        
        public ExchangeController(IExchangeRateService service)
        {
            _exchangeService = service;
            _service = new QueryService();
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
                foreach (var obj in AppSettings.AffSystemAllSupportedCurrencies)
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
            bool isToday = false;

            if (criteria.Time.HasValue && DateTime.Now.Subtract(criteria.Time.Value).Days == 0)
            {
                criteria.Time = null;
                isToday = true;
            }

            var result = _exchangeService.GetList(criteria);

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
                Currencies = AppSettings.AffSystemAllSupportedCurrencies.Select(x => x.CurrencyCode).ToArray(),
                IsToday = isToday
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
        public ActionResult UpdateExRate(ExchangeRateViewModel model)
        {
            return View(model);
        }

        /// <summary>
        /// Update exchange rate.
        /// </summary>
        /// <param name="criteria">Exchange rate criteria.</param>
        /// <returns>Exchange rate view.</returns>
        [HttpPost]
        public ActionResult UpdateExRate(ExchangeRateCriteria criteria)
        {
            criteria.Type = CriteriaType.Update;
            criteria.UserId = SessionManager.CurrentSession.CustID;
            criteria.Ip = Remote.GetClientIPAddress();            
            _service.ExecuteOutput(criteria);

            ViewBag.Result = GetResultViewModel(criteria.ErrCode);

            return View(new ExchangeRateViewModel
                {
                    ExchangeId = criteria.ExchangeId,
                    Rate = criteria.Rate,
                    CodeFrom = criteria.FromCurrency,
                    CodeTo = criteria.ToCurrency
                });
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