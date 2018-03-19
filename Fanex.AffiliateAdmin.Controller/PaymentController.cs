namespace Fanex.AffiliateAdmin.Controller
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
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
    using DTO.PaymentRequest;
    using Fanex.AffiliateAdmin.Resources;
    using Fanex.AffiliateAdmin.Services;
    using Models;

    [FunctionAuth(Function = FunctionPolicyNames.Payment, CurrenciesRequired = true)]
    public class PaymentController : BaseController
    {
        private const string ModelErrorKey = "ErrMsg";
        private const int UserNotExistedCode = 2005;
        private static readonly IEnumerable<string> AllowedStatusListToUpdate = new List<string> { PaymentStatusText.Approved, PaymentStatusText.Rejected };
        private readonly IPaymentService _paymentService;
        private readonly IDbLogger _databaseLogger;

        #region Constructor

        public PaymentController()
            : this(null, null)
        {
        }

        public PaymentController(IPaymentService service, IDbLogger databaseLogger)
        {
            _paymentService = service ?? new PaymentService();
            _databaseLogger = databaseLogger ?? new DbLogger();
        }

        #endregion Constructor

        [HttpGet]
        public ActionResult Index(PaymentViewModel model)
        {
            model.ExcludeTest = true;
            model.FromDate = DateTime.Today.AddDays(-1);
            model.ToDate = DateTime.Today;

            return View(model);
        }

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        [FunctionAuth(Function = FunctionPolicyNames.Payment, IsHostPolicy = true)]
        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed.")]
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
                if (!IsValidStatus(currentStatus, model.Status.ToLower()))
                {
                    ModelState.AddModelError(ModelErrorKey, Lang.InValidPaymentRequestStatus);
                    return View(model);
                }

                var playerPayoutInfo = _paymentService.GetPlayerPayoutInfo(new PlayerPayoutInfoSelCriteria
                {
                    AffId = paymentDetail.AffId,
                    RefId = paymentDetail.RefId
                });

                var custInfo = GetCustInfo(paymentDetail.Transid, playerPayoutInfo.SystemName, playerPayoutInfo.PlayerUsername);
                if (string.IsNullOrEmpty(playerPayoutInfo.DummyBalAcc) || !IsValidCustomer(custInfo))
                {
                    ModelState.AddModelError(ModelErrorKey, Common.GetErrorMsg(UserNotExistedCode));
                    return View(model);
                }

                var checkInProgressResult = _paymentService.CheckPaymentInProgress(new PaymentCheckInProgressCriteria
                {
                    TransId = model.TransId,
                    UserId = SessionManager.CurrentSession.CustID,
                    IP = Remote.GetClientIPAddress(),
                    AdminUserName = SessionManager.CurrentSession.Username
                });

                if (checkInProgressResult.ErrorCode != 0)
                {
                    ModelState.AddModelError(ModelErrorKey, Lang.PaymentRequestBeingProcessed);
                    return View(model);
                }

                var criteria = GetPaymentUpdateCriteria(model, paymentDetail);
                var result = ProcessUpdatePayment(criteria, paymentDetail, playerPayoutInfo);

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
                UserId = SessionManager.CurrentSession.CustID,
                AdminUserName = SessionManager.CurrentSession.Username,
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

        private static bool IsValidCustomer(ApiCustInfo custInfo)
        {
            return AppSettings.LocalTest ||
                                        (!string.IsNullOrEmpty(custInfo.PlayerCurrency) &&
                                        string.IsNullOrEmpty(custInfo.ErrorMessage));
        }

        private static bool IsTransferCompleted(ApiTransferInfo transferInfo)
        {
            return AppSettings.LocalTest ||
                                        (transferInfo.IbcRefNo != 0 &&
                                        transferInfo.IbcTransId != 0 &&
                                        string.IsNullOrEmpty(transferInfo.ErrorMessage));
        }

        private ApiCustInfo GetCustInfo(long transId, string siteName, string userName)
        {
            return AppSettings.LocalTest ?
                new ApiCustInfo { PlayerCurrency = "USD" } :
                ServiceWrapper.GetLicenseeCustInfo(siteName, userName, transId);
        }

        private ApiTransferInfo ProcessTransfer(PlayerPayoutInfo playerPayoutInfo, long transId, double amount, int balAccCustId)
        {
            return AppSettings.LocalTest ?
                new ApiTransferInfo() :
                ServiceWrapper.Transfer(playerPayoutInfo.SystemName, playerPayoutInfo.PlayerUsername, amount, balAccCustId, transId);
        }

        private ResultViewModel ProcessUpdatePayment(PaymentUpdateCriteria criteria, PaymentDetail paymentDetail, PlayerPayoutInfo playerPayoutInfo)
        {
            var apiErrorMessage = string.Empty;
            var status = criteria.Status.ToLower();
            var paymentIsUpdated = status == PaymentStatusText.Rejected;
            if (status == PaymentStatusText.Approved)
            {
                var custInfo = GetCustInfo(paymentDetail.Transid, playerPayoutInfo.SystemName, playerPayoutInfo.PlayerUsername);

                if (IsValidCustomer(custInfo))
                {
                    var balAccCustId = _paymentService.GetCustIDByUsername(new DummyAccIdCriteria
                    {
                        Username = AppSettings.LocalTest ? "i668betUS$011117" : playerPayoutInfo.DummyBalAcc
                    });

                    var transferInfo = ProcessTransfer(
                        playerPayoutInfo,
                        paymentDetail.Transid,
                        Math.Abs(Convert.ToDouble(paymentDetail.TransferAmount)),
                        balAccCustId);

                    paymentIsUpdated = IsTransferCompleted(transferInfo);

                    if (paymentIsUpdated)
                    {
                        criteria.IbcRefNo = transferInfo.IbcRefNo;
                        criteria.IbcTransId = transferInfo.IbcTransId;
                        criteria.IbcUsername = custInfo.IbcUserName ?? string.Empty;
                    }
                    else
                    {
                        apiErrorMessage = transferInfo.ErrorMessage;
                    }
                }
                else
                {
                    paymentIsUpdated = false;
                    apiErrorMessage = custInfo.ErrorMessage;
                }
            }

            if (paymentIsUpdated)
            {
                _paymentService.UpdatePayment(criteria);
                return new ResultViewModel
                {
                    ErrorCode = criteria.ErrCode,
                    Success = criteria.ErrCode == 0,
                };
            }

            return new ResultViewModel
            {
                Success = false,
                ErrorMessage = apiErrorMessage
            };
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