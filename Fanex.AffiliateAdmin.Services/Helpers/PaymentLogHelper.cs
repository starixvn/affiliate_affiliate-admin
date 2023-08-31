namespace Fanex.AffiliateAdmin.Services.Helpers
{
    using System;
    using System.Globalization;
    using System.Text;
    using Core.Session;
    using Criterias;
    using Fanex.AffiliateAdmin.Core.Helpers;
    using Fanex.AffiliateAdmin.Core.Services.Impl;
    using Fanex.AffiliateAdmin.Core.Services.Interface;

    public static class PaymentLogHelper
    {
        private static IDbLogger _logger;

        private static IDbLogger DbLogger
        {
            get { return _logger ?? (_logger = new DbLogger()); }
        }

        public static void WriteLog(string methodName, string paramInfo, long transId, string xmlData)
        {
            var userId = SessionManager.CurrentSession.CustID;
            var resultLogContent = BuildLogContentByResult(methodName, userId, transId, paramInfo, xmlData);
            LogHelper.WriteInfo(resultLogContent);
            DbLogger.WritePaymentLog(new PaymentLogCriteria { UserId = userId, LogContent = resultLogContent });
        }

        private static string BuildLogContentByResult(string methodName, int userId, long transId, string paramInfo, string resultContent)
        {
            var builder = new StringBuilder();

            var timestamp = DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
            builder.AppendLine($"Complete Call Method {methodName} at {timestamp}");
            builder.AppendLine("Infor:");
            builder.AppendLine($"TransId: {transId}");
            builder.AppendLine($"UserId: {userId}");
            builder.AppendLine($"Param Info: {paramInfo}");

            builder.AppendLine($"Result Content: {resultContent ?? string.Empty}");

            return builder.ToString();
        }
    }
}