namespace Fanex.AffiliateAdmin.Controller
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;
    using Core.DbConstants;
    using Fanex.AffiliateAdmin.Core.Configurations;
    using Fanex.AffiliateAdmin.Core.Filter;
    using Fanex.AffiliateAdmin.Core.Services.Interface;
    using Fanex.AffiliateAdmin.Core.Session;
    using Fanex.AffiliateAdmin.Core.Utilities;
    using Fanex.AffiliateAdmin.Criterias;
    using Fanex.AffiliateAdmin.DTO;
    using Fanex.AffiliateAdmin.Resources;
    using Fanex.AffiliateAdmin.Services;

    [FunctionAuth(Function = FunctionPolicyNames.CommissionGroup, CurrenciesRequired = true)]
    public partial class CommGroupController : BaseController
    {
        private static readonly int[] IgnoredProducts = new int[]
        {
            ProductTypeConstant.CashOut
        };

        private readonly ICommGroupService _commGroupService;

        private readonly IExchangeRateService _exchangeRateService;

        public CommGroupController
            (ICommGroupService commGroupService,
            IExchangeRateService exchangeRateService,
            ISystemSettingService systemSettingService) : base(systemSettingService)
        {
            _commGroupService = commGroupService;
            _exchangeRateService = exchangeRateService;
        }

        [HttpGet]
        public ActionResult Add()
        {
            ViewBag.ProductList = AppSettings.Products.Where(x => x.IsEnabled && !IgnoredProducts.Contains(x.ProductID));
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

        [HttpPost]
        public ActionResult Add(CommGroupInsertCriteria criteria)
        {
            CopyTierSettingsForAllProducts(criteria);

            var session = SessionManager.CurrentSession;
            criteria.AdminUserName = session.Username;
            criteria.UserId = session.CustID;
            criteria.IP = Remote.GetClientIPAddress();
            criteria.GroupName = criteria.GroupName.Encode();
            criteria.Description = criteria.Description.Encode();
            criteria.DeductionList = BuildDeductionListXml(criteria.DeductionSettingList);

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

        [HttpPost]
        public string Delete(int groupid)
        {
            var criteria = new CommGroupDeleteCriteria
            {
                GroupId = groupid,
                UserId = SessionManager.CurrentSession.CustID,
                IP = Remote.GetClientIPAddress(),
                LicenseeId = AppSettings.LicenseeID
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

            ViewBag.ProductList = AppSettings.Products.Where(x => x.IsEnabled && !IgnoredProducts.Contains(x.ProductID));
            return View("Edit", group);
        }

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

            criteria.LicenseeId = AppSettings.LicenseeID;

            criteria.DeductionList = BuildDeductionListXml(criteria.DeductionSettingList);

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
            criteria.PageIndex = criteria.PageIndex == 0 ? 1 : criteria.PageIndex;
            criteria.PageSize = criteria.PageSize == 0 ? AppSettings.PageSize : criteria.PageSize;
            criteria.LicenseeId = AppSettings.LicenseeID;
            criteria.UserCurrencies = session.BuildUserCurrencyParam();

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
            criteria.LicenseeId = AppSettings.LicenseeID;
            criteria.UserCurrencies = session.BuildUserCurrencyParam();

            var groups = _commGroupService.GetById(criteria);
            var date = criteria.Date == null ? SystemTimeService.GetSystemDateTime() : criteria.Date.Value;

            _commGroupService.BuildGroupData(groups, date, criteria.GroupID.ToString());

            return View(groups.FirstOrDefault());
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
        private static void CopyTierSettingsForAllProducts(CommGroupInsertCriteria criteria)
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