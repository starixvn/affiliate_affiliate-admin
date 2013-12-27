// --------------------------------------------------------------------------
// <copyright file="MonthlyTransferCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Brian.Dinh" date="2013.12.10">Created</change>
// </history>

namespace Fanex.AffiliateAdmin.Criterias
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MonthlyTransferCriteria : CriteriaBase
    {
        public int UserId { get; set; }

        public int Month { get; set; }

        public int Year { get; set; }

        public override string GetSettingKey()
        {
            return "Task_Aff_MonthlyTransfer";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}