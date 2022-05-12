namespace Fanex.AffiliateAdmin.Controller
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using Core.Configurations;
    using Core.Session;
    using Core.Utilities;
    using Criterias;
    using Fanex.AffiliateAdmin.Core.Filter;
    using Fanex.AffiliateAdmin.Resources;
    using Fanex.AffiliateAdmin.Services;
    using Models;

    /// <summary>
    /// Email History Controller.
    /// </summary>
    [FunctionAuth(Function = FunctionPolicyNames.Affiliate)]
    public partial class EmailHistoryController : BaseController
    {
        #region Services

        /// <summary>
        /// Service used to work with database.
        /// </summary>
        private readonly ISystemSettings _systemSettings;
        private readonly IEmailHistoryService _emailHistoryService;

        #endregion Services

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailHistoryController" /> class.
        /// </summary>
        /// <param name="service">Email History service.</param>
        public EmailHistoryController(
            IAppSetting appSetting,
            ISystemSettings systemSettings,
            IEmailHistoryService emailHistoryService) : base(appSetting)
        {
            _systemSettings = systemSettings;
            _emailHistoryService = emailHistoryService;
        }

        #endregion Constructor

        /// <summary>
        /// Index action.
        /// </summary>
        /// <returns>Default EmailHistoryViewModel.</returns>
        [HttpGet]
        public ActionResult Index()
        {
            var systemDateTime = _systemSettings.SystemDateTime;

            return View(new EmailHistoryViewModel {
                FromDate = systemDateTime,
                ToDate = systemDateTime
            });
        }

        /// <summary>
        /// Load email history data.
        /// </summary>
        /// <param name="viewModel">Default view model.</param>
        /// <returns>List of email.</returns>
        [HttpPost]
        public ActionResult EmailHistory(EmailHistoryViewModel viewModel)
        {
            viewModel.SearchText = viewModel.SearchText.OrDefault();

            var criteria = new EmailHistorySelCriteria();
            if (!viewModel.IsAll)
            {
                criteria.Fromdate = viewModel.FromDate ?? DateTime.Now;
                criteria.Todate = viewModel.ToDate.HasValue
                    ? viewModel.ToDate.Value.AddDays(1).AddMilliseconds(-1)
                    : DateTime.Now.AddDays(1).AddMilliseconds(-1);
            }

            switch (viewModel.SearchOption)
            {
                case 0:
                    criteria.SenderName = viewModel.SearchText;
                    break;

                case 1:
                    criteria.Email = viewModel.SearchText;
                    break;

                case 2:
                    criteria.AffiliateUserName = viewModel.SearchText ?? string.Empty;
                    break;

                case -2:
                    criteria.MID = -1;
                    break;
            }

            criteria.PageIndex = viewModel.PageIndex == 0 ? 1 : viewModel.PageIndex;
            criteria.PageSize = viewModel.PageSize == 0 ? AppSetting.PageSize : viewModel.PageSize;

            var result = _emailHistoryService.GetList(criteria);

            return Json(new
            {
                List = result,
                CurrentPage = criteria.PageIndex - 1,
                ItemsPerPage = criteria.PageSize,
                TotalItem = criteria.NRows
            });
        }

        /// <summary>
        /// Get detail email.
        /// </summary>
        /// <param name="id">Id of Email.</param>
        /// <returns>One EmailHistory object.</returns>
        public ActionResult Detail(int? id)
        {
            var c = new EmailHistorySelCriteria { PageIndex = 1, PageSize = 1, MID = id };
            var email = _emailHistoryService.GetList(c).FirstOrDefault();
            return View(email);
        }

        /// <summary>
        /// Delete email(s).
        /// </summary>
        /// <param name="listID">String of id separated by comma.</param>
        /// <returns>OK or Error message.</returns>
        public ActionResult Delete(string listID)
        {
            var criteria = new EmailHistoryDelCriteria
            {
                Listmid = listID.TrimEnd(','),
                UserID = SessionManager.CurrentSession.CustID,
                IP = Remote.GetClientIPAddress()
            };
            _emailHistoryService.ExecuteSP(criteria);

            return Json(GetResultViewModel(criteria.ErrCode, Lang.DeleteSucc));
        }
    }
}