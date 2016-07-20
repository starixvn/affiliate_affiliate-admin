namespace Fanex.AffiliateAdmin.Core.Mailinator
{
    using System;
    using System.IO;
    using System.Web;
    using Fanex.AffiliateAdmin.Core.Configurations;
    using Fanex.AffiliateAdmin.Core.Entities;
    using Fanex.AffiliateAdmin.Core.Enumerations;
    using Fanex.AffiliateAdmin.Core.Utilities;
    using RazorEngine;
    using RazorEngine.Templating;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AffiliateMailinator
    {
        public HttpServerUtilityBase Server { get; set; }

        public string Language { get; set; }

        public EmailTemplateType EmailTemplateType { get; set; }

        public EmailFormatType FormatType { get; set; }

        public EmailTemplateInfo EmailTemplateInfo { get; set; }

        public string EmailOfAffiliate { get; set; }

        public string SubjectPath { get; set; }

        public string BodyPath { get; set; }

        public static AffiliateMailinator GetMailinator(string affTypeName)
        {
            if (affTypeName.Equals("normal", StringComparison.CurrentCultureIgnoreCase))
            {
                return new NormalAffiliateMailinator();
            }

            return new KioskAffiliateMailinator();
        }

        /// <summary>
        /// Send email to affiliate.
        /// </summary>
        public void Send() 
        {
            if (EmailTemplateType == EmailTemplateType.AffiliateApprove)
            {
                Approve();
            }
            else
            {
                var emailTemplate = new EmailTemplate(EmailTemplateType, FormatType, Language);

                emailTemplate.ResolvePath(Server).Send(EmailOfAffiliate, EmailTemplateInfo);
            }
        }
       
        public virtual void Approve()
        {
            // 1. ApprovalEmail
            ResolvePath("ApprovalEmail");
            Send(EmailOfAffiliate, EmailTemplateInfo);

            // 2. ApprovalEmail
            ResolvePath("Approved");
            Send(EmailOfAffiliate, EmailTemplateInfo);
        }

        /// <summary>
        /// Resolve Path.
        /// </summary>
        /// <param name="emailName">Template Email.</param>
        protected void ResolvePath(string emailName)
        {
            Language = string.IsNullOrEmpty(Language) ? "en-US" : Language;
            
            var virtualSubjectPath = string.Format(
                              "~/Views/Shared/Email/{0}/Subject.{1}.{2}.cshtml",
                              EmailTemplateType.ToString(),
                              emailName,
                              Language);

            var virtualBodyPath = string.Format(
                "~/Views/Shared/Email/{0}/{1}/Body.{2}.{3}.cshtml",
                EmailTemplateType.ToString(), 
                FormatType.ToString(),
                emailName,
                Language);

            SubjectPath = Server.MapPath(virtualSubjectPath);
            BodyPath = Server.MapPath(virtualBodyPath);
        }

        protected void Send(string mailTo, EmailTemplateInfo info)
        {
            var emailSubject = ParseTemplate(SubjectPath, info);
            var emailBody = ParseTemplate(BodyPath, info);
            Utilities.Common.SendEmail(emailBody, mailTo, emailSubject, AppSettings.ITMail);
        }

        /// <summary>
        /// Fill value by Razor view engine.
        /// </summary>
        /// <param name="templatePath">Template Path.</param>
        /// <param name="info">Email template info.</param>
        /// <returns>Return content after replacing.</returns>
        private string ParseTemplate(string templatePath, EmailTemplateInfo info)
        {
            var template = File.ReadAllText(templatePath);
            return Engine.Razor.RunCompile(template, templatePath, typeof(EmailTemplateInfo), info);
        }
    }
}
