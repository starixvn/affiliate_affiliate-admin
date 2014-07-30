// --------------------------------------------------------------------------
// <copyright file="DomainService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Joseph.Pham" date="2013.8.1">Created</change>
// </history>
namespace Fanex.AffiliateAdmin.Services
{
    using System.Collections.Generic;
    using Core.Services;
    using Criterias;
    using DTO;

    /// <summary>
    /// Affiliate Service.
    /// </summary>
    public class DomainService : QueryService, IDomainService
    {
        public IEnumerable<Domain> AffDomainList(DomainAffCriteria criteria)
        {
            return GetData<Domain>(criteria);
        }

        public void AffDomainAdd(DomainAffCriteria criteria)
        {
            ExecuteOutput(criteria);
        }

        public void AffDomainDelete(DomainAffCriteria criteria)
        {
            ExecuteOutput(criteria);
        }

        public void AffDomainUpdate(DomainAffCriteria criteria)
        {
            ExecuteOutput(criteria);
        }

        public Domain AffDomainSelectById(DomainAffCriteria criteria)
        {
            return GetEntity<Domain>(criteria);
        }
        
        public IEnumerable<Domain> MemDomainList(DomainMemCriteria criteria)
        {
            return GetData<Domain>(criteria);
        }

        public void MemDomainAdd(DomainMemCriteria criteria)
        {
            ExecuteOutput(criteria);
        }

        public void MemDomainDelete(DomainMemCriteria criteria)
        {
            ExecuteOutput(criteria);
        }

        public void MemDomainUpdate(DomainMemCriteria criteria)
        {
            ExecuteOutput(criteria);
        }

        public Domain MemDomainSelectById(DomainMemCriteria criteria)
        {
            return GetEntity<Domain>(criteria);
        }

        public IEnumerable<DomainControl> DomainControlList(DomainControlCriteria criteria)
        {
            return GetData<DomainControl>(criteria);
        }

        public void DomainControlAdd(DomainControlCriteria criteria)
        {
            ExecuteOutput(criteria);
        }

        public void DomainControlDelete(DomainControlCriteria criteria)
        {
            ExecuteOutput(criteria);
        }

        public void DomainControlUpdate(DomainControlCriteria criteria)
        {
            ExecuteOutput(criteria);
        }

        public DomainControl DomainControlSelectById(DomainControlCriteria criteria)
        {
            return GetEntity<DomainControl>(criteria);
        }
    }
}