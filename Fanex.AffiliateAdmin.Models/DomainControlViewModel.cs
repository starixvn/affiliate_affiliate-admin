namespace Fanex.AffiliateAdmin.Models
{
    using DTO;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web.Mvc;

    /// <summary>
    /// Affiliate domain control view model.
    /// </summary>
    public class DomainControlViewModel
    {
        #region Fields

        private IEnumerable<Country> _countryList;

        private IEnumerable<Domain> _affDomainList;

        private IEnumerable<Domain> _memberDomainList;

        #endregion

        public int ID { get; set; }

        [Display(Name = "Country", ResourceType = typeof(Resources.Lang))]
        [Required]
        public string Country { get; set; }

        public IEnumerable<Country> CountryList
        {
            get
            {
                return _countryList;
            }
        }

        public IEnumerable<SelectListItem> CountryListSelectItem
        {
            get
            {
                var selectItemList = CountryList.Select(country => new SelectListItem
                {
                    Value = country.CountryID,
                    Text = country.CountryName
                }).ToList();

                return selectItemList;
            }
        }

        [Display(Name = "AffDomain", ResourceType = typeof(Resources.Lang))]
        [Required]
        public int AffDomain { get; set; }

        public IEnumerable<Domain> AffDomainList
        {
            get
            {
                return _affDomainList;
            }
        }

        public IEnumerable<SelectListItem> AffDomainListSelectItem
        {
            get
            {
                var selectItemList = AffDomainList.Select(domain => new SelectListItem
                {
                    Value = domain.DomainID.ToString(),
                    Text = domain.DomainName
                }).ToList();

                return selectItemList;
            }
        }

        [Display(Name = "MemberDomain", ResourceType = typeof(Resources.Lang))]
        [Required]
        public int MemberDomain { get; set; }

        public IEnumerable<Domain> MemberDomainList
        {
            get
            {
                return _memberDomainList;
            }
        }

        public IEnumerable<SelectListItem> MemberDomainListSelectItem
        {
            get
            {
                var selectItemList = MemberDomainList.Select(domain => new SelectListItem
                {
                    Value = domain.DomainID.ToString(),
                    Text = domain.DomainName
                }).ToList();

                return selectItemList;
            }
        }

        [Display(Name = "Status", ResourceType = typeof(Resources.Lang))]
        [Required]
        public bool Status { get; set; }

        #region Methods

        public void AddCountryList(IEnumerable<Country> countryList)
        {
            _countryList = countryList;
        }

        public void AddAffDomainList(IEnumerable<Domain> affDomainList)
        {
            _affDomainList = affDomainList;
        }

        public void AddMemberDomainList(IEnumerable<Domain> memberDomainList)
        {
            _memberDomainList = memberDomainList;
        }

        #endregion
    }
}