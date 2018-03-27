using Fanex.AffiliateAdmin.Core.Services.Interface;
using Fanex.AffiliateAdmin.Criterias;

namespace Fanex.AffiliateAdmin.Core.Services.Impl
{
    public class DbLogger : QueryService, IDbLogger
    {
        public void WritePaymentLog(PaymentLogCriteria criteria)
        {
            ExecuteOutput(criteria);
        }
    }
}
