// --------------------------------------------------------------------------
// <copyright file="CompanyWinlossViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2013.10.16">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Models
{
    using System.Web.Mvc;
    using Fanex.AffiliateAdmin.Core.Configurations;

    /// <summary>
    /// Affiliate List View Model.
    /// </summary>
    public class CompanyWinlossViewModel
    {
        #region Fields

        private MultiSelectList _commissionGroupOptionList;

        private MultiSelectList _preferLanguageOptionList;

        private MultiSelectList _affTypeOptionList;

        private SelectList _baseCurrencyOptionList;

        #endregion

        public CompanyWinlossViewModel()
        {
            ExcludeTest = true;
        }

        public string Affiliate { get; set; }

        public int DateOption { get; set; }

        public int DrawFilter { get; set; }

        public string Status { get; set; }

        public int GroupId { get; set; }

        public bool ExcludeTest { get; set; }

        public MultiSelectList AffTypeOptionList
        {
            get
            {
                if (_affTypeOptionList == null)
                {
                    _affTypeOptionList = new MultiSelectList(AppSettings.AffTypes, "TypeID", "Name");
                }

                return _affTypeOptionList;
            }
        }

        public MultiSelectList CommissionGroupOptionList
        {
            get
            {
                if (_commissionGroupOptionList == null)
                {
                    _commissionGroupOptionList = new MultiSelectList(AppSettings.GetCommGroups(null), "GroupID", "GroupName");
                }

                return _commissionGroupOptionList;
            }
        }

        public MultiSelectList PreferLanguageOptionList
        {
            get
            {
                if (_preferLanguageOptionList == null)
                {
                    _preferLanguageOptionList = new MultiSelectList(AppSettings.AffSupportLanguages, "LanguageID", "LanguageName");
                }

                return _preferLanguageOptionList;
            }
        }

        public MultiSelectList PlayerCurrencyOptionList
        {
            get
            {
                return new MultiSelectList(AppSettings.PlayerCurrencies, "CurrencyID", "CurrencyCode");
            }
        } 

        public SelectList GroupByOptionList
        {
            get
            {
                return new SelectList(AppSettings.GroupByDateMonth, "Value", "Text", DateOption.ToString());
            }
        }

        public SelectList DrawFilterOptionList
        {
            get
            {
                return new SelectList(AppSettings.DrawFilters, "Value", "Text", DrawFilter.ToString());
            }
        }

        public SelectList BaseCurrencyOptionList
        {
            get
            {
                if (_baseCurrencyOptionList == null)
                {
                    _baseCurrencyOptionList = new SelectList(AppSettings.BaseCurrencyInMember, "Value", "Text");
                }

                return _baseCurrencyOptionList;
            }
        }
    }
}