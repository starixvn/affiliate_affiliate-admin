// --------------------------------------------------------------------------
// <copyright file="IAffiliateService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Joseph.Pham" date="2013.8.1">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Services.Interface
{
    using System.Collections.Generic;
    using Criterias;
    using DTO;
    using Models;

    /// <summary>
    /// Interface Affiliate service.
    /// </summary>
    public interface IAffiliateService
    {
        /// <summary>
        /// Change affiliate password.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        void ChangePassword(ChangeAffPasswordCriteria criteria);

        /// <summary>
        /// Get affiliate list.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>Affiliate List.</returns>
        AffiliateListReportViewModel GetAffiliateList(AffiliateListCriteria criteria);

        /// <summary>
        /// Get Customer list.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>List customer.</returns>
        IEnumerable<Customer> CustomerList(MemberListCriteria criteria);

        /// <summary>
        /// Remove Customer.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        void CustomerRemove(CustomerRemoveCriteria criteria);

        /// <summary>
        /// Add affiliate to favorite.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>        
        void AddFavorite(FavoriteCriteria criteria);

        /// <summary>
        /// Remove affiliate from Favorite.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>        
        void RemoveFavorite(FavoriteCriteria criteria);

        /// <summary>
        /// Get Affiliate General Info.
        /// </summary>
        /// <param name="criteria">Affiliate Criteria.</param>
        /// <returns>Affiliate Info View Model.</returns>
        AffiliateInfoViewModel GetAffiliateGeneralInfo(AffiliateInfoCriteria criteria);

        /// <summary>
        /// Get Affiliate Statistic.
        /// </summary>
        /// <param name="criteria">Affiliate Statistic Criteria.</param>
        /// <returns>Affiliate Statistic.</returns>
        AffiliateStatisticViewModel GetAffiliateStatistic(AffiliateStatisticCriteria criteria);

        /// <summary>
        /// Get Affiliate Information By Affiliate Id.
        /// </summary>
        /// <param name="criteria">Affiliate Statistic Criteria.</param>
        /// <returns>Affiliate Information.</returns>
        Affiliate GetAffiliateInfo(AffiliateSelCriteria criteria);

        /// <summary>
        /// Update Affiliate.
        /// </summary>
        /// <param name="criteria">Affiliate Update.</param>
        void UpdateAffiliateInfo(AffiliateUpdCriteria criteria);

        void CheckMemberExist(CheckMemberExistCriteria criteria);

        void InsertMember(InsertMemberCriteria criteria);

        /// <summary>
        /// Affiliate update language.
        /// </summary>
        /// <param name="criteria">Affiliate language criteria.</param>
        void ChangeAffiliateLanguage(AffiliateUpdLanguageCriteria criteria);
        
        /// <summary>
        /// Change affiliate's account type.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        void ChangeAccountType(AffiliateUpdAccountTypeCriteria criteria);       
        
        /// <summary>
        /// Change affiliate's commission group.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        void ChangeCommissionGroup(CommGroupAssignCriteria criteria);
        
         /// <summary>
        /// Change affiliate status.
        /// </summary>
        /// <param name="criteria">Affiliate status criteria.</param>
        void ChangeAffiliateStatus(AffiliateUpdStatusCriteria criteria);

        /// <summary>
        /// Change open close affiliate.
        /// </summary>
        /// <param name="criteria">Affiliate status criteria.</param>
        void ChangeAffiliateOpenClose(AffiliateUpdStatusCriteria criteria);

        /// <summary>
        /// Change affiliate status.
        /// </summary>
        /// <param name="criteria">Affiliate status criteria.</param>
        void AffiliateUnLocked(AffiliateUnlockCriteria criteria);

        /// <summary>
        /// Change affiliate's type.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        void ChangeAffType(AffiliateUpdAffTypeCriteria criteria);

        /// <summary>
        /// Get Customer Info.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        Customer GetCustomer(CustomerSelByUserNameCriteria criteria);  
    }
}