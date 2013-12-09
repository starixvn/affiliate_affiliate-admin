// --------------------------------------------------------------------------
// <copyright file="AccountDataAccess.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.05.30">Created</change>
//   <change who="Kaden.Trinh" date="2013.06.10">Implement funcion get statistics overview account</change>
//   <change who="Kaden.Trinh" date="2013.06.11">Implement funcion get Current Commission Setting</change>
// </history>
namespace Fanex.AffiliateAdmin.DataAccess.Impl
{
    using System;
    using Common;
    using Common.Helper;
    using Criterias;
    using Data;
    using DTO;
    using Interface;

    /// <summary>
    /// Account data access.
    /// </summary>
    public class AccountDataAccess : IAccountDataAccess
    {
        /// <summary>
        /// Get statistics for Affiliate.
        /// </summary>
        /// <param name="criteria">Login Criteria.</param>
        /// <returns>Return Account.</returns>
        public Account Login(LoginCriteria criteria)
        {
            var result = new Account();
            if (criteria.IsValid())
            {
                using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
                {     
                    var param = criteria.GetSpParams();
                    objectDb.ExecuteNonQuery(param);

                    criteria.ErrCode = Convert.ToInt32(param["errcode"]);
                    if (criteria.ErrCode == 0)
                    {
                        result = new Account
                        {
                            CustID = Convert.ToInt32(param["custid"]),
                            IsIT = Convert.ToBoolean(param["isit"]),
                            OnUserID = Convert.ToInt32(param["onuserid"]),
                            Username = criteria.Username
                        };
                    }
                        
                    return result;
                }
            }

            return result;
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