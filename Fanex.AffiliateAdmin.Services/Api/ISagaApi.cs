using Refit;
using System;
using System.Threading.Tasks;

namespace Fanex.AffiliateAdmin.Services.Api
{
    public interface ISagaApi
    {
        [Post("/GetLicenseeCustInfo")]
        Task<string> GetLicenseeCustInfo(LicenseCustInfoParam param);

        [Post("/GetRegList")]
        Task<string> GetRegList(RegParam param);

        [Post("/UpdateBtage")]
        Task<string> UpdateBtage(UpdateBtageParam param);

        [Post("/Transfer")]
        Task<string> Transfer(TranferParam param);
    }

    public class RegParam
    {
        public string StrSiteName { get; set; }

        public DateTime DtimeTargetDate { get; set; }
    }

    public class TranferParam
    {
        public string SiteName { get; set; }

        public string LicenseeUserName { get; set; }

        public double Amount { get; set; }

        public int DummyAccount { get; set; }
    }

    public class LicenseCustInfoParam
    {
        public string StrSiteName { get; set; }

        public string StrLicenseeUserName { get; set; }
    }

    public class UpdateBtageParam : LicenseCustInfoParam
    {
        public string StrNewBtag { get; set; }
    }
}
