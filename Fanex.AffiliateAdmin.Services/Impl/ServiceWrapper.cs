namespace Fanex.AffiliateAdmin.Services
{
    using System;
    using System.Configuration;
    using System.Net.Http;
    using System.Net;
    using System.Xml.Linq;
    using DTO.PaymentRequest;
    using Fanex.AffiliateAdmin.Core.Helpers;
    using Fanex.AffiliateAdmin.Services.Api;
    using Helpers;
    using Refit;

    /// <summary>
    /// Service Wrapper.
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("T4", "1.0.0.0")]
    public class ServiceWrapper
    {
        private static ISagaApi _sagaApi;

        private static readonly Lazy<ISagaApi> _instance = new Lazy<ISagaApi>(() =>
        {
            try
            {
                var apiUrl = ConfigurationManager.AppSettings["SagaApiUrl"];
                if (string.IsNullOrEmpty(apiUrl))
                {
                    throw new ConfigurationErrorsException("SagaApiUrl is not configured in AppSettings");
                }

                var isUsingProxy = Convert.ToBoolean(ConfigurationManager.AppSettings["IsUseProxyToGetSagaApi"]);
                var httpClientHandler = new HttpClientHandler
                {
                    Proxy = isUsingProxy ? GetConfiguredProxy() : null,
                    AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
                    UseCookies = false
                };

                var httpClient = new HttpClient(httpClientHandler)
                {
                    BaseAddress = new Uri(apiUrl)
                };

                return RestService.For<ISagaApi>(httpClient);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to initialize SagaApi client", ex);
            }
        });

        private static ISagaApi SagaApi
        {
            get {
                return _sagaApi ?? (_sagaApi = _instance.Value);
            }
        }

        public static XElement GetRegList(string siteName, DateTime targetDate)
        {
            var param = new RegParam
            {
                StrSiteName = siteName,
                DtimeTargetDate = targetDate
            };

            var rawXml = SagaApi.GetRegList(param).Result;

            return GetXElement(rawXml);
        }

        public static XElement UpdateBtage(string siteName, string licenseeUserName, string newBtag)
        {
            var param = new UpdateBtageParam
            {
                StrSiteName = siteName,
                StrLicenseeUserName = licenseeUserName,
                StrNewBtag = newBtag
            };

            var rawXml = SagaApi.UpdateBtage(param).Result;

            return GetXElement(rawXml);
        }

        public static XElement GetLicenseeCustInfo(string siteName, string licenseeUserName)
        {
            var param = new LicenseCustInfoParam
            {
                StrSiteName = siteName,
                StrLicenseeUserName = licenseeUserName
            };

            var rawXml = SagaApi.GetLicenseeCustInfo(param).Result;

            return GetXElement(rawXml);
        }

        public static ApiCustInfo GetLicenseeCustInfo(string siteName, string licenseeUserName, long transId)
        {
            var custInfo = new ApiCustInfo();

            try
            {
                var xml = GetLicenseeCustInfo(siteName, licenseeUserName);
                var isValid = IsValidApiResult(xml);

                if (isValid)
                {
                    custInfo.IbcUserName = xml.Element("IBCusername")?.Value;
                    custInfo.PlayerCurrency = xml.Element("Currency")?.Value;
                }
                else
                {
                    WriteLogAfterGettingCustInfo(siteName, licenseeUserName, transId, xml);
                    custInfo.ErrorMessage = xml.Element("ReturnMessage")?.Value;
                }
            }
            catch (Exception ex)
            {
                WriteLogAfterGettingCustInfo(siteName, licenseeUserName, transId);
                LogHelper.WriteError(ex);
            }

            return custInfo;
        }

        public static XElement Transfer(string siteName, string licenseeUserName, double amount, int dummyAccount)
        {
            var param = new TranferParam
            {
                SiteName = siteName,
                LicenseeUserName = licenseeUserName,
                Amount = amount,
                DummyAccount = dummyAccount
            };

            var rawXml = SagaApi.Transfer(param).Result;

            return GetXElement(rawXml);
        }

        public static ApiTransferInfo Transfer(string siteName, string licenseeUserName, double amount, int dummyAccount, long transId)
        {
            var transferInfo = new ApiTransferInfo();

            try
            {
                var xml = Transfer(siteName, licenseeUserName, amount, dummyAccount);
                var isValid = IsValidApiResult(xml);

                if (isValid)
                {
                    transferInfo.IbcRefNo = Convert.ToInt64(xml.Element("IBCRefNo").Value);
                    transferInfo.IbcTransId = Convert.ToInt64(xml.Element("IBCTransId").Value);
                }
                else
                {
                    WriteLogAfterTransfering(siteName, licenseeUserName, amount, dummyAccount, transId, xml);
                    transferInfo.ErrorMessage = xml.Element("ReturnMessage")?.Value;
                }
            }
            catch (Exception ex)
            {
                WriteLogAfterTransfering(siteName, licenseeUserName, amount, dummyAccount, transId);
                LogHelper.WriteError(ex);
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

        private static void WriteLogAfterGettingCustInfo(string siteName, string licenseeUserName, long transId, XElement xml = null)
        {
            var methodName = "GetLicenseeCustInfo";
            var paramInfo = $"siteName={siteName},userName={licenseeUserName}";
            PaymentLogHelper.WriteLog(methodName, paramInfo, transId, xml?.ToString());
        }

        private static void WriteLogAfterTransfering(string siteName, string licenseeUserName, double amount, int dummyAccount, long transId, XElement xml = null)
        {
            var methodName = "Transfer";
            var paramInfo = $"siteName={siteName},licenseeUserName={licenseeUserName},Transfer Amount={amount},dummyAccount={dummyAccount}";
            PaymentLogHelper.WriteLog(methodName, paramInfo, transId, xml?.ToString());
        }

        private static IWebProxy GetConfiguredProxy()
        {
            var proxyUrl = ConfigurationManager.AppSettings["ApiProxyUrl"];
            if (string.IsNullOrEmpty(proxyUrl))
            {
                throw new ConfigurationErrorsException("ApiProxyUrl is not configured in AppSettings when proxy is enabled");
            }

            return new WebProxy(new Uri(proxyUrl));
        }

        private static XElement GetXElement(string xmlString)
        {
            try
            {
                return XDocument.Parse(xmlString).Root;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}