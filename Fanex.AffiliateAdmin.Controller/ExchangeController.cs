namespace Fanex.AffiliateAdmin.Controller
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;
    using Fanex.AffiliateAdmin.Core.Configurations;
    using Fanex.AffiliateAdmin.Core.Filter;
    using Fanex.AffiliateAdmin.Core.Services;
    using Fanex.AffiliateAdmin.Core.Session;
    using Fanex.AffiliateAdmin.Core.Utilities;
    using Fanex.AffiliateAdmin.Criterias;
    using Fanex.AffiliateAdmin.Models;
    using Fanex.AffiliateAdmin.Services;

    /// <summary>
    /// Currency Rate Controller.
    /// </summary>
    [FunctionAuth(Function = FunctionPolicyNames.AffExchangeRate, CurrenciesRequired = true)]
    public class ExchangeController : BaseController
    {
        private static readonly Dictionary<string, int> _multipliedCurrencies = new Dictionary<string, int>();

        private static readonly StringBuilder _strNotify = new StringBuilder();

        #region Services

        /// <summary>
        /// Service used to work with database.
        /// </summary>
        private readonly IExchangeRateService _exchangeService;

        private readonly QueryService _service;

        #endregion Services

        #region Constructor

        public ExchangeController()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExchangeController" /> class.
        /// </summary>
        /// <param name="service">Email History service.</param>
        public ExchangeController(IExchangeRateService service)
        {
            _exchangeService = service ?? new ExchangeRateService();
            _service = new QueryService();
        }

        #endregion Constructor

        public ActionResult CopyRate(UpdateExchangeRateViewModel criteria)
        {
            criteria.UserId = SessionManager.CurrentSession.CustID;
            criteria.Ip = Remote.GetClientIPAddress();

            var validCriteria = GetProperCriteria(criteria);

            _service.ExecuteOutput(validCriteria);
            return Json(GetResultViewModel(validCriteria.ErrCode));
        }

        /// <summary>
        /// Load data.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>List of email.</returns>
        public ActionResult GetList(ExchangeSelCriteria criteria)
        {
            bool isToday = false;

            if (criteria.Time.HasValue && SystemTimeService.GetSystemDateTime().Subtract(criteria.Time.Value).Days == 0)
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

            ViewBag.CanUpdateData = SessionManager.CurrentSession.HasHostPolicy(FunctionPolicyNames.AffExchangeRate);
            return View();
        }

        [FunctionAuth(Function = FunctionPolicyNames.AffExchangeRate, IsHostPolicy = true)]
        public ActionResult ToggleAutoUpdate(UpdateExchangeRateViewModel criteria)
        {
            criteria.UserId = SessionManager.CurrentSession.CustID;
            criteria.Ip = Remote.GetClientIPAddress();

            var validCriteria = GetProperCriteria(criteria);

            _service.ExecuteOutput(validCriteria);
            return Json(GetResultViewModel(validCriteria.ErrCode));
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
        public ActionResult UpdateExRate(UpdateExchangeRateViewModel criteria)
        {
            criteria.UserId = SessionManager.CurrentSession.CustID;
            criteria.Ip = Remote.GetClientIPAddress();
            var validCriteria = GetProperCriteria(criteria);

            _service.ExecuteOutput(validCriteria);

            ViewBag.Result = GetResultViewModel(validCriteria.ErrCode);

            return View(new ExchangeRateViewModel
                {
                    ExchangeId = criteria.ExchangeId,
                    Rate = criteria.Rate,
                    CodeFrom = criteria.FromCurrency,
                    CodeTo = criteria.ToCurrency
                });
        }

        /// <summary>
        /// Get Proper Criteria.
        /// </summary>
        /// <param name="criteria">Criteria input.</param>
        /// <returns>Proper Criteria.</returns>
        private SimpleCriteriaBase GetProperCriteria(UpdateExchangeRateViewModel criteria)
        {
            if (!string.IsNullOrWhiteSpace(criteria.ToCurrency))
            {
                return new ExchangeUpdateCriteria
                {
                    ToCurrency = criteria.ToCurrency,

                    ExchangeId = criteria.ExchangeId,
                    IP = criteria.Ip,
                    Rate = criteria.Rate,
                    UserId = criteria.UserId,
                    LicenseeId = AppSettings.LicenseeID
                };
            }

            if (!string.IsNullOrWhiteSpace(criteria.ExchangeIdList))
            {
                return new CopyRateFromMarketCriteria
                {
                    IP = criteria.Ip,
                    UserId = criteria.UserId,
                    ExchangeIdList = criteria.ExchangeIdList,
                    LicenseeId = AppSettings.LicenseeID
                };
            }

            return new ExchangeAutoRefreshUpdCriteria
            {
                ExchangeId = criteria.ExchangeId,
                IP = criteria.Ip,
                UserId = criteria.UserId
            };
        }
    }
}