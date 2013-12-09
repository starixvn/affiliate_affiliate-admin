// --------------------------------------------------------------------------
// <copyright file="AffiliateListViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2013.9.30">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Models
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.AffiliateAdmin.Core.Configurations;
    using Resources;

    /// <summary>
    /// Affiliate List View Model.
    /// </summary>
    public class AffiliateListViewModel
    {
        private readonly List<string> _defaultStatusList = new List<string>
        {
            "approved",
            "closed",
            "lockedout",
        };

        public AffiliateListViewModel()
        {
            IsFavorite = false;
            ExcludeTest = true;
        }

        public string Affiliate { get; set; }

        public int DateOption { get; set; }

        public bool IsFavorite { get; set; }

        public string Status { get; set; }

        public int GroupId { get; set; }

        public bool ExcludeTest { get; set; }

        public SelectList DateOptionList
        {
            get
            {
                return new SelectList(PaymentSettings.ListDateOption, "Value", "Text");
            }
        }

        public SelectList BaseCurrencyOptionList
        {
            get
            {
                return new SelectList(AppSettings.BaseCurrencyInAffiliate, "Value", "Text");
            }
        }

        public IEnumerable<SelectListItem> SortByOptionList
        {
            get
            {
                var selectItemList = new List<SelectListItem> 
                {
                    new SelectListItem 
                    {
                        Text = Lang.Affiliate,
                        Value = "0"
                    },
                    new SelectListItem
                    {
                        Text = Lang.BF,
                        Value = "1"
                    },
                    new SelectListItem
                    {
                        Text = Lang.Member,
                        Value = "2"
                    }
                };

                return selectItemList;
            }
        }

        public IEnumerable<SelectListItem> StatusOptionList
        {
            get
            {
                var status = string.IsNullOrEmpty(Status) ? null : Status.ToLower();

                if (IsFavorite)
                {
                    // Exclude two status: Pending, Disapproved if isFavorite = true.
                    return AppSettings.AffiliateStatuses
                        .Where(t => _defaultStatusList.Contains(t.Value.ToLower()))
                        .Select(t => new SelectListItem
                    {
                        Text = t.Text,
                        Value = t.Value,
                        Selected =
                            string.IsNullOrEmpty(status)
                                ? _defaultStatusList.Contains(t.Value.ToLower())
                                : t.Value.ToLower().Equals(status)
                    });
                }

                return AppSettings.AffiliateStatuses.Select(t => new SelectListItem
                    {
                        Text = t.Text,
                        Value = t.Value,
                        Selected =
                            string.IsNullOrEmpty(status)
                                ? _defaultStatusList.Contains(t.Value.ToLower())
                                : t.Value.ToLower().Equals(status)
                    });
            }
        }

        public IEnumerable<SelectListItem> CommissionGroupOptionList
        {
            get
            {
                return AppSettings.GetCommGroups(null).Select(t => new SelectListItem
                    {
                        Text = t.GroupName,
                        Value = t.GroupID.ToString(),
                        Selected = GroupId <= 0 || t.GroupID == GroupId
                    });
            }
        }
    }
}
