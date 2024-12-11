namespace Fanex.AffiliateAdmin.Services
{
    using Core.Services;
    using Criterias;
    using DTO;
    using System.Collections.Generic;

    /// <summary>
    /// Affiliate Service.
    /// </summary>
    public class DomainService : QueryService, IDomainService
    {
        public IEnumerable<Domain> AffDomainList(DomainAffSelCriteria criteria)
        {
            return GetData<Domain>(criteria);
        }

        public void AffDomainAdd(DomainAffInsCriteria criteria)
        {
            ExecuteOutput(criteria);
        }

        public void AffDomainDelete(DomainAffDelCriteria criteria)
        {
            ExecuteOutput(criteria);
        }

        public void AffDomainUpdate(DomainAffUpdCriteria criteria)
        {
            ExecuteOutput(criteria);
        }

        public Domain AffDomainSelectById(DomainAffSelByIdCriteria criteria)
        {
            return GetEntity<Domain>(criteria);
        }

        public IEnumerable<Domain> MemDomainList(DomainMemSelCriteria criteria)
        {
            return GetData<Domain>(criteria);
        }

        public void MemDomainAdd(DomainMemInsCriteria criteria)
        {
            ExecuteOutput(criteria);
        }

        public void MemDomainDelete(DomainMemDelCriteria criteria)
        {
            ExecuteOutput(criteria);
        }

        public void MemDomainUpdate(DomainMemUpdCriteria criteria)
        {
            ExecuteOutput(criteria);
        }

        public Domain MemDomainSelectById(DomainMemSelByIdCriteria criteria)
        {
            return GetEntity<Domain>(criteria);
        }

        public IEnumerable<DomainControl> DomainControlList(DomainControlSelCriteria criteria)
        {
            return GetData<DomainControl>(criteria);
        }

        public void DomainControlAdd(DomainControlInsCriteria criteria)
        {
            ExecuteOutput(criteria);
        }

        public void DomainControlDelete(DomainControlDelCriteria criteria)
        {
            ExecuteOutput(criteria);
        }

        public void DomainControlUpdate(DomainControlUpdCriteria criteria)
        {
            ExecuteOutput(criteria);
        }

        public DomainControl DomainControlSelectById(DomainControlSelByIdCriteria criteria)
        {
            return GetEntity<DomainControl>(criteria);
        }
    }
}