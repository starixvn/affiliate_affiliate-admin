namespace Fanex.AffiliateAdmin.Services
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Core.Enumerations;
    using Core.Session;
    using Criterias;
    using DTO;
    using Fanex.AffiliateAdmin.Criterias.Account;
    using Fanex.AffiliateAdmin.DTO.Common;
    using Fanex.AffiliateAdmin.Models;
    using Fanex.Data.Repository;

    /// <summary>
    /// Interface Account service.
    /// </summary>
    public interface IAccountService
    {
        [SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", Justification = "Reviewed.")]
        ResultViewModel LoginInternal(LoginInternalCriteria criteria, out Account account);

        [SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", Justification = "Reviewed.")]
        ResultViewModel Login(LoginCriteria criteria, out Account account, string logInToken);

        Account GetByUsername(string userName, int licenseeId = 0, bool isInternal = false);

        void UserLogOut(LogOutCriteria criteria);

        int CheckIn(CheckInCriteria criteria);

        bool ReCheckIn(ReCheckInCriteria criteria);

        IEnumerable<Account> GetListAccountManagement(SelByGroupIDCriteria criteria);

        Account GetAccountInfo(SelByCustIDCriteria criteria);

        void AccountUpdate(AccountUpdateCriteria criteria);

        void AccountInsert(AccountInsCriteria criteria);

        void AccountUpdatePassword(ChangePasswordCriteria criteria);

        void AccountUpdatePolicy(ICriteria criteria);

        void AccountUpdatePassword(AccountPasswordCriteria criteria);

        ICurrentSession CreateSession(Account user, int licenseeId, AppMode appMode = AppMode.Self);

        ResultInfo SyncUserInfo(string licenseeName, string userName);
    }
}