namespace Fanex.AffiliateAdmin.Controller
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Web.Mvc;
    using AffBetListService;
    using Core.Filter;
    using Criterias;
    using Fanex.AffiliateAdmin.Core.Configurations;
    using Fanex.AffiliateAdmin.Core.Session;
    using Fanex.AffiliateAdmin.Core.Utilities;
    using Fanex.AffiliateAdmin.Criteria;
    using Fanex.AffiliateAdmin.DTO;
    using Fanex.AffiliateAdmin.Enumerations;
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
                        criteria,
                        AppSession.Language);
            }
        }

        #endregion BetList Results

        #region BetList.Results.RNGCasino

        [HttpGet]
        public ActionResult RNGCasinoData(BetListResultsRNGCasinoCriteria criteria, int affId)
        {
            var isValid = GetValidationResult(affId, criteria.UserName);

            if (!isValid)
            {
                ViewBag.Data = string.Empty;

                return View("~/Views/BetList/BetListResults_RNGCasino.cshtml", null);
            }

            var customerInfoBetList = _betListService.GetPlayerIbcInfo(AppSettings.SystemName + "$" + criteria.UserName);
            criteria.BaseCurrency = criteria.BaseCurrency ?? string.Empty;
            criteria.ExternalId = customerInfoBetList.Username;
            criteria.CurrencyId = customerInfoBetList.CurrencyId;
            criteria.BaseUrl = UrlUtil.GetBaseUrl(this);

            var data = criteria.IsValid()
                ? _betListService.GetRNGCasinoResult(criteria)
                : new List<RNGCasinoResult>();

            var exchangeRate = criteria.ExchangeRate;

            ExchangeRNGCasinoValues(data, exchangeRate);

            var totalAmount = new List<RNGCasinoResult>();
            if (data != null)
            {
                totalAmount.Add(new RNGCasinoResult
                {
                    Stake = data.Sum(x => x.Stake),
                    Bet = data.Sum(x => x.Bet),
                    WinLoss = data.Sum(x => x.WinLoss),
                    Result = string.Empty
                });
            }

            ViewBag.ToCurrencyId = criteria.ToCurrencyId;
            ViewBag.ToCurrencyCode = criteria.ToCurrencyCode;

            var model = new BetListResultsRNGCasinoViewModel(data, totalAmount, criteria)
            {
                Currency = criteria.ToCurrencyCode,
                AffId = affId
            };

            return View("~/Views/BetList/BetListResults_RNGCasino.cshtml", model);
        }

        public void ExchangeRNGCasinoValues(List<RNGCasinoResult> originalData, decimal exchangeRate)
        {
            foreach (var item in originalData)
            {
                item.BeginBalance *= exchangeRate;
                item.EndBlance *= exchangeRate;
                item.Bet *= exchangeRate;
                item.Stake *= exchangeRate;
                item.Win *= exchangeRate;
                item.WinLoss *= exchangeRate;
            }
        }

        [HttpGet]
        public ViewResult RNGCasinoDetail(BetListResultsRNGCasinoDetailCriteria criteria, int affId, string username)
        {
            var isValid = GetValidationResult(affId, username);

            if (!isValid)
            {
                ViewBag.Data = string.Empty;

                return View("~/Views/BetList/RNGCasinoDetail/Noinfo.cshtml");
            }

            criteria.CurrencyName = criteria.CurrencyName.ToUpper();

            var exchangeRate = criteria.ExchangeRate;

            const string ViewPathTemplate = "~/Views/BetList/RNGCasinoDetail/Group{0}.cshtml";

            var group = GetGroupByGameTypeId(criteria.GameTypeId);
            if (string.IsNullOrEmpty(group))
            {
                return View("~/Views/BetList/RNGCasinoDetail/NoGameErr.cshtml");
            }

            var viewModel = criteria.IsValid()
                ? _betListService.GetRNGCasinoResultDetail(criteria)
                : new BetListResultsRNGCasinoDetailViewModel();

            viewModel.Param = criteria;
            ExchangeRNGCasinoDetailValues(viewModel, exchangeRate);

            return (viewModel.CasinoDetailHeaderList != null && viewModel.CasinoDetailHeaderList.Count > 0)
                ? View(string.Format(ViewPathTemplate, group), viewModel)
                : View("~/Views/BetList/RNGCasinoDetail/Noinfo.cshtml");
        }

        public void ExchangeRNGCasinoDetailValues(BetListResultsRNGCasinoDetailViewModel originalData, decimal exchangeRate)
        {
            foreach (var detailItem in originalData.CasinoDetailList)
            {
                detailItem.Bet *= exchangeRate;
                detailItem.Win *= exchangeRate;
                detailItem.WinLoss *= exchangeRate;
            }

            foreach (var detailHeaderItem in originalData.CasinoDetailHeaderList)
            {
                detailHeaderItem.Win = ExchangeValueIncludingCurrency(detailHeaderItem.Win, exchangeRate);
                detailHeaderItem.BetValue *= exchangeRate;
                detailHeaderItem.BetBaseCurrencyValue *= exchangeRate;
                detailHeaderItem.CoinSize = ExchangeValueIncludingCurrency(detailHeaderItem.CoinSize, exchangeRate);
                detailHeaderItem.CoinSizeBaseCurrencyValue *= exchangeRate;
                detailHeaderItem.CoinSizeValue *= exchangeRate;
                if (detailHeaderItem.CoinSizeAmount.Contains("X"))
                {
                    var coinSizeAmountParts = detailHeaderItem.CoinSizeAmount.Split('X');
                    detailHeaderItem.CoinSizeAmount = ExchangeValueIncludingCurrency(coinSizeAmountParts[0], exchangeRate);
                    detailHeaderItem.CoinSizeAmount = FormaterCurrency.Format(decimal.Parse(detailHeaderItem.CoinSizeAmount));
                    detailHeaderItem.CoinSizeAmount += " X" + coinSizeAmountParts[1];
                }

                detailHeaderItem.WinBaseCurrencyValue *= exchangeRate;
                detailHeaderItem.WinLossResult = ExchangeValueIncludingCurrency(detailHeaderItem.WinLossResult, exchangeRate);
                detailHeaderItem.WinLossResultBaseCurrecyValue *= exchangeRate;
                detailHeaderItem.WinResult = ExchangeValueIncludingCurrency(detailHeaderItem.WinResult, exchangeRate);
                detailHeaderItem.WinResultBaseCurrencyValue *= exchangeRate;
            }
        }

        public string ExchangeValueIncludingCurrency(string originalValue, decimal exchangeRate)
        {
            var regexRemoveLetters = new Regex(@"[a-zA-Z\s]");

            return string.IsNullOrEmpty(originalValue.Trim())
                ? "0.00"
                : (decimal.Parse(regexRemoveLetters.Replace(originalValue, string.Empty)) * exchangeRate).ToString();
        }

        [SuppressMessage(
              "StyleCop.CSharp.StylecopPlus.Rules",
              "SP2101:MethodMustNotContainMoreLinesThan",
              Justification = "Reviewed. Suppression is OK here.")]
        private string GetGroupByGameTypeId(int gameTypeId)
        {
            switch (gameTypeId)
            {
                case (int)CasinoGameType.Baccarat:
                case (int)CasinoGameType.Blackjack:
                case (int)CasinoGameType.CasinoHoldEm:
                case (int)CasinoGameType.BlackjackPerfectPair:
                case (int)CasinoGameType.BaccaratSuper6:
                case (int)CasinoGameType.ThreeKnights:
                case (int)CasinoGameType.DragonTiger:
                case (int)CasinoGameType.BattleWar:
                case (int)CasinoGameType.PokerTri:
                case (int)CasinoGameType.PaiGow:
                case (int)CasinoGameType.FaceUp21:
                case (int)CasinoGameType.ThreeCardsRummy:
                case (int)CasinoGameType.SicBo:
                case (int)CasinoGameType.RoulettePro:
                case (int)CasinoGameType.RoulettePro_Fast:
                case (int)CasinoGameType.MiniJacksorBetter:
                case (int)CasinoGameType.MiniBaccarat:
                case (int)CasinoGameType.MiniSicBo:
                case (int)CasinoGameType.MiniBlackjack:
                case (int)CasinoGameType.MiniCasinoHoldEm:
                    return "1";

                case (int)CasinoGameType.TexasHoldEm:
                    return "1-1";

                case (int)CasinoGameType.RedDog:
                case (int)CasinoGameType.MiniRoulette:
                case (int)CasinoGameType.FishPrawnCrab:
                case (int)CasinoGameType.CardWheel:
                case (int)CasinoGameType.FanTan:
                    return "2";

                case (int)CasinoGameType.MoneySlot:
                case (int)CasinoGameType.Engagement:
                case (int)CasinoGameType.FoxForce5:
                case (int)CasinoGameType.TattooArt:
                case (int)CasinoGameType.MiniMoneySlot:
                    return "3";

                case (int)CasinoGameType.Champions:
                case (int)CasinoGameType.WaterMargin:
                    return "4";

                case (int)CasinoGameType.WesternZodiac:
                    return "4-1";

                case (int)CasinoGameType.MoneyWheel:
                    return "5";

                case (int)CasinoGameType.JourneytotheWest:
                    return "6";

                case (int)CasinoGameType.ChineseZodiac:
                    return "6-1";

                case (int)CasinoGameType.JacksorBetter:
                    return "7-0";

                case (int)CasinoGameType.JungleWild:
                case (int)CasinoGameType.GodofProsperity:
                    return "7-1";

                case (int)CasinoGameType.No4_LineJacksorBetter:
                case (int)CasinoGameType.No25_LineJacksorBetter:
                case (int)CasinoGameType.No50_LineJacksorBetter:
                    return "7-2";

                case (int)CasinoGameType.No_10sOrBetter:
                    return "7-3";

                case (int)CasinoGameType.MonkeyHiLo:
                case (int)CasinoGameType.MahjongHiLo:
                    return "7-4";

                case (int)CasinoGameType.KenoChinese:
                case (int)CasinoGameType.KenoCamera:
                    return "7-5";

                case (int)CasinoGameType.TurfKing:
                    return "7-6";

                case (int)CasinoGameType.MamakCorner:
                case (int)CasinoGameType.WinterSlot:
                case (int)CasinoGameType.DragonSlot:
                    return "7-8";

                case (int)CasinoGameType.MayaPush:
                case (int)CasinoGameType.Knock2Eggs:
                case (int)CasinoGameType.HoHoHo:
                case (int)CasinoGameType.PokeMole:
                case (int)CasinoGameType.GongXiFaCai:
                case (int)CasinoGameType.FruitScratch:
                case (int)CasinoGameType.FlamesofOlympics:
                    return "8";

                case (int)CasinoGameType.PenaltyHat_Trick:
                case (int)CasinoGameType.FancyScratch:
                case (int)CasinoGameType.LoveStory:
                    return "9";

                default:
                    return string.Empty;
            }
        }

        #endregion BetList.Results.RNGCasino

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