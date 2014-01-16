// --------------------------------------------------------------------------
// <copyright file="AffiliateDataAccess.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Joseph.Pham" date="2013.8.1">Created</change>
// </history>
namespace Fanex.AffiliateAdmin.DataAccess.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Common.Extensions;
    using Common.Helper;
    using Criterias;
    using Data;
    using DTO;
    using Interface;
    using Models;
    
    /// <summary>
    /// Account data access.
    /// </summary>
    public class AffiliateDataAccess : IAffiliateDataAccess
    {
        /// <summary>
        /// Get Affiliate General Information.
        /// </summary>
        /// <param name="criteria">Affiliate Information Criteria.</param>
        /// <returns>Affiliate Information View Model.</returns>
        public AffiliateInfoViewModel GetAffiliateGeneralInfo(AffiliateInfoCriteria criteria)
        {
            var affiliateInfo = new AffiliateInfoViewModel();

            Check.Argument.IsNotNull(criteria, "criteria");
            
            if (criteria.IsValid())
            {
                using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
                {
                    var reader = objectDb.ExecuteReader(criteria.GetSpParams());
                    while (reader.Read())
                    {
                        affiliateInfo.AffiliateInfo  = new Affiliate
                        {
                            IsClosed = reader.ToBool("closed"),
                            IsLocked = reader.ToBool("IsLocked"),
                            Username = reader.ToString("Username"),
                            AffId = reader.ToInt("AffID"),
                            FirstName = reader.ToString("firstname"),
                            LastName = reader.ToString("lastname"),
                            Prefix = reader.ToString("Prefix"),
                            Gender = reader.ToBool("gender"),
                            Email = reader.ToString("email"),
                            Phone = reader.ToString("phone"),
                            MobilePhone = reader.ToString("mobilephone"),
                            SiteURL = reader.ToString("siteurl"),
                            LastLoginDate = reader.ToDateTime("lastlogindate"),
                            LastLoginIp = reader.ToString("lastloginip"),
                            PreferredLanguage = reader.ToString("PreferredLanguage"),
                            PreferredPayment = reader.ToString("PreferredPayment"),
                            Player_Username = reader.ToString("Player_Username"),
                            Status = reader.ToString("status").ToLower(),
                            MailOption = reader.ToBool("MailOption"),
                            GroupName = reader.ToString("GroupName"),
                            GroupId = reader.ToInt("GroupId"),
                            AffType = reader.ToByte("AffType"),
                            CurrencyCode = reader.ToString("CurrencyCode"),
                            Currency = reader.ToShort("Currency"),
                            AffLevel = reader.ToShort("TestLevel"),
                            BalanceBF = reader.ToDecimal("BalBroughtForward")
                        };
                    }

                    GetAffiliateOnUser(reader, affiliateInfo);
                }
            }

            return affiliateInfo;
        }

        /// <summary>
        /// Get Affiliate Statistic.
        /// </summary>
        /// <param name="criteria">Affiliate Statistic Criteria.</param>
        /// <returns>Affiliate Statistic.</returns>
        public AffiliateStatisticViewModel GetAffiliateStatistic(AffiliateStatisticCriteria criteria)
        {
            var affiliateStatistic = new AffiliateStatisticViewModel();

            if (criteria.IsValid())
            {
                using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
                {
                    var param = criteria.GetSpParams();
                    affiliateStatistic.Products = objectDb.Query<Product>(param) as List<Product>;

                    affiliateStatistic.Statistic = new AffiliateStatistic
                    {
                        TotalActivePlayer = Convert.ToInt32(param["totalactiveplayer"]),
                        TotalPlayerDepUp = Convert.ToInt32(param["totalplayerdepup"]),
                        NewSignUpDep = Convert.ToInt32(param["newsignupdep"]),
                        NewSignUp = Convert.ToInt32(param["newsignup"]),
                        TotalPlayer = Convert.ToInt32(param["totalplayer"]),
                        BalBroughtforward = Convert.ToDecimal(param["balbroughtforward"]),
                        TotalDeposit = Convert.ToDecimal(param["totaldeposit"]),
                        TotalDepositUp = Convert.ToDecimal(param["totaldepositup"]),
                        TotalTurnover = Convert.ToDecimal(param["totalturnover"]),
                        TotalTurnoverUp = Convert.ToDecimal(param["totalturnoverup"]),
                        TotalWinlostUp = Convert.ToDecimal(param["totalwinlostup"]),
                        TotalWinlost = Convert.ToDecimal(param["totalwinlost"]),
                        TotalUniqueClicks = Convert.ToInt32(param["totaluniqueclicks"]),
                        TotalClicks = Convert.ToInt32(param["totalclicks"])
                    };
                }
            }

            return affiliateStatistic;
        }

        /// <summary>
        /// Get Affiliate Info.
        /// </summary>
        /// <param name="criteria">Affiliate Criteria.</param>
        /// <returns>Affiliate Info.</returns>
        public Affiliate GetAffiliateInfo(AffiliateSelCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");

            if (criteria.IsValid())
            {
                using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
                {
                    var param = criteria.GetSpParams();

                    var affiliate = objectDb.Query<Affiliate>(criteria.GetSpParams()).FirstOrDefault();

                    criteria.AffMinCheque = Convert.ToDecimal(param["AFF_MinWire"]);
                    criteria.AffMinNeteller = Convert.ToDecimal(param["Aff_MinNeteller"]);
                    criteria.AffMinCheque = Convert.ToDecimal(param["AFF_MinCheque"]);
                    criteria.AffMinMoneyBookers = Convert.ToDecimal(param["AFF_MinMoneybookers"]);
                    criteria.AffMinPlayer = Convert.ToDecimal(param["AFF_MinPlayer"]);

                    return affiliate;
                }
            }

            return null;
        }

        /// <summary>
        /// Get Affiliate List.
        /// </summary>
        /// <param name="criteria">The criteria object.</param>
        /// <returns>The list of affiliate.</returns>
        public AffiliateListReportViewModel GetAffiliateList(AffiliateListCriteria criteria)
        {
            var result = new AffiliateListReportViewModel();

            using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
            {
                var reader = objectDb.ExecuteReader(criteria.GetSpParams());

                // Read Currency Statistics.
                result.CurrencyList = GetCurrencyList(reader);

                // Read Affiliate List.
                result.AffiliateList = GetAffiliateList(reader);

                // Close reader before read output.
                if (!reader.IsClosed)
                {
                    reader.Close();    
                }

                // Read output parameters.
                var outputParams = objectDb.GetOuputParams();
                criteria.TotalPages = int.Parse(outputParams["totalpages"].ToString());
                criteria.NRows = int.Parse(outputParams["nrows"].ToString());
                criteria.TotalAmount = decimal.Parse(outputParams["totalamount"].ToString());
            }

            return result;
        }

        #region Helpers

        /// <summary>
        /// Get Affiliate On User.
        /// </summary>
        /// <param name="reader">Data Reader.</param>
        /// <param name="affiliateInfo">Affiliate Information View Model.</param>
        private void GetAffiliateOnUser(IDataReader reader, AffiliateInfoViewModel affiliateInfo)
        {
            if (reader.NextResult())
            {
                while (reader.Read())
                {
                    affiliateInfo.OnUsers.Add(new AffiliateOnUser
                    {
                        On_UserId = reader.ToInt("On_UserId"),
                        On_SessionId = reader.ToInt("On_SessionId"),
                        CustId = reader.ToInt("CustId"),
                        Reload_Time = reader.ToDateTime("Reload_Time"),
                        ServerID = reader.ToInt("ServerID"),
                        Ip = reader.ToString("IP"),
                        IsKickedOut = reader.ToBool("IsKickedOut"),
                        SubAccId = reader.ToInt("subaccid"),
                        NetSessionID = reader.ToString("NetSessionID")
                    });
                }
            }
        }

        /// <summary>
        /// Get affiliate list.
        /// </summary>
        /// <param name="reader">Input reader.</param>
        /// <returns>Affiliate List.</returns>
        private List<AffiliateInfo> GetAffiliateList(IDataReader reader)
        {
            var result = new List<AffiliateInfo>();

            if (reader.NextResult())
            {
                while (reader.Read())
                {
                    result.Add(new AffiliateInfo
                    {
                        RowNo = reader.ToLong("RowNo"),
                        AffID = reader.ToInt("AffID"),
                        UserName = reader.ToString("UserName"),
                        FirstName = reader.ToString("FirstName"),
                        LastName = reader.ToString("LastName"),
                        Favorite = reader.ToBool("Favorite"),
                        LanguageName = reader.ToString("LanguageName"),
                        AffTypeName = reader.ToString("AffTypeName"),
                        RegisteredDate = reader.ToDateTime2("RegisteredDate"),
                        ApprovedDate = reader.ToDateTime2("ApprovedDate"),
                        Status = reader.ToString("Status"),
                        TestLevel = reader.ToByte("TestLevel"),
                        AccountType = reader.ToString("AccountType"),
                        CommGroupID = reader.ToInt("CommGroupID"),
                        CommGroupName = reader.ToString("CommGroupName"),
                        Currency = reader.ToString("Currency"),
                        CurrencyId = reader.ToInt("CurrencyId"),
                        BF = reader.ToDecimal("BF"),
                        TotalMembers = reader.ToInt("TotalMembers")
                    });
                }
            }

            return result;
        }

        /// <summary>
        /// Get currency list.
        /// </summary>
        /// <param name="reader">Input reader.</param>
        /// <returns>Currency list.</returns>
        private List<CurrencyStatistics> GetCurrencyList(IDataReader reader)
        {
            var result = new List<CurrencyStatistics>();
            while (reader.Read())
            {
                result.Add(new CurrencyStatistics
                {
                    Currency = reader.ToShort("Currency"),
                    CurrencyCode = reader.ToString("CurrencyCode"),
                    Total = reader.ToDecimal("Total"),
                    TotalMembers = reader.ToDecimal("TotalMembers")
                });
            }

            return result;
        }

        #endregion
    }
}
