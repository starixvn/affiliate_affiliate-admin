namespace Fanex.AffiliateAdmin.Models
{
    using DTO;
    using System.Collections.Generic;

    /// <summary>
    /// Home page ViewModel.
    /// </summary>
    public class HomeViewModel
    {
        #region Fields

        private IList<UserOnline> _onlineList;

        #endregion

        public int PaymentReqNum { get; set; }

        public int AffInProAllNum { get; set; }

        public int DepositMemAllNum { get; set; }

        public decimal DepositAll { get; set; }

        public decimal WinlostAll { get; set; }

        public decimal BonusAll { get; set; }

        public decimal AdjustmentAll { get; set; }

        public decimal TurnoverAll { get; set; }

        public decimal WithdrawalAll { get; set; }

        public decimal NetWinlostAll { get; set; }

        public decimal PSPFeeAll { get; set; }

        public int BannerNum { get; set; }

        public int ImageBannerNum { get; set; }

        public int FlashBannerNum { get; set; }

        public int MediaImageBannerNum { get; set; }

        public int MediaFlashBannerNum { get; set; }

        public int KioskImageBannerNum { get; set; } // Kiosk special case.

        public int KioskFlashBannerNum { get; set; } // Kiosk special case.

        public int AffInProMonthNum { get; set; }

        public int PendingMonthNum { get; set; }

        public int ApprovedMonthNum { get; set; }

        public int DisapprovedAllNum { get; set; }

        public int DisapprovedMonthNum { get; set; }

        public int ClosedMonthNum { get; set; }

        public int PendingAllNum { get; set; }

        public int ApprovedAllNum { get; set; }

        public int ClosedAllNum { get; set; }

        public int LockedAllNum { get; set; }

        public int LockedMonthNum { get; set; }

        public int DepositMemMonthNum { get; set; }

        public decimal DepositMonth { get; set; }

        public decimal WinlostMonth { get; set; }

        public decimal BonusMonth { get; set; }

        public decimal AdjustmentMonth { get; set; }

        public decimal TurnoverMonth { get; set; }

        public decimal WithdrawalMonth { get; set; }

        public decimal NetWinlostMonth { get; set; }

        public decimal PSPFeeMonth { get; set; }

        public int MemInProAllNum { get; set; }

        public int MemInProMonthNum { get; set; }

        public int NonDepositMemMonthNum { get; set; }

        public IList<UserOnline> OnlineList
        {
            get
            {
                return _onlineList ?? new List<UserOnline>();
            }
        }

        #region Methods

        public void AddOnlineList(IList<UserOnline> onlineList)
        {
            _onlineList = onlineList;
        }

        #endregion
    }
}