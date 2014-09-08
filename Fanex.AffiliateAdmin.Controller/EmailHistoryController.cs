// --------------------------------------------------------------------------
// <copyright file="EmailHistoryController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Joseph.Pham" date="2013.8.2">Created</change>
// </history>

using System;

namespace Fanex.AffiliateAdmin.Controller
{
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
    [FunctionAuth(Function = "[Affiliate]")]
    public partial class EmailHistoryController : BaseController
    {
        #region Services
        /// <summary>
        /// Service used to work with database.
        /// </summary>
        private readonly IEmailHistoryService _service;

        #endregion Services

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailHistoryController" /> class.
        /// </summary>
        /// <param name="service">Email History service.</param>        
        public EmailHistoryController(IEmailHistoryService service)
        {
            _service = service;
        }
        #endregion Constructor

        /// <summary>
        /// Index action.
        /// </summary>        
        /// <returns>Default EmailHistoryViewModel.</returns>
        [HttpGet]
        public ActionResult Index()
        {
            return View(new EmailHistoryViewModel());
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
            
            var criteria = new EmailHistoryCriteria();
            if (!viewModel.IsAll)
            {
                criteria.FromDate = viewModel.FromDate;                
                criteria.ToDate = viewModel.ToDate.Value.AddDays(1).AddMilliseconds(-1);
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
            criteria.PageSize = viewModel.PageSize == 0 ? AppSettings.PageSize : viewModel.PageSize;           
                        
            var result = _service.GetList(criteria);

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
            var c = new EmailHistoryCriteria { PageIndex = 1, PageSize = 1, MID = id };
            var email = _service.GetList(c).FirstOrDefault();
            return View(email);
        }

        /// <summary>
        /// Delete email(s).
        /// </summary>
        /// <param name="listID">String of id separated by comma.</param>
        /// <returns>OK or Error message.</returns>
        public ActionResult Delete(string listID)
        {
            var criteria = new EmailHistoryCriteria
            {
                CriteriaType = CriteriaType.Delete,
                ListMID = listID.TrimEnd(','),
                UserId = SessionManager.CurrentSession.CustID,
                IP = Remote.GetClientIPAddress()
            };
            _service.ExecuteSP(criteria);            
            
            return Json(GetResultViewModel(criteria.ErrCode, Lang.DeleteSucc));
        }
    }
}