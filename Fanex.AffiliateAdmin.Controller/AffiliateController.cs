namespace Fanex.AffiliateAdmin.Controller
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using System.Xml.Linq;
    using Fanex.AffiliateAdmin.Core.Configurations;
    using Fanex.AffiliateAdmin.Core.Filter;
    using Fanex.AffiliateAdmin.Core.Session;
    using Fanex.AffiliateAdmin.Core.Utilities;
    using Fanex.AffiliateAdmin.Criterias;
    using Fanex.AffiliateAdmin.Models;
    using Fanex.AffiliateAdmin.Resources;
    using Fanex.AffiliateAdmin.Services;
    using Fanex.Common;
    using Helpers;

    /// <summary>
    /// Affiliate Controller.
    /// </summary>
    [FunctionAuth(Function = FunctionPolicyNames.Affiliate, CurrenciesRequired = true)]
    public partial class AffiliateController : BaseController
    {
        #region Services

        private readonly IAffiliateService _service;

        #endregion Services

        #region Constructor

        public AffiliateController()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AffiliateController" /> class.
        /// </summary>
        /// <param name="service">Affiliate Service.</param>
        public AffiliateController(IAffiliateService service)
        {
            _service = service ?? new AffiliateService();
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
            criteria.LicenseeId = AppSettings.LicenseeID;
            criteria.UserCurrencies = SessionManager.CurrentSession.BuildUserCurrencyParam();

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

            var hasMultipleCurrency = result.AffiliateList
                                            .GroupBy(t => t.Currency)
                                            .Select(t => new { Currency = t.Key, RowCount = t.Count() })
                                            .Any(t => t.RowCount > 1);

            return Json(new
            {
                List = result.AffiliateList,
                SubTotal = result.CurrencyList,
                Total = new
                {
                    CurrencyCode = AppSettings.BaseExchange.Code,
                    Currency = criteria.TotalAmount,
                    Total = result.CurrencyList.Sum(t => t.Currency),
                    TotalMembers = result.CurrencyList.Sum(t => t.TotalMembers)
                },
                HasMultipleCurrency = hasMultipleCurrency,
                HideTotalRows = hideTotalRows,
                CurrentPage = criteria.PageIndex - 1,
                ItemsPerPage = criteria.PageSize,
                TotalItem = criteria.NRows
            });
        }

        public ActionResult GetCommGroups(CommGroupCurrencyCriteria criteria)
        {
            criteria.LicenseeId = AppSettings.LicenseeID;

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
        /// <param name="password">Affiliate's password.</param>
        /// <param name="confirmPassword">Affiliate's confirm password.</param>
        /// <returns>Change Password Page.</returns>
        public ActionResult ChangePassword(int id, string username, string password, string confirmPassword)
        {
            return View(new ChangeAffPasswordViewModel
            {
                Id = id,
                Username = username,
                Password = password,
                ConfirmPassword = confirmPassword
            });
        }

        /// <summary>
        /// Update Password.
        /// </summary>
        /// <param name="model">Password view model.</param>
        /// <returns>Change Password Result.</returns>
        [HttpPost]
        public ActionResult ChangePassword(ChangeAffPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var criteria = new ChangeAffPasswordCriteria
                {
                    Id = model.Id,
                    Username = model.Username,
                    Password = null,
                    Ip = Remote.GetClientIPAddress(),
                    UserId = SessionManager.CurrentSession.CustID,
                    ConfirmPassword = Cryptography.CfsEnCode(model.ConfirmPassword)
                };

                _service.ChangePassword(criteria);
                ViewBag.Result = GetResultViewModel(criteria.ErrCode);
            }

            return ChangePassword(model.Id, model.Username, model.Password, model.ConfirmPassword);
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
                criteria.LicenseeId = AppSettings.LicenseeID;
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
            model.CommissionGroupOptionList = GetCommGroupList(model.Currency, model.GroupId);

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
                criteria.LicenseeId = AppSettings.LicenseeID;

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
                Currency = criteria.Currency,
                GroupId = criteria.GroupId
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
                var session = SessionManager.CurrentSession;
                criteria.UserId = session.CustID;

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
        /// <param name="affName">Affiliate name.</param>
        /// <param name="memberName">Member name.</param>
        /// <returns>Action Result.</returns>
        [HttpGet]
        public ActionResult MemberList(string affName, string memberName)
        {
            ViewBag.AffName = affName;
            ViewBag.MemberName = memberName;
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
            var session = SessionManager.CurrentSession;
            if (criteria.ByAffiliate)
            {
                criteria.AffUsername = AffiliateInfoCookieHelper.GetAffiliate(HttpContext).UserName;
                criteria.AffType = string.Join(",", AppSettings.AffTypes.Select(t => t.TypeID));
                criteria.ExcludeTest = false;
            }

            criteria.IsITAdmin = session.IsIT;
            criteria.PageIndex = criteria.PageIndex == 0 ? 1 : criteria.PageIndex;
            criteria.PageSize = criteria.PageSize == 0 ? AppSettings.PageSize : criteria.PageSize;
            criteria.UserId = session.CustID;
            criteria.UserCurrencies = session.BuildUserCurrencyParam();
            criteria.LicenseeId = AppSettings.LicenseeID;

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
        [FunctionAuth(Function = FunctionPolicyNames.Affiliate, IsHostPolicy = true)]
        [HttpPost]
        public ActionResult CustomerRemove(string username)
        {
            var criteria = new CustomerRemoveCriteria
            {
                UserId = SessionManager.CurrentSession.CustID,
                Username = username,
                IP = Remote.GetClientIPAddress(),
                LicenseeId = AppSettings.LicenseeID
            };
            _service.CustomerRemove(criteria);

            var model = GetResultViewModel(criteria.ErrCode);

            if (!model.Success)
            {
                model.Message = model.Message ?? criteria.ErrMsg;
            }

            return Json(GetResultViewModel(criteria.ErrCode));
        }

        [FunctionAuth(Function = FunctionPolicyNames.Affiliate, IsHostPolicy = true)]
        public ActionResult AssignMember(AssignMemberViewModel model)
        {
            if (model.ByAffiliate)
            {
                model.AffName = AffiliateInfoCookieHelper.GetAffiliate(HttpContext).UserName;
            }

            return View(model);
        }

        /// <summary>
        /// Assign member to affiliate Action.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>Result Message.</returns>
        [FunctionAuth(Function = FunctionPolicyNames.Affiliate, IsHostPolicy = true)]
        [HttpPost]
        public ActionResult AssignMember(CheckMemberExistCriteria criteria)
        {
            _service.CheckMemberExist(criteria);
            var result = new ResultViewModel();

            if (!IsValidAffiliate(criteria, ref result) || !IsValidMember(criteria, ref result))
            {
                ViewBag.Result = result;

                return AssignMember(new AssignMemberViewModel
                {
                    MemberName = criteria.MemberName,
                    AffName = criteria.AffName,
                    ByAffiliate = criteria.ByAffiliate
                });
            }

            var message = string.Empty;
            var errorCode = 0;

            if (AppSettings.LocalTest)
            {
                errorCode = TestInsertMember(criteria.AffID, criteria.AffName, criteria.MemberName);
            }
            else
            {
                if (criteria.MemberAffID > 0)
                {
                    errorCode = AssignOldMemberToAffiliate(criteria);
                }
                else
                {
                    AssignNewMemberToAffiliate(criteria, ref message, ref errorCode);
                }

                if (errorCode == 0 && string.IsNullOrEmpty(message))
                {
                    ServiceWrapper.UpdateBtage(AppSettings.SystemName, criteria.MemberName, string.Format("{0}_{1}_0", AppSettings.LicenseeID, criteria.AffID));
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
            model.Currency = AffiliateInfoCookieHelper.GetAffiliate(HttpContext).Currency.ToString();
            model.CommissionGroupOptionList = GetCommGroupList(model.Currency, model.GroupId);

            return View(model);
        }

        #region Private Methods

        private static IEnumerable<SelectListItem> GetCommGroupList(string currency, int groupId)
        {
            var session = SessionManager.CurrentSession;

            var comGroups = AppSettings.GetActiveCommGroups(new CommGroupSelActiveListCriteria
            {
                UserId = session.CustID,
                Currency = currency,
                UserCurrencies = session.BuildUserCurrencyParam()
            });

            return comGroups.Select(t => new SelectListItem
            {
                Text = t.GroupName,
                Value = t.GroupID.ToString(),
                Selected = t.GroupID == groupId
            });
        }

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
                    var status = Lang.ResourceManager.GetString(affiliateStatus);
                    message = string.Format(Lang.AffiliateIs, string.IsNullOrEmpty(status) ? affiliateStatus.ToLower() : status);
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
            var now = SystemTimeService.GetSystemDateTime();

            var insertCriteria = new InsertMemberCriteria()
            {
                AffUserName = affname,
                Username = customerName,
                Currency = AppSettings.BaseExchange.Code,
                IBCCustID = 0,
                AgentID = 0,
                Btag = string.Format("{0}_{1}_0", AppSettings.LicenseeID, affid),
                CreatedDate = now,
                RegisterDate = now,
                AffAffectedDate = now,
                IP = Remote.GetClientIPAddress()
            };

            _service.InsertMember(insertCriteria);

            return insertCriteria.ErrCode;
        }

        /// <summary>
        /// Assign old member to affiliate.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>Return error code.</returns>
        private int AssignOldMemberToAffiliate(CheckMemberExistCriteria criteria)
        {
            var insertCriteria = new InsertMemberCriteria()
            {
                AffUserName = criteria.AffName,
                Username = criteria.MemberName,
                AffAffectedDate = SystemTimeService.GetSystemDateTime(),
                ClickID = 0,
                Btag = string.Empty,
                IP = Remote.GetClientIPAddress()
            };

            _service.InsertMember(insertCriteria);
            return insertCriteria.ErrCode;
        }

        /// <summary>
        /// Assign New Member To Affiliate.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <param name="message">Error message.</param>
        /// <param name="errorCode">Error code.</param>
        private void AssignNewMemberToAffiliate(CheckMemberExistCriteria criteria, ref string message, ref int errorCode)
        {
            var xml = ServiceWrapper.GetLicenseeCustInfo(AppSettings.SystemName, criteria.MemberName);

            if (xml.Element("ReturnCode").Value != "0")
            {
                message = xml.Element("ReturnMessage").Value;

                return;
            }

            if (!HasFirstDepositDate(xml))
            {
                message = string.Format(Lang.MemberNotDepositOnMemberSite, criteria.MemberName);
                return;
            }

            var insertCriteria = new InsertMemberCriteria()
            {
                AffUserName = criteria.AffName,
                Username = criteria.MemberName,
                Currency = xml.Element("Currency").Value,
                IBCCustID = 0,
                IBCUserName = xml.Element("IBCusername").Value,
                AgentID = 0,
                AffAffectedDate = SystemTimeService.GetSystemDateTime(),
                Btag = xml.Element("btag").Value,
                Language = AppSession.Language,
                FirstName = xml.Element("FirstName").Value,
                LastName = xml.Element("LastName").Value,
                Phone = xml.Element("Phone").Value,
                Address = xml.Element("Address").Value,
                Email = xml.Element("Email").Value,
                IP = Remote.GetClientIPAddress()
            };

            GetMoreCustomerInfo(xml, insertCriteria);
            _service.InsertMember(insertCriteria);
            errorCode = insertCriteria.ErrCode;
        }

        /// <summary>
        /// Check whether the member has already deposited on member site.
        /// </summary>
        /// <param name="customerInfo">Customer information.</param>
        /// <returns>Has first deposit.</returns>
        private bool HasFirstDepositDate(XElement customerInfo)
        {
            if (customerInfo.Element("FirstDepositDate").Value.Equals("N/A"))
            {
                if (!string.IsNullOrEmpty(customerInfo.Element("IBCusername").Value))
                {
                    var customer = _service.GetCustomer(new CustomerSelByUserNameCriteria { UserName = customerInfo.Element("IBCusername").Value });

                    return customer.CreatedDate.HasValue;
                }

                return false;
            }

            return true;
        }

        /// <summary>
        /// Get more customer information.
        /// </summary>
        /// <param name="customerInfo">Customer Info under XML.</param>
        /// <param name="criteria">Input criteria.</param>
        private void GetMoreCustomerInfo(XElement customerInfo, InsertMemberCriteria criteria)
        {
            long clickid = 0;

            if (!string.IsNullOrEmpty(customerInfo.Element("btag").Value))
            {
                var array = customerInfo.Element("btag").Value.Split('_');
                clickid = long.Parse(array.Length > 2 ? array[2] : "0");
            }

            DateTime? createdate = null;
            if (customerInfo.Element("FirstDepositDate").Value.Equals("N/A"))
            {
                if (!string.IsNullOrEmpty(customerInfo.Element("IBCusername").Value))
                {
                    var customer = _service.GetCustomer(new CustomerSelByUserNameCriteria { UserName = customerInfo.Element("IBCusername").Value });
                    createdate = customer.CreatedDate;
                }
            }
            else
            {
                DateTime tempCreatedDate;
                if (DateTime.TryParse(customerInfo.Element("FirstDepositDate").Value, out tempCreatedDate))
                {
                    createdate = tempCreatedDate;
                }
            }

            DateTime? registerDate = null;
            DateTime tempRegisterDate;
            if (DateTime.TryParse(customerInfo.Element("RegisterDate").Value, out tempRegisterDate))
            {
                registerDate = tempRegisterDate;
            }

            criteria.ClickID = clickid;
            criteria.CreatedDate = createdate;
            criteria.RegisterDate = registerDate;
        }

        /// <summary>
        /// Is valid member.
        /// </summary>
        /// <param name="criteria">Check member exist criteria.</param>
        /// <param name="result">Result view model.</param>
        /// <returns>Valid or invalid member.</returns>
        private bool IsValidMember(CheckMemberExistCriteria criteria, ref ResultViewModel result)
        {
            if (criteria.IsMemberExist)
            {
                result = GetResultViewModel(1, null, Lang.MemberExist);

                return false;
            }

            return true;
        }

        /// <summary>
        /// Is valid affiliate.
        /// </summary>
        /// <param name="criteria">Check member exist criteria.</param>
        /// <param name="result">Result view model.</param>
        /// <returns>Valid or invalid affiliate.</returns>
        private bool IsValidAffiliate(CheckMemberExistCriteria criteria, ref ResultViewModel result)
        {
            if (criteria.AffID < 1)
            {
                result = GetResultViewModel(1, null, Lang.AffNotExist);

                return false;
            }

            if (!string.IsNullOrEmpty(criteria.AffStatus) && !criteria.AffStatus.Equals("approved", StringComparison.CurrentCultureIgnoreCase))
            {
                var message = GetMessageFromAffiliateStatus(criteria.AffStatus);

                result = GetResultViewModel(1, null, message);

                return false;
            }

            return true;
        }

        #endregion Private Methods
    }
}