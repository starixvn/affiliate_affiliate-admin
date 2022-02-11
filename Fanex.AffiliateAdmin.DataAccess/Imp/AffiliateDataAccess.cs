namespace Fanex.AffiliateAdmin.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Core.Configurations;
    using Criterias;
    using Data;
    using DTO;
    using Fanex.AffiliateAdmin.Core.Utilities;
    using Fanex.AffiliateAdmin.Resources;
    using Fanex.Common.Extensions;
    using Fanex.Common.Helper;
    using Fanex.Data.SqlServer;
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
            var viewModel = new AffiliateInfoViewModel();
            Check.Argument.IsNotNull(criteria, "criteria");

            if (criteria.IsValid())
            {
                using (var objectDb = new SqlObjectDb(criteria.GetSettingKey()))
                {
                    using (var reader = objectDb.ExecuteReader(criteria))
                    {
                        if (reader.Read())
                        {
                            ReadAffiliateInfo(viewModel, reader);
                        }

                        GetAffiliateOnUser(reader, viewModel);
                    }
                }
            }

            return viewModel;
        }

        private static void ReadAffiliateInfo(AffiliateInfoViewModel viewModel, IDataReader reader)
        {
            viewModel.AffiliateInfo = new Affiliate
            {
                IsClosed = reader.ToBool("closed"),
                IsLocked = reader.ToBool("IsLocked"),
                UserName = reader.ToString("Username"),
                AffId = reader.ToInt("AffID"),
                FirstName = reader.ToString("firstname"),
                LastName = reader.ToString("lastname"),
                Prefix = reader.ToString("Prefix"),
                Gender = reader.ToBool("gender"),
                Email = reader.ToString("email"),
                Phone = reader.ToString("phone"),
                MobilePhone = reader.ToString("mobilephone"),
                SiteLink = reader.ToString("siteurl"),
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

            viewModel.AffiliateInfo.AffTypeName = AppSettings.AffTypes.FirstOrDefault(t => t.TypeID == viewModel.AffiliateInfo.AffType).Name;
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
                using (var objectDb = new SqlObjectDb(criteria.GetSettingKey()))
                {
                    using (var reader = objectDb.ExecuteReader(criteria))
                    {
                        affiliateStatistic.Products = GetProductList(reader);

                        affiliateStatistic.CommGroupInfo = GetCommGroup(reader);
                    }

                    affiliateStatistic.Statistic = GetAffiliateStatistic(objectDb);
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
                using (var objectDb = new SqlObjectDb(criteria.GetSettingKey()))
                {
                    return objectDb.Query<Affiliate>(criteria).FirstOrDefault();
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

            using (var objectDb = new SqlObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = objectDb.ExecuteReader(criteria))
                {
                    // Read Currency Statistics.
                    result.AddCurrencyList(GetCurrencyList(reader));

                    // Read Affiliate List.
                    result.AddAffiliateList(GetAffiliateList(reader));
                }

                // Read output parameters.
                var outputParams = objectDb.GetOutputParams();
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
                        OriginalStatus = reader.ToString("Status"),
                        TestLevel = reader.ToByte("TestLevel"),
                        AccountType = reader.ToString("AccountType"),
                        CommGroupID = reader.ToInt("CommGroupID"),
                        CommGroupName = reader.ToString("CommGroupName"),
                        Currency = reader.ToString("Currency"),
                        CurrencyId = reader.ToInt("CurrencyId"),
                        BF = reader.ToDecimal("BF"),
                        TotalMembers = reader.ToInt("TotalMembers"),
                        Email = reader.ToString("Email"),
                        AffPlayer = reader.ToString("Player_Username")
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

        /// <summary>
        /// Get Commission Group Information.
        /// </summary>
        /// <param name="reader">Data reader.</param>
        /// <returns>Commission Group Information.</returns>
        private CommGroupInfo GetCommGroup(IDataReader reader)
        {
            var commGroup = new CommGroupInfo();
            if (reader.NextResult())
            {
                while (reader.Read())
                {
                    commGroup.GroupID = reader.ToInt("GroupId");
                    commGroup.GroupName = reader.ToString("CommGroupName");
                    break;
                }
            }

            return commGroup;
        }

        /// <summary>
        /// Get Product List.
        /// </summary>
        /// <param name="reader">Data reader.</param>
        /// <returns>Product List.</returns>
        private IEnumerable<Product> GetProductList(IDataReader reader)
        {
            var productList = new List<Product>();
            var resourceManager = Lang.ResourceManager;
            while (reader.Read())
            {
                var productName = reader.ToString("ProductName");
                productName = resourceManager.GetString(productName.Replace(" ", string.Empty));
                productList.Add(new Product
                {
                    ProductID = reader.ToByte("ProductID"),
                    ProductName = productName,
                    TotalQualifiedMembers = reader.ToInt("TotalQualifiedMembers")
                });
            }

            return productList;
        }

        /// <summary>
        /// Get affiliate statistics.
        /// </summary>
        /// <param name="objectDb">Object database.</param>
        /// <returns>Affiliate statistics.</returns>
        private AffiliateStatistic GetAffiliateStatistic(ObjectDb objectDb)
        {
            var outputParams = objectDb.GetOutputParams();

            var statistics = new AffiliateStatistic();

            var properties = statistics.GetType().GetProperties();

            foreach (string outputParam in outputParams.Keys)
            {
                foreach (var property in properties)
                {
                    if (property.CanWrite && property.Name.ToLower() == outputParam)
                    {
                        statistics.SetPropertyValue(property.Name, outputParams[outputParam]);
                    }
                }
            }

            return statistics;
        }

        #endregion Helpers
    }
}