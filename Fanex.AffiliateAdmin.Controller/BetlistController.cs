// --------------------------------------------------------------------------
// <copyright file="BetlistController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2014.11.13">Created</change>
// </history>

using Fanex.AffiliateAdmin.Models;

namespace Fanex.AffiliateAdmin.Controller
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using Core.Filter;
    using Criterias;
    using Fanex.AffiliateAdmin.Core.Configurations;
    using Fanex.AffiliateAdmin.Services;

    /// <summary>
    /// Report Controller.
    /// </summary>
    [FunctionAuth(Function = "[AffReport]")]
    public class BetListController : BaseController
    {
        private readonly IBetListService _betListService;

        private readonly IProductService _productService;

        public BetListController()
            : this(null, null)
        {
        }

        public BetListController(IBetListService betListService, IProductService productService)
        {
            _betListService = betListService ?? new BetListService();
            _productService = productService ?? new ProductService();
        }

        public ActionResult Index(BetListViewModel model)
        {
            var exchangeRate = _betListService.GetExchangeRate(new GetExchangeRateCriteria
                {
                    Date = model.WinlostDate,
                    FromCurrency = model.PlayerCurrencyId,
                    ToCurrency = model.AffCurrencyId
                });

            var supportedProducts = GetSupportedProductsForWebService(model.AffId);

            var betlistSoap = new AffBetListService.BetlistSOAP
            {
                AffiliateName = AppSettings.AffiliateName.ToLower(),
                Username = CurrentSession.Username,
                Product = model.Product,
                FromDate = model.WinlostDate.HasValue ? model.WinlostDate.Value : DateTime.Now,
                ToDate = model.WinlostDate.HasValue ? model.WinlostDate.Value : DateTime.Now,
                ExchangeRate = exchangeRate,
                SupportedProducts = supportedProducts
            };

            var betlistService = new AffBetListService.AffiliateAPI();

            ViewBag.Data = betlistService.GetBetlist(betlistSoap);

            return View();
        }

        /// <summary>
        /// Get data for running mix parlay bet list.
        /// </summary>
        /// <param name="transId">Transaction Id.</param>
        /// <param name="winlostDate"></param>
        /// <param name="affCurrencyId"></param>
        /// <param name="playerCurrencyId"></param>
        /// <returns>Dynamic objects, contain Mix Parlay result.</returns>
        [HttpGet]
        public dynamic BetListParlay(BetListParlayViewModel model)
        {
            var exchangeRate = _betListService.GetExchangeRate(new GetExchangeRateCriteria
            {
                Date = model.WinlostDate,
                FromCurrency = model.PlayerCurrencyId,
                ToCurrency = model.AffCurrencyId
            });

            var affBetlistService = new AffBetListService.AffiliateAPI();
            var tickets = affBetlistService.GetBetlistParlay(
                AppSettings.AffiliateName,
                model.TransId.ToString(),
                exchangeRate.ToString());

            return tickets;
        }

        /// <summary>
        /// Action Get for System Parlay Detail.
        /// </summary>
        /// <param name="criteria">The metal model.</param>
        /// <returns>The action result.</returns>
        [HttpGet]
        public ActionResult SystemParlayDetail(SystemParlayDetailViewModel model)
        {
            var exchangeRate = _betListService.GetExchangeRate(new GetExchangeRateCriteria
            {
                Date = model.WinlostDate,
                FromCurrency = model.PlayerCurrencyId,
                ToCurrency = model.AffCurrencyId
            });

            var criteria = new AffBetListService.BetListSystemParlayDetailCriteria
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

            var affBetlistService = new AffBetListService.AffiliateAPI();
            var systemParlayDataText = affBetlistService.GetBetlistSystemParlay(
                AppSettings.AffiliateName,
                exchangeRate.ToString(),
                criteria);

            ViewBag.Data = systemParlayDataText;

            return View();
        }

        private AffBetListService.Product[] GetSupportedProductsForWebService(int affId)
        {
            var productCriteria = new GetProductByAffIdCriteria
            {
                AffId = affId,
                MonthDate = null
            };

            var products = _productService.GetProductList(productCriteria);

            var supportedProducts = products.Select(product => new AffBetListService.Product
                {
                    ProductId = product.ProductId, 
                    ProductName = product.ProductName, 
                    OriginalProductName = product.OriginalProductName, 
                    TotalQualifiedMembers = product.TotalQualifiedMembers
                }).ToList();

            return supportedProducts.ToArray();
        }
    }
}