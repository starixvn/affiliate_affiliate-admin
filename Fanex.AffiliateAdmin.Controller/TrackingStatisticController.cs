// --------------------------------------------------------------------------
// <copyright file="TrackingStatisticController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2013.12.19">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Controller
{
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.AffiliateAdmin.Core.Filter;
    using Fanex.AffiliateAdmin.Core.Services.Interface;
    using Fanex.AffiliateAdmin.Criterias;
    using Fanex.AffiliateAdmin.Criterias.Configuration.SystemSetting;
    using Fanex.AffiliateAdmin.Models;
    using Fanex.AffiliateAdmin.Services.Interface;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [FunctionAuth(Function = "[Affiliate]")]
    public class TrackingStatisticController : BaseController
    {
        #region Services

        private readonly ITrackingStatisticService _trackingService;

        private readonly ISystemSettingService _systemSettingService;

        #endregion Services

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="TrackingStatisticController" /> class.
        /// </summary>  
        /// <param name="trackingService">Tracking Service.</param>  
        /// <param name="systemSettingService">System setting Service.</param>  
        public TrackingStatisticController(ITrackingStatisticService trackingService, ISystemSettingService systemSettingService)
        {
            _trackingService = trackingService;
            _systemSettingService = systemSettingService;
        }

        #endregion Constructor

        /// <summary>
        /// Referrer statistic.
        /// </summary>
        /// <param name="model">Model input.</param>
        /// <returns>View model.</returns>
        [HttpGet]
        public ActionResult Referrer(ReferrerViewModel model)
        {
            return View(model);
        }

        /// <summary>
        /// Get List statistic.
        /// </summary>
        /// <param name="criteria">Click statistic criteria.</param>
        /// <returns>Action result.</returns>
        [HttpPost]
        public ActionResult ReferrerList(ClickStatisticBySiteCriteria criteria)
        {
            criteria.AffId = this.CurrentSession.Affiliate.AffiliateId;

            var data = this._trackingService.GetReferrers(criteria);

            return Json(new { List = data });
        }

        /// <summary>
        /// Landing Pages statistic.
        /// </summary>
        /// <returns>View model.</returns>
        [HttpGet]
        public ActionResult LandingPage()
        {
            var customerPages = this._systemSettingService.GetCustomerPages(new SystemSettingDetailCriteria
            {
                SearchBy = SearchByType.ByType,
                Type = "CustomerPage"
            });

            return View(new TrackingStatisticViewModel { CustomerPages = customerPages.ToList() });
        }

        /// <summary>
        /// Get List statistic.
        /// </summary>
        /// <param name="criteria">Click statistic criteria.</param>
        /// <returns>Action result.</returns>
        [HttpPost]
        public ActionResult LandingPageList(ClickStatisticByPageCriteria criteria)
        {
            criteria.AffId = this.CurrentSession.Affiliate.AffiliateId;

            var data = this._trackingService.GetLandingPages(criteria);

            return Json(new { List = data });
        }

        /// <summary>
        /// Tracking statistic detail.
        /// </summary>
        /// <param name="model">Click statistic detail model.</param>
        /// <returns>Action result.</returns>
        [HttpGet]
        public ActionResult StatisticDetail(TrackingStatisticDetailViewModel model)
        {
            return View(model);
        }

        /// <summary>
        /// Tracking statistic detail list.
        /// </summary>
        /// <param name="criteria">Click statistic detail criteria.</param>
        /// <returns>Action result.</returns>
        [HttpPost]
        public ActionResult StatisticDetailList(ClickStatisticDetailCriteria criteria)
        {
            criteria.AffId = this.CurrentSession.Affiliate.AffiliateId;

            var data = this._trackingService.GetTrackingStatisticDetails(criteria);

            return Json(new
            {
                List = data,
                ItemsPerPage = criteria.PageSize,
                CurrentPage = criteria.PageIndex - 1,
                TotalItem = criteria.RowCount
            });
        }
    }
}
