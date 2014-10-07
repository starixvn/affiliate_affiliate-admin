// -----------------------------------------------------------------------
// <copyright file="ViewLogController.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.AffiliateAdmin.Controller
{
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.AffiliateAdmin.Core.Filter;
    using Fanex.AffiliateAdmin.Criterias;
    using Fanex.AffiliateAdmin.Models;
    using Fanex.AffiliateAdmin.Services;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [FunctionAuth(Function = "[Affiliate]")]
    public class ViewLogController : BaseController
    {
        #region Services

        private readonly IViewLogService _viewLogService;

        #endregion Services

        #region Constructor

        public ViewLogController()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewLogController" /> class.
        /// </summary>
        /// <param name="viewLogService">View log Service.</param>
        public ViewLogController(IViewLogService viewLogService)
        {
            _viewLogService = viewLogService ?? new ViewLogService();
        }

        #endregion Constructor

        /// <summary>
        /// Load list log.
        /// </summary>
        /// <returns>View model.</returns>
        [HttpGet]
        public ActionResult Index()
        {
            var model = new ViewLogViewModel();
            model.AddLogTypeList(_viewLogService.GetLogTypes(new LogTypeCriteria()).ToList());

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
            criteria.AffId = this.CurrentSession.Affiliate.AffiliateId;

            var logList = this._viewLogService.GetViewLogList(criteria);

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