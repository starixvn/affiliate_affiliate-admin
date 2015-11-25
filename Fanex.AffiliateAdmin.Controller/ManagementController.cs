// -----------------------------------------------------------------------
// <copyright file="ManagementController.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
// --------------------------------------------------------------------------
// <history>
//   <change who="Muller.Nguyen" date="2013.09.01">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Controller
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using System.Web.Script.Serialization;
    using System.Xml;
    using Common;
    using Core.Session;
    using Core.Utilities;
    using Criterias;
    using Fanex.AffiliateAdmin.Core.Configurations;
    using Fanex.AffiliateAdmin.Core.Filter;
    using Fanex.AffiliateAdmin.Criterias.Account;
    using Fanex.AffiliateAdmin.Services;
    using Models;
    using Resources;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [FunctionAuth(Function = "[UserControl]")]
    public class ManagementController : BaseController
    {
        #region Static Fields

        private static IEnumerable<string> ignoredGroupNames = new string[] { "Administrator", "None", "Aff_IT_Dept" };

        #endregion Static Fields

        #region Services

        private readonly IAccountService _accountService;

        private readonly IGroupService _groupService;

        private readonly IPolicyService _policyService;

        private readonly IPermissionCurrencyService _permissionService;

        #endregion Services

        #region Constructor

        public ManagementController()
            : this(null, null, null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ManagementController" /> class.
        /// </summary>
        /// <param name="accountService">Account Service.</param>
        /// <param name="groupService">Group Service.</param>
        /// <param name="policyService">Policy Service.</param>
        /// <param name="permissionService">Permission Service.</param>
        public ManagementController(
            IAccountService accountService,
            IGroupService groupService,
            IPolicyService policyService,
            IPermissionCurrencyService permissionService)
        {
            _accountService = accountService ?? new AccountService();
            _groupService = groupService ?? new GroupService();
            _policyService = policyService ?? new PolicyService();
            _permissionService = permissionService ?? new PermissionCurrencyService();
        }

        #endregion Constructor

        #region User Management

        /// <summary>
        /// User management.
        /// </summary>
        /// <returns>User management page.</returns>
        [HttpGet]
        public ActionResult Users()
        {
            return View(new UserManagementViewModel());
        }

        /// <summary>
        /// List User admin.
        /// </summary>
        /// <param name="criteria">User criteria.</param>
        /// <returns>Return list user.</returns>
        [HttpPost]
        public ActionResult Users(SelByGroupIDCriteria criteria)
        {
            criteria.IsIt = SessionManager.CurrentSession.IsIT;
            criteria.LicenseeId = AppSettings.LicenseeID;

            return Json(new { List = _accountService.GetListAccountManagement(criteria) });
        }

        #region Edit User

        /// <summary>
        /// Update information account.
        /// </summary>
        /// <param name="custId">Customer Id.</param>
        /// <returns>Return view.</returns>
        [HttpGet]
        public ActionResult EditUser(int custId)
        {
            var criteria = new SelByCustIDCriteria
            {
                CustId = custId
            };

            var accountInfo = _accountService.GetAccountInfo(criteria);
            var groupList = _groupService.GetListGroup(new GroupListCriteria { IsIt = SessionManager.CurrentSession.IsIT });

            return View(new EditUserViewModel
            {
                Username = accountInfo.Username,
                FirstName = accountInfo.FirstName,
                LastName = accountInfo.LastName,
                GroupID = accountInfo.GroupID,
                CustID = custId,
                GroupList = groupList
            });
        }

        /// <summary>
        /// Edit User Info.
        /// </summary>
        /// <param name="model">Edit user view model.</param>
        /// <returns>Action Result.</returns>
        [HttpPost]
        public ActionResult EditUser(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var criteria = new AccountUpdateCriteria
                {
                    CustId = model.CustID,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    GroupId = model.GroupID,
                    UserId = SessionManager.CurrentSession.CustID,
                    Ip = Remote.GetClientIPAddress(),
                };

                _accountService.AccountUpdate(criteria);

                ViewBag.Result = new ResultViewModel
                {
                    ErrorCode = criteria.ErrCode,
                    Message = criteria.ErrCode == 0 ? Lang.UpdateSucc : ErrorMessage(criteria.ErrCode)
                };
            }

            return EditUser(model.CustID);
        }

        #endregion Edit User

        #region Add User

        [HttpGet]
        [ITAuth]
        public ActionResult AddUser(string password)
        {
            return View(new AccountViewModel
                {
                    Password = password
                });
        }

        /// <summary>
        /// Add User information.
        /// </summary>
        /// <param name="model">Account view model.</param>
        /// <returns>Action Result.</returns>
        [HttpPost]
        [ITAuth]
        public ActionResult AddUser(AccountViewModel model)
        {
            if (ModelState.IsValid)
            {
                var criteria = new AccountInsCriteria
                    {
                        UserId = SessionManager.CurrentSession.CustID,
                        Ip = Remote.GetClientIPAddress(),
                        Password = Cryptography.CfsEnCode(model.Password),
                        CustId = model.CustID,
                        UserName = model.Username
                    };

                _accountService.AccountInsert(criteria);

                ViewBag.Result = new ResultViewModel
                {
                    ErrorCode = criteria.ErrCode,
                    Message = criteria.ErrCode == 0 ? Lang.AddUserSuccess : ErrorMessage(criteria.ErrCode)
                };
            }

            return AddUser(model.Password);
        }

        #endregion Add User

        #region Change Policy

        [HttpGet]
        public ActionResult ChangePolicyUser(int custId)
        {
            return View(new PolicyViewModel
            {
                CustId = custId
            });
        }

        [HttpPost]
        public ActionResult ListPolicyUser(UserPolicySelCriteria criteria)
        {
            criteria.LicenseeId = AppSettings.LicenseeID;

            return Json(new { List = _policyService.GetUserPolicyList(criteria) });
        }

        [HttpPost]
        public ActionResult ChangePolicyUser(int custId, string individualPolicy)
        {
            UpdatePolicy(individualPolicy);

            return ChangePolicyUser(custId);
        }

        [HttpGet]
        public ActionResult ChangePolicyMultiUser(string custIds)
        {
            return View(new PolicyMultiUserViewModel
            {
                CustIds = custIds
            });
        }

        [HttpPost]
        public ActionResult ListPolicyMultiUser(PolicyMultiUserCriteria criteria)
        {
            criteria.LicenseeId = AppSettings.LicenseeID;

            return Json(new { List = _policyService.GetPolicyMultiUserList(criteria) });
        }

        [HttpPost]
        public ActionResult ChangePolicyMultiUser(string custIds, string individualPolicy)
        {
            UpdatePolicy(individualPolicy);

            return ChangePolicyMultiUser(custIds);
        }

        /// <summary>
        /// Change user currency.
        /// </summary>
        /// <param name="custId">Customer Id.</param>
        /// <returns>Action Result.</returns>
        [HttpGet]
        public ActionResult ChangeCurrencyUser(int custId)
        {
            var criteria = new UserCurrencySelCriteria
            {
                UserId = custId,
                LicenseeId = AppSettings.LicenseeID
            };

            return View(new PermissionCurrencyViewModel
            {
                CustId = custId,
                ListPermissionCurrency = _permissionService.GetPermissionCurrencyList(criteria)
            });
        }

        /// <summary>
        /// Change Currency User.
        /// </summary>
        /// <param name="custId">Customer Id.</param>
        /// <param name="currencies">Currencies to change.</param>
        /// <returns>Action Result.</returns>
        [HttpPost]
        public ActionResult ChangeCurrencyUser(int custId, string currencies)
        {
            var doc = new XmlDocument();

            var root = doc.CreateElement("currencies");

            doc.AppendChild(root);

            if (!string.IsNullOrEmpty(currencies))
            {
                var currencyList = currencies.Split(';');

                foreach (string currency in currencyList)
                {
                    var transNode = doc.CreateElement("currency");

                    transNode.SetAttribute("currencyid", currency);

                    transNode.SetAttribute("status", "1");

                    root.AppendChild(transNode);
                }
            }

            var criteria = new UserCurrencyInsCriteria
            {
                ModifyBy = SessionManager.CurrentSession.CustID,
                UserId = custId,
                Ip = Remote.GetClientIPAddress(),
                CurrencyXML = doc.OuterXml,
                LicenseeId = AppSettings.LicenseeID
            };

            _permissionService.UpdatePermissionCurrency(criteria);

            ViewBag.Result = new ResultViewModel
            {
                ErrorCode = criteria.ErrCode,
                Message = criteria.ErrCode == 0 ? Lang.UpdatePermissionSucc : ErrorMessage(criteria.ErrCode)
            };

            return ChangeCurrencyUser(custId);
        }

        #endregion Change Policy

        #region Change Password

        [HttpGet]
        [ITAuth]
        public ActionResult ChangePassword(int custId, string userName, string password, string confirmPassword)
        {
            return View(new PasswordViewModel
                {
                    CustId = custId,
                    UserName = userName,
                    Password = password,
                    ConfirmPass = confirmPassword
                });
        }

        /// <summary>
        /// Change user password.
        /// </summary>
        /// <param name="model">Password view model.</param>
        /// <returns>Action Result.</returns>
        [HttpPost]
        [ITAuth]
        public ActionResult ChangePassword(PasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var criteria = new ChangePasswordCriteria
                    {
                        CustId = model.CustId,
                        UserName = model.UserName,
                        UserId = SessionManager.CurrentSession.CustID,
                        Ip = Remote.GetClientIPAddress(),
                        Password = Cryptography.CfsEnCode(model.Password)
                    };

                _accountService.AccountUpdatePassword(criteria);

                ViewBag.Result = new ResultViewModel
                {
                    ErrorCode = criteria.ErrCode,
                    Message = criteria.ErrCode == 0 ? Lang.ResetPassSuccess : ErrorMessage(criteria.ErrCode)
                };
            }

            return ChangePassword(model.CustId, model.UserName, model.Password, model.ConfirmPass);
        }

        #endregion Change Password

        [HttpGet]
        public ActionResult UserInGroup(int groupId)
        {
            return View(new UserManagementViewModel { GroupId = groupId });
        }

        #endregion User Management

        #region Group Management

        [HttpGet]
        public ActionResult Groups()
        {
            var serializer = new JavaScriptSerializer();
            ViewBag.IgnoredGroupNames = serializer.Serialize(ignoredGroupNames);
            return View();
        }

        [HttpPost]
        public ActionResult GroupDataList()
        {
            var criteria = new GroupListCriteria
            {
                IsIt = SessionManager.CurrentSession.IsIT,
                LicenseeId = AppSettings.LicenseeID
            };

            return Json(new { List = _groupService.GetListGroup(criteria) });
        }

        #region Add Group

        [HttpGet]
        public ActionResult AddGroup()
        {
            return View();
        }

        /// <summary>
        /// Add Group user.
        /// </summary>
        /// <param name="model">Group view model.</param>
        /// <returns>Action Result.</returns>
        [HttpPost]
        public ActionResult AddGroup(GroupViewModel model)
        {
            if (ModelState.IsValid)
            {
                var criteria = new GroupInfoInsCriteria
                    {
                        GroupName = model.GroupName,
                        GroupDesc = model.Description,
                        UserId = SessionManager.CurrentSession.CustID,
                        Ip = Remote.GetClientIPAddress(),
                        LicenseeId = AppSettings.LicenseeID
                    };

                _groupService.GroupInsert(criteria);

                ViewBag.Result = new ResultViewModel
                {
                    ErrorCode = criteria.ErrCode,
                    Message = criteria.ErrCode == 0 ? Lang.GroupAddSucc : ErrorMessage(criteria.ErrCode)
                };
            }

            return AddGroup();
        }

        #endregion Add Group

        #region Change Policy

        [HttpGet]
        public ActionResult ChangePolicyGroup(GroupPolicySelCriteria criteria)
        {
            return View(new PolicyViewModel
            {
                GroupId = criteria.GroupId
            });
        }

        [HttpPost]
        public ActionResult ListPolicyGroup(GroupPolicySelCriteria criteria)
        {
            return Json(new { List = _policyService.GetGroupPolicyList(criteria) });
        }

        /// <summary>
        /// Change Policy Group.
        /// </summary>
        /// <param name="groupId">Group Id value.</param>
        /// <param name="groupPolicy">Group Policy.</param>
        /// <returns>Action Result.</returns>
        [HttpPost]
        public ActionResult ChangePolicyGroup(int groupId, string groupPolicy)
        {
            var viewModel = new PolicyUpdateViewModel
            {
                UserId = SessionManager.CurrentSession.CustID,
                Ip = Remote.GetClientIPAddress(),
                GroupPolicys = groupPolicy
            };

            var criteria2 = GetProperPolicyUpdateCriteria(viewModel);

            _groupService.GroupUpdatePolicy(criteria2);

            ViewBag.Result = new ResultViewModel
            {
                ErrorCode = criteria2.ErrCode,
                Message = criteria2.ErrCode == 0 ? Lang.UpdateRolePolicySucc : ErrorMessage(criteria2.ErrCode)
            };

            return ChangePolicyGroup(new GroupPolicySelCriteria { GroupId = groupId });
        }

        #endregion Change Policy

        #region Edit Group

        /// <summary>
        /// Edit group.
        /// </summary>
        /// <param name="groupId">Group Id information.</param>
        /// <returns>Action Result.</returns>
        [HttpGet]
        public ActionResult EditGroup(int groupId)
        {
            var groupInfo = _groupService.GetGroupInfo(new GroupListCriteria
            {
                IsIt = SessionManager.CurrentSession.IsIT,
                GroupId = groupId
            });

            return View(new GroupViewModel
            {
                GroupId = groupInfo.GroupId,
                GroupName = groupInfo.GroupName,
                Description = groupInfo.GroupDesc
            });
        }

        /// <summary>
        /// Edit Group information.
        /// </summary>
        /// <param name="criteria">Group Criteria.</param>
        /// <returns>Action Result.</returns>
        [HttpPost]
        public ActionResult EditGroup(GroupInfoUpdCriteria criteria)
        {
            if (ModelState.IsValid)
            {
                criteria.UserId = SessionManager.CurrentSession.CustID;
                criteria.Ip = Remote.GetClientIPAddress();
                criteria.LicenseeId = AppSettings.LicenseeID;

                _groupService.GroupUpdate(criteria);

                ViewBag.Result = new ResultViewModel
                {
                    ErrorCode = criteria.ErrCode,
                    Message = criteria.ErrCode == 0 ? Lang.UpdateSucc : ErrorMessage(criteria.ErrCode)
                };
            }

            return EditGroup(Convert.ToInt32(criteria.GroupId));
        }

        #endregion Edit Group

        /// <summary>
        /// Delete Group.
        /// </summary>
        /// <param name="criteria">Group Criteria.</param>
        /// <returns>Action Result.</returns>
        [HttpPost]
        public ActionResult DeleteGroup(GroupInfoDelCriteria criteria)
        {
            criteria.UserId = SessionManager.CurrentSession.CustID;
            criteria.Ip = Remote.GetClientIPAddress();

            _groupService.GroupDelete(criteria);

            return Json(new
            {
                ErrorCode = criteria.ErrCode,
                Message = criteria.ErrCode == 0 ? string.Empty : ErrorMessage(criteria.ErrCode)
            });
        }

        #endregion Group Management

        #region Private Method

        /// <summary>
        /// Update Policy.
        /// </summary>
        /// <param name="individualPolicy">Individual Policy.</param>
        private void UpdatePolicy(string individualPolicy)
        {
            var criteria = new PolicyUpdateViewModel
            {
                UserId = SessionManager.CurrentSession.CustID,
                Ip = Remote.GetClientIPAddress(),
                MemPolicys = individualPolicy
            };

            var criteria2 = GetProperPolicyUpdateCriteria(criteria);

            _accountService.AccountUpdatePolicy(criteria2);

            ViewBag.Result = new ResultViewModel
            {
                ErrorCode = criteria2.ErrCode,
                Message = criteria2.ErrCode == 0 ? Lang.UpdateRolePolicySucc : ErrorMessage(criteria2.ErrCode)
            };
        }

        /// <summary>
        /// Get Proper Policy Update Criteria.
        /// </summary>
        /// <param name="criteria">Update Criteria.</param>
        /// <returns>Proper Update Criteria.</returns>
        private SimpleCriteriaBase GetProperPolicyUpdateCriteria(PolicyUpdateViewModel criteria)
        {
            if (string.IsNullOrEmpty(criteria.MemPolicys))
            {
                return new GroupPolicyUpdateCriteria
                {
                    GroupPolicys = criteria.GroupPolicys,
                    IP = criteria.Ip,
                    Language = criteria.Language,
                    UserID = criteria.UserId,
                    LicenseeId = AppSettings.LicenseeID
                };
            }

            return new UserPolicyUpdateCriteria
            {
                IP = criteria.Ip,
                Language = criteria.Language,
                MemPolicys = criteria.MemPolicys,
                UserID = criteria.UserId,
                LicenseeId = AppSettings.LicenseeID
            };
        }

        #endregion Private Method
    }
}