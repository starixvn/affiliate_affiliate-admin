namespace Fanex.AffiliateAdmin.Services
{
    using System;
    using System.Globalization;
    using System.Text;
    using System.Xml.Linq;
    using Core.Services.Impl;
    using Core.Services.Interface;
    using Core.Session;
    using Criterias;
    using DTO.PaymentRequest;

    /// <summary>
    /// Service Wrapper.
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("T4", "1.0.0.0")]
    public class ServiceWrapper
    {
        private static MemberSiteService.ServiceSoapClient _client;
        private static IDbLogger _dbLogger;

        private static MemberSiteService.ServiceSoapClient Client
        {
            get { return _client ?? (_client = new MemberSiteService.ServiceSoapClient("ServiceSoap")); }
        }

        private static IDbLogger DbLogger
        {
            get { return _dbLogger ?? (_dbLogger = new DbLogger()); }
        }

        public static XElement GetRegList(string siteName, DateTime targetDate)
        {
            return Client.GetRegList(siteName, targetDate);
        }

        public static XElement UpdateBtage(string siteName, string licenseeUserName, string newBtag)
        {
            return Client.UpdateBtage(siteName, licenseeUserName, newBtag);
        }

        public static XElement GetLicenseeCustInfo(string siteName, string licenseeUserName)
        {
            return Client.GetLicenseeCustInfo(siteName, licenseeUserName);
        }

        public static ApiCustInfo GetLicenseeCustInfo(string siteName, string licenseeUserName, long transId)
        {
            var custInfo = new ApiCustInfo();

            try
            {
                var xml = Client.GetLicenseeCustInfo(siteName, licenseeUserName);

                var methodName = "GetLicenseeCustInfo";
                var userId = SessionManager.CurrentSession.CustID;
                var paramInfo = $"siteName={siteName}, userName={licenseeUserName}";
                var resultLogContent = BuildLogContentByResult(methodName, userId, transId, paramInfo, xml?.ToString());
                DbLogger.WritePaymentLog(new PaymentLogCriteria { UserId = userId, LogContent = resultLogContent });

                var isValid = IsValidApiResult(xml);

                if (isValid)
                {
                    custInfo.ErrorMessage = xml.Element("ReturnMessage") != null ? xml.Element("ReturnMessage").Value : string.Empty;
                    custInfo.IbcUserName = xml.Element("IBCusername") != null ? xml.Element("IBCusername").Value : string.Empty;
                    custInfo.PlayerCurrency = xml.Element("Currency") != null ? xml.Element("Currency").Value : string.Empty;
                }
            }
            catch (Exception ex)
            {
                Logging.LogService.Error(ex.Message, ex);
            }

            return custInfo;
        }

        public static XElement Transfer(string siteName, string licenseeUserName, double amount, int dummyAccount)
        {
            return Client.Transfer(siteName, licenseeUserName, amount, dummyAccount);
        }

        public static ApiTransferInfo Transfer(string siteName, string licenseeUserName, double amount, int dummyAccount, long transId)
        {
            var transferInfo = new ApiTransferInfo();

            try
            {
                var xml = Client.Transfer(siteName, licenseeUserName, amount, dummyAccount);

                var methodName = "Transfer";
                var userId = SessionManager.CurrentSession.CustID;
                var paramInfo = $"siteName={siteName}, licenseeUserName={licenseeUserName}, Transfer Amount={amount}, dummyAccount={dummyAccount}";
                var resultLogContent = BuildLogContentByResult(methodName, userId, transId, paramInfo, xml?.ToString());
                DbLogger.WritePaymentLog(new PaymentLogCriteria { UserId = userId, LogContent = resultLogContent });

                transferInfo.ErrorMessage = xml?.Element("ReturnMessage")?.Value;
                var isValid = IsValidApiResult(xml);

                if (isValid)
                {
                    transferInfo.IbcRefNo = Convert.ToInt64(xml.Element("IBCRefNo").Value);
                    transferInfo.IbcTransId = Convert.ToInt64(xml.Element("IBCTransId").Value);
                }
            }
            catch (Exception ex)
            {
                Logging.LogService.Error(ex.Message, ex);
            }

            return transferInfo;
        }

        private static string BuildLogContentByResult(string methodName, int userId, long transId, string paramInfo, string resultContent)
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

        private static bool IsValidApiResult(XElement xmlResult)
        {
            if (xmlResult == null || xmlResult.IsEmpty)
            {
                return false;
            }

            var xmlElement = xmlResult.Element("ReturnCode");
            if (xmlElement == null || Convert.ToInt32(xmlElement.Value) != 0)
            {
                return false;
            }

            return true;
        }
    }
}