using System;
using System.Web.Mvc;
using Fanex.AffiliateAdmin.Core.Configurations;
using Fanex.AffiliateAdmin.Core.Entities;
using Fanex.AffiliateAdmin.Core.Enumerations;
using Fanex.AffiliateAdmin.Core.Filter;
using Fanex.AffiliateAdmin.Core.Mailinator;
using Fanex.AffiliateAdmin.Core.Services.Interface;
using Fanex.AffiliateAdmin.Core.Session;
using Fanex.AffiliateAdmin.Core.Utilities;

namespace Fanex.AffiliateAdmin.Controller
{
    [FunctionAuth(Function = FunctionPolicyNames.Affiliate, CurrenciesRequired = true)]
    public class EmailTestingController : BaseController
    {
        public EmailTestingController(ISystemSettingService systemSettingService) : base(systemSettingService)
        {
        }

        [HttpGet]
        public ActionResult Index()
        {
            if (!IsValidUser())
            {
                return HttpNotFound();
            }

            return View();
        }

        [HttpPost]
        public ActionResult SendEmail(TestCriteria criteria)
        {
            if (!IsValidUser())
            {
                return HttpNotFound();
            }

            var success = true;
            var message = string.Empty;

            try
            {
                if (IsPayment(criteria.Function))
                {
                    SendPaymentEmail(criteria);
                }
                else if (IsChangeStatus(criteria.Function))
                {
                    SendMailUpdatingStatus(criteria);
                }
            }
            catch (Exception ex)
            {
                success = false;
                message = ex.Message + "\n" + ex.StackTrace;
            }

            return Json(new
            {
                Success = success,
                Message = message
            });
        }

        private bool IsValidUser()
        {
            return AppSettings.LocalTest || SessionManager.CurrentSession.IsIT;
        }

        private void SendPaymentEmail(TestCriteria criteria)
        {
            var emailType = string.Equals("PaymentApprove", criteria.Function) ? EmailTemplateType.PaymentApprove : EmailTemplateType.PaymentCancel;
            var emailTemplate = new EmailTemplate(emailType, criteria.Language, IsTextFormat(criteria.Format));
            var siteInfo = SiteDomainInfo.Parse(this, false);
            var emailTemplateInfo = new EmailTemplateInfo
            {
                TransId = 123,
                FirstName = "Walter",
                WinlostDate = DateTime.Now,
                Amount = FormaterCurrency.Format(12597, AppSettings.DefaultCurrency),
                AdminSiteUrl = siteInfo.AdminSiteUrl,
                AffiliateSiteUrl = siteInfo.AffiliateSiteUrl,
                MemberSiteUrl = siteInfo.MemberSiteUrl,
                MemberSiteDomain = siteInfo.MemberSiteDomain
            };

            emailTemplate.ResolvePath(Server).Send(criteria.EmailAddress, emailTemplateInfo);
        }

        private static bool IsPayment(string functionName)
        {
            return string.Equals("PaymentApprove", functionName) || string.Equals("PaymentCancel", functionName);
        }

        public void SendMailUpdatingStatus(TestCriteria criteria)
        {
            var status = string.Equals("AffiliateApprove", criteria.Function) ? "approved" : "disapproved";
            var siteInfo = SiteDomainInfo.Parse(this, false);
            var mailinator = AffiliateMailinator.GetMailinator("normal");
            mailinator.Server = Server;
            mailinator.EmailTemplateType = status.Equals("approved", System.StringComparison.CurrentCultureIgnoreCase) ?
                                EmailTemplateType.AffiliateApprove :
                                EmailTemplateType.AffiliateDisapprove;
            mailinator.Language = criteria.Language;
            mailinator.FormatType = IsTextFormat(criteria.Format) ? EmailFormatType.Text : EmailFormatType.Html;
            mailinator.EmailOfAffiliate = criteria.EmailAddress;
            mailinator.EmailTemplateInfo = new EmailTemplateInfo
            {
                Status = status,
                Username = "UserNameTest",
                FirstName = "Walter",
                Password = "123459876",
                AdminSiteUrl = siteInfo.AdminSiteUrl,
                AffiliateSiteUrl = siteInfo.AffiliateSiteUrl,
                MemberSiteUrl = siteInfo.MemberSiteUrl,
                MemberSiteDomain = siteInfo.MemberSiteDomain
            };

            mailinator.Send();
        }

        private static bool IsChangeStatus(string functionName)
        {
            return string.Equals("AffiliateApprove", functionName) || string.Equals("AffiliateDisapprove", functionName);
        }

        private static bool IsTextFormat(string format)
        {
            return format == "txt";
        }
    }

    public class TestCriteria
    {
        public string Language { get; set; }

        public string Function { get; set; }

        public string Format { get; set; }

        public string EmailAddress { get; set; }
    }
}
