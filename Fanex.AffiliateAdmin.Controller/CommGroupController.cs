// --------------------------------------------------------------------------
// <copyright file="CommGroupController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Joseph.Pham" date="2013.9.9">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Controller
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.Script.Serialization;
    using Core.Configurations;
    using Core.Session;
    using Core.Utilities;
    using Criterias;
    using DTO;
    using Fanex.AffiliateAdmin.Core.Filter;
    using Fanex.AffiliateAdmin.Resources;
    using Fanex.AffiliateAdmin.Services;

    /// <summary>
    /// Commission group Controller.
    /// </summary>
    [FunctionAuth(Function = "[CommissionGroup]")]
    public partial class CommGroupController : BaseController
    {
        #region Services

        /// <summary>
        /// Service used to work with database.
        /// </summary>
        private readonly ICommGroupService _commGroupService;

        private readonly IExchangeRateService _exchangeRateService;

        #endregion Services

        #region Constructor

        public CommGroupController()
            : this(null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommGroupController" /> class.
        /// </summary>
        /// <param name="commGroupService">Commission group service.</param>
        /// <param name="exchangeRateService">Exchange rate service.</param>
        public CommGroupController(ICommGroupService commGroupService, IExchangeRateService exchangeRateService)
        {
            _commGroupService = commGroupService ?? new CommGroupService();
            _exchangeRateService = exchangeRateService ?? new ExchangeRateService();
        }

        #endregion Constructor

        /// <summary>
        /// Add commission group.
        /// </summary>
        /// <returns>Group Information.</returns>
        public ActionResult Add()
        {
            var serializer = new JavaScriptSerializer();
            ViewBag.ProductIDList = serializer.Serialize(AppSettings.Products.Where(t => t.IsEnabled).Select(x => x.ProductID).ToArray());
            var currency = SessionManager.CurrentSession.CurrencyList.First();

            var criteria = new GetDefaultPspExchangeCriteria
                {
                    CurrencyCode = currency.CurrencyCode
                };

            var defaultPspExchange = _exchangeRateService.GetDefaultPspExchange(criteria);

            var model = new CommGroupInfo
                {
                    DepositPercent =
                        (double)(defaultPspExchange.DepositPercent.HasValue ? defaultPspExchange.DepositPercent : 0),
                    WithdrawalPercent =
                        (double)(defaultPspExchange.WithdrawalPercent.HasValue ? defaultPspExchange.WithdrawalPercent : 0)
                };

            return View(model);
        }

        /// <summary>
        /// Update commission group.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>Return error message (if any).</returns>
        [HttpPost]
        public ActionResult Add(CommGroupInsertCriteria criteria)
        {
            CopyTierSettingsForAllProducts(criteria);

            criteria.UserId = SessionManager.CurrentSession.CustID;

            criteria.IP = Remote.GetClientIPAddress();

            criteria.GroupName = criteria.GroupName.Encode();

            criteria.Description = criteria.Description.Encode();

            _commGroupService.ExecuteOutput(criteria);

            var errorMsg = string.Empty;

            if (criteria.ErrCode == 9999)
            {
                errorMsg = criteria.ErrMsg;
            }
            else if (criteria.ErrCode != 0)
            {
                errorMsg = Common.GetErrorMsg(criteria.ErrCode);
                if (errorMsg.Contains("{0}"))
                {
                    errorMsg = string.Format(errorMsg, string.IsNullOrEmpty(criteria.ErrMsg) ? criteria.GroupId.ToString() : criteria.ErrMsg);
                }
            }

            return Json(GetResultViewModel(criteria.ErrCode, Lang.AddSucc, errorMsg));
        }

        /// <summary>
        /// Delete a group.
        /// </summary>
        /// <param name="groupid">ID of group.</param>
        /// <returns>Error message (if any).</returns>
        public string Delete(int groupid)
        {
            var criteria = new CommGroupDeleteCriteria
            {
                GroupId = groupid,
                UserId = SessionManager.CurrentSession.CustID,
                IP = Remote.GetClientIPAddress(),
            };

            _commGroupService.ExecuteOutput(criteria);
            var errmsg = Common.GetErrorMsg(criteria.ErrCode);
            if (errmsg.Contains("{0}"))
            {
                errmsg = string.Format(errmsg, criteria.GroupId);
            }

            return errmsg;
        }

        /// <summary>
        /// Edit commission group.
        /// </summary>
        /// <param name="groupid">Commission group Id.</param>
        /// <returns>Group Information.</returns>
        public ActionResult Edit(int groupid)
        {
            var serializer = new JavaScriptSerializer();
            ViewBag.ProductIDList = serializer.Serialize(AppSettings.Products.Select(x => x.ProductID).ToArray());

            var group = _commGroupService.GetListData<CommGroupInfo>(new CommGroupInfoCriteria()
            {
                GroupId = groupid
            }).FirstOrDefault();

            if (group == null)
            {
                return Content("Group is null");
            }

            group.GroupSettings = _commGroupService.GetListData<GroupSetting>(new GroupSettingCriteria()
            {
                GroupId = groupid
            });

            group.DeductionSettings = _commGroupService.GetListData<DeductionSetting>(new DeductionSettingCriteria()
            {
                GroupIDList = groupid.ToString(CultureInfo.InvariantCulture)
            });

            group.GroupName = group.GroupName.Decode();

            group.Description = group.Description.Decode();

            return View("Edit", group);
        }

        /// <summary>
        /// Update commission group.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>Return error message (if any).</returns>
        [HttpPost]
        public ActionResult Edit(CommGroupUpdateCriteria criteria)
        {
            var productCount = AppSettings.Products.Count();

            if (criteria.CopyForAllProducts)
            {
                criteria.MinAmountList = string.Concat(Enumerable.Repeat(criteria.MinAmountList, productCount));
                criteria.MaxAmountList = string.Concat(Enumerable.Repeat(criteria.MaxAmountList, productCount));
                criteria.MinQMList = string.Concat(Enumerable.Repeat(criteria.MinQMList, productCount));
                criteria.MaxQMList = string.Concat(Enumerable.Repeat(criteria.MaxQMList, productCount));
                criteria.TierList = string.Concat(Enumerable.Repeat(criteria.TierList, productCount));
                criteria.CommissionList = string.Concat(Enumerable.Repeat(criteria.CommissionList, productCount));
            }

            criteria.UserId = SessionManager.CurrentSession.CustID;

            criteria.IP = Remote.GetClientIPAddress();

            criteria.GroupName = criteria.GroupName.Encode();

            criteria.Description = criteria.Description.Encode();

            _commGroupService.ExecuteOutput(criteria);

            var errorMsg = string.Empty;

            if (criteria.ErrCode == 9999)
            {
                errorMsg = criteria.ErrMsg;
            }
            else if (criteria.ErrCode != 0)
            {
                errorMsg = Common.GetErrorMsg(criteria.ErrCode);
                if (errorMsg.Contains("{0}"))
                {
                    errorMsg = string.Format(errorMsg, string.IsNullOrEmpty(criteria.ErrMsg) ? criteria.GroupId.ToString() : criteria.ErrMsg);
                }
            }

            return Json(GetResultViewModel(criteria.ErrCode, Lang.UpdateSucc, errorMsg));
        }

        /// <summary>
        /// Get currency notification if multiplied > 1.
        /// </summary>
        /// <param name="currencyid">Currency ID.</param>
        /// <returns>Notification Message.</returns>
        public string GetCurrencyNotification(int currencyid)
        {
            var currency = SessionManager.CurrentSession.CurrencyList.Where(x => x.CurrencyID == currencyid).FirstOrDefault();

            if (currency == null || currency.MultipliedBy == 1)
            {
                return string.Empty;
            }

            return Common.GetCurrencyNotification(currency.CurrencyCode, currency.MultipliedBy);
        }

        public ActionResult GetDefaultPspExchange(GetDefaultPspExchangeCriteria criteria)
        {
            return Json(_exchangeRateService.GetDefaultPspExchange(criteria), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Load data.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>List of data.</returns>
        public ActionResult GetList(CommGroupCriteria criteria)
        {
            criteria.UserId = SessionManager.CurrentSession.CustID;
            criteria.PageIndex = criteria.PageIndex == 0 ? 1 : criteria.PageIndex;
            criteria.PageSize = criteria.PageSize == 0 ? AppSettings.PageSize : criteria.PageSize;

            var groups = GetGridData(criteria);

            return Json(new
            {
                List = groups,
                CurrentPage = criteria.PageIndex - 1,
                ItemsPerPage = criteria.PageSize,
                TotalItem = criteria.NRows,
                HasCashBackDeduction = groups.Any(t => t.HasCashback > 0)
            });
        }

        /// <summary>
        /// Index action.
        /// </summary>
        /// <param name="groupName">Group name.</param>
        /// <returns>Default model.</returns>
        [HttpGet]
        public ActionResult Index(string groupName)
        {
            ViewBag.IsIT = SessionManager.CurrentSession.IsIT;
            ViewBag.GroupName = groupName;
            return View();
        }

        public ActionResult ViewSetting(CommGroupSelByIdCriteria criteria)
        {
            criteria.UserId = CurrentSession.CustID;
            var groups = _commGroupService.GetById(criteria);
            var date = criteria.Date == null ? SystemTimeService.GetSystemDateTime() : criteria.Date.Value;
            BuildGroupData(groups, date, criteria.GroupID.ToString());
            return View(groups.FirstOrDefault());
        }

        /// <summary>
        /// Build Commission Groups data.
        /// </summary>
        /// <param name="groups">Commission groups.</param>
        /// <param name="date">Search date.</param>
        /// <param name="groupIDList">List group id.</param>
        private void BuildGroupData(IEnumerable<CommGroup> groups, DateTime date, string groupIDList)
        {
            var detailCriteria = new TierSettingCriteria { GroupIDList = groupIDList, Date = date };

            var settingCriteria = new DeductionSettingCriteria { GroupIDList = groupIDList, Date = date };

            var tierSettings = _commGroupService.GetTierSettings(detailCriteria);

            var deductionSettings = _commGroupService.GetDeductionSettings(settingCriteria);

            foreach (var group in groups)
            {
                var details = tierSettings.Where(x => x.GroupId == group.GroupID).ToList();

                var products = new Dictionary<string, object>();

                GetCommGroupSettings(details, products, group, deductionSettings);
            }
        }

        /// <summary>
        /// Get commission group settings.
        /// </summary>
        /// <param name="tierSettings">Tier settings.</param>
        /// <param name="products">Product list.</param>
        /// <param name="group">Commission group.</param>
        /// <param name="deductionSettings">Deduction setting.</param>
        private void GetCommGroupSettings(
            List<CommGroupTierSetting> tierSettings,
            Dictionary<string, object> products,
            CommGroup group,
            IEnumerable<DeductionSetting> deductionSettings)
        {
            var allDeductions = AppSettings.DeductionTypes;

            foreach (var detail in tierSettings)
            {
                var id = detail.ProductID.ToString();
                if (!products.ContainsKey(id))
                {
                    products.Add(
                        id,
                        new
                            {
                                NumOfItems = tierSettings.Count(x => x.GroupId == @group.GroupID && x.ProductID == detail.ProductID),
                                Deductions = GetDeductionSettings(deductionSettings, allDeductions, @group, detail)
                            });
                }
            }

            @group.IsAllowedToDelete = @group.NoOfAffiliate == 0 && @group.CurrencyID != 0;
            @group.AddProducts(products.ToDictionary(x => x.Key, x => x.Value));
            @group.AddDetails(tierSettings.ToList());

            if (@group.HasCashback > 0)
            {
                var deduction = new Dictionary<string, decimal>();
                var deductions = allDeductions.Where(x => x.ProductID == 0);
                if (deductions.Count() > 0)
                {
                    var settings = deductionSettings.Where(x => x.GroupID == @group.GroupID).ToList();

                    foreach (var item in deductions)
                    {
                        var setting = settings.Where(x => x.DeductionID == item.DTypeID).FirstOrDefault();
                        deduction.Add(Lang.ResourceManager.GetString(item.DTypeName.Replace(" ", string.Empty)), setting == null ? -1 : setting.DeductedPercentage);
                    }
                }

                @group.AddGeneralDeductions(deduction);
            }
        }

        /// <summary>
        /// Copy Tier Settings For All Enabled Products.
        /// </summary>
        /// <param name="criteria">Criteria input.</param>
        private void CopyTierSettingsForAllProducts(CommGroupInsertCriteria criteria)
        {
            var productCount = AppSettings.Products.Count();

            if (criteria.CopyForAllProducts)
            {
                criteria.MinAmountList = string.Concat(Enumerable.Repeat(criteria.MinAmountList, productCount));
                criteria.MaxAmountList = string.Concat(Enumerable.Repeat(criteria.MaxAmountList, productCount));
                criteria.MinQMList = string.Concat(Enumerable.Repeat(criteria.MinQMList, productCount));
                criteria.MaxQMList = string.Concat(Enumerable.Repeat(criteria.MaxQMList, productCount));
                criteria.TierList = string.Concat(Enumerable.Repeat(criteria.TierList, productCount));
                criteria.CommissionList = string.Concat(Enumerable.Repeat(criteria.CommissionList, productCount));
            }
        }

        /// <summary>
        /// Get Deduction Settings.
        /// </summary>
        /// <param name="deductionSettings">Deduction Setting.</param>
        /// <param name="allDeductions">Deduction Type.</param>
        /// <param name="group">Commission Group.</param>
        /// <param name="detail">Commission Group Detail.</param>
        /// <returns>Deduction settings.</returns>
        private Dictionary<string, decimal> GetDeductionSettings(
            IEnumerable<DeductionSetting> deductionSettings,
            IEnumerable<DeductionType> allDeductions,
            CommGroup group,
            CommGroupTierSetting detail)
        {
            var deduction = new Dictionary<string, decimal>();
            var deductions = allDeductions.Where(x => x.ProductID == detail.ProductID);

            if (deductions.Count() > 0)
            {
                var settings = deductionSettings.Where(x => x.GroupID == group.GroupID);

                foreach (var item in deductions)
                {
                    var setting = settings.Where(x => x.DeductionID == item.DTypeID).FirstOrDefault();
                    deduction.Add(Lang.ResourceManager.GetString(item.DTypeName.Replace(" ", string.Empty)), setting == null ? -1 : setting.DeductedPercentage);
                }
            }

            return deduction;
        }

        /// <summary>
        /// Get Grid Data.
        /// </summary>
        /// <param name="criteria">Input Criteria.</param>
        /// <returns>List Commission Group.</returns>
        private IEnumerable<CommGroup> GetGridData(CommGroupCriteria criteria)
        {
            var groups = _commGroupService.GetListData<CommGroup>(criteria);

            if (!string.IsNullOrEmpty(criteria.GroupIDList) && criteria.GroupIDList.IndexOf(',') == 0)
            {
                criteria.GroupIDList = criteria.GroupIDList.Substring(1);
            }

            BuildGroupData(groups, criteria.Date, criteria.GroupIDList);

            return groups;
        }
    }
}