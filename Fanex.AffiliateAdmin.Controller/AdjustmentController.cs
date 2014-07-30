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
    using Fanex.AffiliateAdmin.Core.Utilities;
    using Fanex.AffiliateAdmin.Criterias;
    using Fanex.AffiliateAdmin.Models;
    using Fanex.AffiliateAdmin.Services;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [FunctionAuth(Function = "[Affiliate]")]
    public class AdjustmentController : BaseController
    {
        #region Services

        private readonly IAdjustmentService _adjustmentService;

        #endregion Services

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="AdjustmentController" /> class.
        /// </summary>  
        /// <param name="adjustmentService">Adjustment Service.</param>  
        public AdjustmentController(IAdjustmentService adjustmentService)
        {
            _adjustmentService = adjustmentService;
        }

        #endregion Constructor

        /// <summary>
        /// Load list log.
        /// </summary>
        /// <returns>View model.</returns>
        [HttpGet]
        public ActionResult Index()
        {
            var adjustment = this._adjustmentService.GetAdjustmentBalance(new AdjustmentCriteria
            {
                AffId = this.CurrentSession.Affiliate.AffiliateId,
                Currency = this.CurrentSession.Affiliate.Currency
            });

            return View(new AdjustmentViewModel
            {
                AffName = this.CurrentSession.Affiliate.Username,
                Adjustment = adjustment,
                Description = string.Empty,
                Currency = AppSettings.AffSystemSupportedCurrencies.FirstOrDefault(c => c.CurrencyCode == CurrentSession.Affiliate.Currency)
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
            var criteria = new AdjustmentEditCriteria()
            {
                Ip = Remote.GetClientIPAddress(),
                AffId = this.CurrentSession.Affiliate.AffiliateId,
                AdminId = this.CurrentSession.CustID,
                Amount = model.Amount,
                Description = model.Description 
            };
            
            this._adjustmentService.UpdateAdjustment(criteria);

            ViewBag.Result = GetResultViewModel(criteria.ErrCode);

            ModelState.Clear();

            return this.Index();
        }
    }
}
