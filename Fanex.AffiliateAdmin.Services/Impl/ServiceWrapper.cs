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
    using System.Threading.Tasks;
    using Fanex.AffiliateAdmin.Core.Configurations;

    /// <summary>
    /// Service Wrapper.
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("T4", "1.0.0.0")]
    public class ServiceWrapper
    {
        private static readonly Lazy<ISagaApi> _instance = new Lazy<ISagaApi>(() =>
        {
            try
            {
                var apiUrl = AppSettings.SagaApiUrl;
                if (string.IsNullOrEmpty(apiUrl))
                {
                    throw new ConfigurationErrorsException("SagaApiUrl is not configured in AppSettings");
                }

                var isUsingProxy = AppSettings.IsUseProxyToGetSagaApi;
                var httpClientHandler = new HttpClientHandler
                {
                    Proxy = isUsingProxy ? GetConfiguredProxy() : null,
                    AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
                    UseCookies = false
                };

                var httpClient = new HttpClient(httpClientHandler)
                {
                    BaseAddress = new Uri(apiUrl),
                    Timeout = TimeSpan.FromSeconds(60)
                };

                return RestService.For<ISagaApi>(httpClient);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to initialize SagaApi client", ex);
            }
        });

        private static ISagaApi SagaApi => _instance.Value;

        public static async Task<XElement> GetRegList(string siteName, DateTime targetDate)
        {
            var param = new RegParam
            {
                StrSiteName = siteName,
                DtimeTargetDate = targetDate
            };

            var xmlString = await SagaApi.GetRegList(param);
            return ParseXml(xmlString);
        }

        public static async Task<XElement> UpdateBtage(string siteName, string licenseeUserName, string newBtag)
        {
            var param = new UpdateBtageParam
            {
                StrSiteName = siteName,
                StrLicenseeUserName = licenseeUserName,
                StrNewBtag = newBtag
            };

            var xmlString = await SagaApi.UpdateBtage(param);
            return ParseXml(xmlString);
        }

        public static async Task<XElement> GetLicenseeCustInfo(string siteName, string licenseeUserName)
        {
            var param = new LicenseCustInfoParam
            {
                StrSiteName = siteName,
                StrLicenseeUserName = licenseeUserName
            };

            var xmlString = await SagaApi.GetLicenseeCustInfo(param);
            return ParseXml(xmlString);
        }

        public static async Task<ApiCustInfo> GetLicenseeCustInfo(string siteName, string licenseeUserName, long transId)
        {
            var custInfo = new ApiCustInfo();

            try
            {
                var xml = await GetLicenseeCustInfo(siteName, licenseeUserName);
                var isValid = IsValidApiResult(xml);

                if (isValid)
                {
                    custInfo.IbcUserName = xml.Element("IBCusername")?.Value;
                    custInfo.PlayerCurrency = xml.Element("Currency")?.Value;
                }
                else
                {
                    await WriteLogAfterGettingCustInfo(siteName, licenseeUserName, transId, xml);
                    custInfo.ErrorMessage = xml.Element("ReturnMessage")?.Value;
                }
            }
            catch (Exception ex)
            {
                await WriteLogAfterGettingCustInfo(siteName, licenseeUserName, transId);
                LogHelper.WriteError(ex);
            }

            return custInfo;
        }

        public static async Task<XElement> Transfer(string siteName, string licenseeUserName, double amount, int dummyAccount)
        {
            var param = new TranferParam
            {
                SiteName = siteName,
                LicenseeUserName = licenseeUserName,
                Amount = amount,
                DummyAccount = dummyAccount
            };

            var xmlString = await SagaApi.Transfer(param);
            return ParseXml(xmlString);
        }

        public static async Task<ApiTransferInfo> Transfer(string siteName, string licenseeUserName, double amount, int dummyAccount, long transId)
        {
            var transferInfo = new ApiTransferInfo();

            try
            {
                var xml = await Transfer(siteName, licenseeUserName, amount, dummyAccount);
                var isValid = IsValidApiResult(xml);

                if (isValid)
                {
                    transferInfo.IbcRefNo = Convert.ToInt64(xml.Element("IBCRefNo")?.Value);
                    transferInfo.IbcTransId = Convert.ToInt64(xml.Element("IBCTransId")?.Value);
                }
                else
                {
                    await WriteLogAfterTransfering(siteName, licenseeUserName, amount, dummyAccount, transId, xml);
                    transferInfo.ErrorMessage = xml.Element("ReturnMessage")?.Value;
                }
            }
            catch (Exception ex)
            {
                await WriteLogAfterTransfering(siteName, licenseeUserName, amount, dummyAccount, transId);
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

            var returnCode = xmlResult.Element("ReturnCode")?.Value;
            return returnCode != null && Convert.ToInt32(returnCode) == 0;
        }

        private static async Task WriteLogAfterGettingCustInfo(string siteName, string licenseeUserName, long transId, XElement xml = null)
        {
            var methodName = "GetLicenseeCustInfo";
            var paramInfo = $"siteName={siteName},userName={licenseeUserName}";
            await PaymentLogHelper.WriteLogAsync(methodName, paramInfo, transId, xml?.ToString());
        }

        private static async Task WriteLogAfterTransfering(string siteName, string licenseeUserName, double amount, int dummyAccount, long transId, XElement xml = null)
        {
            var methodName = "Transfer";
            var paramInfo = $"siteName={siteName},licenseeUserName={licenseeUserName},Transfer Amount={amount},dummyAccount={dummyAccount}";
            await PaymentLogHelper.WriteLogAsync(methodName, paramInfo, transId, xml?.ToString());
        }

        private static IWebProxy GetConfiguredProxy()
        {
            var proxyUrl = AppSettings.ApiProxyUrl;
            if (string.IsNullOrEmpty(proxyUrl))
            {
                throw new ConfigurationErrorsException("ApiProxyUrl is not configured in AppSettings when proxy is enabled");
            }

            return new WebProxy(new Uri(proxyUrl));
        }

        private static XElement ParseXml(string xmlString)
        {
            try
            {
                return XDocument.Parse(xmlString).Root;
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex);

                return null;
            }
        }
    }
}