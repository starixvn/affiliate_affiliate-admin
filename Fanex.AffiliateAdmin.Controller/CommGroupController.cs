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
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;
    using System.Web.Script.Serialization;
    using Core.Configurations;
    using Core.Session;
    using Core.Utilities;
    using Criterias;
    using DTO;
    using Fanex.AffiliateAdmin.Core.Filter;
    using Fanex.AffiliateAdmin.Resources;
    using Services.Interface;

    /// <summary>
    /// Commission group Controller.
    /// </summary>
    [HasCurrencyPermission]
    public partial class CommGroupController : BaseController
    {
        #region Services
        /// <summary>
        /// Service used to work with database.
        /// </summary>
        private readonly ICommGroupService _service;

        #endregion Services

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="CommGroupController" /> class.
        /// </summary>
        /// <param name="service">Commission group service.</param>        
        public CommGroupController(ICommGroupService service)
        {
            _service = service;
        }
        #endregion Constructor

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

        /// <summary>
        /// Load data.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>List of data.</returns>
        public ActionResult GetList(CommGroupCriteria criteria)
        {
            criteria.UserId = SessionManager.CurrentSession.CustID;

            var groups = GetGridData(criteria);

            return Json(new
            {                
                List = groups,
                CurrentPage = criteria.PageIndex - 1,
                ItemsPerPage = criteria.PageSize,
                TotalItem = criteria.NRows,
                HasCashBackDeduction = AppSettings.DeductionTypes.Any(x => x.ProductID == 0)
            });
        }

        public ActionResult ViewSetting(CommGroupSelByIdCriteria criteria)
        {
            criteria.UserId = CurrentSession.CustID;
            var groups = _service.GetById(criteria);
            BuildGroupData(groups, DateTime.Now, criteria.GroupID.ToString());
            ViewBag.HasCashBackDeduction = AppSettings.DeductionTypes.Any(x => x.ProductID == 0);
            return View(groups.FirstOrDefault());
        }

        /// <summary>
        /// View of Add/Update commission group.
        /// </summary>
        /// <param name="groupid">ID of group.</param>
        /// <returns>Group Information.</returns>
        public ActionResult Add(int? groupid)
        {
            var serializer = new JavaScriptSerializer();
            ViewBag.ProductIDList = serializer.Serialize(AppSettings.Products.Select(x => x.ProductID).ToArray());
            ViewBag.HasCashbackDeduction = AppSettings.DeductionTypes.Any(x => x.ProductID == 0);

            if (groupid.HasValue)
            {
                var group = _service.GetListData<CommGroupInfo>(new CommGroupInfoCriteria()
                {
                    GroupId = groupid.Value
                }).First();

                group.GroupSettings = _service.GetListData<GroupSetting>(new GroupSettingCriteria()
                {
                    GroupId = groupid.Value
                });

                group.DeductionSettings = _service.GetListData<DeductionSetting>(new DeductionSettingCriteria()
                {
                    GroupIDList = groupid.Value.ToString()
                });

                return View(group);
            }

            return View();
        }

        /// <summary>
        /// Add or update commission group.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>Return error message (if any).</returns>
        [HttpPost]
        public ActionResult Add(CommGroupActionCriteria criteria)
        {
            criteria.CriteriaType = criteria.GroupId.HasValue ? CriteriaType.Update : CriteriaType.Insert;
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

            _service.ExecuteOutput(criteria);

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
                    errorMsg = string.Format(errorMsg, string.IsNullOrEmpty(criteria.ErrMsg) ? (criteria.GroupId ?? 0).ToString() : criteria.ErrMsg);
                }
            }

            return Json(GetResultViewModel(criteria.ErrCode, criteria.CriteriaType == CriteriaType.Insert ? Lang.AddSucc : Lang.UpdateSucc, errorMsg));
        }

        /// <summary>
        /// Delete a group.
        /// </summary>
        /// <param name="groupid">ID of group.</param>
        /// <returns>Error message (if any).</returns>
        public string Delete(int groupid)
        {            
            var criteria = new CommGroupActionCriteria
            {
                CriteriaType = CriteriaType.Delete,
                GroupId = groupid,
                UserId = SessionManager.CurrentSession.CustID,
                IP = Remote.GetClientIPAddress(),
            };
            
            _service.ExecuteOutput(criteria);
            var errmsg = Common.GetErrorMsg(criteria.ErrCode);
            if (errmsg.Contains("{0}"))
            {
                errmsg = string.Format(errmsg, criteria.GroupId);
            }

            return errmsg;     
        }        

        /// <summary>
        /// Get Deduction Settings.
        /// </summary>
        /// <param name="deductionSettings">Deduction Setting.</param>
        /// <param name="allDeductions">Deduction Type.</param>
        /// <param name="group">Commission Group.</param>
        /// <param name="detail">Commission Group Detail.</param>
        /// <returns>Deduction settings.</returns>
        private static Dictionary<string, decimal> GetDeductionSettings(
            IEnumerable<DeductionSetting> deductionSettings, 
            IEnumerable<DeductionType> allDeductions, 
            CommGroup group, 
            CommGroupDetail detail)
        {
            var deduction = new Dictionary<string, decimal>();
            var deductions = allDeductions.Where(x => x.ProductID == detail.ProductID);

            if (deductions.Count() > 0)
            {
                var settings = deductionSettings.Where(x => x.GroupID == group.GroupID);

                foreach (var item in deductions)
                {
                    var setting = settings.Where(x => x.DeductionID == item.DTypeID).FirstOrDefault();
                    deduction.Add(item.DTypeName, setting == null ? -1 : setting.DeductedPercentage);
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
            var groups = _service.GetListData<CommGroup>(criteria);
            
            if (criteria.GroupIDList.IndexOf(',') == 0)
            {
                criteria.GroupIDList = criteria.GroupIDList.Substring(1);
            }

            BuildGroupData(groups, criteria.Date, criteria.GroupIDList);

            return groups;
        }

        /// <summary>
        /// Build Commission Groups data.
        /// </summary>
        /// <param name="groups">Commission groups.</param>
        /// <param name="date">Search date.</param>
        /// <param name="groupIDList">List group id.</param>
        private void BuildGroupData(IEnumerable<CommGroup> groups, DateTime date, string groupIDList)
        {
            var detailCriteria = new CommGroupDetailCriteria { GroupIDList = groupIDList, Date = date };
            var settingCriteria = new DeductionSettingCriteria { GroupIDList = groupIDList, Date = date };
            var groupDetails = _service.GetGroupDetails(detailCriteria);
            var deductionSettings = _service.GetDeductionSettings(settingCriteria);
            var allDeductions = AppSettings.DeductionTypes;
            var hasGeneralDeduction = AppSettings.DeductionTypes.FirstOrDefault(x => x.ProductID == 0);
            foreach (var group in groups)
            {
                var details = groupDetails.Where(x => x.GroupId == group.GroupID).ToList();
                var products = new Dictionary<string, object>();

                foreach (var detail in details)
                {
                    var id = detail.ProductID.ToString();
                    if (!products.ContainsKey(id))
                    {
                        products.Add(
                            id,
                            new
                            {
                                NumOfItems = details.Count(x => x.GroupId == group.GroupID && x.ProductID == detail.ProductID),
                                Deductions = GetDeductionSettings(deductionSettings, allDeductions, group, detail)
                            });
                    }
                }

                group.IsAllowedToDelete = group.NoOfAffiliate == 0 && group.CurrencyID != 0;
                group.Products = products.ToDictionary(x => x.Key, x => x.Value);
                group.Details = details.ToList();
                if (hasGeneralDeduction != null)
                {
                    var deduction = new Dictionary<string, decimal>();
                    var deductions = allDeductions.Where(x => x.ProductID == 0);
                    if (deductions.Count() > 0)
                    {
                        var settings = deductionSettings.Where(x => x.GroupID == group.GroupID).ToList();

                        foreach (var item in deductions)
                        {
                            var setting = settings.Where(x => x.DeductionID == item.DTypeID).FirstOrDefault();
                            deduction.Add(item.DTypeName, setting == null ? -1 : setting.DeductedPercentage);
                        }
                    }

                    group.GeneralDeductions = deduction;
                }
            }
        }
    }
}