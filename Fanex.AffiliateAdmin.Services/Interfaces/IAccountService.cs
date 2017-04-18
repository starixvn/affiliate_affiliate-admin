namespace Fanex.AffiliateAdmin.Services
{
    using System.Collections.Generic;
    using Criterias;
    using DTO;
    using Fanex.AffiliateAdmin.Criterias.Account;
    using Fanex.Data.Repository;

    public interface IAccountService
    {
        IEnumerable<Account> GetListAccountManagement(SelByGroupIDCriteria criteria);

        Account GetAccountInfo(SelByCustIDCriteria criteria);

        void AccountUpdate(AccountUpdateCriteria criteria);

        void AccountInsert(AccountInsCriteria criteria);

        void AccountUpdatePassword(ChangePasswordCriteria criteria);

        void AccountUpdatePolicy(ICriteria criteria);

        void AccountUpdatePassword(AccountPasswordCriteria criteria);
    }
}