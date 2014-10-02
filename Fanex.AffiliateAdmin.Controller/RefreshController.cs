// --------------------------------------------------------------------------
// <copyright file="RefreshController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2014.9.30">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Controller
{
    using System;
    using System.Configuration;
    using System.Web.Configuration;
    using System.Web.Mvc;
    using Fanex.AffiliateAdmin.Criterias;
    using Fanex.AffiliateAdmin.DTO;
    using Fanex.AffiliateAdmin.Services;

    /// <summary>
    /// Refresh Web Config.
    /// </summary>
    public class RefreshController : Controller
    {
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

        /// <summary>
        /// View System Time.
        /// </summary>
        /// <returns>System Time.</returns>
        public ActionResult SystemTime()
        {
            var webTime = DateTime.Now;

            var systemTime = SystemTimeService.GetSystemDateTime();

            return View(new Tuple<DateTime, DateTime>(webTime, systemTime));
        }
    }
}