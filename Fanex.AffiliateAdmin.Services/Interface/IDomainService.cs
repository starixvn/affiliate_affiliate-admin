// --------------------------------------------------------------------------
// <copyright file="IDomainService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Joseph.Pham" date="2013.8.1">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Services
{
    using System.Collections.Generic;
    using Criterias;
    using DTO;

    /// <summary>
    /// Interface Affiliate service.
    /// </summary>
    public interface IDomainService
    {
        /// <summary>
        /// Affiliate Domain list.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>List affiliate.</returns>
        IEnumerable<Domain> AffDomainList(DomainAffSelCriteria criteria);

        /// <summary>
        /// Add Affiliate Domain.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        void AffDomainAdd(DomainAffInsCriteria criteria);

        /// <summary>
        /// Remove Affiliate Domain.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        void AffDomainDelete(DomainAffDelCriteria criteria);

        /// <summary>
        /// Update Affiliate Domain.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        void AffDomainUpdate(DomainAffUpdCriteria criteria);

        /// <summary>
        /// Get Affiliate Domain by ID.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>Domain info.</returns>
        Domain AffDomainSelectById(DomainAffSelByIdCriteria criteria);

        /// <summary>
        /// Member Domain list.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>List domain.</returns>
        IEnumerable<Domain> MemDomainList(DomainMemSelCriteria criteria);

        /// <summary>
        /// Add Member Domain.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        void MemDomainAdd(DomainMemInsCriteria criteria);

        /// <summary>
        /// Remove MemberDomain.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        void MemDomainDelete(DomainMemDelCriteria criteria);

        /// <summary>
        /// Update Member Domain.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        void MemDomainUpdate(DomainMemUpdCriteria criteria);

        /// <summary>
        /// Get MemberDomain by ID.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>Domain info.</returns>
        Domain MemDomainSelectById(DomainMemSelByIdCriteria criteria);

        IEnumerable<DomainControl> DomainControlList(DomainControlSelCriteria criteria);

        void DomainControlAdd(DomainControlInsCriteria criteria);

        void DomainControlDelete(DomainControlDelCriteria criteria);

        void DomainControlUpdate(DomainControlUpdCriteria criteria);

        DomainControl DomainControlSelectById(DomainControlSelByIdCriteria criteria);
    }
}