using Fanex.AffiliateAdmin.Core.Services.Interface;
using Fanex.AffiliateAdmin.Criterias;

namespace Fanex.AffiliateAdmin.Core.Services.Impl
{
    using Data.Repository;

    public class DbLogger : QueryService, IDbLogger
    {
        public void WritePaymentLog(PaymentLogCriteria criteria)
        {
            ExecuteOutput(criteria);
        }

        public void AffWriteLog(ICriteria criteria)
        {
            ExecuteOutput(criteria);
        }
    }
}
