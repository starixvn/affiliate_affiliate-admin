// --------------------------------------------------------------------------
// <copyright file="CashBackUpdateCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2013.12.10">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Criterias
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class CashBackUpdateCriteria : CriteriaBase
    {
        public int UserId { get; set; }

        public int AffId { get; set; }

        public DateTime MonthDate { get; set; }

        public decimal Amount { get; set; }

        public int ErrCode { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_CashBackUpdate";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}