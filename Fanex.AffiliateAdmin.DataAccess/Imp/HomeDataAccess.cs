// --------------------------------------------------------------------------
// <copyright file="HomeDataAccess.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Johnny.Trinh" date="2013.08.02">Created</change>
// </history>
namespace Fanex.AffiliateAdmin.DataAccess.Impl
{
    using System;
    using Criterias;
    using Data;
    using Interface;
    using Models;

    /// <summary>
    /// Account data access.
    /// </summary>
    public class HomeDataAccess : IHomeDataAccess
    {
        /// <summary>
        /// Home General Information Select.
        /// </summary>
        /// <param name="criteria">Home General Information Criteria.</param>
        /// <returns>Home View Model.</returns>
        public HomeViewModel HomeGeneralInfoSel(HomeGeneralInfoCriteria criteria)
        {
            var result = new HomeViewModel();
            if (criteria.IsValid())
            {
                using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
                {
                    var param = criteria.GetSpParams();
                    objectDb.ExecuteNonQuery(param);
                    result = new HomeViewModel
                    {
                        PendingAllNum = Convert.ToInt32(param["pendingallnum"]),
                        PaymentRequestNum = Convert.ToInt32(param["paymentrequestnum"]),
                        AffInProAllNum = Convert.ToInt32(param["affinproallnum"]),
                        DisapprovedAllNum = Convert.ToInt32(param["disapprovedallnum"]),
                        DepositMemAllNum = Convert.ToInt32(param["depositmemallnum"]),
                        DepositAll = Convert.ToDecimal(param["depositall"]),
                        WinlostAll = Convert.ToDecimal(param["winlostall"]),
                        BonusAll = Convert.ToDecimal(param["bonusall"]),
                        AdjustmentAll = Convert.ToDecimal(param["adjustmentall"]),
                        NetWinlostAll = Convert.ToDecimal(param["netwinlostall"]),
                        PSPFeeAll = Convert.ToDecimal(param["pspfeeall"]),
                        BannerNum = Convert.ToInt32(param["bannernum"]),
                        ImageBannerNum = Convert.ToInt32(param["imagebannernum"]),
                        FlashBannerNum = Convert.ToInt32(param["flashbannernum"]),
                        AffInProMonthNum = Convert.ToInt32(param["affinpromonthnum"]),
                        PendingMonthNum = Convert.ToInt32(param["pendingmonthnum"]),
                        ApprovedMonthNum = Convert.ToInt32(param["approvedmonthnum"]),
                        DisapprovedMonthNum = Convert.ToInt32(param["disapprovedmonthnum"]),
                        ClosedMonthNum = Convert.ToInt32(param["closedmonthnum"]),
                        ClosedAllNum = Convert.ToInt32(param["closedallnum"]),
                        LockedAllNum = Convert.ToInt32(param["lockedallnum"]),
                        LockedMonthNum = Convert.ToInt32(param["lockedmonthnum"]),
                        DepositMemMonthNum = Convert.ToInt32(param["depositmemmonthnum"]),
                        DepositMonth = Convert.ToDecimal(param["depositmonth"]),
                        WinlostMonth = Convert.ToDecimal(param["winlostmonth"]),
                        BonusMonth = Convert.ToDecimal(param["bonusmonth"]),
                        AdjustmentMonth = Convert.ToDecimal(param["adjustmentmonth"]),
                        NetWinlostMonth = Convert.ToDecimal(param["netwinlostmonth"]),
                        PSPFeeMonth = Convert.ToDecimal(param["pspfeemonth"]),
                        MemInProAllNum = Convert.ToInt32(param["meminproallnum"]),
                        MemInProMonthNum = Convert.ToInt32(param["meminpromonthnum"])
                    };
                    return result;
                }
            }

            return result;
        }
    }
}