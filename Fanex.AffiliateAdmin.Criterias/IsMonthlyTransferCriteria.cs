// --------------------------------------------------------------------------
// <copyright file="IsMonthlyTransferCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2013.12.11">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Criterias
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class IsMonthlyTransferCriteria : CriteriaBase
    {
        public DateTime MonthDate { get; set; }

        public bool IsMonthly { get; set; }

        public override string GetSettingKey()
        {
            return "AffAdmin_IsMonthlyTransfer";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}