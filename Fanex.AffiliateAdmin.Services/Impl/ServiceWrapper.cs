namespace Fanex.AffiliateAdmin.Services
{
    using System;
    using System.Xml.Linq;
    using DTO.PaymentRequest;
    using Helpers;

    /// <summary>
    /// Service Wrapper.
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("T4", "1.0.0.0")]
    public class ServiceWrapper
    {
        private static MemberSiteService.ServiceSoapClient _client;

        private static MemberSiteService.ServiceSoapClient Client
        {
            get { return _client ?? (_client = new MemberSiteService.ServiceSoapClient("ServiceSoap")); }
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
                WriteLogAfterGettingCustInfo(siteName, licenseeUserName, transId, xml);

                var isValid = IsValidApiResult(xml);

                if (isValid)
                {
                    custInfo.IbcUserName = xml.Element("IBCusername")?.Value;
                    custInfo.PlayerCurrency = xml.Element("Currency")?.Value;
                }
                else
                {
                    custInfo.ErrorMessage = xml.Element("ReturnMessage")?.Value;
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
                WriteLogAfterTransfering(siteName, licenseeUserName, amount, dummyAccount, transId, xml);

                var isValid = IsValidApiResult(xml);

                if (isValid)
                {
                    transferInfo.IbcRefNo = Convert.ToInt64(xml.Element("IBCRefNo").Value);
                    transferInfo.IbcTransId = Convert.ToInt64(xml.Element("IBCTransId").Value);
                }
                else
                {
                    transferInfo.ErrorMessage = xml.Element("ReturnMessage")?.Value;
                }
            }
            catch (Exception ex)
            {
                Logging.LogService.Error(ex.Message, ex);
            }

            return transferInfo;
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

        private static void WriteLogAfterGettingCustInfo(string siteName, string licenseeUserName, long transId, XElement xml)
        {
            var methodName = "GetLicenseeCustInfo";
            var paramInfo = $"siteName={siteName},userName={licenseeUserName}";
            PaymentLogHelper.WriteLog(methodName, paramInfo, transId, xml?.ToString());
        }

        private static void WriteLogAfterTransfering(string siteName, string licenseeUserName, double amount, int dummyAccount, long transId, XElement xml)
        {
            var methodName = "Transfer";
            var paramInfo = $"siteName={siteName},licenseeUserName={licenseeUserName},Transfer Amount={amount},dummyAccount={dummyAccount}";
            PaymentLogHelper.WriteLog(methodName, paramInfo, transId, xml?.ToString());
        }
    }
}