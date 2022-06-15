namespace Fanex.AffiliateAdmin.Controller
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;
    using Core.DbConstants;
    using Fanex.AffiliateAdmin.Core.Configurations;
    using Fanex.AffiliateAdmin.Core.Constants;
    using Fanex.AffiliateAdmin.Core.Filter;
    using Fanex.AffiliateAdmin.Core.Session;
    using Fanex.AffiliateAdmin.Core.Utilities;
    using Fanex.AffiliateAdmin.Criterias;
    using Fanex.AffiliateAdmin.DTO;
    using Fanex.AffiliateAdmin.Models;
    using Fanex.AffiliateAdmin.Resources;
    using Fanex.AffiliateAdmin.Services;

    [FunctionAuth(Function = FunctionPolicyNames.CommissionGroup, CurrenciesRequired = true)]
    public partial class CommGroupController : BaseController
    {
        private static readonly int[] IgnoredProducts = new int[]
        {
            ProductTypeConstant.CashOut
        };

        private readonly ISystemSettings _systemSettings;
        private readonly ICommGroupService _commGroupService;
        private readonly IExchangeRateService _exchangeRateService;

        public CommGroupController(
            IAppSetting appSetting,
            ISystemSettings systemSettings,
            ICommGroupService commGroupService,
            IExchangeRateService exchangeRateService) : base(appSetting)
        {
            _systemSettings = systemSettings;
            _commGroupService = commGroupService;
            _exchangeRateService = exchangeRateService;
        }

        [HttpGet]
        public ActionResult Add()
        {
            ViewBag.ProductList = _systemSettings.Products.Where(x => x.IsEnabled && !IgnoredProducts.Contains(x.ProductID));
            var currency = SessionManager.CurrentSession.CurrencyList.First();

            var criteria = new GetDefaultPspExchangeCriteria
            {
                CurrencyCode = currency.CurrencyCode
            };

            var defaultPspExchange = _exchangeRateService.GetDefaultPspExchange(criteria);

            var model = new CommGroupInfo
            {
                DepositPercent = defaultPspExchange.DepositPercent ?? 0,
                WithdrawalPercent = defaultPspExchange.WithdrawalPercent ?? 0
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult Add(CommGroupInsertCriteria criteria)
        {
            var session = SessionManager.CurrentSession;
            criteria.AdminUserName = session.Username;
            AssignCommGroupInsertUpdateCriteria(criteria, session);

            _commGroupService.ExecuteOutput(criteria);

            return Json(GetCommGroupInsertUpdateResult(criteria, Lang.AddSucc));
        }

        [HttpPost]
        public string Delete(int groupid)
        {
            var criteria = new CommGroupDeleteCriteria
            {
                GroupId = groupid,
                UserId = SessionManager.CurrentSession.CustID,
                IP = Remote.GetClientIPAddress(),
                LicenseeId = _systemSettings.LicenseeID
            };

            _commGroupService.ExecuteOutput(criteria);
            var errmsg = Common.GetErrorMsg(criteria.ErrCode);
            if (errmsg.Contains("{0}"))
            {
                errmsg = string.Format(errmsg, criteria.GroupId);
            }

            return errmsg;
        }

        [HttpGet]
        public ActionResult Edit(int groupid)
        {
            var group = _commGroupService.GetListData<CommGroupInfo>(new CommGroupInfoCriteria
            {
                GroupId = groupid
            }).FirstOrDefault();

            if (group == null)
            {
                return Content("Group is null");
            }

            group.GroupSettings = _commGroupService.GetListData<GroupSetting>(new GroupSettingCriteria
            {
                GroupId = groupid
            });
            group.DeductionSettings = _commGroupService.GetListData<DeductionSetting>(new DeductionSettingCriteria
            {
                GroupIDList = groupid.ToString(CultureInfo.InvariantCulture)
            });
            group.GroupName = group.GroupName.Decode();
            group.Description = group.Description.Decode();

            ViewBag.ProductList = _systemSettings.Products.Where(x => x.IsEnabled && !IgnoredProducts.Contains(x.ProductID));
            return View("Edit", group);
        }

        [HttpPost]
        public ActionResult Edit(CommGroupUpdateCriteria criteria)
        {
            criteria.LicenseeId = _systemSettings.LicenseeID;
            AssignCommGroupInsertUpdateCriteria(criteria);

            _commGroupService.ExecuteOutput(criteria);

            return Json(GetCommGroupInsertUpdateResult(criteria, Lang.UpdateSucc));
        }

        [HttpGet]
        [NoCache]
        public string GetCurrencyNotification(int currencyid)
        {
            var currency = SessionManager.CurrentSession.CurrencyList.Where(x => x.CurrencyID == currencyid).FirstOrDefault();

            if (currency == null || currency.MultipliedBy == 1)
            {
                return string.Empty;
            }

            return Common.GetCurrencyNotification(currency.CurrencyCode, currency.MultipliedBy);
        }

        [HttpGet]
        [NoCache]
        public ActionResult GetDefaultPspExchange(GetDefaultPspExchangeCriteria criteria)
        {
            return Json(_exchangeRateService.GetDefaultPspExchange(criteria), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetList(CommGroupCriteria criteria)
        {
            var session = SessionManager.CurrentSession;

            criteria.UserId = session.CustID;
            criteria.LicenseeId = _systemSettings.LicenseeID;
            criteria.UserCurrencies = session.BuildUserCurrencyParam();
            AssignPagingCriteriaBase(criteria);

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

        [HttpGet]
        public ActionResult Index(string groupName)
        {
            ViewBag.IsIT = SessionManager.CurrentSession.IsIT;
            ViewBag.GroupName = groupName;
            return View();
        }

        public ActionResult ViewSetting(CommGroupSelByIdCriteria criteria)
        {
            var session = SessionManager.CurrentSession;

            criteria.UserId = session.CustID;
            criteria.LicenseeId = _systemSettings.LicenseeID;
            criteria.UserCurrencies = session.BuildUserCurrencyParam();

            var groups = _commGroupService.GetById(criteria);
            var date = criteria.Date ?? _systemSettings.SystemDateTime;

            _commGroupService.BuildGroupData(groups, date, criteria.GroupID.ToString());

            return View(groups.FirstOrDefault());
        }

        private ResultViewModel GetCommGroupInsertUpdateResult(CommGroupInsertUpdateCriteria criteria, string message)
        {
            var errorMsg = string.Empty;

            if (criteria.ErrCode == ErrorCodes.GeneralErrorCode)
            {
                errorMsg = criteria.ErrMsg;
            }
            else if (criteria.ErrCode != ErrorCodes.Success)
            {
                errorMsg = Common.GetErrorMsg(criteria.ErrCode);
                if (errorMsg.Contains("{0}"))
                {
                    errorMsg = string.Format(errorMsg, string.IsNullOrEmpty(criteria.ErrMsg) ? criteria.GroupId.ToString() : criteria.ErrMsg);
                }
            }

            return GetResultViewModel(criteria.ErrCode, message, errorMsg);
        }

        private void AssignCommGroupInsertUpdateCriteria(CommGroupInsertUpdateCriteria criteria, ICurrentSession currentSession = null)
        {
            CopyTierSettingsForAllProducts(criteria);

            var session = currentSession ?? SessionManager.CurrentSession;
            criteria.UserId = session.CustID;
            criteria.IP = Remote.GetClientIPAddress();
            criteria.GroupName = criteria.GroupName.Encode();
            criteria.Description = criteria.Description.Encode();
            criteria.DeductionList = BuildDeductionListXml(criteria.DeductionSettingList);
        }

        private static string BuildDeductionListXml(string deductionSettingList)
        {
            if (string.IsNullOrEmpty(deductionSettingList))
            {
                return null;
            }

            var xmlContent = new StringBuilder("<root>");
            var deductionList = deductionSettingList.TrimEnd('|').Split('|');
            foreach (var deduction in deductionList)
            {
                var values = deduction.Split(';');
                xmlContent.Append($"<row id='{values[0]}'"
                    + (string.IsNullOrEmpty(values[1]) ? string.Empty : $" percent='{values[1]}'")
                    + (string.IsNullOrEmpty(values[2]) ? string.Empty : $" nextpercent='{values[2]}'")
                    + " />");
            }

            xmlContent.Append("</root>");

            return xmlContent.ToString();
        }

        /// <summary>
        /// Copy Tier Settings For All Enabled Products.
        /// </summary>
        /// <param name="criteria">Criteria input.</param>
        private void CopyTierSettingsForAllProducts(CommGroupInsertUpdateCriteria criteria)
        {
            var productCount = _systemSettings.Products.Count();

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

        private IEnumerable<CommGroup> GetGridData(CommGroupCriteria criteria)
        {
            var groups = _commGroupService.GetListData<CommGroup>(criteria);

            if (!string.IsNullOrEmpty(criteria.GroupIDList) && criteria.GroupIDList.IndexOf(',') == 0)
            {
                criteria.GroupIDList = criteria.GroupIDList.Substring(1);
            }

            _commGroupService.BuildGroupData(groups, criteria.Date, criteria.GroupIDList);

            return groups;
        }
    }
}