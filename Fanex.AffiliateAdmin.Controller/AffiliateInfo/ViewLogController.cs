// -----------------------------------------------------------------------
// <copyright file="ViewLogController.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.AffiliateAdmin.Controller
{
    using System.Web.Mvc;
    using Fanex.AffiliateAdmin.Core.Configurations;
    using Fanex.AffiliateAdmin.Core.Filter;
    using Fanex.AffiliateAdmin.Criterias;
    using Fanex.AffiliateAdmin.Models.AffiliateInfo;
    using Fanex.AffiliateAdmin.Services;
    using Helpers;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [FunctionAuth(Function = FunctionPolicyNames.Affiliate)]
    public class ViewLogController : BaseController
    {
        #region Services

        private readonly IViewLogService _viewLogService;
        private readonly ISystemSettings _systemSettings;

        #endregion Services

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewLogController" /> class.
        /// </summary>
        /// <param name="viewLogService">View log Service.</param>
        public ViewLogController(
            IAppSetting appSetting,
            ISystemSettings systemSettings,
            IViewLogService viewLogService) : base(appSetting)
        {
            _viewLogService = viewLogService;
            _systemSettings = systemSettings;
        }

        #endregion Constructor

        /// <summary>
        /// Load list log.
        /// </summary>
        /// <returns>View model.</returns>
        [HttpGet]
        public ActionResult Index()
        {
            var systemStartDate = _systemSettings.SystemStartDate;
            var systemDateTime = _systemSettings.SystemDateTime;
            var dateFrom = systemDateTime.AddMonths(AppSetting.ViewLogBackMonth);

            var model = new ViewLogViewModel
            {
                DateFrom = systemStartDate.CompareTo(dateFrom) <= 0 ? dateFrom : systemStartDate,
                DateTo = systemDateTime,
                LogTypeList = _viewLogService.GetLogTypes(new LogTypeCriteria())
            };

            return View(model);
        }

        /// <summary>
        /// Get List Log.
        /// </summary>
        /// <param name="criteria">View Log criteria.</param>
        /// <returns>View model.</returns>
        [HttpPost]
        public ActionResult ListLog(ViewLogListCriteria criteria)
        {
            criteria.AffId = AffiliateInfoCookieHelper.GetAffiliate(HttpContext).AffId;
            var logList = _viewLogService.GetViewLogList(criteria);

            return Json(new
            {
                List = logList,
                CurrentPage = criteria.PageIndex - 1,
                ItemsPerPage = criteria.PageSize,
                TotalItem = criteria.RowCount
            });
        }
    }
}