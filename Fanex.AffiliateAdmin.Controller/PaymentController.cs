// --------------------------------------------------------------------------
// <copyright file="PaymentController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Leo.Vo" date="2013.8.21">Created.</change>
//   <change who="Johnny.Trinh" date="2013.09.19">Apply new filter.</change>
// </history>

namespace Fanex.AffiliateAdmin.Controller
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.Script.Serialization;
    using Core.Configurations;
    using Core.Entities;
    using Core.Enumerations;
    using Core.Filter;
    using Core.Session;
    using Core.Utilities;
    using Criterias;
    using Fanex.AffiliateAdmin.Resources;
    using Models;
    using Services.Impl;
    using Services.Interface;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [FunctionAuth(Function = "[Payment]")]
    public class PaymentController : BaseController
    {
        private readonly IPaymentService _paymentService;

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentController" /> class.
        /// </summary>
        /// <param name="service">Payment service.</param>
        public PaymentController(IPaymentService service)
        {
            _paymentService = service;
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
                TransId = transId
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
        public ActionResult UpdatePayment(PaymentUpdateViewModel model)
        {
            var paymentDetail = _paymentService.GetPaymentDetail(new PaymentDetailCriteria
            {
                TransId = model.TransId
            });
            model.PaymentDetail = paymentDetail;

            if (ModelState.IsValid)
            {
                var criteria = new PaymentUpdateCriteria
                {
                    TransId = model.TransId,
                    Status = model.Status,
                    AdminRemark = Common.CutLongText(model.AdminRemark.Replace("\r\n", "<br>").Replace("\n", "<br>"), 55),
                    AffRemark = Common.CutLongText(model.AffRemark.Replace("\r\n", "<br>").Replace("\n", "<br>"), 55),
                    PreferredLang = paymentDetail.PreferredLanguage,
                    UserId = CurrentSession.CustID,
                    IP = Remote.GetClientIPAddress()
                };

                UpdatePayment(criteria, paymentDetail.AffId, paymentDetail.RefId, paymentDetail.TransferAmount);

                if (criteria.ErrCode == 0)
                {
                    var emailType = criteria.Status.ToLower() == "approved" ? EmailTemplateType.PaymentApprove : EmailTemplateType.PaymentCancel;
                    var emailTemplate = new EmailTemplate(emailType, criteria.PreferredLang, criteria.MailOption);
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
                    ViewBag.Result = GetResultViewModel(criteria.ErrCode);
                }
                else
                {
                    ModelState.AddModelError("ErrMsg", Common.GetErrorMsg(criteria.ErrCode));
                }
            }

            return View(model);
        }

        /// <summary>
        /// Processing Commission.
        /// </summary>
        /// <returns>Processing Commission Page.</returns>
        [FunctionAuth(Function = "[MonthlyTransfer]")]
        public ActionResult ProcessingCommission()
        {
            var serializer = new JavaScriptSerializer();

            ViewBag.Products = serializer.Serialize(AppSettings.Products.Select(x => new
            {
                Id = x.ProductID,
                Name = x.ProductName
            }));

            var criteria = new IsMonthlyTransferCriteria { MonthDate = DateTime.Now.AddMonths(-1).Date };

            _paymentService.CheckMonthlyTransfer(criteria);

            ViewBag.IsMonthlyTransfer = criteria.IsMonthly;

            return View(new ProcessingCommissionViewModel());
        }

        /// <summary>
        /// Processing Commission.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>Processing Commission Result.</returns>
        [FunctionAuth(Function = "[MonthlyTransfer]")]
        [HttpPost]
        public ActionResult ProcessingCommission(ProcessingCommCriteria criteria)
        {
            criteria.UserId = CurrentSession.CustID;
            criteria.IsITAdmin = CurrentSession.IsIT;

            var data = _paymentService.GetProcessingCommissions(criteria);

            return Json(new
                {
                    List = data,
                    ItemsPerPage = data.Count(),
                    TotalItem = data.Count(),
                    HasCashbackDeduction = AppSettings.DeductionTypes.Any(x => x.ProductID == 0)
                });
        }

        /// <summary>
        /// Update Cash Back.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>Update Cash Back Result.</returns>
        [HttpPost]
        [FunctionAuth(Function = "[MonthlyTransfer]")]
        public ActionResult UpdateCashBack(CashBackUpdateCriteria criteria)
        {
            criteria.UserId = CurrentSession.CustID;
            _paymentService.UpdateCashBack(criteria);

            var result = new ResultViewModel
            {
                Success = criteria.ErrCode == 0,
                ErrorCode = criteria.ErrCode,
                Message = criteria.ErrCode == 0 ? string.Empty : ErrorMessage(criteria.ErrCode)
            };

            return Json(result);
        }

        /// <summary>
        /// Monthly Transfer.
        /// </summary>
        /// <returns>MonthlyTransfer Result.</returns>
        [HttpPost]
        [FunctionAuth(Function = "[MonthlyTransfer]")]
        public ActionResult MonthlyTransfer()
        {
            if (SessionManager.CurrentSession.RoleAccess.HasPolicy("[MonthlyTransfer]", 1))
            {
                var lastMonth = DateTime.Now.AddMonths(-AppSettings.PastNthMonth);

                var criteria = new IsMonthlyTransferCriteria { MonthDate = lastMonth.Date };

                _paymentService.CheckMonthlyTransfer(criteria);

                if (criteria.IsMonthly)
                {
                    return Json(new ResultViewModel
                        {
                            Success = false,
                            Message = string.Format(Lang.AlreadyTransfersForThisMonth, lastMonth.ToString("MMMM yyyy"))
                        });
                }

                var result = _paymentService.MonthlyTransfer(new MonthlyTransferCriteria { Month = lastMonth.Month, Year = lastMonth.Year, UserId = CurrentSession.CustID });

                return Json(new ResultViewModel
                {
                    Success = string.IsNullOrEmpty(result),
                    Message = result
                });
            }

            return Json(new ResultViewModel
                {
                    Success = false,
                    Message = Lang.YouDontHaveMonthlyTransfer
                });
        }

        #region Helpers

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
        /// <param name="errMsg">The error MSG.</param>
        /// <returns>Returns result when calling service to transfer amount to player account.</returns>
        private bool TransferToPlayerAcc(
            string systemName,
            string playerUsername,
            double amount,
            int balAccCustId,
            ref long ibcRefNo,
            ref long ibcTransId,
            ref string errMsg)
        {
            if (AppSettings.LocalTest)
            {
                return true;
            }

            var xml = ServiceWrapper.Transfer(systemName, playerUsername, amount, balAccCustId);
            if (xml == null || xml.IsEmpty)
            {
                return false;
            }

            int errCode = Convert.ToInt32(xml.Element("ReturnCode").Value);
            if (errCode == 0)
            {
                ibcRefNo = Convert.ToInt64(xml.Element("IBCRefNo").Value);
                ibcTransId = Convert.ToInt64(xml.Element("IBCTransId").Value);
                return true;
            }

            errMsg = xml.Element("ReturnMessage").Value;
            return false;
        }

        /// <summary>
        /// Updates the payment status.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <param name="affId">The affiliate id.</param>
        /// <param name="refId">The preference unique identifier.</param>
        /// <param name="transferAmount">The amount.</param>
        private void UpdatePayment(PaymentUpdateCriteria criteria, long affId, long refId, decimal transferAmount)
        {
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
                var errMsg = string.Empty;
                if (TransferToPlayerAcc(
                    playerPayoutInfo.SystemName,
                    playerPayoutInfo.PlayerUsername,
                    Math.Abs(Convert.ToDouble(transferAmount)),
                    balAccCustId,
                    ref ibcRefNo,
                    ref ibcTransId,
                    ref errMsg))
                {
                    string ibcUsername = string.Empty, playerCurrency = string.Empty;
                    ////Update IbcUsername for the payout with non-deposited Acc
                    IsExistLicenseeCustInfo(playerPayoutInfo.SystemName, playerPayoutInfo.PlayerUsername, ref ibcUsername, ref playerCurrency, ref errMsg);
                    criteria.IbcRefNo = ibcRefNo;
                    criteria.IbcTransId = ibcTransId;
                    criteria.IbcUsername = ibcUsername;
                    _paymentService.UpdatePayment(criteria);
                }
            }
            else
            {
                _paymentService.UpdatePayment(criteria);
            }
        }

        #endregion
    }
}