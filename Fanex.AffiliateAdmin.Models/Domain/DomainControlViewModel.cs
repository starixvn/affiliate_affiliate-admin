namespace Fanex.AffiliateAdmin.Models.Domain
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web.Mvc;
    using DTO;

    /// <summary>
    /// Affiliate domain control view model.
    /// </summary>
    public class DomainControlViewModel
    {
        public int ID { get; set; }

        [Display(Name = "Country", ResourceType = typeof(Resources.Lang))]
        [Required]
        public string Country { get; set; }

        public IEnumerable<Country> CountryList { get; set; }

        public IEnumerable<SelectListItem> CountryListSelectItem => CountryList.Select(country => new SelectListItem
                {
                    Value = country.CountryID,
                    Text = country.CountryName
                });

        [Display(Name = "AffDomain", ResourceType = typeof(Resources.Lang))]
        [Required]
        public int AffDomain { get; set; }

        public IEnumerable<Domain> AffDomainList { get; set; }

        public IEnumerable<SelectListItem> AffDomainListSelectItem => AffDomainList.Select(domain => new SelectListItem
                                                                        {
                                                                            Value = domain.DomainID.ToString(),
                                                                            Text = domain.DomainName
                                                                        });


        [Display(Name = "MemberDomain", ResourceType = typeof(Resources.Lang))]
        [Required]
        public int MemberDomain { get; set; }

        public IEnumerable<Domain> MemberDomainList { get; set; }

        public IEnumerable<SelectListItem> MemberDomainListSelectItem => MemberDomainList.Select(domain => new SelectListItem
                                                                            {
                                                                                Value = domain.DomainID.ToString(),
                                                                                Text = domain.DomainName
                                                                            });

        [Display(Name = "Status", ResourceType = typeof(Resources.Lang))]
        [Required]
        public bool Status { get; set; }
    }
}