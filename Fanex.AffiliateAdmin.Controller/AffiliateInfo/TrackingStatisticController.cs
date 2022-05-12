namespace Fanex.AffiliateAdmin.Controller
{
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.AffiliateAdmin.Core.Configurations;
    using Fanex.AffiliateAdmin.Core.Filter;
    using Fanex.AffiliateAdmin.Core.Services.Interface;
    using Fanex.AffiliateAdmin.Criterias;
    using Fanex.AffiliateAdmin.Criterias.Configuration.SystemSetting;
    using Fanex.AffiliateAdmin.Models.AffiliateInfo;
    using Fanex.AffiliateAdmin.Services;
    using Helpers;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [FunctionAuth(Function = FunctionPolicyNames.Affiliate)]
    public class TrackingStatisticController : BaseController
    {
        #region Services

        private readonly ISystemSettingService _systemSettingService;
        private readonly ITrackingStatisticService _trackingStatisticService;

        #endregion Services

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="TrackingStatisticController" /> class.
        /// </summary>
        /// <param name="trackingService">Tracking Service.</param>
        /// <param name="systemSettingService">System setting Service.</param>
        public TrackingStatisticController(
            IAppSetting appSetting,
            ISystemSettingService systemSettingService,
            ITrackingStatisticService trackingStatisticService) : base(appSetting)
        {
            _systemSettingService = systemSettingService;
            _trackingStatisticService = trackingStatisticService;
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
            criteria.AffId = AffiliateInfoCookieHelper.GetAffiliate(HttpContext).AffId;
            var data = _trackingStatisticService.GetReferrers(criteria);

            return Json(new { List = data });
        }

        /// <summary>
        /// Landing Pages statistic.
        /// </summary>
        /// <returns>View model.</returns>
        [HttpGet]
        public ActionResult LandingPage()
        {
            var customerPages = _systemSettingService.GetCustomerPages(new SystemSettingsSelByTypeCriteria
            {
                Type = "CustomerPage"
            });

            var model = new TrackingStatisticViewModel();
            model.AddCustomerPageList(customerPages.ToList());

            return View(model);
        }

        /// <summary>
        /// Get List statistic.
        /// </summary>
        /// <param name="criteria">Click statistic criteria.</param>
        /// <returns>Action result.</returns>
        [HttpPost]
        public ActionResult LandingPageList(ClickStatisticByPageCriteria criteria)
        {
            criteria.AffId = AffiliateInfoCookieHelper.GetAffiliate(HttpContext).AffId;
            var data = _trackingStatisticService.GetLandingPages(criteria);

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
            criteria.AffId = AffiliateInfoCookieHelper.GetAffiliate(HttpContext).AffId;
            var data = _trackingStatisticService.GetTrackingStatisticDetails(criteria);

            return Json(new
            {
                List = data,
                ItemsPerPage = criteria.PageSize,
                CurrentPage = criteria.PageIndex - 1,
                TotalItem = criteria.NRows
            });
        }
    }
}