// -----------------------------------------------------------------------
// <copyright file="AdjustmentController.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.AffiliateAdmin.Controller
{
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.AffiliateAdmin.Core.Configurations;
    using Fanex.AffiliateAdmin.Core.Filter;
    using Fanex.AffiliateAdmin.Core.Session;
    using Fanex.AffiliateAdmin.Core.Utilities;
    using Fanex.AffiliateAdmin.Criterias;
    using Fanex.AffiliateAdmin.Models;
    using Fanex.AffiliateAdmin.Services;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [FunctionAuth(Function = FunctionPolicyNames.Affiliate)]
    public class AdjustmentController : BaseController
    {
        #region Services

        private readonly IAdjustmentService _adjustmentService;

        #endregion Services

        #region Constructor

        public AdjustmentController()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AdjustmentController" /> class.
        /// </summary>
        /// <param name="adjustmentService">Adjustment Service.</param>
        public AdjustmentController(IAdjustmentService adjustmentService)
        {
            _adjustmentService = adjustmentService ?? new AdjustmentService();
        }

        #endregion Constructor

        /// <summary>
        /// Load list log.
        /// </summary>
        /// <returns>View model.</returns>
        [HttpGet]
        public ActionResult Index()
        {
            var affiliateSession = SessionManager.CurrentSession.Affiliate;
            var adjustment = this._adjustmentService.GetAdjustmentBalance(new AdjustmentCriteria
            {
                AffId = affiliateSession.AffiliateId,
                Currency = affiliateSession.Currency
            });

            return View(new AdjustmentViewModel
            {
                AffName = affiliateSession.Username,
                Adjustment = adjustment,
                Description = string.Empty,
                Currency = AppSettings.AffSystemSupportedCurrencies.FirstOrDefault(c => c.CurrencyCode == affiliateSession.Currency)
            });
        }

        /// <summary>
        /// Edit Adjustment.
        /// </summary>
        /// <param name="model">Adjustment view model.</param>
        /// <returns>View Model.</returns>
        [HttpPost]
        public ActionResult Index(AdjustmentViewModel model)
        {
            var session = SessionManager.CurrentSession;
            var criteria = new AdjustmentEditCriteria()
            {
                Ip = Remote.GetClientIPAddress(),
                AffId = session.Affiliate.AffiliateId,
                AdminId = session.CustID,
                Amount = model.Amount,
                Description = model.Description,
                LicenseeId = AppSettings.LicenseeID
            };

            this._adjustmentService.UpdateAdjustment(criteria);

            ViewBag.Result = GetResultViewModel(criteria.ErrCode);

            ModelState.Clear();

            return this.Index();
        }
    }
}