namespace Fanex.AffiliateAdmin.Core.Services.Interface
{
    using Fanex.AffiliateAdmin.Criterias;

    public interface IDbLogger
    {
        void WriteLogForCallingApi(CallingApiLogCriteria criteria);
    }
}