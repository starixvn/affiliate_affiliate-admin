namespace Fanex.AffiliateAdmin.DTO
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Commission Group Information.
    /// </summary>
    public class CommGroupInfo
    {
        public int GroupID { get; set; }

        public string GroupName { get; set; }

        public DateTime CreatedDate { get; set; }

        public int Creator { get; set; }

        public string Description { get; set; }

        public short ExchangeID { get; set; }

        public short Currency { get; set; }

        public decimal QualifiedAmount { get; set; }

        public int IsCurrencyEditable { get; set; }

        public IEnumerable<GroupSetting> GroupSettings { get; set; }

        public IEnumerable<DeductionSetting> DeductionSettings { get; set; }

        public int HasCashback { get; set; }

        public double DepositPercent { get; set; }

        public double WithdrawalPercent { get; set; }
    }
}