// --------------------------------------------------------------------------
// <copyright file="CommGroupActionCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Joseph.Pham" date="2013.9.20">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// COMMGroupAddCriteria class.
    /// </summary>
    public class CommGroupActionCriteria : CriteriaBase
    {
        public CriteriaType CriteriaType { get; set; }
 
        [SpParam(Ignored = true)]
        public bool CopyForAllProducts { get; set; }

        public int UserId { get; set; }

        public int? GroupId { get; set; }

        public string Currency { get; set; }

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

        public string IP { get; set; }

        public string Language { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public decimal DepositPercent { get; set; }

        public decimal WithdrawalPercent { get; set; }

        /// <summary>
        /// Get Setting Key.
        /// </summary>
        /// <returns>Setting Key.</returns>
        public override string GetSettingKey()
        {
            switch (this.CriteriaType)
            {
                case CriteriaType.Delete:
                    return "AffAdmin_CommGroupDel";
                case CriteriaType.Insert:
                    return "AffAdmin_CommGroupIns";
                case CriteriaType.Update:
                    return "AffAdmin_CommGroupUpd";
                default:
                    return null;
            }            
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}