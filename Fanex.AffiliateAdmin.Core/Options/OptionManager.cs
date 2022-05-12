namespace Fanex.AffiliateAdmin.Core.Options
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Fanex.AffiliateAdmin.Resources;

    /// <summary>
    /// Manage the hard-code options.
    /// </summary>
    public static class OptionManager
    {
        public static readonly IList<SelectListItem> AffiliateStatuses = new List<SelectListItem>
        {
            new SelectListItem { Value = "approved", Text = Lang.Approved },
            new SelectListItem { Value = "pending", Text = Lang.Pending },
            new SelectListItem { Value = "disapproved", Text = Lang.Disapproved },
            new SelectListItem { Value = "closed", Text = Lang.Closed },
            new SelectListItem { Value = "lockedOut", Text = Lang.Locked }
        };

        public static readonly IList<SelectListItem> AffiliateFavoriteStatuses = new List<SelectListItem>
        {
            new SelectListItem { Value = "approved", Text = Lang.Approved },
            new SelectListItem { Value = "closed", Text = Lang.Closed },
            new SelectListItem { Value = "lockedOut", Text = Lang.Locked }
        };

        public static readonly IList<SelectListItem> DrawFilters = new List<SelectListItem>
        {
            new SelectListItem { Value = "0", Text = Lang.Normal },
            new SelectListItem { Value = "1", Text = Lang.NoDraw },
            new SelectListItem { Value = "2", Text = Lang.NoDrawForTurnover }
        };

        public static readonly IList<SelectListItem> TransferStatusList = new List<SelectListItem>
        {
            new SelectListItem { Text = Lang.All, Value = "0" },
            new SelectListItem { Text = Lang.Processing, Value = "1" },
            new SelectListItem { Text = Lang.Transfered, Value = "2" }
        };

        public static readonly IList<SelectListItem> GroupByListByAffiliateMonth = new List<SelectListItem>
        {
            new SelectListItem { Text = Lang.Affiliate, Value = "0" },
            new SelectListItem { Text = Lang.AffiliateMonth, Value = "1" }
        };

        public static readonly IEnumerable<SelectListItem> SortByForAffiliateCommission = new List<SelectListItem>
        {
            new SelectListItem { Text = Lang.Affiliate, Value = "0" },
            new SelectListItem { Text = Lang.Month, Value = "1" },
            new SelectListItem { Text = Lang.TotalPayment, Value = "2" }
        };

        public static readonly IList<SelectListItem> DateOptions = new List<SelectListItem>
        {
            new SelectListItem { Value = "0", Text = Lang.All },
            new SelectListItem { Value = "1", Text = Lang.RegisteredDate },
            new SelectListItem { Value = "2", Text = Lang.ApprovalDate }
        };

        public static readonly IList<SelectListItem> SortByForAffiliateList = new List<SelectListItem>
        {
            new SelectListItem { Text = Lang.Affiliate, Value = "0" },
            new SelectListItem { Text = Lang.BF, Value = "1" },
            new SelectListItem { Text = Lang.Member, Value = "2" }
        };

        public static readonly IEnumerable<SelectListItem> SortByForAffiliateWinLoss = new List<SelectListItem>
        {
            new SelectListItem { Text = Lang.ActiveMembers, Value = "0" },
            new SelectListItem { Text = Lang.Affiliate, Value = "1" },
            new SelectListItem { Text = Lang.BetCount, Value = "2" },
            new SelectListItem { Text = Lang.MemberComm, Value = "3" },
            new SelectListItem { Text = Lang.Deposit, Value = "4" },
            new SelectListItem { Text = Lang.Month, Value = "5" },
            new SelectListItem { Text = Lang.Turnover, Value = "6" },
            new SelectListItem { Text = Lang.AffiliateWinLoss, Value = "7" },
            new SelectListItem { Text = Lang.Withdrawal, Value = "8" }
        };

        public static readonly IList<SelectListItem> GroupByListByDateMonth = new List<SelectListItem>
        {
            new SelectListItem { Text = Lang.Date, Value = "0" },
            new SelectListItem { Text = Lang.Month, Value = "1" }
        };

        public static readonly IList<SelectListItem> GroupByListByDateMember = new List<SelectListItem>
        {
            new SelectListItem { Text = Lang.Date, Value = "0" },
            new SelectListItem { Text = Lang.Member, Value = "1" }
        };

        public static readonly IList<SelectListItem> SortByForMemberStatistics = new List<SelectListItem>
        {
            new SelectListItem { Text = Lang.Affiliate, Value = "0" },
            new SelectListItem { Text = Lang.Signups, Value = "1" },
            new SelectListItem { Text = Lang.Active, Value = "2" }
        };

        public static readonly IList<SelectListItem> GroupByForMemberStatistics = new List<SelectListItem>
        {
            new SelectListItem { Text = Lang.All,  Value = "0" },
            new SelectListItem { Text = Lang.Date, Value = "1" },
            new SelectListItem { Text = Lang.Month, Value = "2", Selected = true }
        };
    }
}