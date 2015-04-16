// --------------------------------------------------------------------------
// <copyright file="AffiliateService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Joseph.Pham" date="2013.8.1">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Services
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading;
    using Common.Helper;
    using Core.Services;
    using Criterias;
    using DTO;
    using Fanex.AffiliateAdmin.DataAccess;
    using Fanex.AffiliateAdmin.Resources;
    using Models;

    /// <summary>
    /// Affiliate Service.
    /// </summary>
    public class AffiliateService : QueryService, IAffiliateService
    {
        #region Fields

        /// <summary>
        /// Define DataAccess.
        /// </summary>
        private readonly IAffiliateDataAccess _affiliateDataAccess;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="AffiliateService" /> class.
        /// </summary>
        public AffiliateService()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AffiliateService" /> class.
        /// </summary>
        /// <param name="dataAccess">Account data access.</param>
        private AffiliateService(IAffiliateDataAccess dataAccess)
        {
            _affiliateDataAccess = dataAccess ?? new AffiliateDataAccess();
        }

        #endregion Constructor

        /// <summary>
        /// Change affiliate password.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        public void ChangePassword(ChangeAffPasswordCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            ExecuteOutput(criteria);
        }

        /// <summary>
        /// Get affiliate list.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>Return affiliate list.</returns>
        public AffiliateListReportViewModel GetAffiliateList(AffiliateListCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            var result = _affiliateDataAccess.GetAffiliateList(criteria);

            foreach (var affiliate in result.AffiliateList)
            {
                var keyAccountType = affiliate.AccountType.Replace(" ", string.Empty).Replace("-", string.Empty);
                var translatedAccountType = Lang.ResourceManager.GetString(keyAccountType);
                affiliate.AccountType = string.IsNullOrEmpty(translatedAccountType) ? affiliate.AccountType : translatedAccountType;

                CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
                TextInfo textInfo = cultureInfo.TextInfo;
                var capitalStatus = textInfo.ToTitleCase(affiliate.Status);

                var translatedStatus = Lang.ResourceManager.GetString(capitalStatus);
                affiliate.Status = string.IsNullOrEmpty(translatedStatus) ? affiliate.Status : translatedStatus;

                var translatedAffTypeName = Lang.ResourceManager.GetString(affiliate.AffTypeName);
                affiliate.AffTypeName = string.IsNullOrEmpty(translatedAffTypeName) ? affiliate.AffTypeName : translatedAffTypeName;
            }

            return result;
        }

        /// <summary>
        /// Get Customer list.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>Return customer list.</returns>
        public IEnumerable<Customer> CustomerList(MemberListCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            return GetDataOutput<Customer>(criteria);
        }

        /// <summary>
        /// Get Customer list.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        public void CustomerRemove(CustomerRemoveCriteria criteria)
        {
            ExecuteOutput(criteria);
        }

        /// <summary>
        /// Add affiliate to favorite.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        public void AddFavorite(FavoriteCriteria criteria)
        {
            criteria.Type = CriteriaType.Insert;
            ExecuteOutput(criteria);
        }

        /// <summary>
        /// Remove affiliate from Favorite.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        public void RemoveFavorite(FavoriteCriteria criteria)
        {
            criteria.Type = CriteriaType.Delete;
            ExecuteOutput(criteria);
        }

        /// <summary>
        /// Get affiliate general info.
        /// </summary>
        /// <param name="criteria">Affiliate Info Criteria.</param>
        /// <returns>Affiliate View Model.</returns>
        public AffiliateInfoViewModel GetAffiliateGeneralInfo(AffiliateInfoCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");

            return _affiliateDataAccess.GetAffiliateGeneralInfo(criteria);
        }

        public AffiliateStatisticViewModel GetAffiliateStatistic(AffiliateStatisticCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");

            return _affiliateDataAccess.GetAffiliateStatistic(criteria);
        }

        /// <summary>
        /// Get Affiliate Information By Affiliate Id.
        /// </summary>
        /// <param name="criteria">Affiliate Statistic Criteria.</param>
        /// <returns>Affiliate Information.</returns>
        public Affiliate GetAffiliateInfo(AffiliateSelCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");

            return _affiliateDataAccess.GetAffiliateInfo(criteria);
        }

        /// <summary>
        /// Update Affiliate.
        /// </summary>
        /// <param name="criteria">Affiliate Update.</param>
        public void UpdateAffiliateInfo(AffiliateUpdCriteria criteria)
        {
            ExecuteOutput(criteria);
        }

        public void CheckMemberExist(CheckMemberExistCriteria criteria)
        {
            ExecuteOutput(criteria);
        }

        public void InsertMember(InsertMemberCriteria criteria)
        {
            ExecuteOutput(criteria);
        }

        public void ChangeAccountType(AffiliateUpdAccountTypeCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            ExecuteOutput(criteria);
        }

        public void ChangeCommissionGroup(CommGroupAssignCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            ExecuteOutput(criteria);
        }

        /// <summary>
        /// Affiliate change language.
        /// </summary>
        /// <param name="criteria">Affiliate language criteria.</param>
        public void ChangeAffiliateLanguage(AffiliateUpdLanguageCriteria criteria)
        {
            ExecuteOutput(criteria);
        }

        /// <summary>
        /// Change affiliate status.
        /// </summary>
        /// <param name="criteria">Affiliate status criteria.</param>
        public void ChangeAffiliateStatus(AffiliateUpdStatusCriteria criteria)
        {
            ExecuteOutput(criteria);
        }

        /// <summary>
        /// Change open close affiliate.
        /// </summary>
        /// <param name="criteria">Affiliate status criteria.</param>
        public void ChangeAffiliateOpenClose(AffiliateUpdClosedStatusCriteria criteria)
        {
            ExecuteOutput(criteria);
        }

        /// <summary>
        /// Change affiliate status.
        /// </summary>
        /// <param name="criteria">Affiliate status criteria.</param>
        public void AffiliateUnLocked(AffiliateUnlockCriteria criteria)
        {
            ExecuteOutput(criteria);
        }

        /// <summary>
        /// Change affiliate's type.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        public void ChangeAffType(AffiliateUpdAffTypeCriteria criteria)
        {
            ExecuteOutput(criteria);
        }

        /// <summary>
        /// Get Customer.
        /// </summary>
        /// <param name="criteria">Input Criteria.</param>
        /// <returns>Customer Info.</returns>
        public Customer GetCustomer(CustomerSelByUserNameCriteria criteria)
        {
            return GetEntity<Customer>(criteria);
        }
    }
}