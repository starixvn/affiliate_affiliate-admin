namespace Fanex.AffiliateAdmin.Core.Services.Interface
{
    using Data.Repository;
    using Fanex.AffiliateAdmin.Criterias;

    public interface IDbLogger
    {
        void WritePaymentLog(PaymentLogCriteria criteria);

        void AffWriteLog(ICriteria criteria);
    }
}