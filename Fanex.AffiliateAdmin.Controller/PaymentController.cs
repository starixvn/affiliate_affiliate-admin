namespace Fanex.AffiliateAdmin.Controller
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Web.Mvc;
    using Core.Configurations;
    using Core.Entities;
    using Core.Enumerations;
    using Core.Filter;
    using Core.Session;
    using Core.Utilities;
    using Criterias;
    using Fanex.AffiliateAdmin.Resources;
    using Fanex.AffiliateAdmin.Services;
    using Models;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [FunctionAuth(Function = FunctionPolicyNames.Payment, CurrenciesRequired = true)]
    public class PaymentController : BaseController
    {
        private readonly IPaymentService _paymentService;

        #region Constructor

        public PaymentController()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentController" /> class.
        /// </summary>
        /// <param name="service">Payment service.</param>
        public PaymentController(IPaymentService service)
        {
            _paymentService = service ?? new PaymentService();
        }

        #endregion Constructor

        /// <summary>
        /// Payment index page.
        /// </summary>
        /// <param name="model">Payment View Model.</param>
        /// <returns>Action Result.</returns>
        [HttpGet]
        public ActionResult Index(PaymentViewModel model)
        {
            model.ExcludeTest = true;
            model.FromDate = DateTime.Today.AddDays(-1);
            model.ToDate = DateTime.Today;

            return View(model);
        }

        /// <summary>
        /// Payment list.
        /// </summary>
        /// <param name="criteria">Payment Criteria.</param>
        /// <returns>Action Result.</returns>
        [HttpPost]
        public ActionResult DataList(PaymentCriteria criteria)
        {
            criteria.IsITAdmin = SessionManager.CurrentSession.IsIT;
            criteria.LicenseeId = AppSettings.LicenseeID;

            var data = _paymentService.GetPayments(criteria).ToList();

            var subTotalRow =
                from p in data
                group p by p.Currency into g
                select new { Currency = g.Key, Amt = g.Sum(p => p.Amt), ConvertedAmt = g.Sum(p => p.ConvertedAmt) };

            var totalRow = from x in data
                           group x by 1
                               into g
                           select new
                           {
                               Currency = AppSettings.BaseExchange.Code,
                               Amt = g.Sum(y => y.ConvertedAmt),
                               ConvertedAmt = g.Sum(p => p.ConvertedAmt)
                           };

            return Json(
                new
                {
                    List = data,
                    SubTotal = subTotalRow,
                    Total = totalRow
                },
            JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Update Payment.
        /// </summary>
        /// <param name="transId">Payment Id.</param>
        /// <param name="fromStatement">From Statement.</param>
        /// <returns>Action Result.</returns>
        [HttpGet]
        public ActionResult UpdatePayment(int transId, bool fromStatement = false)
        {
            var paymentDetail = _paymentService.GetPaymentDetail(new PaymentDetailCriteria
            {
                TransId = transId,
                LicenseeId = AppSettings.LicenseeID
            });

            var model = new PaymentUpdateViewModel
            {
                TransId = transId,
                Status = paymentDetail.Status,
                AdminRemark = paymentDetail.AdminDesc,
                AffRemark = paymentDetail.AffDesc,
                PaymentDetail = paymentDetail,
                FromStatement = fromStatement
            };

            return View(model);
        }

        /// <summary>
        /// Update Payment information.
        /// </summary>
        /// <param name="model">Payment Update View Model.</param>
        /// <returns>Action Result.</returns>
        [HttpPost]
        [FunctionAuth(Function = FunctionPolicyNames.Payment, IsHostPolicy = true)]
        public ActionResult UpdatePayment(PaymentUpdateViewModel model)
        {
            var paymentDetail = _paymentService.GetPaymentDetail(new PaymentDetailCriteria
            {
                TransId = model.TransId,
                LicenseeId = AppSettings.LicenseeID
            });

            model.PaymentDetail = paymentDetail;

            if (ModelState.IsValid)
            {
                var session = SessionManager.CurrentSession;

                var criteria = new PaymentUpdateCriteria
                {
                    TransId = model.TransId,
                    Status = model.Status,
                    AdminRemark = CutLongText(model.AdminRemark.Replace("\r\n", "<br>").Replace("\n", "<br>"), 55),
                    AffRemark = CutLongText(model.AffRemark.Replace("\r\n", "<br>").Replace("\n", "<br>"), 55),
                    PreferredLanguage = paymentDetail.PreferredLanguage,
                    UserId = session.CustID,
                    AdminUserName = session.Username,
                    IP = Remote.GetClientIPAddress()
                };

                var result = UpdatePayment(criteria, paymentDetail.AffId, paymentDetail.RefId, paymentDetail.TransferAmount);

                if (result.Success)
                {
                    var emailType = criteria.Status.ToLower() == "approved" ? EmailTemplateType.PaymentApprove : EmailTemplateType.PaymentCancel;
                    var emailTemplate = new EmailTemplate(emailType, criteria.PreferredLanguage, criteria.MailOption);
                    var siteInfo = SiteDomainInfo.Parse(this, false);
                    var emailTemplateInfo = new EmailTemplateInfo
                    {
                        TransId = criteria.TransId,
                        FirstName = paymentDetail.Firstname,
                        WinlostDate = paymentDetail.Transdate,
                        Amount = FormaterCurrency.Format(paymentDetail.Amt, paymentDetail.AffCurrency),
                        AdminSiteUrl = siteInfo.AdminSiteUrl,
                        AffiliateSiteUrl = siteInfo.AffiliateSiteUrl,
                        MemberSiteUrl = siteInfo.MemberSiteUrl,
                        MemberSiteDomain = siteInfo.MemberSiteDomain
                    };

                    emailTemplate.ResolvePath(Server).Send(paymentDetail.Email, emailTemplateInfo);
                    ViewBag.Result = GetResultViewModel(result.ErrorCode, result.ErrorMessage);
                }
                else
                {
                    var message = string.IsNullOrEmpty(result.ErrorMessage) ? Common.GetErrorMsg(result.ErrorCode) : result.ErrorMessage;

                    ModelState.AddModelError("ErrMsg", string.IsNullOrEmpty(message) ? "Error" + result.ErrorCode + ": " + Lang.ErrorRequest : message);
                }
            }

            return View(model);
        }

        #region Helpers

        private static string CutLongText(string strText, int maxLength)
        {
            if (strText.Length <= maxLength - 3)
            {
                return strText;
            }

            return strText.Substring(0, maxLength - 3) + "...";
        }

        /// <summary>
        /// Determines whether [is exist licensee customer information] [the specified site name].
        /// </summary>
        /// <param name="siteName">Name of the site.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="ibcUsername">The IBC username.</param>
        /// <param name="player_Currency">The player_ currency.</param>
        /// <param name="errMsg">The error MSG.</param>
        /// <returns>Returns whether the customer is existed.</returns>
        private static bool IsExistLicenseeCustInfo(
            string siteName,
            string userName,
            ref string ibcUsername,
            ref string player_Currency,
            ref string errMsg)
        {
            if (AppSettings.LocalTest)
            {
                return true;
            }

            var xml = ServiceWrapper.GetLicenseeCustInfo(siteName, userName);
            if (xml == null || xml.IsEmpty)
            {
                return false;
            }

            int errCode = Convert.ToInt32(xml.Element("ReturnCode").Value);
            errMsg = xml.Element("ReturnMessage").Value;
            if (errCode == 0)
            {
                ibcUsername = Convert.ToString(xml.Element("IBCusername").Value);
                player_Currency = Convert.ToString(xml.Element("Currency").Value);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Transfers the automatic player account.
        /// </summary>
        /// <param name="systemName">Name of the system.</param>
        /// <param name="playerUsername">The player username.</param>
        /// <param name="amount">The amount.</param>
        /// <param name="balAccCustId">The balance account customer id.</param>
        /// <param name="ibcRefNo">The IBC preference no.</param>
        /// <param name="ibcTransId">The IBC trans id.</param>
        /// <returns>Returns result when calling service to transfer amount to player account.</returns>
        private ResultViewModel TransferToPlayerAcc(
            string systemName,
            string playerUsername,
            double amount,
            int balAccCustId,
            ref long ibcRefNo,
            ref long ibcTransId)
        {
            var result = new ResultViewModel();

            if (AppSettings.LocalTest)
            {
                result.Success = true;
                return result;
            }

            var xml = ServiceWrapper.Transfer(systemName, playerUsername, amount, balAccCustId);
            if (xml == null || xml.IsEmpty)
            {
                result.ErrorMessage = "Empty result from API!";
                return result;
            }

            result.ErrorCode = Convert.ToInt32(xml.Element("ReturnCode").Value);
            if (result.ErrorCode == 0)
            {
                result.Success = true;
                ibcRefNo = Convert.ToInt64(xml.Element("IBCRefNo").Value);
                ibcTransId = Convert.ToInt64(xml.Element("IBCTransId").Value);
            }
            else
            {
                result.ErrorMessage = xml.Element("ReturnMessage").Value;
            }

            return result;
        }

        /// <summary>
        /// Updates the payment status.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <param name="affId">The affiliate id.</param>
        /// <param name="refId">The preference unique identifier.</param>
        /// <param name="transferAmount">The amount.</param>
        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed.")]
        private ResultViewModel UpdatePayment(PaymentUpdateCriteria criteria, long affId, long refId, decimal transferAmount)
        {
            ResultViewModel result;

            if (criteria.Status.ToLower() == "approved")
            {
                var playerPayoutInfo = _paymentService.GetPlayerPayoutInfo(new PlayerPayoutInfoSelCriteria
                {
                    AffId = affId,
                    RefId = refId
                });

                var balAccCustId = _paymentService.GetCustIDByUsername(new DummyAccIdCriteria
                {
                    Username = AppSettings.LocalTest ? "i668betUS$011117" : playerPayoutInfo.DummyBalAcc
                });

                long ibcRefNo = 0, ibcTransId = 0;
                result = TransferToPlayerAcc(
                    playerPayoutInfo.SystemName,
                    playerPayoutInfo.PlayerUsername,
                    Math.Abs(Convert.ToDouble(transferAmount)),
                    balAccCustId,
                    ref ibcRefNo,
                    ref ibcTransId);

                if (result.Success)
                {
                    string ibcUsername = string.Empty, playerCurrency = string.Empty, errorMessage = string.Empty;

                    // Update IbcUsername for the payout with non-deposited Acc
                    if (IsExistLicenseeCustInfo(
                        playerPayoutInfo.SystemName,
                        playerPayoutInfo.PlayerUsername,
                        ref ibcUsername,
                        ref playerCurrency,
                        ref errorMessage))
                    {
                        criteria.IbcRefNo = ibcRefNo;
                        criteria.IbcTransId = ibcTransId;
                        criteria.IbcUsername = ibcUsername;
                        _paymentService.UpdatePayment(criteria);
                    }
                    else
                    {
                        result.ErrorMessage = errorMessage;
                    }
                }
            }
            else
            {
                result = new ResultViewModel();
                _paymentService.UpdatePayment(criteria);
            }

            result.ErrorCode = criteria.ErrCode;
            result.Success = result.ErrorCode == 0;
            return result;
        }

        #endregion Helpers
    }
}