namespace Fanex.AffiliateAdmin.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using Common.Helper;
    using Core.Services;
    using Criterias;
    using DTO;
    using Fanex.AffiliateAdmin.Criterias.Account;
    using Fanex.AffiliateAdmin.Resources;
    using Fanex.Data.Repository;

    public class AccountService : QueryService, IAccountService
    {
        public IEnumerable<Account> GetListAccountManagement(SelByGroupIDCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            var result = GetData<Account>(criteria);

            foreach (var account in result)
            {
                var translated = Lang.ResourceManager.GetString(account.GroupName.Replace(" ", string.Empty));
                account.GroupName = string.IsNullOrEmpty(translated) ? account.GroupName : translated;
            }

            return result;
        }

        public Account GetAccountInfo(SelByCustIDCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            var data = GetData<Account>(criteria);

            return data.FirstOrDefault();
        }

        public void AccountUpdate(AccountUpdateCriteria criteria)
        {
            ExecuteOutput(criteria);
        }

        public void AccountInsert(AccountInsCriteria criteria)
        {
            ExecuteOutput(criteria);
        }

        public void AccountUpdatePassword(ChangePasswordCriteria criteria)
        {
            ExecuteOutput(criteria);
        }

        public void AccountUpdatePolicy(ICriteria criteria)
        {
            ExecuteOutput(criteria);
        }

        public void AccountUpdatePassword(AccountPasswordCriteria criteria)
        {
            ExecuteOutput(criteria);
        }
    }
}