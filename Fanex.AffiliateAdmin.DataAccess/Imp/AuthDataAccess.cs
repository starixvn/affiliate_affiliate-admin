namespace Fanex.AffiliateAdmin.DataAccess
{
    using System;
    using Common;
    using Common.Helper;
    using Criterias;
    using Data;
    using DTO;
    using Fanex.AffiliateAdmin.Models;

    public class AuthDataAccess : IAuthDataAccess
    {
        public ResultViewModel Login(LoginCriteria criteria, out Account account)
        {
            using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
            {
                objectDb.ExecuteNonQuery(criteria.GetSpParams());
                var outputParams = objectDb.GetOuputParams();
                var result = new ResultViewModel();
                account = new Account();
                result.ErrorCode = Convert.ToInt32(outputParams["errcode"]);
                result.ErrorMessage = Convert.ToString(outputParams["errmsg"]);
                if (result.ErrorCode == 0)
                {
                    account.CustID = Convert.ToInt32(outputParams["userid"]);
                    account.IsIT = true;
                    account.ServerId = Convert.ToInt32(outputParams["serverid"]);
                    account.Username = criteria.Username;
                }

                return result;
            }
        }

        /// <summary>
        /// User Logout.
        /// </summary>
        /// <param name="criteria">Logout criteria.</param>
        public void UserLogOut(LogOutCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
            {
                var currentParams = criteria.GetSpParams();
                objectDb.ExecuteNonQuery(currentParams);
            }
        }

        public int CheckIn(CheckInCriteria criteria)
        {
            using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
            {
                var currentParams = criteria.GetSpParams();                
                objectDb.ExecuteNonQuery(currentParams);
                return Converter.ToInt(currentParams["on_userid"]);
            }
        }

        /// <summary>
        /// Check Is kicked out.
        /// </summary>
        /// <param name="criteria">Criteria check kicked out.</param>
        /// <returns>Return boolean.</returns>
        public bool ReCheckIn(ReCheckInCriteria criteria)
        {
            using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
            {
                var currentParams = criteria.GetSpParams();
                objectDb.ExecuteNonQuery(currentParams);
                return Converter.ToBool(currentParams["iskickedout"]);
            }
        }
    }
}