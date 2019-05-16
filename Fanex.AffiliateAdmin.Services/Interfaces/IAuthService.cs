namespace Fanex.AffiliateAdmin.Services
{
    using System.Diagnostics.CodeAnalysis;
    using Core.Session;
    using Criterias;
    using DTO;
    using DTO.OneAuth;
    using Fanex.AffiliateAdmin.DTO.Common;
    using Fanex.AffiliateAdmin.Models;

    public interface IAuthService
    {
        [SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", Justification = "Reviewed.")]
        ResultViewModel Login(LoginCriteria criteria, out Account account, string logInToken);

        Account GetByUsername(string userName, int licenseeId = 0, bool isInternal = false);

        void LogOut(LogOutCriteria criteria);

        int CheckIn(CheckInCriteria criteria);

        bool ReCheckIn(ReCheckInCriteria criteria);

        ICurrentSession CreateOneAuthSession(OneAuthToken token, Account user);

        void UpdateOneAuthSession(ICurrentSession session, OneAuthToken token);

        ICurrentSession CreateInternalSession(Account user, int licenseeId);

    }
}