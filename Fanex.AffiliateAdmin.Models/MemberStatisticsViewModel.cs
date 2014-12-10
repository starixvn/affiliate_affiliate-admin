// --------------------------------------------------------------------------
// <copyright file="MemberStatisticsViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2014.11.26">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Models
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Fanex.AffiliateAdmin.Core.Configurations;
    using Fanex.AffiliateAdmin.Core.Session;
    using Fanex.AffiliateAdmin.Resources;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MemberStatisticsViewModel
    {
        private readonly List<SelectListItem> _sortByOptionList = new List<SelectListItem>
                {
                    new SelectListItem
                    {
                        Text = Lang.Affiliate,
                        Value = "0"
                    },
                    new SelectListItem
                    {
                        Text = Lang.Signups,
                        Value = "1"
                    },
                    new SelectListItem
                    {
                        Text = Lang.Active,
                        Value = "2"
                    }
                };

        private readonly List<SelectListItem> _groupByOptionList = new List<SelectListItem>
                {
                    new SelectListItem
                    {
                        Text = Lang.All,
                        Value = "0"
                    },
                    new SelectListItem
                    {
                        Text = Lang.Date,
                        Value = "1"
                    },
                    new SelectListItem
                    {
                        Text = Lang.Month,
                        Value = "2"
                    }
                };

        public MemberStatisticsViewModel()
        {
            ExcludeTest = true;
        }

        public bool ExcludeTest { get; set; }

        public IEnumerable<SelectListItem> SortByOptionList
        {
            get { return _sortByOptionList; }
        }

        public IEnumerable<SelectListItem> GroupByOptionList
        {
            get
            {
                return _groupByOptionList;
            }
        }

        public IEnumerable<SelectListItem> PreferLanguageOptionList
        {
            get
            {
                return new SelectList(AppSettings.AffSupportLanguages, "LanguageID", "LanguageName");
            }
        }

        public IEnumerable<SelectListItem> AffiliateTypeOptionList
        {
            get
            {
                return new SelectList(AppSettings.AffTypes, "TypeID", "Name");
            }
        }

        public IEnumerable<SelectListItem> CurrencyOptionList
        {
            get
            {
                return new SelectList(SessionManager.CurrentSession.CurrencyList, "CurrencyID", "CurrencyCode");
            }
        }
    }
}