namespace Fanex.AffiliateAdmin.Models
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.AffiliateAdmin.Core.Configurations;
    using Fanex.AffiliateAdmin.Criterias;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ChangeCommissionGroupViewModel
    {
        public string AffIds { get; set; }

        public string Usernames { get; set; }

        public int GroupId { get; set; }

        public int UserId { get; set; }

        public string Currency { get; set; }

        public string Status { get; set; }

        public IEnumerable<SelectListItem> CommissionGroupOptionList
        {
            get
            {
                var comGroups = AppSettings.GetActiveCommGroups(new CommGroupSelActiveListCriteria
                    {
                        UserId = UserId,
                        Currency = Currency
                    });

                return comGroups.Select(t => new SelectListItem
                        {
                            Text = t.GroupName,
                            Value = t.GroupID.ToString(),
                            Selected = t.GroupID == GroupId
                        });
            }
        }
    }
}
