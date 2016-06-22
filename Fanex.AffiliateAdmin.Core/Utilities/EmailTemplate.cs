namespace Fanex.AffiliateAdmin.Core.Utilities
{
    using System.IO;
    using System.Web;
    using Fanex.AffiliateAdmin.Core.Configurations;
    using Fanex.AffiliateAdmin.Core.Entities;
    using Fanex.AffiliateAdmin.Core.Enumerations;
    using RazorEngine;
    using RazorEngine.Templating;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class EmailTemplate
    {
        private string _subjectPath;
        private string _bodyPath;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailTemplate"/> class.
        /// </summary>
        /// <param name="templateType">Template Type.</param>
        /// <param name="formatType">Format Type.</param>
        /// <param name="language">Language of email.</param>
        public EmailTemplate(EmailTemplateType templateType, EmailFormatType formatType, string language)
        {
            TemplateType = templateType;
            FormatType = formatType;
            Language = language;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailTemplate"/> class.
        /// </summary>
        /// <param name="templateType">Template Type.</param>
        /// <param name="language">Language of email.</param>
        /// <param name="mailOption">Mail Option.</param>
        public EmailTemplate(EmailTemplateType templateType, string language, bool mailOption)
            : this(templateType, mailOption ? EmailFormatType.Text : EmailFormatType.Html, language)
        {
        }

        public string Language { get; private set; }

        public EmailTemplateType TemplateType { get; private set; }

        public EmailFormatType FormatType { get; private set; }

        /// <summary>
        /// Resolve Path.
        /// </summary>
        /// <param name="server">Http Server Utility.</param>
        /// <returns>Return correct path of email template.</returns>
        public EmailTemplate ResolvePath(HttpServerUtilityBase server)
        {
            var language = "en-US";
            if (!string.IsNullOrEmpty(this.Language))
            {
                language = this.Language;
            }

            var virtualSubjectPath = string.Format(
                              "~/Views/Shared/Email/{0}/Subject.{1}.cshtml",
                              TemplateType.ToString(),
                              language);
            var virtualBodyPath = string.Format(
                "~/Views/Shared/Email/{0}/{1}/Body.{2}.cshtml",
                TemplateType.ToString(),
                FormatType.ToString(),
                language);

            _subjectPath = server.MapPath(virtualSubjectPath);
            _bodyPath = server.MapPath(virtualBodyPath);

            return this;
        }

        /// <summary>
        /// Send Email.
        /// </summary>
        /// <param name="mailTo">Mail To someone.</param>
        /// <param name="info">Email template info.</param>
        public void Send(string mailTo, EmailTemplateInfo info)
        {
            var emailSubject = ParseTemplate(_subjectPath, info);
            var emailBody = ParseTemplate(_bodyPath, info);
            Common.SendEmail(emailBody, mailTo, emailSubject, AppSettings.ITMail);
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