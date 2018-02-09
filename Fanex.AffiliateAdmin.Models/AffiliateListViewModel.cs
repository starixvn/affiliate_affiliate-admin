namespace Fanex.AffiliateAdmin.Models
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.AffiliateAdmin.Core.Configurations;
    using Fanex.AffiliateAdmin.Core.Options;
    using Resources;

    /// <summary>
    /// Affiliate List View Model.
    /// </summary>
    public class AffiliateListViewModel
    {
        private readonly List<string> _favoriteStatusList = new List<string>
        {
            "approved",
            "closed",
            "lockedout"
        };

        private readonly List<string> _defaultStatusList = new List<string>
        {
            "approved",
            "pending",
            "lockedout"
        };

        private readonly List<SelectListItem> _dateOptions = new List<SelectListItem>
        {
            new SelectListItem { Value = "0", Text = Lang.All },
            new SelectListItem { Value = "1", Text = Lang.RegisteredDate },
            new SelectListItem { Value = "2", Text = Lang.ApprovalDate }
        };

        private readonly List<SelectListItem> _sortByOptionList = new List<SelectListItem>
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

        private readonly SelectList _baseCurrencyOptionList = new SelectList(AppSettings.BaseCurrencyInAffiliate, "Value", "Text");

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
                return new SelectList(_dateOptions, "Value", "Text");
            }
        }

        public SelectList BaseCurrencyOptionList
        {
            get
            {
                return _baseCurrencyOptionList;
            }
        }

        public IEnumerable<SelectListItem> SortByOptionList
        {
            get
            {
                return _sortByOptionList;
            }
        }

        public IEnumerable<SelectListItem> StatusOptionList
        {
            get
            {
                var statusList = string.IsNullOrEmpty(Status)
                    ? new List<string>()
                    : Status.ToLower().Split(',').ToList();

                if (IsFavorite)
                {
                    // Exclude two status: Pending, Disapproved if isFavorite = true.
                    return OptionManager.AffiliateStatuses
                        .Where(t => _favoriteStatusList.Contains(t.Value.ToLower()))
                        .Select(t => new SelectListItem
                        {
                            Text = t.Text,
                            Value = t.Value,
                            Selected =
                                statusList.Count == 0
                                ? _defaultStatusList.Contains(t.Value.ToLower())
                                : statusList.Contains(t.Value.ToLower())
                        });
                }

                return OptionManager.AffiliateStatuses.Select(t => new SelectListItem
                {
                    Text = t.Text,
                    Value = t.Value,
                    Selected =
                            statusList.Count == 0
                                ? _defaultStatusList.Contains(t.Value.ToLower())
                                : statusList.Contains(t.Value.ToLower())
                });
            }
        }

        public IEnumerable<SelectListItem> CommissionGroupOptionList
        {
            get
            {
                return AppSettings.GetCommGroups().Select(t => new SelectListItem
                {
                    Text = t.GroupName,
                    Value = t.GroupID.ToString(),
                    Selected = GroupId <= 0 || t.GroupID == GroupId
                });
            }
        }
    }
}
