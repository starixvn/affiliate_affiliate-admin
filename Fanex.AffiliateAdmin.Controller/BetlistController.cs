namespace Fanex.AffiliateAdmin.Controller
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using AffBetListService;
    using Core.Filter;
    using Criterias;
    using Fanex.AffiliateAdmin.Core.Configurations;
    using Fanex.AffiliateAdmin.Core.Session;
    using Fanex.AffiliateAdmin.Models;
    using Fanex.AffiliateAdmin.Services;

    [FunctionAuth(Function = FunctionPolicyNames.AffReport)]
    public class BetListController : BaseController
    {
        private readonly IBetListService _betListService;

        public BetListController()
            : this(null)
        {
        }

        public BetListController(IBetListService betListService)
        {
            _betListService = betListService ?? new BetListService();
        }

        #region BetList

        public ActionResult Index(BetListViewModel model)
        {
            var isValid = GetValidationResult(model.AffId, model.Username);

            if (!isValid)
            {
                ViewBag.Data = string.Empty;

                return View();
            }

            var exchangeRate = _betListService.GetExchangeRate(new GetExchangeRateCriteria
            {
                Date = model.WinlostDate,
                FromCurrency = model.FromCurrencyId,
                ToCurrency = model.ToCurrencyId
            });

            var supportedProducts = GetSupportedProductsForWebService();

            ViewBag.SupportedProducts = string.Join(",", supportedProducts.Select(t => t.ProductName));

            var betlistSoap = new BetlistSOAP
            {
                AffiliateName = AppSettings.SystemName.ToLower(),
                Username = model.Username,
                Product = model.Product,
                FromDate = model.WinlostDate.HasValue ? model.WinlostDate.Value : DateTime.Now,
                ToDate = model.WinlostDate.HasValue ? model.WinlostDate.Value : DateTime.Now,
                ExchangeRate = exchangeRate,
                SupportedProducts = supportedProducts,
                Language = AppSession.Language
            };

            using (var betlistService = new AffiliateAPI())
            {
                ViewBag.Data = betlistService.GetBetlist(betlistSoap);
            }

            model.ExchangeRate = exchangeRate;

            return View(model);
        }

        [HttpGet]
        public dynamic BetListParlay(BetListParlayViewModel model)
        {
            var exchangeRate = model.ExchangeRate;

            using (var affBetlistService = new AffiliateAPI())
            {
                var tickets = affBetlistService.GetBetlistParlay(
                    AppSettings.SystemName,
                    model.TransId.ToString(),
                    exchangeRate.ToString(),
                    AppSession.Language);

                return tickets;
            }
        }

        [HttpGet]
        public ActionResult SystemParlayDetail(SystemParlayDetailViewModel model)
        {
            var exchangeRate = model.ExchangeRate;

            var criteria = new BetListSystemParlayDetailCriteria
            {
                AffiliateGroup = model.AffiliateGroup,
                BalanceUpDay = model.BalanceUpDay,
                BetId = model.BetId,
                CustId = model.CustId,
                MoveBetsDay = model.MoveBetsDay,
                Outstanding = model.Outstanding,
                RefNo = model.RefNo,
                TransId = model.TransId,
                UserId = model.UserId
            };

            using (var affBetlistService = new AffiliateAPI())
            {
                var systemParlayDataText = affBetlistService.GetBetlistSystemParlay(
                        AppSettings.SystemName,
                        exchangeRate.ToString(),
                        criteria,
                        AppSession.Language);

                ViewBag.Data = systemParlayDataText;
            }

            return View();
        }

        [HttpGet]
        public ActionResult RngCasinoDetailTicket(CasinoDetailViewModel model)
        {
            var criteria = new BetListDetailCriteria
            {
                CustId = model.CustId,
                RefNo = model.TransId,
                ExchangeRate = model.ExchangeRate
            };

            using (var affBetlistService = new AffiliateAPI())
            {
                ViewBag.Data = affBetlistService.GetBetListRngCasinoDetail(SessionManager.CurrentSession.Username, criteria);
            }

            return View("~/Views/BetList/CasinoDetailTicket.cshtml");
        }

        [HttpPost]
        public ActionResult RngCasinoSingleWalletResult(string refNo)
        {
            using (var affBetlistService = new AffiliateAPI())
            {
                var result = affBetlistService.GetRngCasinoSingleWalletResult(refNo);
                return Content(result, "application/json");
            }
        }

        [HttpGet]
        public ActionResult AgCasinoDetailTicket(CasinoDetailViewModel model)
        {
            var criteria = new BetListDetailCriteria
            {
                CustId = model.CustId,
                RefNo = model.TransId,
                ExchangeRate = model.ExchangeRate
            };

            using (var affBetlistService = new AffiliateAPI())
            {
                ViewBag.Data = affBetlistService.GetBetListAgCasinoDetail(SessionManager.CurrentSession.Username, criteria);
            }

            return View("~/Views/BetList/CasinoDetailTicket.cshtml");
        }

        [HttpGet]
        public ActionResult CashOutDetailTicket(CashOutDetailViewModel model)
        {
            var criteria = new BetListDetailCriteria
            {
                TransId = model.TransId,
                RefNo = model.RefNo,
                BetType = model.BetType,
                BetdaqId = model.BetdaqId,
                ExchangeRate = model.ExchangeRate
            };

            using (var affBetlistService = new AffiliateAPI())
            {
                ViewBag.Data = affBetlistService.GetBetListCashOutDetail(criteria, model.IsCashOut, AppSession.Language);
                ViewBag.IsCashOut = model.IsCashOut;
            }

            return View("~/Views/BetList/CashOutDetailTicket.cshtml");
        }

        [HttpGet]
        public dynamic NewBetListParlay(long transId, string betTeam, string betCheck, decimal handicap1, decimal handicap2, string exchangeRate)
        {
            using (var affBetlistService = new AffBetListService.AffiliateAPI())
            {
                var tickets = affBetlistService.GetNewBetlistParlay(
                        AppSettings.SystemName,
                        transId.ToString(),
                        exchangeRate,
                        betTeam,
                        betCheck,
                        handicap1,
                        handicap2,
                        AppSession.Language);

                return tickets;
            }
        }

        #endregion BetList

        #region BetList Results

        [HttpGet]
        public ActionResult BetListResults(AffBetListService.BetListResultsCriteria criteria, int affId)
        {
            var isValid = GetValidationResult(affId, criteria.UserName);

            if (!isValid)
            {
                ViewBag.Data = string.Empty;

                return View();
            }

            ViewBag.Data = GetPopUpData(criteria);

            return View();
        }

        public string GetPopUpData(AffBetListService.BetListResultsCriteria criteria)
        {
            using (var affBetlistService = new AffiliateAPI())
            {
                return affBetlistService.GetBetlistResults(
                        AppSettings.SystemName,
                        AppSession.Language,
                        criteria);
            }
        }

        #endregion BetList Results

        #region Helpers

        private AffBetListService.Product[] GetSupportedProductsForWebService()
        {
            var supportedProducts = AppSettings.Products.Select(product => new AffBetListService.Product
            {
                ProductId = product.ProductID,
                ProductName = product.ProductName
            }).ToList();

            return supportedProducts.ToArray();
        }

        private bool GetValidationResult(int affId, string username)
        {
            var criteria = new ValidationCriteria
            {
                AffId = affId,
                UserCurrencies = SessionManager.CurrentSession.BuildUserCurrencyParam(),
                MemberName = username ?? string.Empty
            };

            _betListService.ValidatePermission(criteria);

            return criteria.BelongLevel1 && criteria.BelongLevel2;
        }

        #endregion Helpers
    }
}