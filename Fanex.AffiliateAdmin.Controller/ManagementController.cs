namespace Fanex.AffiliateAdmin.Controller
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using System.Xml;
    using Core.Enumerations;
    using Core.Services;
    using Core.Services.Interface;
    using Fanex.AffiliateAdmin.Core.Configurations;
    using Fanex.AffiliateAdmin.Core.Filter;
    using Fanex.AffiliateAdmin.Core.Session;
    using Fanex.AffiliateAdmin.Core.Utilities;
    using Fanex.AffiliateAdmin.Criterias;
    using Fanex.AffiliateAdmin.Criterias.Account;
    using Fanex.AffiliateAdmin.DTO;
    using Fanex.AffiliateAdmin.Models;
    using Fanex.AffiliateAdmin.Resources;
    using Fanex.AffiliateAdmin.Services;
    using Fanex.Common;
    using Services.Helpers;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [FunctionAuth(Function = FunctionPolicyNames.UserControl)]
    [AuthorizeRestriction(AppMode.Self)]
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
            ViewBag.CanUpdateData = SessionManager.CurrentSession.HasHostPolicy(FunctionPolicyNames.UserControl);
            ViewBag.IsIt = SessionManager.CurrentSession.IsIT;
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
        [FunctionAuth(Function = FunctionPolicyNames.UserControl, IsHostPolicy = true)]
        [HttpGet]
        public ActionResult EditUser(int custId)
        {
            var account = GetAccount(custId);
            if (account.IsIT)
            {
                return RedirectToAccessDenied();
            }

            var groupListCriteria = new GroupListCriteria
            {
                IsIt = SessionManager.CurrentSession.IsIT,
                LicenseeId = AppSettings.LicenseeID
            };

            return View(new EditUserViewModel
            {
                Username = account.Username,
                GroupID = account.GroupID,
                CustID = custId,
                GroupList = _groupService.GetListGroup(groupListCriteria, true)
            });
        }

        /// <summary>
        /// Edit User Info.
        /// </summary>
        /// <param name="model">Edit user view model.</param>
        /// <returns>Action Result.</returns>
        [FunctionAuth(Function = FunctionPolicyNames.UserControl, IsHostPolicy = true)]
        [HttpPost]
        public ActionResult EditUser(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var criteria = new AccountUpdateCriteria
                {
                    CustId = model.CustID,
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

        [FunctionAuth(Function = FunctionPolicyNames.UserControl, IsHostPolicy = true)]
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
        [FunctionAuth(Function = FunctionPolicyNames.UserControl, IsHostPolicy = true)]
        [HttpPost]
        [ITAuth]
        public ActionResult AddUser(AccountViewModel model)
        {
            if (ModelState.IsValid)
            {
                var salt = Cryptography.CreateSalt();
                var criteria = new AccountInsCriteria
                {
                    UserId = SessionManager.CurrentSession.CustID,
                    Password = Cryptography.CreateHash(model.Password, salt),
                    CustId = model.CustID,
                    UserName = model.Username,
                    UserSalt = salt,
                    LicenseeId = AppSettings.LicenseeID
                };

                _accountService.AccountInsert(criteria);

                if (criteria.ErrCode == 0)
                {
                    var logDetails = $"NEW AFFILIATE Admin custid: {criteria.CustId} username: {model.Username}";
                    AffWriteLogHelper.WriteLog("UserControl", criteria.GetSettingKey(), logDetails, criteria.CustId);
                }

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
            ViewBag.CanUpdateData = SessionManager.CurrentSession.HasHostPolicy(FunctionPolicyNames.UserControl);
            var account = GetAccount(custId);
            return View(new PolicyViewModel
            {
                CustId = custId,
                Menus = SubMenu.GetPermissionMenus(account.IsIT)
            });
        }

        [HttpPost]
        public ActionResult ListPolicyUser(UserPolicySelCriteria criteria)
        {
            criteria.LicenseeId = AppSettings.LicenseeID;

            return Json(new { List = _policyService.GetUserPolicyList(criteria) });
        }

        [FunctionAuth(Function = FunctionPolicyNames.UserControl, IsHostPolicy = true)]
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

        [FunctionAuth(Function = FunctionPolicyNames.UserControl, IsHostPolicy = true)]
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
            var account = GetAccount(custId);
            if (account.IsIT)
            {
                return RedirectToAccessDenied();
            }

            ViewBag.CanUpdateData = SessionManager.CurrentSession.HasHostPolicy(FunctionPolicyNames.UserControl);
            var criteria = new UserCurrencySelCriteria
            {
                UserId = custId,
                LicenseeId = AppSettings.LicenseeID
            };
            return View(new PermissionCurrencyViewModel
            {
                CustId = custId,
                ListPermissionCurrency = _permissionService.GetPermissionCurrencyList(criteria),
                Menus = SubMenu.GetPermissionMenus(account.IsIT)
            });
        }

        /// <summary>
        /// Change Currency User.
        /// </summary>
        /// <param name="custId">Customer Id.</param>
        /// <param name="currencies">Currencies to change.</param>
        /// <returns>Action Result.</returns>
        [FunctionAuth(Function = FunctionPolicyNames.UserControl, IsHostPolicy = true)]
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

        [FunctionAuth(Function = FunctionPolicyNames.UserControl, IsHostPolicy = true)]
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
        [FunctionAuth(Function = FunctionPolicyNames.UserControl, IsHostPolicy = true)]
        [HttpPost]
        [ITAuth]
        public ActionResult ChangePassword(PasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var salt = Cryptography.CreateSalt();
                var criteria = new ChangePasswordCriteria
                {
                    CustId = model.CustId,
                    LicenseeId = AppSettings.LicenseeID,
                    UserPwd = Cryptography.CreateHash(model.Password, salt),
                    UserSalt = salt
                };

                _accountService.AccountUpdatePassword(criteria);

                if (criteria.ErrCode == 0)
                {
                    AffWriteLogHelper.WriteLog("Password", criteria.GetSettingKey(), $"Changed password CustID: {model.CustId}");
                }

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
            ViewBag.CanUpdateData =
                SessionManager.CurrentSession.HasHostPolicy(FunctionPolicyNames.UserControl) &&
                _groupService.IsItGroup(groupId);
            return View(new UserManagementViewModel { GroupId = groupId });
        }

        #endregion User Management

        #region Group Management

        [HttpGet]
        public ActionResult Groups()
        {
            ViewBag.CanUpdateData = SessionManager.CurrentSession.HasHostPolicy(FunctionPolicyNames.UserControl);
            ViewBag.IgnoredGroupNames = ignoredGroupNames.ToJson();
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

            return Json(new { List = _groupService.GetListGroup(criteria, !SessionManager.CurrentSession.IsIT) });
        }

        #region Add Group

        [FunctionAuth(Function = FunctionPolicyNames.UserControl, IsHostPolicy = true)]
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
        [FunctionAuth(Function = FunctionPolicyNames.UserControl, IsHostPolicy = true)]
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
            ViewBag.CanUpdateData = SessionManager.CurrentSession.HasHostPolicy(FunctionPolicyNames.UserControl);
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
        [FunctionAuth(Function = FunctionPolicyNames.UserControl, IsHostPolicy = true)]
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
        [FunctionAuth(Function = FunctionPolicyNames.UserControl, IsHostPolicy = true)]
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
        [FunctionAuth(Function = FunctionPolicyNames.UserControl, IsHostPolicy = true)]
        [HttpPost]
        public ActionResult EditGroup(GroupInfoUpdCriteria criteria)
        {
            if (ModelState.IsValid)
            {
                criteria.UserId = SessionManager.CurrentSession.CustID;
                criteria.LicenseeId = AppSettings.LicenseeID;

                _groupService.GroupUpdate(criteria);

                if (criteria.ErrCode == 0)
                {
                    AffWriteLogHelper.WriteLog("UserControl", criteria.GetSettingKey(), $"[AFFILIATE] Update groupinfo: {criteria.GroupId}");
                }

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
        [FunctionAuth(Function = FunctionPolicyNames.UserControl, IsHostPolicy = true)]
        [HttpPost]
        public ActionResult DeleteGroup(GroupInfoDelCriteria criteria)
        {
            criteria.LicenseeId = AppSettings.LicenseeID;
            _groupService.GroupDelete(criteria);

            if (criteria.ErrCode == 0)
            {
                AffWriteLogHelper.WriteLog("UserControl", criteria.GetSettingKey(), $"[AFFILIATE] Delete GroupID: {criteria.GroupId}");
            }

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

        private Account GetAccount(int custId)
        {
            var criteria = new SelByCustIDCriteria
            {
                CustId = custId
            };

            return _accountService.GetAccountInfo(criteria);
        }

        #endregion Private Method
    }
}