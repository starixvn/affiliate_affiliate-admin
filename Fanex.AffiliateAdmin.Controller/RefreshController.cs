// --------------------------------------------------------------------------
// <copyright file="RefreshController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2014.9.30">Created</change>
// </history>

using Fanex.AffiliateAdmin.Criterias;
using Fanex.AffiliateAdmin.DTO;
using Fanex.AffiliateAdmin.Services;

namespace Fanex.AffiliateAdmin.Controller
{
    using System;
    using System.Configuration;
    using System.Web.Configuration;
    using System.Web.Mvc;

    /// <summary>
    /// Refresh Web Config.
    /// </summary>
    public class RefreshController : Controller
    {
        private readonly IHomeService _homeService;

        public RefreshController(IHomeService homeService)
        {
            _homeService = homeService;
        }

        /// <summary>
        /// Refresh Web Config.
        /// </summary>
        /// <returns>Action Result.</returns>
        public ActionResult Index()
        {
            Configuration configuration = WebConfigurationManager.OpenWebConfiguration("~");
            var appSettings = (AppSettingsSection)configuration.GetSection("appSettings");
            appSettings.Settings["RefreshTime"].Value = DateTime.Now.Ticks.ToString();

            configuration.Save();
            ConfigurationManager.RefreshSection("appSettings");

            return RedirectToAction("Index", "Home");
        }

        public ActionResult ServerTime()
        {
            var webServerTime = DateTime.Now;

            var dbServerTime = _homeService.GetSystemDateTime(new SystemDateTimeCriteria());

            return View(new Tuple<DateTime, SystemDateTime>(webServerTime, dbServerTime));
        }
    }
}