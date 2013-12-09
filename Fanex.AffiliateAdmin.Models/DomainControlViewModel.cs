// --------------------------------------------------------------------------
// <copyright file="DomainControlViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Johnny.Trinh" date="2013.09.04">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Models
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

        [Display(Name = "Market", ResourceType = typeof(Resources.Lang))]
        [Required]
        public string Country { get; set; }

        public List<Country> CountryList { get; set; }

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

        public List<Domain> AffDomainList { get; set; }

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

        public List<Domain> MemberDomainList { get; set; }

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
    }
}