// --------------------------------------------------------------------------
// <copyright file="ReportController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2013.10.15">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Controller
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.Script.Serialization;
    using Core.Filter;
    using Criterias;
    using Fanex.AffiliateAdmin.Core.Configurations;
    using Fanex.AffiliateAdmin.Core.Utilities;
    using Fanex.AffiliateAdmin.Resources;
    using Fanex.AffiliateAdmin.Services;
    using Models;

    /// <summary>
    /// Report Controller.
    /// </summary>
    [FunctionAuth(Function = "[AffReport]")]
    public class BetListController : BaseController
    {
        public BetListController()
        {

        }

        public ActionResult Index(
            string username,
            string products,
            string winlostdate,
            string affCurrencyId,
            string playerCurrencyId
            )
        {
            DateTime winlostDate;
            DateTime.TryParse(winlostdate, out winlostDate);
            //var exchangeRate = GetExchangeRate(currency, winlostDate);
            //CurrentSession.ExchangeRate = exchangeRate;
            //var supportedProducts = GetSupportedProductsForWebService();

            //var betlistSOAP = new BetListService.BetlistSOAP
            //{
            //    AffiliateName = AppSettings.AffiliateName.ToLower(),
            //    Username = CurrentSession.Username,
            //    Product = product,
            //    FromDate = winlostDate,
            //    ToDate = winlostDate,
            //    ExchangeRate = exchangeRate,
            //    SupportedProducts = supportedProducts
            //};

            var betlistSoap = new BetListService.BetlistSOAP();
            var betlistService = new BetListService.AffiliateAPI();
            ViewBag.Data = betlistService.GetBetlist(betlistSoap);


            return View();
        }
    }
}