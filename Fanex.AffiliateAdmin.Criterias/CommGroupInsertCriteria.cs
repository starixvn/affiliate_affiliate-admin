// --------------------------------------------------------------------------
// <copyright file="CommGroupInsertCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Joseph.Pham" date="2013.9.20">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.Data.Repository;

    /// <summary>
    /// COMMGroupAddCriteria class.
    /// </summary>
    public class CommGroupInsertCriteria : CriteriaBase
    {
        [SpParam(Ignored = true)]
        public bool CopyForAllProducts { get; set; }

        public int UserId { get; set; }

        public int Currency { get; set; }

        public decimal QualifiedAmount { get; set; }

        public string GroupName { get; set; }

        public string Description { get; set; }

        public string ProductIDList { get; set; }

        public string DeductionList { get; set; }

        public string DeductedPercentageList { get; set; }

        public string TierList { get; set; }

        public string MinAmountList { get; set; }

        public string MaxAmountList { get; set; }

        public string MinQMList { get; set; }

        public string MaxQMList { get; set; }

        public string CommissionList { get; set; }

        public string Language { get; set; }

        public string IP { get; set; }

        public double DepositPercent { get; set; }

        public double WithdrawalPercent { get; set; }

        public int GroupId { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_CommGroupIns";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}