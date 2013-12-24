// --------------------------------------------------------------------------
// <copyright file="AffiliateController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Joseph.Pham" date="2013.8.1">Created</change>
// </history>

using System.Collections.Generic;

namespace Fanex.AffiliateAdmin.Controller
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using System.Xml.Linq;
    using Core.Configurations;
    using Core.Filter;
    using Core.Session;
    using Core.Utilities;
    using Criterias;
    using Fanex.Common;
    using Models;
    using Resources;
    using Services.Impl;
    using Services.Interface;

    /// <summary>
    /// Affiliate Controller.
    /// </summary>    
    [HasCurrencyPermission]
    public partial class AffiliateController : BaseController
    {
        #region Services

        private readonly IAffiliateService _service;

        #endregion Services

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="AffiliateController" /> class.
        /// </summary>
        /// <param name="service">Affiliate Service.</param>        
        public AffiliateController(IAffiliateService service)
        {
            _service = service;
        }
        #endregion Constructor

        [HttpGet]
        public ActionResult Index(AffiliateListViewModel model)
        {
            return View(model);
        }

        /// <summary>
        /// Get Affiliate list.
        /// </summary>
        /// <param name="criteria">Affiliate List Criteria.</param>
        /// <returns>Action Result.</returns>
        [HttpPost]
        public ActionResult AffiliateList(AffiliateListCriteria criteria)
        {
            criteria.PageIndex = criteria.PageIndex == 0 ? 1 : criteria.PageIndex;
            criteria.PageSize = criteria.PageSize == 0 ? AppSettings.PageSize : criteria.PageSize;
            criteria.UserId = SessionManager.CurrentSession.CustID;
            var result = _service.GetAffiliateList(criteria);

            var approveStatusList = new List<string> { "approved", "closed", "lockedout" };

            var hideTotalRows = true;

            if (!string.IsNullOrEmpty(criteria.Status))
            {
                var selectedStatusList = criteria.Status.Split(',');
                if (approveStatusList.Intersect(selectedStatusList).Any())
                {
                    hideTotalRows = false;
                }
            }

            return Json(new
            {
                List = result.AffiliateList,
                SubTotal = result.CurrencyList,
                Total = new
                    {
                        CurrencyCode = AppSettings.BaseExchange.Code,
                        Currency = criteria.TotalAmount,
                        Total = result.CurrencyList.ToList().Sum(t => t.Currency),
                        TotalMembers = result.CurrencyList.ToList().Sum(t => t.TotalMembers)
                    },
                HideTotalRows = hideTotalRows,
                CurrentPage = criteria.PageIndex - 1,
                ItemsPerPage = criteria.PageSize,
                TotalItem = criteria.NRows
            });
        }

        public ActionResult GetCommGroups(CommGroupCurrencyCriteria criteria)
        {
            return Json(new
            {
                List = AppSettings.GetCommGroups(criteria)
            });
        }

        /// <summary>
        /// Change Password.
        /// </summary>
        /// <param name="id">Affiliate Id.</param>
        /// <param name="username">Affiliate's username.</param>
        /// <returns>Change Password Page.</returns>
        public ActionResult ChangePassword(int id, string username)
        {
            return View(new ChangeAffPasswordViewModel
                {
                    Id = id,
                    Username = username
                });
        }

        /// <summary>
        /// Update Password.
        /// </summary>
        /// <param name="criteria">Input Criteria.</param>
        /// <returns>Change Password Result.</returns>
        [HttpPost]
        public ActionResult ChangePassword(ChangeAffPasswordCriteria criteria)
        {
            if (criteria.IsValid())
            {
                // Cheat sp.
                criteria.Password = null;
                criteria.Ip = Remote.GetClientIPAddress();
                criteria.UserId = SessionManager.CurrentSession.CustID;
                criteria.ConfirmPassword = Cryptography.CfsEnCode(criteria.ConfirmPassword);
                _service.ChangePassword(criteria);
                ViewBag.Result = GetResultViewModel(criteria.ErrCode);
            }
            else
            {
                ViewBag.Result = new ResultViewModel
                    {
                        ErrorCode = 16,
                        Success = false,
                        Message = "Invalid Request"
                    };
            }

            return ChangePassword(criteria.Id, criteria.Username);
        }

        [HttpGet]
        public ActionResult ChangeAccountType(ChangeAccountTypeViewModel model)
        {
            return View(model);
        }

        /// <summary>
        /// Update Account Type.
        /// </summary>
        /// <param name="criteria">Input Criteria.</param>
        /// <returns>Change Account Type Result.</returns>
        [HttpPost]
        public ActionResult ChangeAccountType(AffiliateUpdAccountTypeCriteria criteria)
        {
            if (criteria.IsValid())
            {
                criteria.Ip = Remote.GetClientIPAddress();
                criteria.UserId = SessionManager.CurrentSession.CustID;
                _service.ChangeAccountType(criteria);
                ViewBag.Result = GetResultViewModel(criteria.ErrCode);                
            }
            else
            {
                ViewBag.Result = new ResultViewModel
                {
                    ErrorCode = 16,
                    Success = false,
                    Message = Lang.InvalidRequest
                };
            }

            return ChangeAccountType(new ChangeAccountTypeViewModel
                {
                    AffIds = criteria.AffIds, 
                    Usernames = criteria.Usernames,
                    TestLevel = criteria.TestLevel
                });
        }

        /// <summary>
        /// Change Commission Group.
        /// </summary>
        /// <param name="model">Input Model.</param>
        /// <returns>Change Commission Group Page.</returns>
        [HttpGet]
        public ActionResult ChangeCommissionGroup(ChangeCommissionGroupViewModel model)
        {
            model.UserId = SessionManager.CurrentSession.CustID;
            return View(model);
        }

        /// <summary>
        /// Change Commission Group.
        /// </summary>
        /// <param name="criteria">Input Criteria.</param>
        /// <returns>Assign Commission Group Result.</returns>
        [HttpPost]
        public ActionResult ChangeCommissionGroup(CommGroupAssignCriteria criteria)
        {
            if (criteria.IsValid())
            {
                criteria.IP = Remote.GetClientIPAddress();
                criteria.UserId = SessionManager.CurrentSession.CustID;
                _service.ChangeCommissionGroup(criteria);
                ViewBag.Result = GetResultViewModel(criteria.ErrCode);
            }
            else
            {
                ViewBag.Result = new ResultViewModel
                {
                    ErrorCode = 16,
                    Success = false,
                    Message = "Invalid Request"
                };
            }

            return ChangeCommissionGroup(new ChangeCommissionGroupViewModel
            {
                AffIds = criteria.AffIds,
                Usernames = criteria.Usernames,
                Currency = criteria.Currency
            });
        }

        /// <summary>
        /// Add favorite.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>Add favorite result.</returns>
        [HttpPost]
        public ActionResult AddFavorite(FavoriteCriteria criteria)
        {
            if (criteria.IsValid())
            {
                criteria.UserId = SessionManager.CurrentSession.CustID;
                _service.AddFavorite(criteria);
                return Json(GetResultViewModel(criteria.ErrCode));
            }

            return Json(new ResultViewModel
                {
                    ErrorCode = 16,
                    Success = false,
                    Message = Lang.InvalidRequest
                });
        }

        /// <summary>
        /// Remove favorite.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>Remove favorite result.</returns>
        [HttpPost]
        public ActionResult RemoveFavorite(FavoriteCriteria criteria)
        {
            if (criteria.IsValid())
            {
                criteria.UserId = SessionManager.CurrentSession.CustID;
                _service.RemoveFavorite(criteria);
                return Json(GetResultViewModel(criteria.ErrCode));
            }

            return Json(new ResultViewModel
            {
                ErrorCode = 16,
                Success = false,
                Message = Lang.InvalidRequest
            });
        }

        /// <summary>
        /// Customer list action.
        /// </summary>
        /// <param name="affname">Affiliate name.</param>
        /// <returns>Action Result.</returns>
        [HttpGet]
        public ActionResult MemberList(string affname)
        {
            ViewBag.affname = affname;
            return View();
        }

        /// <summary>
        /// Member List.
        /// </summary>
        /// <param name="criteria">Member List Criteria.</param>
        /// <returns>Action Result.</returns>
        [HttpPost]
        public ActionResult MemberList(MemberListCriteria criteria)
        {
            if (criteria.ByAffiliate)
            {
                criteria.AffUsername = this.CurrentSession.Affiliate.UserName;
                criteria.AffType = string.Join(",", AppSettings.AffTypes.Select(t => t.TypeID));
                criteria.ExcludeTest = false;
            }

            criteria.IsITAdmin = this.CurrentSession.IsIT;
            criteria.PageIndex = criteria.PageIndex == 0 ? 1 : criteria.PageIndex;
            criteria.PageSize = criteria.PageSize == 0 ? AppSettings.PageSize : criteria.PageSize;
            criteria.UserId = SessionManager.CurrentSession.CustID;

            var result = _service.CustomerList(criteria);

            return Json(new
            {
                List = result,
                CurrentPage = criteria.PageIndex - 1,
                ItemsPerPage = criteria.PageSize,
                TotalItem = criteria.NRows
            });
        }

        /// <summary>
        /// Remove Customer.
        /// </summary>
        /// <param name="username">Username of customer.</param>
        /// <returns>Action Result.</returns>
        [HttpPost]
        public ActionResult CustomerRemove(string username)
        {
            var criteria = new CustomerRemoveCriteria
                                            {
                                                UserId = SessionManager.CurrentSession.CustID,
                                                Username = username,
                                                IP = Remote.GetClientIPAddress()
                                            };
            _service.CustomerRemove(criteria);

            return Json(GetResultViewModel(criteria.ErrCode));
        }

        public ActionResult AssignMember(AssignMemberViewModel model)
        {
            if (model.ByAffiliate)
            {
                model.AffName = CurrentSession.Affiliate.UserName;
            }

            return View(model);
        }

        /// <summary>
        /// Assign member to affiliate Action.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>Result Message.</returns>
        [HttpPost]
        public ActionResult AssignMember(CheckMemberExistCriteria criteria)
        {
            this._service.CheckMemberExist(criteria);
            var message = string.Empty;
            var errorCode = 0;
            if (criteria.AffID < 1)
            {
                message = Lang.AffNotExist;
            }
            else if (!string.IsNullOrEmpty(criteria.AffStatus) && !criteria.AffStatus.Equals("approved"))
            {
                message = GetMessageFromAffiliateStatus(criteria.AffStatus);
            }
            else
            {
                if (criteria.IsMemberExist)
                {
                    message = Lang.MemberExist;
                }
                else if (AppSettings.Environment != "Local Development")
                {
                    var xml = ServiceWrapper.GetLicenseeCustInfo(AppSettings.SystemName, criteria.MemberName);
                    if (xml.Element("ReturnCode").Value != "0")
                    {
                        message = xml.Element("ReturnMessage").Value;
                    }
                    else
                    {
                        errorCode = InsertMember(criteria.AffID, criteria.AffName, criteria.MemberName, xml);
                    }
                }
                else
                {
                    errorCode = TestInsertMember(criteria.AffID, criteria.AffName, criteria.MemberName);
                }
            }

            ViewBag.Result = errorCode == 0 ? GetResultViewModel(string.IsNullOrEmpty(message) ? 0 : 1, null, message) : GetResultViewModel(errorCode);

            return AssignMember(new AssignMemberViewModel
                {
                    MemberName = criteria.MemberName,
                    AffName = criteria.AffName,
                    ByAffiliate = criteria.ByAffiliate
                });
        }        

        /// <summary>
        /// Member list.
        /// </summary>
        /// <param name="model">Model input.</param>
        /// <returns>Action Result.</returns>
        [HttpGet]
        public ActionResult MemberListByAffiliate(MemberListViewModel model)
        {
            return View(model);
        }

        /// <summary>
        /// Change Commission Group when affiliate approve.
        /// </summary>
        /// <param name="model">Input Model.</param>
        /// <returns>Change Commission Group Page.</returns>
        [HttpGet]
        public ActionResult ChangeCommissionApprove(ChangeCommissionGroupViewModel model)
        {
            model.UserId = SessionManager.CurrentSession.CustID;
            return View(model);
        }

        #region Private Methods

        /// <summary>
        /// Get message from affiliate status.
        /// </summary>
        /// <param name="affiliateStatus">Affiliate status.</param>
        /// <returns>The result message.</returns>
        private static string GetMessageFromAffiliateStatus(string affiliateStatus)
        {
            var message = string.Empty;
            switch (affiliateStatus.ToLower())
            {
                case "closed":
                    message = Lang.AffClosed;
                    break;
                case "approved":
                    message = Lang.AffApproved;
                    break;
                default:
                    message = string.Format(Lang.AffiliateIs, affiliateStatus);
                    break;
            }

            return message;
        }

        /// <summary>
        /// Insert member to affiliate (This method is only used for testing at local).
        /// </summary>
        /// <param name="affid">Affiliate ID.</param>
        /// <param name="affname">Affiliate Name.</param>
        /// <param name="customerName">Member Name.</param>      
        /// <returns>Error code.</returns>
        private int TestInsertMember(int affid, string affname, string customerName)
        {            
            var insertCriteria = new InsertMemberCriteria()
            {
                AffUserName = affname,
                Username = customerName,
                Currency = AppSettings.BaseExchange.Code,
                IBCCustID = 0,                
                AgentID = 0,
                CreatedDate = DateTime.Now,
                RegisterDate = DateTime.Now,
                IP = Remote.GetClientIPAddress()
            };
            _service.InsertMember(insertCriteria);
            return insertCriteria.ErrCode;
        }

        /// <summary>
        /// Insert member to affiliate.
        /// </summary>
        /// <param name="affid">Affiliate ID.</param>
        /// <param name="affname">Affiliate Name.</param>
        /// <param name="customerName">Member Name.</param>
        /// <param name="customerInfo">Customer Information.</param>
        /// <returns>Error code.</returns>
        private int InsertMember(int affid, string affname, string customerName, XElement customerInfo)
        {            
            long clickid = 0;
            var btag = customerInfo.Element("btag").Value;

            if (!string.IsNullOrEmpty(btag))
            {
                var array = btag.Split('_');
                //// var licensee = array[0];
                //// var affid = array.Length > 1 ? array[1] : String.Empty;
                clickid = long.Parse(array.Length > 2 ? array[2] : "0");
            }

            var createdate = DateTime.MinValue;
            DateTime.TryParse(customerInfo.Element("FirstDepositDate").Value, out createdate);
            var registerDate = DateTime.Now;
            DateTime.TryParse(customerInfo.Element("RegisterDate").Value, out registerDate);
            var insertCriteria = new InsertMemberCriteria()
            {
                AffUserName = affname,
                Username = customerName,
                Currency = customerInfo.Element("Currency").Value,
                IBCCustID = 0,
                IBCUserName = customerInfo.Element("IBCusername").Value,
                AgentID = 0,
                CreatedDate = createdate,
                RegisterDate = registerDate,
                Btag = btag,
                ClickID = clickid,
                Language = AppSession.Language,
                FirstName = customerInfo.Element("FirstName").Value,
                LastName = customerInfo.Element("LastName").Value,
                Phone = customerInfo.Element("Phone").Value,
                Address = customerInfo.Element("Address").Value,
                IP = Remote.GetClientIPAddress()
            };
            _service.InsertMember(insertCriteria);
            ServiceWrapper.UpdateBtage(
                AppSettings.SystemName,
                customerName,
                string.Format("{0}_{1}_0", AppSettings.LicenseeID, affid));
            return insertCriteria.ErrCode;
        }

        #endregion
    }
}
