// --------------------------------------------------------------------------
// <copyright file="Common.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.06.25">Created</change>
//   <change who="Leo.Vo" date="2013.06.30">GetLanguageOriginal</change>
// </history>

namespace Fanex.AffiliateAdmin.Core.Utilities
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Resources;
    using System.Text;
    using Fanex.AffiliateAdmin.Core.Configurations;
    using Fanex.AffiliateAdmin.Core.Enumerations;
    using Fanex.AffiliateAdmin.Resources;
    using Fanex.MailDeliver.MailClient.API;
    using RazorEngine;

    /// <summary>
    /// Common class.
    /// </summary>
    public static class Common
    {
        /// <summary>
        /// The lang resource manager.
        /// </summary>
        private static readonly ResourceManager _RrrResourceManager = new ResourceManager(typeof(ErrorMsg));

        /// <summary>
        /// Convert to date.
        /// </summary>
        /// <param name="dtmTime">Date Time.</param>
        /// <param name="slash">Separator character.</param>
        /// <param name="fullDate">Format full date.</param>
        /// <param name="hasTime">Has time or not.</param>
        /// <returns>Return string of date time.</returns>
        public static string ConvertDateTime(object dtmTime, string slash, bool fullDate, bool hasTime)
        {
            var culture = new CultureInfo("en-US");
            DateTime date = Convert.ToDateTime(dtmTime, culture);
            if (fullDate)
            {
                return date.ToString("MM" + slash + "dd" + slash) + date.ToString("yyyy", culture)
                    + (hasTime ? " " + date.ToString("hh:mm:ss") : string.Empty);
            }
            else
            {
                return date.ToString("MMMM" + slash) + date.ToString("yyyy", culture);
            }
        }

        /// <summary>
        /// Get First Day of Month.
        /// </summary>
        /// <param name="sampleDate">Sample Date.</param>
        /// <param name="minDate">The Min Date.</param>
        /// <param name="maxDate">The Max Date.</param>
        /// <returns>The First Day of Month.</returns>
        public static DateTime GetFirstDayOfMonth(DateTime sampleDate, DateTime minDate, DateTime maxDate)
        {
            var firstDay = new DateTime(sampleDate.Year, sampleDate.Month, 1);

            if (firstDay < minDate)
            {
                firstDay = minDate;
            }

            if (firstDay > maxDate)
            {
                firstDay = maxDate;
            }

            return firstDay;
        }

        /// <summary>
        /// Get Last Day of Month.
        /// </summary>
        /// <param name="sampleDate">Sample Date.</param>
        /// <param name="minDate">The Min Date.</param>
        /// <param name="maxDate">The Max Date.</param>
        /// <returns>The Last Day of Month.</returns>
        public static DateTime GetLastDayOfMonth(DateTime sampleDate, DateTime minDate, DateTime maxDate)
        {
            var firstDay = new DateTime(sampleDate.Year, sampleDate.Month, 1);

            var lastDay = firstDay.AddMonths(1).AddDays(-1);

            if (lastDay < minDate)
            {
                lastDay = minDate;
            }

            if (lastDay > maxDate)
            {
                lastDay = maxDate;
            }

            return lastDay;
        }

        /// <summary>
        /// Gets status name.
        /// </summary>
        /// <param name="status">Status need to translate.</param>
        /// <returns>Return status.</returns>
        public static string GetStatus(string status)
        {
            switch (status.ToLower())
            {
                case "running":
                case "pending":
                    return Lang.Processing;
                case "completed":
                    return Lang.Approved;
                case "cancelled":
                    return Lang.Cancelled;
                case "approved":
                    return Lang.Approved;
                case "rejected":
                    return Lang.Rejected;
                case "current":
                    return Lang.Current;
                default:
                    return status;
            }
        }

        /// <summary>
        /// Get email body use RAZOR.
        /// </summary>
        /// <param name="templatePath">Template Path.</param>
        /// <param name="model">Model to bind data.</param>
        /// <returns>Body content of email.</returns>
        public static string GetEmailBody(string templatePath, dynamic model)
        {
            try
            {
                var template = File.ReadAllText(templatePath);
                return Razor.Parse(template, model);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Send Email message.
        /// </summary>
        /// <param name="message">Message body.</param>
        /// <param name="mailTo">Mail To someone.</param>
        /// <param name="subject">Subject of Email.</param>
        /// <param name="mailBcc">Mail Bcc someone.</param>
        /// <param name="mailCC">Mail CC someone.</param>
        public static void SendEmail(string message, string mailTo, string subject, string mailBcc = "", string mailCC = "")
        {
            var mailItem = new Fanex.MailDeliver.Model.MailRecord
            {
                MailTo = mailTo,
                Subject = subject,
                Title = subject,
                ////IsBodyHtml ready
                Body = message,
                MailBcc = mailBcc,
                MailCc = mailCC,
            };

            using (var client = new MailDeliver())
            {
                client.SendMail(mailItem);
            }
        }

        /// <summary>
        /// Gets language name from language code.
        /// </summary>
        /// <param name="strLangCode">Language code.</param>
        /// <returns>Return original language.</returns>
        public static string GetLanguageOriginal(string strLangCode)
        {
            switch (strLangCode.ToLower())
            {
                case "en-us": return "English";
                case "eneuro": return "English_Euro";
                case "zh-tw": return "繁體中文";
                case "zh-cn": return "简体中文";
                case "th-th": return "ภาษาไทย";
                case "es-es": return "Español";
                case "vi-vn":
                case "vi": return "Tiếng Việt";
                case "id-id": return "Indonesian";
                default:
                    return "en";
            }
        }

        /// <summary>
        /// Gets error message from Language resource by error code.
        /// </summary>
        /// <param name="errCode">Error code.</param>
        /// <returns>Return error message.</returns>
        public static string GetErrorMsg(int errCode)
        {
            var errMsg = _RrrResourceManager.GetString(string.Format("Msg{0}", errCode));
            return string.IsNullOrEmpty(errMsg) ? string.Empty : errMsg;
        }

        /// <summary>
        /// Cut long text.
        /// </summary>
        /// <param name="strText">Content need to cut.</param>
        /// <param name="maxLength">Maximum Number of Char.</param>
        /// <returns>Return short text with title.</returns>
        public static string CutLongText(string strText, int maxLength)
        {
            if (strText.Length <= maxLength - 3)
            {
                return strText;
            }

            return strText.Substring(0, maxLength - 3) + "...";
        }

        /// <summary>
        /// Gets Status of affiliate.
        /// </summary>
        /// <param name="status">Status need to translate.</param>
        /// <returns>Return status.</returns>
        public static string GerStatus(string status)
        {
            switch (status.ToLower())
            {
                case "pending":
                    return Lang.Processing;
                case "completed":
                    return Lang.Approved;
                case "cancelled":
                    return Lang.Cancelled;
                case "approved":
                    return Lang.Approved;
                case "rejected":
                    return Lang.Rejected;
                default:
                    return status;
            }
        }

        /// <summary>
        /// Gets payment color.
        /// </summary>
        /// <param name="status">Status of payment.</param>
        /// <returns>Return style sheet class of payment.</returns>
        public static string GetPaymentCss(string status)
        {
            switch (status.ToLower())
            {
                case "pending":
                    return "affPending";
                case "cancelled":
                    return "affCancelled";
                case "rejected":
                    return "affRejected";
                default:
                    return string.Empty;
            }
        }

        /// <summary>
        /// Delete a file.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        public static void DeleteFile(string filePath)
        {
            var fileInfo = new FileInfo(filePath);
            if (fileInfo.Exists)
            {
                fileInfo.Delete();
            }
        }

        /// <summary>
        /// Move a file to a new position.
        /// </summary>
        /// <param name="oldFilePath">The old file path.</param>
        /// <param name="newFilePath">The new file path.</param>
        public static void MoveFile(string oldFilePath, string newFilePath)
        {
            var directory = Path.GetDirectoryName(newFilePath);

            if (directory != null && Directory.Exists(directory) == false)
            {
                Directory.CreateDirectory(directory);
            }

            // Delete it if exist.
            DeleteFile(newFilePath);
            File.Move(oldFilePath, newFilePath);
        }

        /// <summary>
        /// Get currency notification if multiplied > 1.
        /// </summary>
        /// <param name="currencycode">Currency code.</param>
        /// <param name="multipliedby">Multiplied By.</param>
        /// <returns>Notification Message.</returns>
        public static string GetCurrencyNotification(string currencycode, int multipliedby)
        {
            var sampleValue = 2;
            if (multipliedby == 1)
            {
                return string.Empty;
            }

            var s = new StringBuilder("<p>").AppendFormat(
                Lang.PleaseInput1For,
                multipliedby,
                currencycode,
                FormaterCurrency.Format(sampleValue, currencycode),
                FormaterCurrency.Format(sampleValue * multipliedby, currencycode));
            return s.Append("</p>").ToString();
        }

        /// <summary>
        /// Get report information.
        /// </summary>
        /// <param name="reportType">Report type.</param>
        /// <returns>Report information.</returns>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Accept this function")]
        public static string GetReportInfo(ReportType reportType)
        {
            var hasPlayCasino = AppSettings.Products.Any(t => t.ProductName.Equals("Playtech Casino"));

            switch (reportType)
            {
                case ReportType.CompanyWinLoss:
                case ReportType.MemberWinLoss:
                    return string.Format(Lang.CompanyWinLossReportInfo, hasPlayCasino ? Lang.PCNetGamingInfo : string.Empty);
                case ReportType.AffiliateWinLoss:
                    return string.Format(Lang.AffiliateWinLossReportInfo, hasPlayCasino ? Lang.PCNetGamingInfo : string.Empty);
                case ReportType.AffiliateCommission:
                    return Lang.AffiliateCommissionReportInfo;
                default:
                    return string.Empty;
            }
        }
    }
}