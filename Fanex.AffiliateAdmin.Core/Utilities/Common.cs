namespace Fanex.AffiliateAdmin.Core.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Resources;
    using System.Text;
    using Fanex.AffiliateAdmin.Resources;
    using Fanex.MailDeliver.MailClient.API;

    public static class Common
    {
        private static readonly ResourceManager _RrrResourceManager = new ResourceManager(typeof(ErrorMsg));

        public static DateTime GetFirstDayOfMonth(DateTime sampleDate, DateTime minDate)
        {
            var firstDay = new DateTime(sampleDate.Year, sampleDate.Month, 1);

            if (firstDay < minDate)
            {
                return minDate;
            }

            return firstDay;
        }

        public static DateTime GetLastDayOfMonth(DateTime sampleDate, DateTime maxDate)
        {
            var firstDay = new DateTime(sampleDate.Year, sampleDate.Month, 1);

            var lastDay = firstDay.AddMonths(1).AddDays(-1);

            if (lastDay > maxDate)
            {
                return maxDate;
            }

            return lastDay;
        }

        public static void SendEmail(string message, string mailTo, string subject, string mailBcc = "", string mailCC = "")
        {
            var mailItem = new Fanex.MailDeliver.Model.MailRecord
            {
                MailTo = mailTo,
                Subject = subject,
                Title = subject,
                Body = message,
                MailBcc = mailBcc,
                MailCc = mailCC,
            };

            using (var client = new MailDeliver())
            {
                client.SendMail(mailItem);
            }
        }

        public static string GetErrorMsg(int errCode)
        {
            var errMsg = _RrrResourceManager.GetString(string.Format("Msg{0}", errCode));
            return string.IsNullOrEmpty(errMsg) ? string.Empty : errMsg;
        }

        public static void DeleteFiles(IEnumerable<string> filePaths)
        {
            foreach (var file in filePaths)
            {
                DeleteFile(file);
            }
        }

        public static void DeleteFile(string filePath)
        {
            var fileInfo = new FileInfo(filePath);
            if (fileInfo.Exists)
            {
                fileInfo.Delete();
            }
        }

        public static void MoveFile(string oldFilePath, string newFilePath)
        {
            var directory = Path.GetDirectoryName(newFilePath);

            if (directory != null && Directory.Exists(directory) == false)
            {
                Directory.CreateDirectory(directory);
            }

            DeleteFile(newFilePath);

            File.Move(oldFilePath, newFilePath);
        }

        public static bool AnyFileExists(IEnumerable<string> filePaths)
        {
            foreach (var file in filePaths)
            {
                if (File.Exists(file))
                {
                    return true;
                }
            }

            return false;
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
    }
}