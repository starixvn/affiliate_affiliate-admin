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
    using System.Web.Mvc;
    using System.Xml;
    using Common;
    using Core.Session;
    using Core.Utilities;
    using Criterias;
    using Fanex.AffiliateAdmin.Core.Filter;
    using Models;
    using Resources;
    using Services.Interface;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [FunctionAuth(Function = "[UserControl]")]
    public class ManagementController : BaseController
    {
        #region Services

        private readonly IAccountService _accountService;

        private readonly IGroupService _groupService;

        private readonly IPolicyService _policyService;

        private readonly IPermissionCurrencyService _permissionService;

        #endregion Services

        #region Constructor

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
            _accountService = accountService;
            _groupService = groupService;
            _policyService = policyService;
            _permissionService = permissionService;
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
        public ActionResult Users(UserCriteria criteria)
        {
            criteria.IsIt = SessionManager.CurrentSession.IsIT;
            criteria.Type = "ByGroup";

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
            var criteria = new UserCriteria
            {
                CustId = custId,
                IsIt = SessionManager.CurrentSession.IsIT,
                Type = "ByCustId"
            };

            var accountInfo = _accountService.GetAccountInfo(criteria);
            var groupList = _groupService.GetListGroup(new GroupListCriteria { IsIt = SessionManager.CurrentSession.IsIT });

            return View(new AccountViewModel
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
        /// <param name="criteria">Account Criteria.</param>
        /// <returns>Action Result.</returns>
        [HttpPost]
        public ActionResult EditUser(AccountCriteria criteria)
        {
            if (ModelState.IsValid)
            {
                criteria.UserId = SessionManager.CurrentSession.CustID;
                criteria.Ip = Remote.GetClientIPAddress();
                criteria.Type = "Edit";

                _accountService.AccountUpdate(criteria);

                ViewBag.Result = new ResultViewModel
                {
                    ErrorCode = criteria.ErrCode,
                    Message = criteria.ErrCode == 0 ? Lang.UpdateSucc : ErrorMessage(criteria.ErrCode)
                };
            }

            return EditUser(criteria.CustId);
        }
        #endregion Edit User

        #region Add User
        [HttpGet]
        public ActionResult AddUser()
        {
            var groupList = _groupService.GetListGroup(new GroupListCriteria { IsIt = SessionManager.CurrentSession.IsIT });

            return View(new AccountViewModel { GroupList = groupList });
        }

        /// <summary>
        /// Add User information.
        /// </summary>
        /// <param name="criteria">Account Criteria.</param>
        /// <returns>Action Result.</returns>
        [HttpPost]
        public ActionResult AddUser(AccountCriteria criteria)
        {
            if (ModelState.IsValid)
            {
                criteria.UserId = SessionManager.CurrentSession.CustID;
                criteria.Ip = Remote.GetClientIPAddress();
                criteria.Type = "Add";
                criteria.Password = Cryptography.CfsEnCode(criteria.Password);

                _accountService.AccountInsert(criteria);

                ViewBag.Result = new ResultViewModel
                {
                    ErrorCode = criteria.ErrCode,
                    Message = criteria.ErrCode == 0 ? Lang.AddUserSuccess : ErrorMessage(criteria.ErrCode)
                };
            }

            return AddUser();
        }
        #endregion Add User

        #region Change Status
        /// <summary>
        /// Change Status.
        /// </summary>
        /// <param name="criteria">Account Criteria.</param>
        /// <returns>Action Result.</returns>
        [HttpPost]
        public ActionResult ChangeStatus(AccountCriteria criteria)
        {
            criteria.UserId = SessionManager.CurrentSession.CustID;
            criteria.Ip = Remote.GetClientIPAddress();
            criteria.Type = "Status";

            _accountService.AccountUpdateStatus(criteria);

            return Json(new
            {
                ErrorCode = criteria.ErrCode,
                Message = criteria.ErrCode == 0 ? Lang.UpdateStaSucc : ErrorMessage(criteria.ErrCode)
            });
        }
        #endregion Change Status

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
        public ActionResult ListPolicyUser(PolicyCriteria criteria)
        {
            criteria.IsUser = true;

            return Json(new { List = _policyService.GetPolicyList(criteria) });
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
            var criteria = new PermissionCurrencyCriteria
            {
                UserId = custId,
                Type = CriteriaType.Select
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

            var criteria = new PermissionCurrencyCriteria
            {
                ModifyBy = SessionManager.CurrentSession.CustID,
                UserId = custId,
                Type = CriteriaType.Insert,
                Ip = Remote.GetClientIPAddress(),
                CurrencyXML = doc.OuterXml
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
        public ActionResult ChangePassword(int custId, string userName)
        {
            return View(new PasswordViewModel { CustId = custId, UserName = userName });
        }

        /// <summary>
        /// Change user password.
        /// </summary>
        /// <param name="criteria">Change Password Criteria.</param>
        /// <returns>Action Result.</returns>
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordCriteria criteria)
        {
            if (ModelState.IsValid)
            {
                criteria.UserId = SessionManager.CurrentSession.CustID;
                criteria.Ip = Remote.GetClientIPAddress();
                criteria.Password = Cryptography.CfsEnCode(criteria.Password);

                _accountService.AccountUpdatePassword(criteria);

                ViewBag.Result = new ResultViewModel
                {
                    ErrorCode = criteria.ErrCode,
                    Message = criteria.ErrCode == 0 ? Lang.ResetPassSuccess : ErrorMessage(criteria.ErrCode)
                };
            }

            return ChangePassword(criteria.CustId, criteria.UserName);
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
            return View();
        }

        [HttpPost]
        public ActionResult GroupDataList()
        {
            GroupListCriteria criteria = new GroupListCriteria
            {
                IsIt = SessionManager.CurrentSession.IsIT
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
        /// <param name="criteria">Group Criteria.</param>
        /// <returns>Action Result.</returns>
        [HttpPost]
        public ActionResult AddGroup(GroupCriteria criteria)
        {
            if (ModelState.IsValid)
            {
                criteria.UserId = SessionManager.CurrentSession.CustID;
                criteria.Ip = Remote.GetClientIPAddress();
                criteria.Type = "Add";

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
        public ActionResult ChangePolicyGroup(PolicyCriteria criteria)
        {
            return View(new PolicyViewModel
            {
                GroupId = criteria.GroupId
            });
        }

        [HttpPost]
        public ActionResult ListPolicyGroup(PolicyCriteria criteria)
        {
            criteria.IsUser = false;

            return Json(new { List = _policyService.GetPolicyList(criteria) });
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
            var criteria = new PolicyUpdateCriteria
            {
                UserId = SessionManager.CurrentSession.CustID,
                Ip = Remote.GetClientIPAddress(),
                GroupPolicys = groupPolicy
            };

            _groupService.GroupUpdatePolicy(criteria);

            ViewBag.Result = new ResultViewModel
            {
                ErrorCode = criteria.ErrCode,
                Message = criteria.ErrCode == 0 ? Lang.UpdateRolePolicySucc : ErrorMessage(criteria.ErrCode)
            };

            return ChangePolicyGroup(new PolicyCriteria { GroupId = groupId });
        }
        #endregion ChangePolicy

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
        public ActionResult EditGroup(GroupCriteria criteria)
        {
            if (ModelState.IsValid)
            {
                criteria.UserId = SessionManager.CurrentSession.CustID;
                criteria.Ip = Remote.GetClientIPAddress();
                criteria.Type = "Edit";

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
        public ActionResult DeleteGroup(GroupCriteria criteria)
        {
            criteria.UserId = SessionManager.CurrentSession.CustID;
            criteria.Ip = Remote.GetClientIPAddress();
            criteria.Type = "Delete";

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
            var criteria = new PolicyUpdateCriteria
            {
                UserId = SessionManager.CurrentSession.CustID,
                Ip = Remote.GetClientIPAddress(),
                MemPolicys = individualPolicy
            };

            _accountService.AccountUpdatePolicy(criteria);

            ViewBag.Result = new ResultViewModel
            {
                ErrorCode = criteria.ErrCode,
                Message = criteria.ErrCode == 0 ? Lang.UpdateRolePolicySucc : ErrorMessage(criteria.ErrCode)
            };
        }
        #endregion Private Method
    }
}
