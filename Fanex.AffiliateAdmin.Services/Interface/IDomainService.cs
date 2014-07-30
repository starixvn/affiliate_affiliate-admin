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
        IEnumerable<Domain> AffDomainList(DomainAffCriteria criteria);

        /// <summary>
        /// Add Affiliate Domain.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        void AffDomainAdd(DomainAffCriteria criteria);

        /// <summary>
        /// Remove Affiliate Domain.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        void AffDomainDelete(DomainAffCriteria criteria);

        /// <summary>
        /// Update Affiliate Domain.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        void AffDomainUpdate(DomainAffCriteria criteria);

        /// <summary>
        /// Get Affiliate Domain by ID.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>Domain info.</returns>
        Domain AffDomainSelectById(DomainAffCriteria criteria);

        /// <summary>
        /// Member Domain list.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>List domain.</returns>
        IEnumerable<Domain> MemDomainList(DomainMemCriteria criteria);

        /// <summary>
        /// Add Member Domain.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        void MemDomainAdd(DomainMemCriteria criteria);

        /// <summary>
        /// Remove MemberDomain.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        void MemDomainDelete(DomainMemCriteria criteria);

        /// <summary>
        /// Update Member Domain.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        void MemDomainUpdate(DomainMemCriteria criteria);

        /// <summary>
        /// Get MemberDomain by ID.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>Domain info.</returns>
        Domain MemDomainSelectById(DomainMemCriteria criteria);

        IEnumerable<DomainControl> DomainControlList(DomainControlCriteria criteria);

        void DomainControlAdd(DomainControlCriteria criteria);

        void DomainControlDelete(DomainControlCriteria criteria);

        void DomainControlUpdate(DomainControlCriteria criteria);

        DomainControl DomainControlSelectById(DomainControlCriteria criteria);
    }
}