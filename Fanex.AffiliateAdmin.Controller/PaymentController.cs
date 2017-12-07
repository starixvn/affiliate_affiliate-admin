namespace Fanex.AffiliateAdmin.Controller
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;
    using Core.Configurations;
    using Core.Constants;
    using Core.Entities;
    using Core.Enumerations;
    using Core.Filter;
    using Core.Services.Impl;
    using Core.Services.Interface;
    using Core.Session;
    using Core.Utilities;
    using Criterias;
    using DTO;
    using Fanex.AffiliateAdmin.Resources;
    using Fanex.AffiliateAdmin.Services;
    using Models;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [FunctionAuth(Function = FunctionPolicyNames.Payment, CurrenciesRequired = true)]
    public class PaymentController : BaseController
    {
        private const string ModelErrorKey = "ErrMsg";
        private static readonly IEnumerable<string> AllowedStatusListToUpdate = new List<string> { PaymentStatusText.Approved, PaymentStatusText.Rejected };
        private readonly IPaymentService _paymentService;
        private readonly IDbLogger _databaseLogger;

        #region Constructor

        public PaymentController()
            : this(null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentController" /> class.
        /// </summary>
        /// <param name="service">Payment service.</param>
        public PaymentController(IPaymentService service, IDbLogger databaseLogger)
        {
            _paymentService = service ?? new PaymentService();
            _databaseLogger = databaseLogger ?? new DbLogger();
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
        [ValidateAntiForgeryToken]
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
                var currentStatus = paymentDetail.Status;
                var statusToUpdate = model.Status.ToLower();

                if (!IsValidStatus(currentStatus, statusToUpdate.ToLower()))
                {
                    ModelState.AddModelError(ModelErrorKey, Lang.InValidPaymentRequestStatus);
                    return View(model);
                }

                var session = SessionManager.CurrentSession;

                var checkInProgressResult = _paymentService.CheckPaymentInProgress(new PaymentCheckInProgressCriteria
                {
                    TransId = model.TransId,
                    UserId = SessionManager.CurrentSession.CustID,
                    IP = Remote.GetClientIPAddress()
                });

                if (checkInProgressResult.ErrorCode != 0)
                {
                    ModelState.AddModelError(ModelErrorKey, Lang.PaymentRequestBeingProcessed);
                    return View(model);
                }

                var criteria = GetPaymentUpdateCriteria(model, paymentDetail);
                var result = ProcessUpdatePayment(criteria, paymentDetail);

                if (result.Success)
                {
                    SendEmail(paymentDetail, criteria);
                    ViewBag.Result = GetResultViewModel(result.ErrorCode, result.ErrorMessage);
                }
                else
                {
                    var message = string.IsNullOrEmpty(result.ErrorMessage) ? Common.GetErrorMsg(result.ErrorCode) : result.ErrorMessage;
                    ModelState.AddModelError(ModelErrorKey, string.IsNullOrEmpty(message) ? "Error" + result.ErrorCode + ": " + Lang.ErrorRequest : message);
                }
            }

            return View(model);
        }

        #region Helpers
        private static PaymentUpdateCriteria GetPaymentUpdateCriteria(PaymentUpdateViewModel model, PaymentDetail paymentDetail)
        {
            return new PaymentUpdateCriteria
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
        }

        private static bool IsValidStatus(string currentStatus, string statusToUpdate)
        {
            return currentStatus.Equals(PaymentStatusText.Pending, StringComparison.OrdinalIgnoreCase)
                && AllowedStatusListToUpdate.Contains(statusToUpdate);
        }

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
        private bool IsExistLicenseeCustInfo(
            long transId,
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

            var methodName = "GetLicenseeCustInfo";
            var userId = SessionManager.CurrentSession.CustID;
            var paramInfo = $"siteName={siteName}, userName={userName}";
            var resultLogContent = BuildLogContentByResult(methodName, userId, transId, paramInfo, xml?.ToString());
            _databaseLogger.WritePaymentLog(new PaymentLogCriteria { UserId = userId, LogContent = resultLogContent });

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
            PlayerPayoutInfo playerPayoutInfo,
            long transId,
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

            var siteName = playerPayoutInfo.SystemName;
            var licenseeUserName = playerPayoutInfo.PlayerUsername;
            var xml = ServiceWrapper.Transfer(siteName, licenseeUserName, amount, balAccCustId);

            var methodName = "Transfer";
            var userId = SessionManager.CurrentSession.CustID;
            var paramInfo = $"siteName={siteName}, licenseeUserName={licenseeUserName}, amount={amount}, dummyAccount={balAccCustId}";
            var resultLogContent = BuildLogContentByResult(methodName, userId, transId, paramInfo, xml?.ToString());
            _databaseLogger.WritePaymentLog(new PaymentLogCriteria { UserId = userId, LogContent = resultLogContent });

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
        private ResultViewModel ProcessUpdatePayment(PaymentUpdateCriteria criteria, PaymentDetail paymentDetail)
        {
            ResultViewModel result;

            if (criteria.Status.ToLower() == PaymentStatusText.Approved)
            {
                var playerPayoutInfo = _paymentService.GetPlayerPayoutInfo(new PlayerPayoutInfoSelCriteria
                {
                    AffId = paymentDetail.AffId,
                    RefId = paymentDetail.RefId
                });

                var balAccCustId = _paymentService.GetCustIDByUsername(new DummyAccIdCriteria
                {
                    Username = AppSettings.LocalTest ? "i668betUS$011117" : playerPayoutInfo.DummyBalAcc
                });

                long ibcRefNo = 0, ibcTransId = 0;
                result = TransferToPlayerAcc(
                    playerPayoutInfo,
                    paymentDetail.Transid,
                    Math.Abs(Convert.ToDouble(paymentDetail.TransferAmount)),
                    balAccCustId,
                    ref ibcRefNo,
                    ref ibcTransId);

                if (result.Success)
                {
                    string ibcUsername = string.Empty, playerCurrency = string.Empty, errorMessage = string.Empty;

                    var isCustomerExisted = IsExistLicenseeCustInfo(
                                                paymentDetail.Transid,
                                                playerPayoutInfo.SystemName,
                                                playerPayoutInfo.PlayerUsername,
                                                ref ibcUsername,
                                                ref playerCurrency,
                                                ref errorMessage);
                    // Update IbcUsername for the payout with non-deposited Acc
                    if (isCustomerExisted)
                    {
                        criteria.IbcRefNo = ibcRefNo;
                        criteria.IbcTransId = ibcTransId;
                        criteria.IbcUsername = ibcUsername;
                        _paymentService.UpdatePayment(criteria);
                    }
                    else
                    {
                        result.Success = false;
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

        private string BuildLogContentByResult(string methodName, int userId, long transId, string paramInfo, string resultContent)
        {
            var builder = new StringBuilder();

            builder.AppendLine($"Complete Call Method {methodName} at {DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture)}");
            builder.AppendLine("Infor:");
            builder.AppendLine($"TransId: {transId}");
            builder.AppendLine($"UserId: {userId}");
            builder.AppendLine($"Param Info: {paramInfo}");

            builder.AppendLine($"Result Content: {resultContent}");

            return builder.ToString();
        }

        private void SendEmail(PaymentDetail paymentDetail, PaymentUpdateCriteria criteria)
        {
            var emailType = criteria.Status.ToLower() == PaymentStatusText.Approved ? EmailTemplateType.PaymentApprove : EmailTemplateType.PaymentCancel;
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
        }

        #endregion Helpers
    }
}