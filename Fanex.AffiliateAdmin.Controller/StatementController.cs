// -----------------------------------------------------------------------
// <copyright file="StatementController.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.AffiliateAdmin.Controller
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.AffiliateAdmin.Core.Configurations;
    using Fanex.AffiliateAdmin.Core.Filter;
    using Fanex.AffiliateAdmin.Core.Services.Interface;
    using Fanex.AffiliateAdmin.Core.Session;
    using Fanex.AffiliateAdmin.Criterias;
    using Fanex.AffiliateAdmin.Models;
    using Fanex.AffiliateAdmin.Services;
    using Helpers;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [FunctionAuth(Function = FunctionPolicyNames.Affiliate)]
    public class StatementController : BaseController
    {
        #region Services

        private readonly ITransTypeService _transTypeService;

        private readonly IStatementService _statementService;

        #endregion Services

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="StatementController" /> class.
        /// </summary>
        /// <param name="transTypeService">Transaction Service.</param>
        /// <param name="statementService">Statement Service.</param>
        public StatementController(
            ITransTypeService transTypeService,
            IStatementService statementService,
            ISystemSettingService systemSettingService) : base(systemSettingService)
        {
            _transTypeService = transTypeService;
            _statementService = statementService;
        }

        #endregion Constructor

        /// <summary>
        /// Get statement.
        /// </summary>
        /// <returns>View model.</returns>
        [HttpGet]
        public ActionResult Index()
        {
            var affiliate = AffiliateInfoCookieHelper.GetAffiliate(HttpContext);
            var session = SessionManager.CurrentSession;
            var baseCurrencyList = new List<SelectListItem>
            {
                new SelectListItem { Value = "0", Text = affiliate.CurrencyCode }
            };

            var transTypeList = _transTypeService.GetTransTypes(new TransTypeCriteria());

            var model = new StatementViewModel();
            model.AddBaseCurrencyList(baseCurrencyList);
            model.AddTransTypeList(transTypeList.ToList());

            return View(model);
        }

        /// <summary>
        /// Get List statement.
        /// </summary>
        /// <param name="criteria">Statement criteria.</param>
        /// <returns>JSON data.</returns>
        [HttpPost]
        public ActionResult ListStatement(StatementCriteria criteria)
        {
            criteria.AffId = AffiliateInfoCookieHelper.GetAffiliate(HttpContext).AffId;

            var data = _statementService.GetAffiliateStatement(criteria);

            return Json(new { List = data });
        }
    }
}